﻿using Microsoft.EntityFrameworkCore;
using DinerViewDatabaseImplement.Models;

namespace DinerViewDatabaseImplement
{
    public class DinerViewDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DinerViewDatabaseComplicated;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Food> Foods { set; get; }
        public virtual DbSet<Snack> Snacks { set; get; }
        public virtual DbSet<SnackFood> SnackFoods { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<Client> Clients { set; get; }
        public virtual DbSet<Implementer> Implementers { set; get; }
        public virtual DbSet<MessageInfo> MessageInfoes { set; get; }
        public virtual DbSet<StoreHouse> StoreHouses { get; set; }
        public virtual DbSet<StoreHouseFood> StoreHouseFoods { set; get; }
    }
}
