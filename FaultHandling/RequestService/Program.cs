using RequestService.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Http Client
builder.Services.AddHttpClient(
    "TestClient",
    options =>
    {
      options.DefaultRequestHeaders.Clear();
      options.BaseAddress = new Uri(
          "https://localhost:7100/api/response/25"
      );
    }
).AddPolicyHandler(
    req => req.Method == HttpMethod.Get
        ? new ClientPolicy().ExponentialHttpRetry
        : new ClientPolicy().LinearHttpRetry
);

// POLLY
builder.Services.AddSingleton<ClientPolicy>(new ClientPolicy());


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
