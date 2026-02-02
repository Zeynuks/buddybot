using Application.Extensions;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Extensions;
using Scheduler.Jobs;
using Scheduler.Schedulers;
using Telegram.Bot;
using TelegramBot.Notifiers;
using TelegramBot.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddInfrastructure( builder.Configuration, false );
builder.Services.AddApplication();

string? botToken = builder.Configuration[ "TelegramBot:Token" ];
if ( string.IsNullOrWhiteSpace( botToken ) )
{
    throw new InvalidOperationException( "TelegramBot:Token не задан в конфигурации!" );
}

builder.Services.AddSingleton<ITelegramBotClient>( new TelegramBotClient( botToken! ) );

builder.Services.AddScoped<OnboardingAccessScheduler>();
builder.Services.AddScoped<AccountCreationTokenExpirationScheduler>();
builder.Services.AddScoped<OnboardingNotifier>();
builder.Services.AddScoped<FeedbackNotifier>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<CandidateService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddHangfire( config =>
    config.SetDataCompatibilityLevel( CompatibilityLevel.Version_180 )
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage( c =>
            c.UseNpgsqlConnection( builder.Configuration.GetConnectionString( "HangfireConnection" ) ) ) );

builder.Services.AddHangfireServer();

WebApplication app = builder.Build();
app.UseHangfireDashboard();

RecurringJobsRegistration.Register( app.Services );

app.Run();
