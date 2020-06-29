using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace DAL.Contexts
{
    public class AuctionDbContextSeed
    {
        private const string pass = "Pass123$";
        private const string adm_role = "Admins";
        private const string mod_role = "GlobalModerators";

        private const string adm_user_name = "admin";
        private const string mod_user_name = "globalmoderator";
        private const string fmod_user_name = "moderator"; 
        private const string def_user_name = "bob";
        private const string def_user2_name = "alice";

        private static string adm_user_id = "";
        private static string mod_user_id = "";
        private static string fmod_user_id = "";
        private static string def_user_id = "";
        private static string def_user2_id = "";

        private static UserManager<User> _userManager { get; set; }
        private static RoleManager<Role> _roleManager { get; set; }
        private static AuctionDbContext Context { get; set; }

        public static async Task SeedAsync(AuctionDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            Context = context;
            _userManager = userManager;
            _roleManager = roleManager;

            await UsersSeedAsync();
            await EntitiesSeedAsync();
        }

        private static async Task UsersSeedAsync()
        {
            await _roleManager.CreateAsync(new Role(adm_role));
            await _roleManager.CreateAsync(new Role(mod_role));

            var defaultUser = new User { UserName = def_user_name };
            await _userManager.CreateAsync(defaultUser, pass);
            defaultUser = await _userManager.FindByNameAsync(def_user_name);
            def_user_id = defaultUser.Id;

            var defaultUser2 = new User { UserName = def_user2_name };
            await _userManager.CreateAsync(defaultUser2, pass);
            defaultUser2 = await _userManager.FindByNameAsync(def_user2_name);
            def_user2_id = defaultUser2.Id;

            var fmodUser = new User { UserName = fmod_user_name };
            await _userManager.CreateAsync(fmodUser, pass);
            fmodUser = await _userManager.FindByNameAsync(fmod_user_name);
            fmod_user_id = fmodUser.Id;

            var adminUser = new User { UserName = adm_user_name };
            await _userManager.CreateAsync(adminUser, pass);
            adminUser = await _userManager.FindByNameAsync(adm_user_name);
            await _userManager.AddToRoleAsync(adminUser, adm_role);
            adm_user_id = adminUser.Id;

            var modUser = new User { UserName = mod_user_name };
            await _userManager.CreateAsync(modUser, pass);
            modUser = await _userManager.FindByNameAsync(mod_user_name);
            await _userManager.AddToRoleAsync(modUser, mod_role);
            mod_user_id = modUser.Id;
        }

        private static async Task EntitiesSeedAsync(int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                if (!Context.Auctions.Any())
                {
                    Context.Auctions.AddRange(
                        GetPreconfiguredauctions());

                    await Context.SaveChangesAsync();
                }
                if (!Context.Categories.Any())
                {
                    Context.Categories.AddRange(
                        GetPreconfiguredCategories());

                    await Context.SaveChangesAsync();
                }
                if (!Context.Lots.Any())
                {
                    Context.Lots.AddRange(
                        GetPreconfiguredLots());

                    await Context.SaveChangesAsync();
                }
                if (!Context.AuctionToModerators.Any())
                {
                    Context.AuctionToModerators.AddRange(
                        GetPreconfiguredAuctionToModerators());

                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    Console.WriteLine(ex.Message);
                    await EntitiesSeedAsync(retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<Auction> GetPreconfiguredauctions()
        {
            return new List<Auction>()
            {
                new Auction() {Name = "Example auction #1 name", Description = "Description of sample auction #1" },
                new Auction() {Name = "Example auction #2 name", Description = "Description of sample auction #2" },
                new Auction() {Name = "Example auction #3 name", Description = "Description of sample auction #3" },
            };
        }

        static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>()
            {
                new Category() {Title = "Sample Category Number One", AuctionId = 1, UserId = def_user_id},
                new Category() {Title = "Second Sample Category", AuctionId = 1, UserId = def_user_id},
                new Category() {Title = "Sample Category Number 3", AuctionId = 2, UserId = def_user_id},
                new Category() {Title = "Sample Category Number 4", AuctionId = 2, UserId = def_user2_id},
                new Category() {Title = "Sample Category Number 5", AuctionId = 3, UserId = def_user2_id},
                new Category() {Title = "Sample Category Number 6", AuctionId = 3, UserId = def_user2_id}
            };
        }

        static IEnumerable<Lot> GetPreconfiguredLots()
        {
            return new List<Lot>()
            {
                new Lot() {Description = "initial lot", CategoryId = 1, User = def_user_id, Title = "Iphone 6", BuyNowPrice = 100, Expiring = DateTime.Now.AddDays(1), Bid = 30, BidUser = "bob"},
                new Lot() {Description = "ij lot", CategoryId = 1, User = def_user_id, Title = "Iphone 6", BuyNowPrice = 74, Expiring = DateTime.Now.AddDays(1), Bid = 20, BidUser = "bob"},
                new Lot() {Description = "gy lot", CategoryId = 1, User = def_user_id, Title = "Iphone 6", BuyNowPrice = 69, Expiring = DateTime.Now.AddDays(1), Bid = 39, BidUser = "bob"},
                new Lot() {Description = "ft lot", CategoryId = 1, User = def_user_id, Title = "Iphone 6", BuyNowPrice = 24, Expiring = DateTime.Now.AddDays(1), Bid = 37, BidUser = "bob"},
                new Lot() {Description = "rd lot", CategoryId = 1, User = def_user_id, Title = "Iphone 6", BuyNowPrice = 88, Expiring = DateTime.Now.AddDays(1), Bid = 20, BidUser = "bob"},
            };
        }

        static IEnumerable<AuctionToModerator> GetPreconfiguredAuctionToModerators()
        {
            return new List<AuctionToModerator>()
            {
                new AuctionToModerator() {AuctionId = 2, UserId = fmod_user_id},
                new AuctionToModerator() {AuctionId = 3, UserId = fmod_user_id},
            };
        }
    }
}
