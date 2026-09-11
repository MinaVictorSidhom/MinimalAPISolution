using MinimalAPI.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Product> products = new List<Product>()
{
    new Product() {Id=1,ProductName="Smart Phone"},
    new Product() {Id=2,ProductName="Samrt TV"}
};


app.MapGet("/products", async (HttpContext context) =>
{
    //var content = string.Join('\n', products.Select(temp => temp.ToString())); //override ToString()
    await context.Response.WriteAsync(JsonSerializer.Serialize(products));
});

app.MapGet("/products/{id:int}", async (HttpContext context,int id) =>
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

app.MapPost("/products", async (HttpContext context,Product product) =>
{
    products.Add(product);
    await context.Response.WriteAsync("Product Added");
});

app.Run();
