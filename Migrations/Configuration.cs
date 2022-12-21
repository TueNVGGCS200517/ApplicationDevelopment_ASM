namespace GreenwichBooks.Migrations
{

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                // If the database is empty, populate sample data in it

                CreateUser(context, "admin@gmail.com", "123", "System Administrator");
                CreateUser(context, "pesho@gmail.com", "123", "Peter Ivanov");
                CreateUser(context, "merry@gmail.com", "123", "Maria Petrova");
                CreateUser(context, "geshu@gmail.com", "123", "George Petrov");

                CreateRole(context, "Administrators");
                AddUserToRole(context, "admin@gmail.com", "Administrators");

                CreatePost(context,
                     title: "Check Your English Vocabulary for Computers and Information Technology",
                     body: @"This book is designed to help non-native English speakers understand and grasp the IT terminology and core knowledge of this industry. Therefore, the knowledge is presented in a way that is as easy to understand and as user-friendly as possible.
                     Check Your English Vocabulary for Computers and Information Technology offers classroom activities and activities suitable for self-study. The content of the book uses a variety of interesting and engaging activities such as crossword puzzles, speaking activities, and more. From there, learning will become less stressful and no longer boring.",
                     price: 220000,
                     date: new DateTime(2014, 06, 22, 21, 36, 34),
                     authorUsername: "pesho@gmail.com"
                );

                CreatePost(context,
                     title: "Dac Nhan Tam",
                     body: @"Dac Nhan Tam is a famous work of Dale Carnegie. Dac means gain, Nhan means a person, Tam means heart, Dac Nhan Tam can simply be understood as the art of winning people's hearts. People must understand and be honest with themselves, care and understand the people around them, so that they can realize and reveal the hidden potentials in them, helping them to develop more and more.",
                     price: 109000,
                     date: new DateTime(2018, 08, 17, 18, 50, 22),
                     authorUsername: "merry@gmail.com"
                );

                CreatePost(context,
                     title: "Projects in Computing and Information Systems: A Student'S Guide, 3rd edition",
                     body: @"This book is the essential guide for any student undertaking a computing/IS project, and will give you everything you need to achieve outstanding results.
                     Undertaking a project is a key component of nearly all computing/information systems degree programmes at both undergraduate and postgraduate levels. Projects in Computing and Information Systems covers the four key aspects of project work (planning, conducting, presenting and taking the project further) in chronological fashion, and provides the reader with the skills to excel.",
                     price: 189000,
                     date: new DateTime(2019, 11, 12, 14, 45, 15),
                     authorUsername: "merry@gmail.com"
                );

                CreatePost(context,
                     title: "The Godfather",
                     body: @"The Godfather is a classic good book by Mario Puzo. The content of the book revolves around an Italian mafia family with the main character being the boss Vito Corleone. The highlight of The Godfather is that although the content is about the mafia, the author does not discuss much about drugs or gambling, but focuses on the events of that family and how they improvise to challenges. The person who creates great karma, helps the family overcome difficulties and plans everything, that person is the Godfather.",
                     price: 299000,
                     date: new DateTime(2020, 02, 15, 18, 30, 12),
                     authorUsername: "geshu@gmail.com"
                );

                CreatePost(context,
                     title: "The Thorn Birds",
                     body: @"“The Thorn Birds” is a hit and miss novel written by Australian author Colleen McCulough. Throughout the work, it tells the love story of the beautiful Meggie and the Reverend Ralph. But because of the difference in status and status, the parish has had to face and run away from this feeling for the rest of his life.",
                     price: 259000,
                     date: new DateTime(2016, 07, 14, 17, 20, 22),
                     authorUsername: "merry@gmail.com"
                );

                CreatePost(context,
                     title: "Think and grow rich",
                     body: @"Think and grow rich gives you a philosophy of success, and provides you with a detailed plan to achieve it. Not only theory, this work is cited from real cases, such as Edison - a brilliant inventor, Henry Ford - tycoon of the car industry,... Napoleon Hill, author author of How To Think For Success, spent 30 years interviewing more than 500 successful people in various fields, from which to summarize the philosophies and write this book.",
                     price: 109000,
                     date: new DateTime(2015, 04, 09, 15, 37, 07),
                     authorUsername: "geshu@gmail.com"
                );

                context.SaveChanges();
            }
        }

        private void CreateUser(ApplicationDbContext context,
            string email, string password, string fullName)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            var userCreateResult = userManager.Create(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        private void CreateRole(ApplicationDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(roleName));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
        }

        private void AddUserToRole(ApplicationDbContext context, string userName, string roleName)
        {
            var user = context.Users.First(u => u.UserName == userName);
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var addAdminRoleResult = userManager.AddToRole(user.Id, roleName);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void CreatePost(ApplicationDbContext context,
            string title, string body, int price, DateTime date, string authorUsername)
        {
            var post = new Post();
            post.Title = title;
            post.Body = body;
            post.Price = price;
            post.Date = date;
            post.Author = context.Users.Where(u => u.UserName == authorUsername).FirstOrDefault();
            context.Posts.Add(post);
        }
    }
}
