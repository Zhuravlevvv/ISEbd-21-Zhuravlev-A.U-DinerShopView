using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using DinerBusinessLogic.Enums;
using DinerViewFileImplement.Models;
namespace DinerViewFileImplement
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;

        private readonly string FoodFileName = "Food.xml";
        private readonly string SnackFileName = "Snack.xml";
        private readonly string OrderFileName = "Order.xml";

        public List<Food> Foods { get; set; }

        public List<Snack> Snacks { get; set; }

        public List<Order> Orders { get; set; }

        private FileDataListSingleton()
        {
            Foods = LoadFoods();
            Snacks = LoadSnacks();
            Orders = LoadOrders();
        }

        public static FileDataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FileDataListSingleton();
            }

            return instance;
        }

        ~FileDataListSingleton()
        {
            SaveFoods();
            SaveSnacks();
            SaveOrders();
        }

        private List<Food> LoadFoods()
        {
            var list = new List<Food>();

            if (File.Exists(FoodFileName))
            {
                XDocument xDocument = XDocument.Load(FoodFileName);

                var xElements = xDocument.Root.Elements("Food").ToList();

                foreach (var food in xElements)
                {
                    list.Add(new Food
                    {
                        Id = Convert.ToInt32(food.Attribute("Id").Value),
                        FoodName = food.Element("FoodName").Value
                    });
                }
            }

            return list;
        }

        private List<Snack> LoadSnacks()
        {
            var list = new List<Snack>();

            if (File.Exists(SnackFileName))
            {
                XDocument xDocument = XDocument.Load(SnackFileName);

                var xElements = xDocument.Root.Elements("Snack").ToList();

                foreach (var snack in xElements)
                {
                    var snackFoods = new Dictionary<int, int>();

                    foreach (var food in snack.Element("SnackFoods").Elements("SnackFood").ToList())
                    {
                        snackFoods.Add(Convert.ToInt32(food.Element("Key").Value), Convert.ToInt32(food.Element("Value").Value));
                    }

                    list.Add(new Snack
                    {
                        Id = Convert.ToInt32(snack.Attribute("Id").Value),
                        SnackName = snack.Element("SnackName").Value,
                        Price = Convert.ToDecimal(snack.Element("Price").Value),
                        SnackFoods = snackFoods
                    });
                }
            }

            return list;
        }

        private List<Order> LoadOrders()
        {
            var list = new List<Order>();

            if (File.Exists(OrderFileName))
            {
                XDocument xDocument = XDocument.Load(OrderFileName);
                var xElements = xDocument.Root.Elements("Order").ToList();

                foreach (var order in xElements)
                {
                    OrderStatus status = 0;
                    switch(order.Element("Status").Value)
                    {
                        case "Принят":
                            status = OrderStatus.Принят;
                            break;
                        case "Выполняется":
                            status = OrderStatus.Выполняется;
                            break;
                        case "Готов":
                            status = OrderStatus.Готов;
                            break;
                        case "Оплачен":
                            status = OrderStatus.Оплачен;
                            break;
                    }
                    DateTime? date = null;
                    if (order.Element("DateImplement").Value != "")
                    {
                        date = Convert.ToDateTime(order.Element("DateImplement").Value);
                    }
                    list.Add(new Order
                    {
                        Id = Convert.ToInt32(order.Attribute("Id").Value),
                        SnackId = Convert.ToInt32(order.Element("SnackId").Value),
                        Count = Convert.ToInt32(order.Element("Count").Value),
                        Sum = Convert.ToDecimal(order.Element("Sum").Value),
                        Status = status,
                        DateCreate = Convert.ToDateTime(order.Element("DateCreate").Value),
                        DateImplement = date
                    });
                }
            }

            return list;
        }

        private void SaveFoods()
        {
            if (Foods != null)
            {
                var xElement = new XElement("Foods");

                foreach (var food in Foods)
                {
                    xElement.Add(new XElement("Food",
                        new XAttribute("Id", food.Id),
                        new XElement("FoodName", food.FoodName)));
                }

                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(FoodFileName);
            }
        }

        private void SaveSnacks()
        {
            if (Snacks != null)
            {
                var xElement = new XElement("Snacks");

                foreach (var snack in Snacks)
                {
                    var foodsElement = new XElement("SnackFoods");

                    foreach (var food in snack.SnackFoods)
                    {
                        foodsElement.Add(new XElement("SnackFood",
                            new XElement("Key", food.Key),
                            new XElement("Value", food.Value)));
                    }

                    xElement.Add(new XElement("Snack",
                        new XAttribute("Id", snack.Id),
                        new XElement("SnackName", snack.SnackName),
                        new XElement("Price", snack.Price),
                        foodsElement));
                }

                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(SnackFileName);
            }
        }

        private void SaveOrders()
        {
            if (Orders != null)
            {
                var xElement = new XElement("Orders");

                foreach (var order in Orders)
                {
                    xElement.Add(new XElement("Order",
                        new XAttribute("Id", order.Id),
                        new XElement("SnackId", order.SnackId),
                        new XElement("Count", order.Count),
                        new XElement("Sum", order.Sum),
                        new XElement("Status", (int)order.Status),
                        new XElement("DateCreate", order.DateCreate.ToString()),
                        new XElement("DateImplement", order.DateImplement.ToString())));
                }

                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(OrderFileName);
            }
        }
    }
}
