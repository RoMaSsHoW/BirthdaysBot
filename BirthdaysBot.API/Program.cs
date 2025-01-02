using BirthdaysBot.API.TestService;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var botToken = configuration["TelegramSettings:BotToken"];
var webhookUrl = configuration["TelegramSettings:WebhookUrl"];

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
builder.Services.ConfigureTelegramBotMvc();

builder.Services.AddScoped<ServiceTest>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
