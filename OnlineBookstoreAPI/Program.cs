using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnlineBookstoreAPI;
using OnlineBookstoreApplication.Services;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreInfrastructure;
using OnlineBookstoreInfrastructure.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add configurations to services
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("DatabaseConfigurations:MongoDB"));
builder.Services.Configure<MySqlSettings>(
    builder.Configuration.GetSection("DatabaseConfigurations:MySqlDB"));
builder.Services.Configure<RedisSettings>(
    builder.Configuration.GetSection("DatabaseConfigurations:RedisDB"));

// Configure MongoDB for Books and Authors
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoSettings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(mongoSettings.ConnectionString);
});

// Configure MySQL for Customers and Orders
builder.Services.AddDbContext<BookstoreDbContext>(options =>
{
    var mysqlSettings = builder.Configuration
        .GetSection("DatabaseConfigurations:CustomersAndOrders")
        .Get<MySqlSettings>();

    options.UseMySql(
        mysqlSettings.ConnectionString,
        Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(mysqlSettings.ConnectionString));
});

// Configure Redis for Inventory Management
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    return ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
});

//Repos
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();

//Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

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
