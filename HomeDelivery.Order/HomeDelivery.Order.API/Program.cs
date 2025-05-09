using Confluent.Kafka;
using FluentValidation;
using HomeDelivery.Order.API;
using HomeDelivery.Order.Business.ServiceRegistrations;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Business.UseCase.Order;
using HomeDelivery.Order.Business.Validator.Order;
using HomeDelivery.Order.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration, builder.Environment);
builder.Services.AddScoped<IValidator<OrderCreateCommand>, OrderCreateCommandValidator>();
builder.Services.AddTransient<System.Random>();
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<HostOptions>(options =>
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost);

builder.Services
    .AddSingleton(new ConsumerBuilder<byte[], byte[]>(new ConsumerConfig()
        {
            BootstrapServers = "localhost:9092",
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "newTestTopic",
        })
    .Build());

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "order_test";
});

builder.Services.AddSingleton(new ProducerConfig
{
    BootstrapServers = "localhost:9092" // Укажите ваш Kafka сервер
});

builder.Services.AddScoped<ProducerOrderService>();

builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var config = sp.GetRequiredService<ProducerConfig>();
    
    return new ProducerBuilder<Null, string>(config).Build();
});

var redisConnection = ConnectionMultiplexer.Connect("localhost:9191, abortConnect=false, password=Admin123!" ); // Замените строку подключения на вашу
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);
builder.Services.AddScoped<IDatabase>(sp => redisConnection.GetDatabase());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

#region MigrateAuto
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    
    var appliedMigrations = dbContext.Database.GetAppliedMigrations();
    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    var missingMigrations = pendingMigrations.Except(appliedMigrations).ToList();
        
    if (missingMigrations.Any())
    {
        Console.WriteLine("There are pending migrations:");
        foreach (var migration in missingMigrations)
        {
            Console.WriteLine(migration);
        }
    }
    else
    {
        Console.WriteLine("All migrations are up-to-date.");
    }
    
    try
    {
        dbContext.Database.Migrate();
        
        
        Console.WriteLine("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
    
}

#endregion

app.Run();

