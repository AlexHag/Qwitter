namespace Qwitter.Ledger.Contract.Transactions.Domain;

public enum TransactionCategory
{
    Unknown,
    InternalTransfer,
    CryptoDeposit,
    CryptoWithdrawal,
    // SwishDeposit,
    // SwishWithdrawal
}