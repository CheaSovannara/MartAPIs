using Metrix_MartAPIs.DbContexts;
using Metrix_MartAPIs.Repositories.IRepository;
using Metrix_MartAPIs.Repositories.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Serilog;

namespace Metrix_MartAPIs
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<MetrixMartDbContext>(options =>
                     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddDbContext<MetrixMartDbContext>(options =>
            //           options.UseSqlServer(GetConnectionString("DefaultConnection"))
            //                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            //                .UseSqlServerTimeout(60));
            builder.Services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepsoitory>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>(); // If not already registered
            builder.Services.AddHttpLogging(loggingOptions =>
            {
                // Configure logging options here
                loggingOptions.RequestHeaders.Add("Authorization"); // Example: Log Authorization header
                loggingOptions.ResponseHeaders.Add("Content-Type");
                loggingOptions.RequestBodyLogLimit = 4096; // Limit request body logging to 4KB
                loggingOptions.ResponseBodyLogLimit = 4096;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddRazorPages();
            builder.Logging.AddConsole();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("D:\\C# Wedeskill\\Metrix_MartAPIs\\MartAPIs\\Log\\", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(c => c.SerializeAsV2 = true);
                app.UseSwaggerUI();
            }
            app.UseHttpLogging();
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}