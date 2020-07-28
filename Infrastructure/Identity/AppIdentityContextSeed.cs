using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedIdentityAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>{
                        new AppUser{
                            UserName = "test@test.com",
                            Email = "test@test.com",
                            DisplayName = "Test",
                            AddressHistory = new AddressHistory{
                                DeliveryName = "Test",
                                City = "Padang",
                                Street = "Pondok TKP",
                                PostalCode = 22000,
                                PhoneNumber = 082222222222,
                            }
                        }, new AppUser{
                            UserName = "admin@admin.com",
                            Email = "admin@admin.com",
                            DisplayName = "Admin"
                        }
                    };
                var roles = new List<AppRole>{
                        new AppRole{Name = "Admin"},
                        new AppRole{Name = "Member"}
                    };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "Member");
                    if (user.Email == "admin@admin.com") await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}