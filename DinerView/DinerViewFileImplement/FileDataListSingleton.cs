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
        private readonly string ClientFileName = "Client.xml";
        private readonly string ImplementerFileName = "Implementer.xml";
        private readonly string StoreHouseFileName = "StoreHouse.xml";

        public List<Food> Foods { get; set; }
        public List<Snack> Snacks { get; set; }
        public List<Order> Orders { get; set; }
        public List<Client> Clients { get; set; }
        public List<Implementer> Implementers { get; set; }
        public List<StoreHouse> StoreHouses { get; set; }

        private FileDataListSingleton()
        {
            Foods = LoadFoods();
            Snacks = LoadSnacks();
            Orders = LoadOrders();
            Clients = LoadClients();
            Implementers = LoadImplementers();
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
            SaveSnacks();
            SaveOrders();
            SaveClients();
            SaveImplementers();
            SaveStoreHouses();
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
        private List<Client> LoadClients()
        {
            var list = new List<Client>();
            if (File.Exists(ClientFileName))
            {
                XDocument xDocument = XDocument.Load(ClientFileName);
                var xElements = xDocument.Root.Elements("Client").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Client
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ClientFIO = elem.Element("ClientFIO").Value,
                        Email = elem.Element("Email").Value,
                        Password = elem.Element("Password").Value,
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
                        ClientId = Convert.ToInt32(order.Element("ClientId").Value),
                        SnackId = Convert.ToInt32(order.Element("SnackId").Value),
                        ImplementerId = Convert.ToInt32(order.Element("ImplementerId").Value),
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
        private List<Implementer> LoadImplementers()
        {
            var list = new List<Implementer>();
            if (File.Exists(ImplementerFileName))
            {
                XDocument xDocument = XDocument.Load(ImplementerFileName);
                var xElements = xDocument.Root.Elements("Implementer").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Implementer
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ImplementerFIO = elem.Element("ImplementerFIO").Value,
                        WorkingTime = Convert.ToInt32(elem.Element("WorkingTime").Value),
                        PauseTime = Convert.ToInt32(elem.Element("PauseTime").Value),
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
                    var storeHouseFoods = new Dictionary<int, int>();
                    foreach (var component in elem.Element("StoreHouseFoods").Elements("StoreHouseComponent").ToList())
                    {
                        storeHouseFoods.Add(Convert.ToInt32(component.Element("Key").Value),
                            Convert.ToInt32(component.Element("Value").Value));
                    }
                    list.Add(new StoreHouse
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        StoreHouseName = elem.Element("StoreHouseName").Value,
                        ResponsiblePersonFCS = elem.Element("ResponsiblePersonFCS").Value,
                        StoreHouseFoods = storeHouseFoods,
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
                        new XElement("ClientId", order.ClientId),
                        new XElement("SnackId", order.SnackId),
                        new XElement("ImplementerId", order.ImplementerId),
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
        private void SaveClients()
        {
            if (Clients != null)
            {
                var xElement = new XElement("Clients");
                foreach (var client in Clients)
                {
                    xElement.Add(new XElement("Client",
                    new XAttribute("Id", client.Id),
                    new XElement("ClientFIO", client.ClientFIO),
                    new XElement("Email", client.Email),
                    new XElement("Password", client.Password)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ClientFileName);
            }
        }
        private void SaveImplementers()
        {
            if (Implementers != null)
            {
                var xElement = new XElement("Implementers");
                foreach (var implementer in Implementers)
                {
                    xElement.Add(new XElement("Implementer",
                    new XAttribute("Id", implementer.Id),
                    new XElement("ImplementerFIO", implementer.ImplementerFIO),
                    new XElement("WorkingTime", implementer.WorkingTime),
                    new XElement("PauseTime", implementer.PauseTime)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ImplementerFileName);
            }
        }
        private void SaveStoreHouses()
        {
            if (StoreHouses != null)
            {
                var xElement = new XElement("StoreHouses");

                foreach (var storeHouse in StoreHouses)
                {
                    var compElement = new XElement("StoreHouseFoods");
                    foreach (var component in storeHouse.StoreHouseFoods)
                    {
                        compElement.Add(new XElement("StoreHouseFoods",
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
