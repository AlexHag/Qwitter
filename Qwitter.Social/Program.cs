using Microsoft.EntityFrameworkCore;
using Qwitter.Social.Database;
using Qwitter.Social.Kafka;
using Qwitter.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddUserClient(builder.Configuration["Services:UsersBaseAddress"]!);

builder.Services.AddKafka(AppDomain.CurrentDomain.GetAssemblies());

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
