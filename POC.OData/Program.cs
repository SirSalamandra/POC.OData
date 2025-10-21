
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using POC.OData.Domain.Entities;
using POC.OData.Domain.Enums;
using POC.OData.Infrastructure.Data;
using POC.OData.Routers;

namespace POC.OData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure OData Entity Data Model (EDM)
            var edmBuilder = new ODataConventionModelBuilder();
            edmBuilder.EntitySet<Client>("Clients");
            edmBuilder.EntitySet<Product>("Products");
            edmBuilder.EntitySet<Order>("Orders");
            edmBuilder.EntitySet<OrderItem>("OrderItems");

            // Register the enums and complex type
            edmBuilder.EnumType<OrderStatus>();

            var edmModel = edmBuilder.GetEdmModel();

            builder.Services.AddOData(options => options
                    .Select()
                    .Filter()
                    .OrderBy()
                    .Expand()
                    .Count()
                    .SetMaxTop(100));

            // Add DbContext with SQLite
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=poc.db";
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            //app.UseAuthorization();
            //app.MapControllers();

            // Map the routers
            app.MapClientRoutes();
            app.MapProductRoutes();

            app.Run();
        }
    }
}
