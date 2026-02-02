using Application;
using Application.PasswordHasher;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Foundation.Database.Seeding;
public class DefaultAdminSeeder
{
    public static async Task Seed( IServiceProvider services )
    {
        using ( IServiceScope scope = services.CreateScope() )
        {
            IUserRepository userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            IRoleRepository roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
            IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            IPasswordHasher passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            Role? adminRole = await roleRepository.Get( RoleName.Admin );
            if ( adminRole is null )
            {
                throw new Exception( "Роль 'Admin' не найдена в БД." );
            }

            string adminPassword = "admin";

            List<SeedAdmin> adminsToSeed = new List<SeedAdmin>
            {
                new SeedAdmin("admin", adminPassword, "Admin", "Admin"),
            };

            foreach ( SeedAdmin admin in adminsToSeed )
            {
                User? exists = await userRepository.GetByLogin( admin.Login );
                if ( exists != null )
                {
                    continue;
                }

                string passwordHash = passwordHasher.GeneratePassword( admin.Password );
                User user = new( DateTime.UtcNow, admin.Login, passwordHash );
                user.Roles.Add( adminRole );

                UserContactInfo contactInfo = new( user.Id, admin.FirstName, admin.LastName );
                user.SetContactInfo( contactInfo );

                userRepository.Add( user );
            }

            await unitOfWork.CommitAsync();
        }
    }
    private record SeedAdmin( string Login, string Password, string FirstName, string LastName );
}
