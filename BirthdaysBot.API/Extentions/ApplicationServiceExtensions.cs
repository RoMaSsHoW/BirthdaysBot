namespace BirthdaysBot.API.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureTelegramBotClient(services, configuration);

            ConfigureDbContext(services, configuration);

            ConfigureServices(services);

            ConfigureQuartz(services);

            ConfigureCorsPolicy(services);

            AddSwagger(services);

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgeDbConnection"));
            });
        }

        private static void ConfigureCorsPolicy(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        private static void ConfigureQuartz(IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("BirthdayReminderJob");

                q.AddJob<BirthdayReminderJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("BirthdayReminderTrigger")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(9, 0)
                        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tashkent")))); // Запуск в 9:00 по Ташкенту
        });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBirthdayRepository, BirthdayRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUpdateHandler, UpdateHandler>();
            services.AddScoped<BaseCommand, StartCommand>();
            services.AddScoped<BaseCommand, AddBirthdayCommand>();
            services.AddScoped<BaseCommand, ShowBirthdaysCommand>();
            services.AddScoped<BaseCommand, DeleteBirthdayCommand>();
            services.AddScoped<BaseCommand, UpdateBirthdayCommand>();
            services.AddSingleton<StateMachine>();
        }

        private static void ConfigureTelegramBotClient(IServiceCollection services, IConfiguration configuration)
        {
            var botToken = configuration.GetSection("TelegramSettings").Get<TelegramConstant>()?.BotToken;

            if (string.IsNullOrEmpty(botToken))
            {
                throw new InvalidOperationException("Bot token is not configured.");
            }

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

            services.ConfigureTelegramBotMvc();
        }
    }
}
