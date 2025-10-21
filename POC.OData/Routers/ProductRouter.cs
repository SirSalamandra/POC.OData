
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using POC.OData.Domain.Entities;
using POC.OData.Infrastructure.Data;

namespace POC.OData.Routers
{
    public static class ProductRouter
    {
        public static void MapProductRoutes(this IEndpointRouteBuilder app)
        {
            var odataGroup = app.MapGroup("odata/products");
            var group = app.MapGroup("products");

            // OData GET
            odataGroup.MapGet("/", GetProducts);
            odataGroup.MapGet("/{key:int}", GetProductById);

            // Standard CUD
            group.MapPost("/", CreateProduct);
            group.MapPut("/{key:int}", UpdateProduct);
            group.MapDelete("/{key:int}", DeleteProduct);
        }

        // GET /odata/products
        private static IResult GetProducts(ODataQueryOptions<Product> options, AppDbContext context)
        {
            var productsQuery = context.Products.AsNoTracking();
            return Results.Ok(options.ApplyTo(productsQuery));
        }

        // GET: /odata/products/1
        private static IResult GetProductById(int key, ODataQueryOptions<Product> options, AppDbContext context)
        {
            var productQuery = context.Products.AsNoTracking().Where(x => x.Id == key);
            return Results.Ok(options.ApplyTo(productQuery));
        }

        // POST /products
        private static async Task<IResult> CreateProduct(Product product, AppDbContext context)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return Results.Created($"/odata/Products/{product.Id}", product);
        }

        // PUT /products?key=1
        private static async Task<IResult> UpdateProduct(int key, Product updatedProduct, AppDbContext context)
        {
            if (key != updatedProduct.Id)
            {
                return Results.BadRequest("ID mismatch");
            }

            var product = await context.Products.FindAsync(key);
            if (product is null)
            {
                return Results.NotFound();
            }

            context.Entry(product).CurrentValues.SetValues(updatedProduct);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }

        // DELETE /products?key=1
        private static async Task<IResult> DeleteProduct(int key, AppDbContext context)
        {
            var product = await context.Products.FindAsync(key);
            if (product is null)
            {
                return Results.NotFound();
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
