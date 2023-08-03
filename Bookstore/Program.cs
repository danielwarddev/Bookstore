using Bookstore.Books;
using Bookstore.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IBookClient, BookClient>(client =>
{
    client.BaseAddress = new Uri("https://gutendex.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddDbContext<BookstoreContext>(options => options.UseNpgsql("Host=localhost;Database=bookstore;Username=postgres;Password=postgres"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }