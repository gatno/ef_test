using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ef_test.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace server.Database
{
    public class DatabaseCore
    {

        private Stopwatch _lastUpdate;

        public delegate void DatabaseInitialized();

        public void OnResourceStartHandler(DatabaseInitialized? onDatabaseInitialized)
        {

            if (!SetDatabaseConnection())
                return;

            var stopWatch = Stopwatch.StartNew();
            
            ContextFactory.Instance.TestModel.Load();
  

            stopWatch.Stop();
            

            _lastUpdate = Stopwatch.StartNew();
            onDatabaseInitialized?.Invoke();
        }

        public static bool SetDatabaseConnection()
        {

            var connectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Server = "127.0.0.1",
                UserID = "root",
                Password = "",
                Database = "test",
                Port = 3306,
                TreatTinyAsBoolean = false,
            };     

            return ContextFactory.Instance != null;
        }
        public async Task OnUpdateHandler()
        {
            if (ContextFactory.Instance == null)
                return;

            if (_lastUpdate.Elapsed.Seconds < 20) return; // .TotalMinutes >= 5

            _lastUpdate.Stop();
            _lastUpdate.Reset();

            await SaveChangeToDatabaseAsync();
            _lastUpdate.Start();
        }

        public static async Task SaveChangeToDatabaseAsync()
        {
            var results = await ContextFactory.Instance.SaveChangesAsync();
            if (results > 0)
                Console.Write($"{results} changes saved to the database...");
        }

        public static void SaveChangeToDatabase()
        {
            var results = ContextFactory.Instance.SaveChanges();
            Console.Write($"{results} changes saved to the database.");
        }

        public static ILoggerFactory GetLoggerFactory(LogLevel logLevel)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                builder.AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name,
                        logLevel));
            return serviceCollection.BuildServiceProvider()
                .GetService<ILoggerFactory>();
        }

    }
}
