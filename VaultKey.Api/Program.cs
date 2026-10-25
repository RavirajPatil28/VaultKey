using Microsoft.EntityFrameworkCore;
using VaultKey.Business.Extensions;
using VaultKey.Business.Security;

namespace VaultKey.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.Configure<EncryptionOptions>(
                builder.Configuration.GetSection(EncryptionOptions.SectionName));

            builder.Services
                .AddVaultKeyServices()
                .AddBankAuthDatabases(opt =>
                {
                    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
                    opt.UseSqlServer(connStr, opt => opt.EnableRetryOnFailure(5));
                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "v1"); // Points to the native OpenAPI JSON
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
