using Core.Contracts;
using Core.Entities;
using Persistence;


Console.WriteLine("Import der Datensätze in die Datenbank");
Console.WriteLine("Daten werden eingelesen!\n");

using IUnitOfWork unitOfWork = new UnitOfWork();

Console.WriteLine("Devices, People und Usages werden eingelesen");
await unitOfWork.FillDbAsync();

int cnt = await unitOfWork.UsageRepository.GetCountAsync();
Console.WriteLine("{0} Usages in Datenbank importiert!", cnt);
Console.WriteLine("<Taste drücken>");
Console.ReadKey();

