using System.Reflection;
using Infrastructure.Extensions;
using Application.Extensions;
using Telegram.Bot;
using TelegramBot.BackgroundServices.Services;
using TelegramBot.Interfaces;
using TelegramBot.Notifiers;
using TelegramBot.Routers;
using TelegramBot.Services;
using Hangfire;
using Hangfire.PostgreSql;

IHost host = Host.CreateDefaultBuilder( args )
    .ConfigureServices( ( context, services ) =>
    {
        IConfiguration configuration = context.Configuration;

        services.AddInfrastructure( configuration, false );
        services.AddApplication();

        string? botToken = configuration[ "TelegramBot:Token" ];
        if (string.IsNullOrWhiteSpace(botToken))
        {
            throw new InvalidOperationException("TelegramBot:Token не задан в конфигурации!");
        }

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<CandidateService>();
        services.AddScoped<UserService>();
        services.AddScoped<HRNotifier>();

        services.AddHangfire(config =>
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(c =>
                    c.UseNpgsqlConnection(configuration.GetConnectionString("HangfireConnection")))
        );

        services.AddHttpClient( "telegram_bot_client" )
            .RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>( ( httpClient, sp ) =>
            {
                TelegramBotClientOptions options = new( botToken );
                return new TelegramBotClient( options, httpClient );
            } );

        services.AddScoped<NotificationService>();
        services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();
        services.AddScoped<MediaService>();
        services.AddScoped<FeedbackService>();


        foreach ( Type type in Assembly.GetExecutingAssembly().GetTypes() )
        {
            if ( typeof( IMessageHandler ).IsAssignableFrom( type ) && type.IsClass && !type.IsAbstract )
            {
                services.AddScoped( typeof( IMessageHandler ), type );
            }

            if ( typeof( IStepHandler ).IsAssignableFrom( type ) && type.IsClass && !type.IsAbstract )
            {
                services.AddScoped( typeof( IStepHandler ), type );
            }

            if ( typeof( ICallbackHandler ).IsAssignableFrom( type ) && type.IsClass && !type.IsAbstract )
            {
                services.AddScoped( typeof( ICallbackHandler ), type );
            }
        }

        services.AddScoped<MessageRouter>();
        services.AddScoped<StepRouter>();

        services.AddHostedService<StartupBotInitializer>();
        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
    } )
    .Build();

await host.RunAsync();
