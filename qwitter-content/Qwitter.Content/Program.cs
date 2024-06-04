using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Content.Posts.Repositories;
using Qwitter.Content.Users.Consumers;
using Qwitter.Core.Application.Kafka;
using Qwitter.Core.Application;
using Qwitter.Content.Users.Repositories;
using Mapster;
using Qwitter.Content.Posts.Models;
using Qwitter.Content.Contract.Posts.Models;
using Qwitter.Users.Premium.Consumers;
using Qwitter.Content.Comments.Repositories;

namespace Qwitter.Content;

public static class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureServices()
            .Build()
            .ConfigureApp()
            .Run();
        
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPostsRepository, PostsRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();

        builder.Services.AddScoped<IMapper, Mapper>();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.RegisterConsumer<UserCreatedConsumer>("content");
        builder.RegisterConsumer<PremiumPurchasedConsumer>("content");

        builder.UseKafka();

        ConfigureMappings();

        return builder;
    }

    public static void ConfigureMappings()
    {
        TypeAdapterConfig<PostEntity, PostResponse>.NewConfig()
            .Map(dest => dest.Username, src => src.User.Username)
            .Map(dest => dest.HasPremium, src => src.User.HasPremium);
    }
}