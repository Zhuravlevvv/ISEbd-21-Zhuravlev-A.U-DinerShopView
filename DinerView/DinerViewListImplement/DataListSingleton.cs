using System;
using System.Collections.Generic;
using System.Text;

namespace DinerViewListImplement.Models
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Food> Foods { get; set; }
        public List<Order> Orders { get; set; }
        public List<Snack> Snacks { get; set; }
        public List<Client> Clients { get; set; }
        private DataListSingleton()
        {
            Foods = new List<Food>();
            Orders = new List<Order>();
            Snacks = new List<Snack>();
        }
        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }
    }
}
