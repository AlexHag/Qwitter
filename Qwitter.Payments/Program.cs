using Microsoft.EntityFrameworkCore;
using Qwitter.Payment.Database;
using Qwitter.Payment.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<INethereumService, NethereumService>(n => 
    new NethereumService(builder.Configuration["Nethereum:ApiBaseAddress"] + builder.Configuration["Nethereum:ApiKey"]));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
