using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

// Определение контекста данных
public class RealEstateContext : DbContext
{
    public DbSet<Type> Types { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<BuildingMaterial> BuildingMaterials { get; set; }
    public DbSet<RealEstateObject> RealEstateObjects { get; set; }
    public DbSet<EvaluationCriterion> EvaluationCriteria { get; set; }
    public DbSet<Evaluation> Evaluations { get; set; }
    public DbSet<Realtor> Realtors { get; set; }
    public DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost; Database=corp2; Username=postgres; Password=postgres");
    }
}



class Program
{
    static void Main(string[] args)
    {
        using (var context = new RealEstateContext())
        {
            // Очищаем базу данных перед заполнением
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Пример заполнения базы данных тестовыми данными
            var types = new List<Type>
            {
                new Type { TypeName = "Квартира" },
                new Type { TypeName = "Дом" },
                // Добавьте другие типы...
            };
            context.Types.AddRange(types);

            var districts = new List<District>
            {
                new District { DistrictName = "Кировский" },
                new District { DistrictName = "Ленинский" },
                // Добавьте другие районы...
            };
            context.Districts.AddRange(districts);

            var materials = new List<BuildingMaterial>
            {
                new BuildingMaterial { MaterialName = "Кирпич" },
                new BuildingMaterial { MaterialName = "Панель" },
                // Добавьте другие материалы...
            };
            context.BuildingMaterials.AddRange(materials);

            var realEstates = new List<RealEstateObject>
            {
                new RealEstateObject {
                    DistrictId = 1,
                    Address = "ул. Ленина, д. 10",
                    Floor = 3,
                    RoomCount = 2,
                    TypeId = 1,
                    Status = 1,
                    Price = 1500000,
                    Description = "Прекрасная квартира в центре города",
                    MaterialId = 1,
                    Area = 80,
                    AnnouncementDate = DateTime.UtcNow.AddDays(-30)
                },
                new RealEstateObject {
                    DistrictId = 2,
                    Address = "ул. Пушкина, д. 5",
                    Floor = 5,
                    RoomCount = 3,
                    TypeId = 1,
                    Status = 0,
                    Price = 2000000,
                    Description = "Просторная квартира с видом на парк",
                    MaterialId = 2,
                    Area = 100,
                    AnnouncementDate = DateTime.UtcNow.AddDays(-20)
                },
                // Добавьте другие объекты недвижимости...
            };
            context.RealEstateObjects.AddRange(realEstates);

            var realtors = new List<Realtor>
            {
                new Realtor {
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    ContactPhone = "123-456-789"
                },
                new Realtor {
                    LastName = "Петров",
                    FirstName = "Петр",
                    MiddleName = "Петрович",
                    ContactPhone = "987-654-321"
                },
                // Добавьте других риэлторов...
            };
            context.Realtors.AddRange(realtors);

            var evaluations = new List<Evaluation>
            {
                new Evaluation {
                    ObjectId = 1,
                    EvaluationDate = DateTime.UtcNow.AddDays(-5),
                    CriterionId = 1,
                    Score = 95 // Пример оценки
                },
                // Добавьте другие оценки...
            };
            context.Evaluations.AddRange(evaluations);

            var criteria = new List<EvaluationCriterion>
            {
                new EvaluationCriterion { CriterionName = "Безопасность" },
                new EvaluationCriterion { CriterionName = "Удобство транспорта" },
                // Добавьте другие критерии оценки...
            };
            context.EvaluationCriteria.AddRange(criteria);

            var sales = new List<Sale>
            {
                new Sale {
                    ObjectId = 1,
                    SaleDate = DateTime.UtcNow.AddDays(-10),
                    RealtorId = 1,
                    Price = 1450000 // Пример стоимости продажи
                },
                // Добавьте другие продажи...
            };
            context.Sales.AddRange(sales);

            context.SaveChanges();
        }

        // После заполнения базы данных тестовыми данными, вы можете вызвать методы выполнения LINQ-запросов

        // Пример вызова метода для выполнения запроса 2.1
        var program = new Program();
        program.GetRealEstateByPriceAndDistrict(1000000, 2000000, "Кировский");
    }

    // LINQ-запрос для задания 2.1
    public void GetRealEstateByPriceAndDistrict(double minPrice, double maxPrice, string districtName)
    {
        using (var context = new RealEstateContext())
        {
            var realEstates = from re in context.RealEstateObjects
                              join d in context.Districts on re.DistrictId equals d.DistrictId
                              where re.Price >= minPrice && re.Price <= maxPrice && d.DistrictName == districtName
                              orderby re.Price descending
                              select new
                              {
                                  re.Address,
                                  re.Area,
                                  re.Floor
                              };

            foreach (var realEstate in realEstates)
            {
                Console.WriteLine($"Адрес: {realEstate.Address}, Площадь: {realEstate.Area}, Этаж: {realEstate.Floor}");
            }
        }
    }
}
