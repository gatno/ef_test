using System.IO;
using System.Reflection;
using System.Text;
using ef_test.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.Database;

namespace ef_test.Database
{
    public class DatabaseContext : DbContext
    {
        // If a new entity is added it need to be load "DatabaseCore.OnResourceStartHandler()"
        public DbSet<TestModel> TestModel { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ContextFactory.ConnectionString == null)
            {
                DatabaseCore.SetDatabaseConnection();
            }
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            optionsBuilder.UseLoggerFactory(DatabaseCore.GetLoggerFactory(LogLevel.Error)).UseMySql(ContextFactory.ConnectionString);
        }
    }
}
