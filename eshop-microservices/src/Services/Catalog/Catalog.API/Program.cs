var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// will manage the API's end points
builder.Services.AddCarter();

// Register MediatR services - tell MediatR to scan the assembly for handlers (Commands/Query)
builder.Services.AddMediatR(config =>
{ 
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(optn =>
{
    optn.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

//6-6 completed

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.Run();
