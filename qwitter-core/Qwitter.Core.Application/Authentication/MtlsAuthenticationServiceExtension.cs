using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qwitter.Core.Application.Extensions;

namespace Qwitter.Core.Application.Authentication;

public static class MtlsAuthenticationServiceExtension
{
    public static WebApplicationBuilder AddMtlsAuthentication(this WebApplicationBuilder builder)
    {
        var rootCaCertificate = builder.GetRootMtlsCaCertificate();

        if (rootCaCertificate == null)
        {
            Console.WriteLine("Root CA certificate is not found. mTLS authentication is disabled.");
            return builder;
        }

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.AllowAnyClientCertificate();
                httpsOptions.CheckCertificateRevocation = false;
                httpsOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
            });
        });

        builder.Services.AddAuthentication()
            .AddCertificate("mTLS", options =>
            {
                options.AllowedCertificateTypes = CertificateTypes.All;
                options.RevocationMode = X509RevocationMode.NoCheck;
                options.ChainTrustValidationMode = X509ChainTrustMode.CustomRootTrust;
                options.CustomTrustStore =
                [
                    rootCaCertificate
                ];

                options.Events = new CertificateAuthenticationEvents
                {
                    OnCertificateValidated = context =>
                    {
                        var chain = new X509Chain
                        {
                            ChainPolicy = new X509ChainPolicy
                            {
                                RevocationMode = X509RevocationMode.NoCheck,
                                VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority,
                                ExtraStore = { rootCaCertificate }
                            }
                        };

                        var isChainValid = chain.Build(context.ClientCertificate);

                        if (isChainValid && chain.ChainElements[^1].Certificate.Thumbprint == rootCaCertificate.Thumbprint)
                        {
                            context.Success();
                        }
                        else
                        {
                            context.Fail("Client certificate is not signed by the trusted root CA.");
                        }

                        return Task.CompletedTask;
                    }
                };
            }
        );

        return builder;
    }

    private static X509Certificate2? GetRootMtlsCaCertificate(this WebApplicationBuilder builder)
    {
        var clientCertificateConfiguration = builder.Configuration.GetSection("certificates")?.Get<Dictionary<string, string>>();

        if (clientCertificateConfiguration == null ||
            !clientCertificateConfiguration.TryGetValue("root_ca_path", out var rootCertPath) ||
            !File.Exists(PathExtensions.AssemblyPath(rootCertPath)))
        {
            return null;
        }

        return new X509Certificate2(PathExtensions.AssemblyPath(rootCertPath));
    }
}
