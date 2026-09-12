using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MinimalAPI.RouteGroups
{
    public static class ProductsMapGroup
    {
        private static List<Product> products = new List<Product>()
    {
        new Product() {Id=1,ProductName="Smart Phone"},
        new Product() {Id=2,ProductName="Samrt TV"}
    };
        public static RouteGroupBuilder ProductAPI(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (HttpContext context) =>
            {
                //var content = string.Join('\n', products.Select(temp => temp.ToString())); //override ToString()
                await context.Response.WriteAsync(JsonSerializer.Serialize(products));
            });

            group.MapGet("/{id:int}", async (HttpContext context, int id) =>
            {
                Product? product = products.FirstOrDefault(temp => temp.Id == id);

                if (product == null)
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Incorrect Product ID");
                    return;
                }
                await context.Response.WriteAsync(JsonSerializer.Serialize(product));
            });

            group.MapPost("/", async (HttpContext context, Product product) =>
            {
                products.Add(product);
                await context.Response.WriteAsync("Product Added");
            });

            group.MapPut("/{id}", async (HttpContext context, int id, [FromBody] Product product) =>
            {
                Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);

                if (productFromCollection == null)
                {
                    context.Response.StatusCode=400; // Bad Request
                    await context.Response.WriteAsync("Incorrect Product ID");
                    return;
                }

                productFromCollection.ProductName = product.ProductName;
                await context.Response.WriteAsync("Product Updated");
            });

            group.MapDelete("/{id}", async (HttpContext context, int id) =>
            {
                Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);

                if (productFromCollection == null)
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Incorrect Product ID");
                    return;
                }

                products.Remove(productFromCollection);
                await context.Response.WriteAsync("Product Deleted");
            });

            return group;
        }
    }
}
