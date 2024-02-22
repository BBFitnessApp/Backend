using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Utils;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }

        public IUserRepository UserRepository { get; }

        public ICalorieDataRepository CalorieDataRepository { get; }

        private ApplicationDbContext _dbContext;
        public UnitOfWork() : this(new ApplicationDbContext())
        { }

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
            ProductRepository = new ProductRepository(_dbContext);
            UserRepository = new UserRepository(_dbContext);
            CalorieDataRepository = new CalorieDataRepository(_dbContext);
        }

        public UnitOfWork(IConfiguration configuration) : this(new ApplicationDbContext(configuration))
        { }


        public async Task<int> SaveChangesAsync()
        {
            var entities = _dbContext!.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity)
                .ToArray();  // Geänderte Entities ermitteln

            // Allfällige Validierungen der geänderten Entities durchführen
            foreach (var entity in entities)
            {
                ValidateEntity(entity);
            }
            return await _dbContext.SaveChangesAsync();

        }

        private void ValidateEntity(object entity)
        {

        }

        public async Task DeleteDatabaseAsync() => await _dbContext!.Database.EnsureDeletedAsync();
        public async Task MigrateDatabaseAsync() => await _dbContext!.Database.MigrateAsync();
        public async Task CreateDatabaseAsync() => await _dbContext!.Database.EnsureCreatedAsync();

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await _dbContext.DisposeAsync();
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        /*public async Task FillDbAsync()
        {
            await DeleteDatabaseAsync();
            await MigrateDatabaseAsync();

            // Read CSV files
            var userData = await MyFile.ReadStringMatrixFromCsvAsync("User_Data.csv", true);
            var productData = await MyFile.ReadStringMatrixFromCsvAsync("Product_Data.csv", true);
            var calorieData = await MyFile.ReadStringMatrixFromCsvAsync("CalorieData.csv", true);

            // Process user data
            //0 Email,1 UserName,2 Password,3 Age,4 Weight,5 Gender,6 ZielSpezifikation,7 BMI,8 Height,9 Kalorienziel
            List<User> users = userData.Select(line =>
                new User()
                {
                    Email = line[0],
                    UserName = line[1],
                    Password = line[2],
                    Age = int.Parse(line[3]),
                    Weight = int.Parse(line[4]),
                    Gender = (Gender)Enum.Parse(typeof(Gender), line[5]),
                    ZielSpezifikation = line[6],
                    BMI = double.Parse(line[7]),
                    Height = int.Parse(line[8]),
                    Kalorienziel = int.Parse(line[9])
                }).ToList();

            // Process product data

            //0 Id,1 Barcode,2 Produktname,3 Kalorien,4 Fette,5 Proteine,6 Kohlenhydrate
            List<Product> products = productData.Select(line =>
                new Product()
                {
                    Id = Convert.ToInt32(line[0]) ,
                    Barcode = line[1],
                    Produktname = line[2],
                    Kalorien = double.Parse(line[3]),
                    Fette = double.Parse(line[4]),
                    Proteine = double.Parse(line[5]),
                    Kohlenhydrate = double.Parse(line[6])
                }).ToList();

            // Process calorie data
      
            //0 UserId,1 Datum,2 Kalorienaufnahme,3 Fette,4 Proteine,5 Kohlenhydrate
            List<CalorieData> calorieDataList = calorieData.Select(line =>
                new CalorieData()
                {
                    UserId = int.Parse(line[0]), // Assuming UserId is at index 0
                    Datum = DateTime.Parse(line[1]), // Assuming Datum is at index 1
                    Kalorienaufnahme = int.Parse(line[2]), // Assuming Kalorienaufnahme is at index 2
                    Fette = int.Parse(line[3]), // Assuming Fette is at index 3
                    Proteine = int.Parse(line[4]), // Assuming Proteine is at index 4
                    Kohlenhydrate = int.Parse(line[5]) // Assuming Kohlenhydrate is at index 5
                }).ToList();

            // Add entities to the database context
            _dbContext.Users.AddRange(users);
            _dbContext.Products.AddRange(products);
            _dbContext.CalorieData.AddRange(calorieDataList);

            // Save changes to the database
            await SaveChangesAsync();
        }*/

        public void SeedData()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                try
                {
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();

                    // User data
                    List<User> users = new List<User>
                {
                    new User
                    {
                        Email = "john@example.com",
                        UserName = "JohnDoe",
                        //Password = "password123",
                        Age = 30,
                        Weight = 75,
                        Gender = Gender.Male,
                        ZielSpezifikation = "Gewichtsverlust",
                        BMI = 25,
                        Height = 180,
                        Kalorienziel = 2000
                    },
                    new User
                    {
                         Email = "jane@example.com",
                        UserName = "JaneDoe",
                        //Password = "password456",
                        Age = 35,
                        Weight = 65,
                        Gender = Gender.Female,
                        ZielSpezifikation = "Gewichthaltung",
                        BMI = 22,
                        Height = 165,
                        Kalorienziel = 1800
                    },
                    new User
                    {

                        Email = "alice@example.com",
                        UserName = "AliceSmith",
                        //Password = "password789",
                        Age = 28,
                        Weight = 60,
                        Gender = Gender.Female,
                        ZielSpezifikation = "Muskelaufbau",
                        BMI = 23,
                        Height = 170,
                        Kalorienziel = 2200
                    },
                    new User
                    {
                        Email = "bob@example.com",
                        UserName = "BobJohnson",
                        //Password = "passwordabc",
                        Age = 40,
                        Weight = 85,
                        Gender = Gender.Male,
                        ZielSpezifikation = "Gewichtsverlust",
                        BMI = 27,
                        Height = 175,
                        Kalorienziel = 1900
                    },
                    new User
                    {

                        Email = "emma@example.com",
                        UserName = "EmmaTaylor",
                        //Password = "passwordxyz",
                        Age = 45,
                        Weight = 70,
                        Gender = Gender.Female,
                        ZielSpezifikation = "Gewichtsverlust",
                        BMI = 24,
                        Height = 160,
                        Kalorienziel = 2100
                    },
                    new User
                    {
                        Email = "will@example.com",
                        UserName = "WilliamBrown",
                        //Password = "password123456",
                        Age = 32,
                        Weight = 80,
                        Gender = Gender.Male,
                        ZielSpezifikation = "Muskelaufbau",
                        BMI = 26,
                        Height = 185,
                        Kalorienziel = 2400
                    },
                    new User
                    {
                         Email = "lisa@example.com",
                        UserName = "LisaJones",
                        //Password = "password7891011",
                        Age = 29,
                        Weight = 55,
                        Gender = Gender.Female,
                        ZielSpezifikation = "Gewichthaltung",
                        BMI = 21,
                        Height = 155,
                        Kalorienziel = 1700
                    },
                    new User
                    {   
                        Email = "mike@example.com",
                        UserName = "MikeWilliams",
                        //Password = "password123abc",
                        Age = 38,
                        Weight = 90,
                        Gender = Gender.Male,
                        ZielSpezifikation = "Gewichtsverlust",
                        BMI = 28,
                        Height = 190,
                        Kalorienziel = 2000
                    },
                    new User
                    {

                           
                        Email = "sarah@example.com",
                        UserName = "SarahWilson",
                        //Password = "passwordxyz789",
                        Age = 27,
                        Weight = 63,
                        Gender = Gender.Female,
                        ZielSpezifikation = "Muskelaufbau",
                        BMI = 22.5,
                        Height = 168,
                        Kalorienziel = 2300
                    },
                    new User
                    {
                          
                        Email = "david@example.com",
                        UserName = "DavidAnderson",
                        //Password = "password456abc",
                        Age = 42,
                        Weight = 78,
                        Gender = Gender.Male,
                        ZielSpezifikation = "Gewichthaltung",
                        BMI = 25,
                        Height = 178,
                        Kalorienziel = 1800
                    }
                };



                    // Product data
                    List<Product> products = new List<Product>
                {
                    new Product
                    {
                        Barcode = "2141243243",
                        Produktname = "Product2",
                        Kalorien = 250,
                        Fette = 10,
                        Proteine = 20,
                        Kohlenhydrate = 30
                    },
                    new Product
                    {
                        Barcode = "2141243244",
                        Produktname = "Product3",
                        Kalorien = 260,
                        Fette = 11,
                        Proteine = 21,
                        Kohlenhydrate = 31
                    },
                    new Product
                    {
                        Barcode = "2141243245",
                        Produktname = "Product4",
                        Kalorien = 270,
                        Fette = 12,
                        Proteine = 22,
                        Kohlenhydrate = 32
                    },
                    new Product
                    {
                        Barcode = "2141243246",
                        Produktname = "Product5",
                        Kalorien = 280,
                        Fette = 13,
                        Proteine = 23,
                        Kohlenhydrate = 33
                    },
                    new Product
                    {
                        Barcode = "2141243247",
                        Produktname = "Product6",
                        Kalorien = 290,
                        Fette = 14,
                        Proteine = 24,
                        Kohlenhydrate = 34
                    },
                    new Product
                    {
                        Barcode = "2141243248",
                        Produktname = "Product7",
                        Kalorien = 300,
                        Fette = 15,
                        Proteine = 25,
                        Kohlenhydrate = 35
                    },
                    new Product
                    {
                        Barcode = "2141243249",
                        Produktname = "Product8",
                        Kalorien = 310,
                        Fette = 16,
                        Proteine = 26,
                        Kohlenhydrate = 36
                    },
                    new Product
                    {
                        Barcode = "2141243250",
                        Produktname = "Product9",
                        Kalorien = 320,
                        Fette = 17,
                        Proteine = 27,
                        Kohlenhydrate = 37
                    },
                    new Product
                    {
                        Barcode = "2141243251",
                        Produktname = "Product10",
                        Kalorien = 330,
                        Fette = 18,
                        Proteine = 28,
                        Kohlenhydrate = 38
                    },
                    new Product
                    {
                        Barcode = "2141243252",
                        Produktname = "Product11",
                        Kalorien = 340,
                        Fette = 19,
                        Proteine = 29,
                        Kohlenhydrate = 39
                    }
                };

                    // Add products to the context
                    context.Products.AddRange(products);

                    List<CalorieData> calorieDataList = new List<CalorieData>
                {
                   new CalorieData
                {
                    UserId = users[0].Id, // Assuming the user ID is 1 for the first user
                    User = users[0],  // Assuming the user ID is 1 for the first user
                    Datum = DateTime.Now.Date,
                    Kalorienaufnahme = 1800,
                    Fette = 80,
                    Proteine = 160,
                    Kohlenhydrate = 240
                },
                new CalorieData
                {
                    UserId = users[1].Id, // Assuming the user ID is 1 for the first user
                    User = users[1], // Assuming the user ID is 2 for the second user
                    Datum = DateTime.Now.Date.AddDays(-1), // Set Datum to yesterday's date
                    Kalorienaufnahme = 2100,
                    Fette = 97,
                    Proteine = 162,
                    Kohlenhydrate = 227
                },
                new CalorieData
                {
                    UserId = users[2].Id, // Assuming the user ID is 1 for the first user
                    User = users[2], // Assuming the user ID is 3 for the third user
                    Datum = DateTime.Now.Date.AddDays(-2), // Set Datum to two days ago
                    Kalorienaufnahme = 1950,
                    Fette = 132,
                    Proteine = 242,
                    Kohlenhydrate = 352
                },

                    new CalorieData
                    {
                        UserId = users[4].Id,
                                            User = users[4], // Assuming the user ID is 10 for the tenth user
// Assuming the user ID is 5 for the fifth user
                        Datum = DateTime.Now.Date.AddDays(-4), // Set Datum to four days ago
                        Kalorienaufnahme = 1850,
                        Fette = 90,
                        Proteine = 180,
                        Kohlenhydrate = 270
                    },
                    new CalorieData
                    {
                        UserId = users[5].Id,
                                            User = users[5], // Assuming the user ID is 10 for the tenth user
// Assuming the user ID is 6 for the sixth user
                        Datum = DateTime.Now.Date.AddDays(-5), // Set Datum to five days ago
                        Kalorienaufnahme = 2350,
                        Fette = 160,
                        Proteine = 240,
                        Kohlenhydrate = 320
                    },
                    new CalorieData
                    {
                        UserId = users[6].Id,
                                            User = users[6], // Assuming the user ID is 10 for the tenth user
// Assuming the user ID is 7 for the seventh user
                        Datum = DateTime.Now.Date.AddDays(-6), // Set Datum to six days ago
                        Kalorienaufnahme = 2000,
                        Fette = 132,
                        Proteine = 242,
                        Kohlenhydrate = 352
                    },
                    new CalorieData
                    {
                        UserId = users[7].Id,
                                            User = users[7], // Assuming the user ID is 10 for the tenth user
// Assuming the user ID is 8 for the eighth user
                        Datum = DateTime.Now.Date.AddDays(-7), // Set Datum to seven days ago
                        Kalorienaufnahme = 2500,
                        Fette = 176,
                        Proteine = 256,
                        Kohlenhydrate = 336
                    },
                    new CalorieData
                    {
                        UserId = users[8].Id, // Assuming the user ID is 9 for the ninth user
                                            User = users[8], // Assuming the user ID is 10 for the tenth user

                        Datum = DateTime.Now.Date.AddDays(-8), // Set Datum to eight days ago
                        Kalorienaufnahme = 1900,
                        Fette = 126,
                        Proteine = 216,
                        Kohlenhydrate = 324
                    },
                    new CalorieData
                    {
                         UserId = users[9].Id, // Assuming the user ID is 1 for the first user
                    User = users[9], // Assuming the user ID is 10 for the tenth user
                        Datum = DateTime.Now.Date.AddDays(-9), // Set Datum to nine days ago
                        Kalorienaufnahme = 2100,
                        Fette = 124,
                        Proteine = 208,
                        Kohlenhydrate = 312
                    }
                    // Add 9 more CalorieData with similar data...
                };

                    context.Users.AddRange(users);
                    context.CalorieData.AddRange(calorieDataList);
                    context.Products.AddRange(products);

                    // Save changes to the database
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Console.WriteLine("Error occurred: " + ex.Message);
                    throw; // Rethrow the exception to indicate failure
                }
            }
        }
    }
}
