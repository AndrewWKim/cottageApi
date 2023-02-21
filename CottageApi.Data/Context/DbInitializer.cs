using System.Collections.Generic;
using System.Linq;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace CottageApi.Data.Context
{
	public static class DbInitializer
	{
		public static void Initialize(CottageDbContext context)
		{
			context.Database.Migrate();

			Seed(context);
		}

		public static void Seed(CottageDbContext context)
		{
			SeedUsers(context);
			SeedResidentTypes(context);
			SeedNewSideSettings(context);
		}

		private static void SeedUsers(CottageDbContext context)
		{
			if (context.Users.Any())
			{
				return;
			}

			var users = new List<User>()
			{
				new User
				{
					Name = "admin",
					Password = "37d86a04fca1d04e24a486be163df912",
					Role = UserRole.Admin
				},
				new User
				{
					Name = "security",
					Password = "e240df57530c2081072dc6f42d2175fa",
					Role = UserRole.Security
				}
			};

			context.Users.AddRange(users);
			context.SaveChanges();
		}

		private static void SeedNewSideSettings(CottageDbContext context)
        {
			if (context.NewSideSettings.Any())
            {
				return;
            }

			var settings = new List<NewSideSettings>()
			{
				new NewSideSettings
				{
					SecurityPhoneNumber = "0990950125",
					CottageRulesHTML = "<html><div>Правила проживания в коттеджном городке</div></html>",
					TelegramChannelForSecurity = "-1001736065087"
				}
			};

			context.NewSideSettings.AddRange(settings);
			context.SaveChanges();
        }

		private static void SeedResidentTypes(CottageDbContext context)
		{
			if (context.ResidentTypes.Any())
			{
				return;
			}

			var residentTypes = new List<ResidentType>()
			{
				new ResidentType
				{
					Type = "Персонал"
				},
				new ResidentType
				{
					Type = "Член Семьи"
				}
			};

			context.ResidentTypes.AddRange(residentTypes);

			context.SaveChanges();
		}
	}
}