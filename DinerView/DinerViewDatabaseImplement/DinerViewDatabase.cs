using Microsoft.EntityFrameworkCore;
using DinerViewDatabaseImplement.Models;

namespace DinerViewDatabaseImplement
{
    public class DinerViewDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DinerViewDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<Snack> Snacks { get; set; }
        public virtual DbSet<SnackFood> SnackFoods { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}
