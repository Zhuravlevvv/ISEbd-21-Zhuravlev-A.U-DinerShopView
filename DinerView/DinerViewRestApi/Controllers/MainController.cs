using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DinerBusinessLogic.BusinessLogics;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.BindingModels;

namespace DinerViewRestApi.Controllers
{    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly OrderLogic _order;
        private readonly SnackLogic _snack;
        private readonly OrderLogic _main;
        public MainController(OrderLogic order, SnackLogic snack, OrderLogic main)
        {
            _order = order;
            _snack = snack;
            _main = main;
        }
        [HttpGet]
        public List<SnackViewModel> GetSnackList() => _snack.Read(null)?.ToList();

        [HttpGet]
        public SnackViewModel GetSnack(int snackId) => _snack.Read(new SnackBindingModel { Id = snackId })?[0];

        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => _order.Read(new OrderBindingModel { ClientId = clientId });

        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) => _main.CreateOrder(model);
    }
}
