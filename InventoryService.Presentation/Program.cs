using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Messenger
builder.Services.AddHttpClient();
builder.Services.AddScoped<Shared.Infrastructure.Interfaces.IMessenger, Shared.Infrastructure.Services.HttpMessenger>();

// Register Gateway Auth
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<InventoryService.Presentation.Services.GatewayAuthenticationService>();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InventoryService.Application.Handlers.CreateInventoryItemCommandHandler).Assembly));

// Register DbContext (InMemory for now as placeholder)
builder.Services.AddDbContext<InventoryService.Infrastructure.Persistence.InventoryDbContext>(options =>
    options.UseInMemoryDatabase("InventoryDb"));

// Register Repository
builder.Services.AddScoped<InventoryService.Application.Interfaces.IInventoryRepository, InventoryService.Infrastructure.Persistence.InventoryRepository>();

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