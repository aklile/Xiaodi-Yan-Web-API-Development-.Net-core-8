using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Extensions;
using MyFirstApi.Services;
using Serilog;
using Serilog.Formatting.Json;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Development
});
builder.Services.AddDbContext<InvoiceDbContext>(options =>
options.UseSqlServer(builder.Configuration.
GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPostService, PostsService>();
builder.Services.AddSingleton<IDemoService, DemoService>();
builder.Services.AddLifetimeServices();
builder.Services.AddConfigigration(builder.Configuration);
/*builder.Services.Configure<DatabaseOption>(builder.Configuration.
 GetSection(DatabaseOption.SectionName));*/
builder.Services.AddRateLimiter(_ =>
_.AddFixedWindowLimiter(policyName: "fixed", options =>
{
    options.PermitLimit = 5;
    options.Window = TimeSpan.FromSeconds(10);
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    options.QueueLimit = 2;
}
));
builder.Services.AddRequestTimeouts();

var logger = new LoggerConfiguration().WriteTo.File(Path.
Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log.txt"),
rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90)
.WriteTo.Console(new JsonFormatter())
.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Read the environment variable ASPNETCORE_ENVIRONMENT
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

Console.WriteLine($"ASPNETCORE_ENVIRONMENT is {environmentName}");



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRateLimiter();
app.UseRequestTimeouts();
app.UseCorrelationId();
app.MapControllers();
app.UseRouting();
app.MapGet("/rate-limiting-mini", () => Results.Ok($"Hello {DateTime.Now.Ticks.ToString()}")).RequireRateLimiting("fixed"); ;
/*app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request Host: {context.Request.Host}");
    logger.LogInformation("My Middleware - Before");
    await next(context);
    logger.LogInformation("My Middleware - After");
    logger.LogInformation($"Response StatusCode: {context.Response.
    StatusCode}");
});
    app.Use(async (context, next) =>
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"ClientName HttpHeader in Middleware 1:{ context.Request.Headers["ClientName"]}");
        logger.LogInformation($"Add a ClientName HttpHeader in Middleware 1");
       context.Request.Headers.TryAdd("ClientName", "Windows");
        logger.LogInformation("My Middleware 1 - Before");
        await next(context);
        logger.LogInformation("My Middleware 1 - After");
        logger.LogInformation($"Response StatusCode in Middleware 1:{ context.Response.StatusCode}");
});
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"ClientName HttpHeader in Middleware 2:{ context.Request.Headers["ClientName"]} ");
logger.LogInformation("My Middleware 2 - Before");
    context.Response.StatusCode = StatusCodes.Status202Accepted; 
    await next(context);
    logger.LogInformation("My Middleware 2 - After");
    logger.LogInformation($"Response StatusCode in Middleware 2:{ context.Response.StatusCode}  ");
});
app.Map("/lottery", app =>
{
  var random = new Random();
  var luckyNumber = random.Next(1, 6);
  app.UseWhen(context => context.Request.QueryString.Value ==$"?{luckyNumber.ToString()}", app =>
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"You win! You got the lucky number { luckyNumber}!");
            });
        });
  app.UseWhen(context => string.IsNullOrWhiteSpace(context.Request.QueryString.Value), app =>
    {
        app.Use(async (context, next) =>
        {
            var number = random.Next(1, 6);
            context.Request.Headers.TryAdd("number", number.ToString());
            await next(context);
        });
        app.UseWhen(context => context.Request.Headers["number"] == luckyNumber.ToString(), app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync($"You win! You got the lucky number { luckyNumber} !");
                 });
            });
    });
    app.Run(async context =>
    {
        var number = "";
        if (context.Request.QueryString.HasValue)
        {
            number = context.Request.QueryString.Value?.Replace("?",""); 
        }
        else
        {
            number = context.Request.Headers["number"];
        }
        await context.Response.WriteAsync($"Your number is {number}. Try again!");
    });
});
*/
/*app.Run(async context =>
{

    await context.Response.WriteAsync($"Use the /lottery URL to play.You can choose your number with the format / lottery ? 1.");
});*/

app.Run();
