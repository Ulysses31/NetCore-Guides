using GlobalErrorHandling.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//***** Setup Middlewares *****//

// Generic middleware example
// app.Use((context, next) =>
// {
//     // Logic
//     Console.WriteLine("Middleware before next.....");
// 
//     var task = next(context);
// 
//     // Additional Logic
//     Console.WriteLine("Middleware after next.....");
// 
//     return task;
// });

// Global Error Handling
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
