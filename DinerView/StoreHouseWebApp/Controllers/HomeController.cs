using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.ViewModels;
using StoreHouseWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StoreHouseWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            if (!Program.IsAuthorized)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<StoreHouseViewModel>>($"api/storehouse/getstorehouses"));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public void Enter(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                bool check = password == configuration["Password"];

                if (!check)
                {
                    throw new Exception("Попробуйте другой пароль");
                }

                Program.IsAuthorized = check;
                Response.Redirect("Index");
                return;
            }

            throw new Exception("Введите пароль");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public void Create(string name, string FIO)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FIO))
            {
                return;
            }
            APIClient.PostRequest("api/storehouse/createstorehouse", new StoreHouseBindingModel
            {
                StoreHouseName = name,
                ResponsiblePersonFCS = FIO,
                StoreHouseFoods = new Dictionary<int, (string, int)>()
            });
            Response.Redirect("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Склад не найден");
            }

            var storeHouse = APIClient.GetRequest<List<StoreHouseViewModel>>(
                $"api/storehouse/getstorehouses").FirstOrDefault(rec => rec.Id == id);
            if (storeHouse == null)
            {
                throw new Exception("Склад не найден");
            }

            return View(storeHouse);
        }

        [HttpPost]
        public IActionResult Edit(int id, string name, string FIO)
        {
            var storeHouse = APIClient.GetRequest<List<StoreHouseViewModel>>(
                $"api/storehouse/getstorehouses").FirstOrDefault(rec => rec.Id == id);


            APIClient.PostRequest("api/storehouse/updatestorehouse", new StoreHouseBindingModel
            {
                Id = id,
                StoreHouseName = name,
                ResponsiblePersonFCS = FIO,
                StoreHouseFoods = storeHouse.StoreHouseFoods
            });
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Склад не найден");
            }

            var storeHouse = APIClient.GetRequest<List<StoreHouseViewModel>>(
                 $"api/storehouse/getstorehouses").FirstOrDefault(rec => rec.Id == id);
            if (storeHouse == null)
            {
                throw new Exception("Склад не найден");
            }

            return View(storeHouse);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            APIClient.PostRequest("api/storehouse/deletestorehouse", new StoreHouseBindingModel { Id = id });
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult Replenishment()
        {
            if (!Program.IsAuthorized)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.StoreHouses = APIClient.GetRequest<List<StoreHouseViewModel>>("api/storehouse/getstorehouses");
            ViewBag.Components = APIClient.GetRequest<List<FoodViewModel>>($"api/storehouse/getfoods");

            return View();
        }

        [HttpPost]
        public IActionResult Replenishment(int storeHouseId, int foodId, int count)
        {
            if (storeHouseId == 0 || foodId == 0 || count <= 0)
            {
                throw new Exception("Проверьте данные");
            }

            var storeHouse = APIClient.GetRequest<List<StoreHouseViewModel>>(
                 $"api/storehouse/getstorehouses").FirstOrDefault(rec => rec.Id == storeHouseId);

            if (storeHouse == null)
            {
                throw new Exception("Склад не найден");
            }

            var component = APIClient.GetRequest<List<StoreHouseViewModel>>(
                "api/storehouse/getfoods").FirstOrDefault(rec => rec.Id == foodId);

            if (component == null)
            {
                throw new Exception("Компонент не найден");
            }

            APIClient.PostRequest("api/storehouse/replenishment", new StoreHouseReplenishmentBindingModel
            {
                StoreHouseId = storeHouseId,
                FoodId = foodId,
                Count = count
            });
            return Redirect("~/Home/Index");
        }
    }
}
