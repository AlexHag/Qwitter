using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Qwitter.Web.Api;
using Qwitter.Web.Services;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserClient, UserClient>();

builder.Services.AddSingleton<AuthenticationService>
(
    new AuthenticationService
    (
        X509Certificate2.CreateFromPemFile(builder.Configuration["Jwt:CertificatePath"]!),
        builder.Configuration["Jwt:Issuer"]!,
        builder.Configuration["Jwt:Audience"]!
    )
);

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => 
{
    string keyText = File.ReadAllText(builder.Configuration["Jwt:PublicKeyPath"]!);
    RSA rsa = RSA.Create();
    rsa.ImportFromPem(keyText);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
        ValidAudience = builder.Configuration["Jwt:Audience"]!,
        IssuerSigningKey = new RsaSecurityKey(rsa),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
