using System.Net;
using HttpsTest.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



// Configure Certificates Secrets
builder.Host.ConfigureServices(
    (context, services) =>
    {
      HostConfig.CertPath = context.Configuration["CertPath"];
      HostConfig.CertPassword = context.Configuration["CertPassword"];
    }
);

// Configure Kestrel to use our local Certificate
builder.WebHost.ConfigureKestrel(
    options =>
    {
      if (builder.Environment.IsDevelopment())
      {
        var host = Dns.GetHostEntry("weather.io");
        options.Listen(host.AddressList[0], 5000);
        options.Listen(
            host.AddressList[0],
            5001,
            (listOptions) =>
            {
              listOptions.UseHttps(
                     HostConfig.CertPath,
                     HostConfig.CertPassword
                 );
            });
      }
      else
      {
        options.ListenAnyIP(5000);
        options.ListenAnyIP(
          5001,
          (listOptions) =>
          {
            listOptions.UseHttps(
                   HostConfig.CertPath,
                   HostConfig.CertPassword
               );
          });
      }
    }
);

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
