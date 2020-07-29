namespace FinancialPortal.Migrations
{
    using FinancialPortal.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Head"))
            {
                roleManager.Create(new IdentityRole { Name = "Head" });
            }
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }
            if (!context.Roles.Any(r => r.Name == "NewUser"))
            {
                roleManager.Create(new IdentityRole { Name = "NewUser" });
            }
            {
                roleManager.Create(new IdentityRole { Name = "DemoHead" });
            }

            //  to avoid creating duplicate seed data.
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];


            if (!context.Users.Any(u => u.Email == "arussell@coderfoundry.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "arussell@coderfoundry.com",
                    Email = "arussell@coderfoundry.com",
                    FirstName = "Drew",
                    LastName = "Russell",
                    AvatarPath = "/Avatars/download.jpg"
                };

                userManager.Create(user, "ABC&123");

                userManager.AddToRoles(user.Id, "Head");
            }
            if (!context.Users.Any(u => u.Email == "demohead@mailinator.com"))

            {
                var user = new ApplicationUser

                {
                    UserName = "demohead@mailinator.com",
                    Email = "demohead@mailinator.com",
                    FirstName = "Henrietta",
                    LastName = "Lacks",
                    AvatarPath = "/Avatars/download.jpg"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id, "DemoHead");

            }

            //context.Categories.AddOrUpdate(
            //    c => c.Name,
            //    new Category { Name = "Housing" },
            //     new Category { Name = "Entertainment" },
            //      new Category { Name = "Utilities" },
            //       new Category { Name = "Groceries" },
            //        new Category { Name = "Vehicles" },
            //         new Category { Name = "Health" }
            //    ); 

            context.BankAccountTypes.AddOrUpdate(
                b => b.Type,
                new BankAccountType { Type = "Checking" },
                new BankAccountType { Type = "Savings" },
                new BankAccountType { Type = "Joint" },
                 new BankAccountType { Type = "Other" }
                 );
            context.TransactionTypes.AddOrUpdate(
                t => t.Type,
                new TransactionType { Type = "Deposit", Description = "Any transaction that increases account balance" },
                new TransactionType { Type = "Withdrawal", Description = "Any transaction that decreases account balance" },
                new TransactionType { Type = "Transfer", Description = "Any transaction that transfers an amount between accounts" }
                );
        }
    }
}
