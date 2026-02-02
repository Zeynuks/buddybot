using System.Text;
using System.Text.Json.Serialization;
using Application.Options;
using Application.Extensions;
using Domain.Enums;
using Infrastructure.Extensions;
using Infrastructure.Foundation.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Infrastructure.Foundation.Database.Seeding;

var builder = WebApplication.CreateBuilder( args );

Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration( builder.Configuration )
   .Enrich.FromLogContext()
   .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddInfrastructure( builder.Configuration, true );
builder.Services.AddApplication();
builder.Services.AddAutoMapper( AppDomain.CurrentDomain.GetAssemblies() );

builder.Services.AddControllers()
    .AddJsonOptions( options =>
     {
         options.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter() );
     } );

builder.Services.AddCors( options =>
{
    options.AddPolicy( "AllowLocalhost", policy =>
    {
        policy.WithOrigins( "http://localhost:5173" )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    } );
} );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TelegramBotOptions>( builder.Configuration.GetSection( "TelegramBot" ) );
builder.Services.Configure<JwtOptions>( builder.Configuration.GetSection( "JwtOptions" ) );
builder.Services.Configure<FileStorageOptions>( builder.Configuration.GetSection( "FileStorageOptions" ) );

var jwtOptions = builder.Configuration.GetSection( "JwtOptions" ).Get<JwtOptions>();
if ( string.IsNullOrWhiteSpace( jwtOptions?.Secret ) )
{
    throw new InvalidOperationException( "JWT Secret is not configured! Проверьте секцию JwtOptions:Secret в appsettings." );
}

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
    .AddJwtBearer( options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            NameClaimType = "userId",
            IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( jwtOptions.Secret! ) )
        };
    } );
builder.Services.AddAuthorization( options =>
{
    foreach ( PermissionName permission in Enum.GetValues<PermissionName>() )
    {
        options.AddPolicy(
            permission.ToString(),
            policy => policy.RequireClaim( "Permission", permission.ToString() ) );
    }
} );

var app = builder.Build();

using ( var scope = app.Services.CreateScope() )
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BuddyBotDbContext>();
    dbContext.Database.Migrate();
}

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await DefaultAdminSeeder.Seed( app.Services );
}

app.UseCors( "AllowLocalhost" );
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
