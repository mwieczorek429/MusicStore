using Microsoft.AspNetCore.Identity;
using MusicStore.Areas.Identity.Data;

namespace MusicStore.Extensions
{
	public static class RoleInitializerExtensions
	{
		public static async Task InitializeRolesAsync(this IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MusicStoreUser>>();

				if (!await roleManager.RoleExistsAsync("Administrator"))
				{
					await roleManager.CreateAsync(new IdentityRole("Administrator"));
				}
				var adminEmail = "admin@admin.com";
				var adminPassword = "ZAQ!2wsx";

				var adminUser = await userManager.FindByEmailAsync(adminEmail);

				if (adminUser == null) 
				{
					adminUser = new MusicStoreUser
					{
						UserName = adminEmail,
						Email = adminEmail,
						EmailConfirmed = true 
					};

					var result = await userManager.CreateAsync(adminUser, adminPassword);
					if (!result.Succeeded)
					{
						throw new Exception("Failed to create an administrator account.");
					}
				}
				if (!await userManager.IsInRoleAsync(adminUser, "Administrator"))
				{
					await userManager.AddToRoleAsync(adminUser, "Administrator");
				}
			}
		}
	}
}
