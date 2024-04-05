using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Content.Posts.Repositories;
using Qwitter.Core.Application.Kafka;

namespace Qwitter.Content;

public static class UserModule
{
    public static WebApplicationBuilder ConfigureContentService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPostsRepository, PostsRepository>();
        builder.Services.AddScoped<IMapper, Mapper>();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.UseKafka();

        return builder;
    }
}
