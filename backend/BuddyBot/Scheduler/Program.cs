using Application.Extensions;
using Application.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Extensions;
using Scheduler.Jobs;
using Scheduler.Notifications;
using Scheduler.Schedulers;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddInfrastructure( builder.Configuration, false );
builder.Services.AddApplication();

builder.Services.AddScoped<OnboardingAccessScheduler>();
builder.Services.AddScoped<AccountCreationTokenExpirationScheduler>();

builder.Services.AddHttpClient<INotificationPublisher, NotificationHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TelegramBot:TelegramBotUrl"] ?? "http://telegram-bot-container:5000");
});

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
