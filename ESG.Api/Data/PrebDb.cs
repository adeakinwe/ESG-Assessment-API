using ESG.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ESG.Api.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isSQL)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                Console.WriteLine("Starting database migration...");
                try
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                    SeedData(context, isSQL);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during database migration: {ex}");
                    throw;  // Preserve stack trace
                }
            }

        }

        public static void SeedData(AppDbContext context, bool isSQL)
        {
            if (isSQL)
            {
                Console.WriteLine("Running database migration...");
                bool migrationSuccess = false;
                int maxRetries = 5;
                int retryDelay = 5000; // 5 seconds

                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        context.Database.Migrate();
                        migrationSuccess = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Migration attempt {i + 1} failed: {ex.Message}");
                        Thread.Sleep(retryDelay); // Wait before retrying
                    }
                }

                if (!migrationSuccess)
                {
                    Console.WriteLine("Migration failed after multiple attempts. Exiting...");
                    return;
                }
            }
            else
            {
                if (!context.CUSTOMER.Any())
                {
                    Console.WriteLine("Seeding customer data...");
                    context.CUSTOMER.AddRange(
                        new CUSTOMER() { CUSTOMERCODE = "EA101", FIRSTNAME = "Martin", LASTNAME = "Levi", GENDER = "Male", SECTOR = 1, ADDRESS = "Lagos", CREATEDBY = 1, DATETIMECREATED = DateTime.Now },
                        new CUSTOMER() { CUSTOMERCODE = "EA102", FIRSTNAME = "Matthias", LASTNAME = "Joy", GENDER = "Female", SECTOR = 2, ADDRESS = "Kano", CREATEDBY = 1, DATETIMECREATED = DateTime.Now }
                    );

                    context.SaveChanges();
                    Console.WriteLine("Customer data seeded successfully.");
                }
                else
                {
                    Console.WriteLine("Customer data already exists. Skipping seeding.");
                }
            }
        }
    }
}