using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
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
            _dbContext=context;
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


        /*
        public async Task FillDbAsync()
        {
            await DeleteDatabaseAsync();
            await MigrateDatabaseAsync();

            var csvLines = await MyFile.ReadStringMatrixFromCsvAsync("usages.csv", true);

            List<User> devices = csvLines.GroupBy(line =>
                   new
                   {
                       SerialNr = line[0],
                       DeviceName = line[1],
                       DeviceType = line[2]
                   }).Select(grp =>
                   new Device()
                   {
                       SerialNumber = grp.Key.SerialNr,
                       Name = grp.Key.DeviceName,
                       DeviceType = grp.Key.DeviceType
                   }).ToList();


            List<Person> people = csvLines.GroupBy(line =>
                  new
                  {
                      LastName = line[3],
                      FirstName = line[4],
                      EMail = line[5]
                  }).Select(grp =>
                  new Person()
                  {
                      FirstName = grp.Key.FirstName,
                      LastName = grp.Key.LastName,
                      MailAddress = grp.Key.EMail
                  }).ToList();

            List<Usage> usages = csvLines.Select(line =>
                new Usage()
                {
                    Device = devices.Single(d => d.SerialNumber == line[0]),
                    Person = people.Single(p => p.FirstName == line[4] && p.LastName == line[3]),
                    From = Convert.ToDateTime(line[6]),
                    To = (line[7] == "") ? default(DateTime?) : Convert.ToDateTime(line[7])  //statt default(DateTime?) auch (DateTime?) null möglich

                }).ToList();

            _dbContext.Usages.AddRange(usages);


            await SaveChangesAsync();
        }*/

    }
}
