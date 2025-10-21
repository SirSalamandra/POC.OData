//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace POC.OData.Infrastructure.Data
//{
//    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//    {
//        public AppDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

//            // This is the connection string for your local SQLite database file.
//            // The database file (ODataPoc.db) will be created in your project's root directory.
//            optionsBuilder.UseSqlite("Data Source=database.db");

//            return new AppDbContext(optionsBuilder.Options);
//        }
//    }
//}
