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

        private readonly string OrderFileName = "Order.xml";

        private readonly string SnackFileName = "Snack.xml";

        private readonly string StoreHouseFileName = "StoreHouse.xml";

        public List<Food> Foods { get; set; }

        public List<Order> Orders { get; set; }

        public List<Snack> Snacks { get; set; }

        public List<StoreHouse> StoreHouses { get; set; }

        private FileDataListSingleton()
        {
            Foods = LoadFoods();
            Orders = LoadOrders();
            Snacks = LoadSnacks();
            StoreHouses = LoadStoreHouses();
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
            SaveOrders();
            SaveSnacks();
            SaveStoreHouses();
        }

        private List<Food> LoadFoods()
        {
            var list = new List<Food>();

            if (File.Exists(FoodFileName))
            {
                XDocument xDocument = XDocument.Load(FoodFileName);

                var xElements = xDocument.Root.Elements("Component").ToList();

                foreach (var elem in xElements)
                {
                    list.Add(new Food
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        FoodName = elem.Element("ComponentName").Value
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

                foreach (var elem in xElements)
                {
                    list.Add(new Order
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        SnackId = Convert.ToInt32(elem.Element("SnackId").Value),
                        Count = Convert.ToInt32(elem.Element("Count").Value),
                        Sum = Convert.ToDecimal(elem.Element("Sum").Value),
                        Status = (OrderStatus)Convert.ToInt32(elem.Element("Status").Value),
                        DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value),
                        DateImplement = (!string.IsNullOrEmpty(elem.Element("DateImplement").Value)) ? Convert.ToDateTime(elem.Element("DateImplement").Value) : (DateTime?)null
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

                foreach (var elem in xElements)
                {
                    var prodComp = new Dictionary<int, int>();
                    foreach (var component in elem.Element("SnackFoods").Elements("SnackFood").ToList())
                    {
                        prodComp.Add(Convert.ToInt32(component.Element("Key").Value),
                            Convert.ToInt32(component.Element("Value").Value));
                    }
                    list.Add(new Snack
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        SnackName = elem.Element("SnackName").Value,
                        Price = Convert.ToDecimal(elem.Element("Price").Value),
                        SnackFoods = prodComp
                    });
                }
            }
            return list;
        }

        private List<StoreHouse> LoadStoreHouses()
        {
            var list = new List<StoreHouse>();

            if (File.Exists(StoreHouseFileName))
            {
                XDocument xDocument = XDocument.Load(StoreHouseFileName);

                var xElements = xDocument.Root.Elements("StoreHouse").ToList();

                foreach (var elem in xElements)
                {
                    var storeHouseComponents = new Dictionary<int, int>();
                    foreach (var component in elem.Element("StoreHouseComponents").Elements("StoreHouseComponent").ToList())
                    {
                        storeHouseComponents.Add(Convert.ToInt32(component.Element("Key").Value),
                            Convert.ToInt32(component.Element("Value").Value));
                    }
                    list.Add(new StoreHouse
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        StoreHouseName = elem.Element("StoreHouseName").Value,
                        ResponsiblePersonFCS = elem.Element("ResponsiblePersonFCS").Value,
                        StoreHouseComponents = storeHouseComponents,
                        DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value)
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
                        new XElement("DateCreate", order.DateCreate),
                        new XElement("DateImplement", order.DateImplement)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(OrderFileName);
            }
        }

        private void SaveSnacks()
        {
            if (Snacks != null)
            {
                var xElement = new XElement("Snacks");

                foreach (var Snack in Snacks)
                {
                    var compElement = new XElement("SnackFoods");
                    foreach (var component in Snack.SnackFoods)
                    {
                        compElement.Add(new XElement("SnackFood",
                            new XElement("Key", component.Key),
                            new XElement("Value", component.Value)));
                    }
                    xElement.Add(new XElement("Snack",
                        new XAttribute("Id", Snack.Id),
                        new XElement("SnackName", Snack.SnackName),
                        new XElement("Price", Snack.Price),
                        compElement));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(SnackFileName);
            }
        }

        private void SaveStoreHouses()
        {
            if (StoreHouses != null)
            {
                var xElement = new XElement("StoreHouses");

                foreach (var storeHouse in StoreHouses)
                {
                    var compElement = new XElement("StoreHouseComponents");
                    foreach (var component in storeHouse.StoreHouseComponents)
                    {
                        compElement.Add(new XElement("StoreHouseComponent",
                            new XElement("Key", component.Key),
                            new XElement("Value", component.Value)));
                    }
                    xElement.Add(new XElement("StoreHouse",
                        new XAttribute("Id", storeHouse.Id),
                        new XElement("StoreHouseName", storeHouse.StoreHouseName),
                        new XElement("ResponsiblePersonFCS", storeHouse.ResponsiblePersonFCS),
                        new XElement("DateCreate", storeHouse.DateCreate),
                        compElement));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(StoreHouseFileName);
            }
        }
    }
}
