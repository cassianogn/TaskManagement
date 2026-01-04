using TaskManagement.Application;
using TaskManagement.Application.TaskItems.Commands.AddTaskItem;
using TaskManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var addHandler = services.GetRequiredService<AddTaskItemCommandHandler>();
    var addCommand = new AddTaskItemCommand("Test Task");
    await addHandler.HandleAsync(addCommand, default);

    var addCommand2 = new AddTaskItemCommand("Test Task 2");
    await addHandler.HandleAsync(addCommand2, default);
}

app.Run();


