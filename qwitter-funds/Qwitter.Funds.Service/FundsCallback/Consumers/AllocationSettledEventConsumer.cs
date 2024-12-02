using MassTransit;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Funds.Contract.Events;
using Qwitter.Funds.Contract.FundsCallback;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Allocations.Repositories;
using Qwitter.Funds.Service.Clients.Repositories;

namespace Qwitter.Funds.Service.FundsCallback.Consumers;

public class AllocationSettledEventConsumer : IConsumer<AllocationSettledEvent>
{
    private readonly IAllocationRepository _allocationRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IRestApiClientManager _restApiClientManager;
    private readonly ILogger _logger;

    public AllocationSettledEventConsumer(
        IAllocationRepository allocationRepository,
        IAccountRepository accountRepository,
        IClientRepository clientRepository,
        IRestApiClientManager restApiClientManager,
        ILogger logger)
    {
        _allocationRepository = allocationRepository;
        _accountRepository = accountRepository;
        _clientRepository = clientRepository;
        _restApiClientManager = restApiClientManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AllocationSettledEvent> context)
    {
        var allocation = await _allocationRepository.GetById(context.Message.AllocationId);
        var sourceAccount = await _accountRepository.GetById(allocation.SourceAccountId);
        var destinationAccount = await _accountRepository.GetById(allocation.DestinationAccountId!.Value);

        var sourceClient = await _clientRepository.GetById(sourceAccount.ClientId);
        var destinationClient = await _clientRepository.GetById(destinationAccount.ClientId);

        var sourceApiClient = _restApiClientManager.GetApiClientFor<IFundsCallbackService>(sourceClient.CallbackUrl);
        var destinationApiClient = _restApiClientManager.GetApiClientFor<IFundsCallbackService>(destinationClient.CallbackUrl);

        try
        {
            await sourceApiClient.OnFundsSettledFrom(context.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to notify source client about settled funds");
        }

        try
        {
            await destinationApiClient.OnFundsSettledInto(context.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to notify destination client about settled funds");
        }
    }
}
