
using System.Text.Json.Serialization;
using ERP.Ticketing.HttpApi.Commons.Hubs;
using ERP.Ticketing.HttpApi.Configuration;
using Opw.HttpExceptions.AspNetCore;
using SixLabors.ImageSharp.Web.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
    .AddHttpExceptions(options =>
    {
        options.IncludeExceptionDetails = _ => false;
        options.ShouldLogException = _ => true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services
    .AddAppIdentity(builder.Configuration)
    .AddAppDatabase(builder.Configuration)
    .AddAppAuthentication(builder.Configuration)
    .AddAppServices()
    .AddOpenTelemetryService()
    .AddMapping()
    .AddFluentValidation();

builder.Services.AddImageSharp();
// builder.Services.

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder
            // .WithOrigins(
            //     "https://ticketing.weezh.ir",
            //     "https://ticketing-api.weezh.ir",
            //     "http://localhost:3000",
            //     "http://localhost:5173",
            //     "http://192.168.1.253:5173",
            //     "http://192.168.1.253",
            //     "http://192.168.1.236",
            //     "http://192.168.1.236:5173",
            //     "http://0.0.0.0:5292"
            // )
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
            // .AllowCredentials();
    }));

var app = builder.Build();

app.UseDbMigrate();
app.UseRouting();
app.UseHttpExceptions();
app.UseImageSharp();
app.UseStaticFiles();
app.UseHttpLogging();
app.UseCors("CorsPolicy");
app.UseOpenTelemetryPrometheusScrapingEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<MessageHub>("/messageHub", options => { });

app.Run();