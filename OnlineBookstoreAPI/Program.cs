using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OnlineBookstoreApplication.Services;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreInfrastructure;
using OnlineBookstoreInfrastructure.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var databaseConfigurations = builder.Configuration.GetSection("DatabaseConfigurations");

// MongoDB for Books and Authors
var booksAndAuthorsConfig = databaseConfigurations.GetSection("BooksAndAuthors");
if (booksAndAuthorsConfig.GetValue<string>("DatabaseType") == "MongoDB")
{
    builder.Services.AddSingleton<IMongoClient>(sp =>
        new MongoClient(booksAndAuthorsConfig.GetValue<string>("ConnectionString")));
    builder.Services.AddScoped<IBookRepository, MongoDbBookRepository>(sp =>
        new MongoDbBookRepository(
            sp.GetRequiredService<IMongoClient>(),
            booksAndAuthorsConfig.GetValue<string>("DatabaseName")));
}

// MySQL for Customers and Orders
var customersAndOrdersConfig = databaseConfigurations.GetSection("CustomersAndOrders");
if (customersAndOrdersConfig.GetValue<string>("DatabaseType") == "MySQL")
{
    builder.Services.AddDbContext<BookstoreDbContext>(options =>
        options.UseMySql(
            customersAndOrdersConfig.GetValue<string>("ConnectionString"),
            Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(customersAndOrdersConfig.GetValue<string>("ConnectionString"))));
    //builder.Services.AddScoped<ICustomerRepository, MySqlCustomerRepository>();
    //builder.Services.AddScoped<IOrderRepository, MySqlOrderRepository>();
}

// Redis for Inventory Management
var inventoryManagementConfig = databaseConfigurations.GetSection("InventoryManagement");
if (inventoryManagementConfig.GetValue<string>("DatabaseType") == "Redis")
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = inventoryManagementConfig.GetValue<string>("ConnectionString");
    });
}

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
