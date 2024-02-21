using Core.Contracts;
using Core.Entities;
using Persistence;


Console.WriteLine("Import der Datensätze in die Datenbank");
Console.WriteLine("Daten werden eingelesen!\n");

using IUnitOfWork unitOfWork = new UnitOfWork();

Console.WriteLine("Einlesen");
unitOfWork.SeedData();

int cnt = await unitOfWork.UserRepository.GetCountAsync();
Console.WriteLine("{0} User in Datenbank importiert!", cnt);

int cnt1 = await unitOfWork.CalorieDataRepository.GetCountAsync();
Console.WriteLine("{0} mal CalorieData in Datenbank importiert!", cnt1);

int cnt3 = await unitOfWork.ProductRepository.GetCountAsync();
Console.WriteLine("{0} Products in Datenbank importiert!", cnt3);

Console.WriteLine("<Taste drücken>");
Console.ReadKey();

