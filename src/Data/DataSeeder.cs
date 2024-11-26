using Bogus;
using UserApi.Models;

namespace UserApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            Console.WriteLine("Iniciando el seeding de datos...");

            if (!context.Users.Any())
            {
                Console.WriteLine("No se encontraron usuarios. Agregando datos iniciales...");

                // Crear un usuario administrativo inicial
                var adminId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
                var admin = new User
                {
                    Id = adminId,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("AdminPassword123"),
                    IsDeleted = false
                };

                context.Users.Add(admin);

                Console.WriteLine($"Usuario administrador agregado con ID fijo: {adminId}");

                // Configuración de Bogus para generar usuarios válidos
                var faker = new Faker<User>()
                    .RuleFor(u => u.Id, f => Guid.NewGuid()) // Solo para usuarios generados
                    .RuleFor(u => u.FirstName, f =>
                    {
                        var firstName = f.Name.FirstName();
                        return firstName.Length > 15 ? firstName.Substring(0, 15) : firstName;
                    })
                    .RuleFor(u => u.LastName, f =>
                    {
                        var lastName = f.Name.LastName();
                        return lastName.Length > 100 ? lastName.Substring(0, 100) : lastName;
                    })
                    .RuleFor(u => u.Email, f =>
                    {
                        var email = f.Internet.Email();
                        return email.Length > 100 ? email.Substring(0, 100) : email;
                    })
                    .RuleFor(u => u.Password, f => BCrypt.Net.BCrypt.HashPassword("password123"))
                    .RuleFor(u => u.IsDeleted, f => false);

                // Generar 100 usuarios únicos
                var users = faker.Generate(100).GroupBy(u => u.Email).Select(g => g.First()).ToList();
                context.Users.AddRange(users);

                Console.WriteLine($"{users.Count} usuarios generados y agregados.");

                context.SaveChanges();
                Console.WriteLine("Datos guardados en la base de datos.");
            }
            else
            {
                Console.WriteLine("Se encontraron usuarios existentes. No se agregó nada.");
            }
        }
    }
}
