using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using POC.OData.Domain.Entities;
using POC.OData.Infrastructure.Data;

namespace POC.OData.Routers
{
    public static class ClientRouter
    {
        public static void MapClientRoutes(this IEndpointRouteBuilder app)
        {
            var odataGroup = app.MapGroup("odata/clients");
            var group = app.MapGroup("clients");

            // OData GET
            odataGroup.MapGet("/", GetClients);
            odataGroup.MapGet("/{key}", GetClientById);

            // Standard
            group.MapPost("/", CreateClient);
            group.MapPut("/{key}", UpdateClient);
            group.MapDelete("/{key}", DeleteClient);
        }

        // GET /odata/clients
        private static IResult GetClients(ODataQueryOptions<Client> options, AppDbContext context)
        {
            var clientsQuery = context.Clients.AsNoTracking();
            return Results.Ok(options.ApplyTo(clientsQuery));
        }

        // GET /odata/clients/1
        private static IResult GetClientById(int key, ODataQueryOptions<Client> options, AppDbContext context)
        {
            var clientQuery = context.Clients.AsNoTracking().Where(x => x.Id == key);
            return Results.Ok(options.ApplyTo(clientQuery));
        }

        // POST /clients
        private static async Task<IResult> CreateClient(Client client, AppDbContext context)
        {
            context.Clients.Add(client);
            await context.SaveChangesAsync();
            return Results.Created($"/odata/Clients/{client.Id}", client);
        }

        // PUT /clients?key=1
        private static async Task<IResult> UpdateClient(int key, Client updatedClient, AppDbContext context)
        {
            if (key != updatedClient.Id)
            {
                return Results.BadRequest("ID mismatch");
            }

            var client = await context.Clients.FindAsync(key);
            if (client is null)
            {
                return Results.NotFound();
            }

            context.Entry(client).CurrentValues.SetValues(updatedClient);

            await context.SaveChangesAsync();
            return Results.NoContent();
        }

        // DELETE /clients?key=1
        private static async Task<IResult> DeleteClient(int key, AppDbContext context)
        {
            var client = await context.Clients.FindAsync(key);
            if (client is null)
            {
                return Results.NotFound();
            }

            context.Clients.Remove(client);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
