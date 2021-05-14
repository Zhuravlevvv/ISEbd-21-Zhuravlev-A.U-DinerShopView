using Microsoft.AspNetCore.Mvc;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.BusinessLogics;
using DinerBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinerViewRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreHouseController : Controller
    {
        private readonly StoreHouseLogic _storeHouseLogic;

        private readonly FoodLogic _foodLogic;

        public StoreHouseController(StoreHouseLogic storeHouseLogic, FoodLogic foodLogic)
        {
            _storeHouseLogic = storeHouseLogic;
            _foodLogic = foodLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public List<StoreHouseViewModel> GetStoreHouses() => _storeHouseLogic.Read(null);

        [HttpPost]
        public void CreateStoreHouse(StoreHouseBindingModel model) => _storeHouseLogic.CreateOrUpdate(model);

        [HttpPost]
        public void UpdateStoreHouse(StoreHouseBindingModel model) => _storeHouseLogic.CreateOrUpdate(model);

        [HttpPost]
        public void DeleteStoreHouse(StoreHouseBindingModel model) => _storeHouseLogic.Delete(model);

        [HttpPost]
        public void Replenishment(StoreHouseReplenishmentBindingModel model) => _storeHouseLogic.Replenishment(model);

        [HttpGet]
        public List<FoodViewModel> GetFoods() => _foodLogic.Read(null);
    }
}
