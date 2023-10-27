using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Qwitter.Domain;
using Qwitter.Web.Services;
using System.Security.Cryptography.X509Certificates;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserClient(builder.Configuration["Services:UsersBaseAddress"]!);
builder.Services.AddContentClient(builder.Configuration["Services:ContentBaseAddress"]!);
builder.Services.AddPaymentClient(builder.Configuration["Services:PaymentsBaseAddress"]!);

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
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Qwitter API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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
