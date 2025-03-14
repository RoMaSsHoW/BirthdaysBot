using BirthdaysBot.BLL.Commands;
using BirthdaysBot.BLL.Helpers;
using BirthdaysBot.BLL.Repositories;
using BirthdaysBot.BLL.Services;
using BirthdaysBot.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var botToken = configuration["TelegramSettings:BotToken"];

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgeDbConnection"));
});

builder.Services.ConfigureTelegramBotMvc();

builder.Services.AddScoped<IBirthdayRepository, BirthdayRepository>();
builder.Services.AddScoped<IUpdateHandler, UpdateHandler>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<BaseCommand, StartCommand>();
builder.Services.AddScoped<BaseCommand, AddBirthdayCommand>();
builder.Services.AddScoped<BaseCommand, ShowBirthdaysCommand>();
builder.Services.AddScoped<BaseCommand, DeleteBirthdayCommand>();
builder.Services.AddScoped<BaseCommand, UpdateBirthdayCommand>();
builder.Services.AddSingleton<StateMachine>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

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
