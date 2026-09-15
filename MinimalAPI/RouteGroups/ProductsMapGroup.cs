using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Endpoint_Filters;
using MinimalAPI.Models;
using System.ComponentModel.DataAnnotations;
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
                // await context.Response.WriteAsync("Product Added");
                return Results.Ok("Product Added");
            })
                .AddEndpointFilter<CustomEndpointFilter>()
                .AddEndpointFilter(async (EndpointFilterInvocationContext context, EndpointFilterDelegate next) =>
                {
                    var product = context.Arguments.OfType<Product>().FirstOrDefault();

                    if (product == null)
                    {
                        return Results.BadRequest("Product details are not found in the request");
                    }

                    var validationContext = new ValidationContext(product);
                    List<ValidationResult> errors = new List<ValidationResult>();
                    bool isValid = Validator.TryValidateObject(product, validationContext, errors,true);

                    if (!isValid)
                    {
                        return Results.BadRequest(errors.FirstOrDefault()?.ErrorMessage);
                    }
                     

                    var result = await next(context); //invokes the subsequent endpoint filter or endpoint's request delegate


                    //After logic here
                    return result;
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
                    //context.Response.StatusCode = 400; // Bad Request
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id",new string[]{ " Incorrect product ID"} }
                    });
                }

                products.Remove(productFromCollection);
                // await context.Response.WriteAsync("Product Deleted");

                return Results.Ok(new { message= "Product Deleted "});
            });

            return group;
        }
    }
}
