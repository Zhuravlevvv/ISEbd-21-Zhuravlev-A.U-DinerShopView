using System;
using System.Collections.Generic;
using System.Linq;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Enums;
using System.Threading.Tasks;
using System.Threading;

namespace DinerBusinessLogic.BusinessLogics
{
    public class WorkModeling
    {
        private readonly IImplementerStorage _implementerStorage;

        private readonly IOrderStorage _orderStorage;

        private readonly OrderLogic _orderLogic;

        private readonly Random rnd;

        public WorkModeling(IImplementerStorage implementerStorage, IOrderStorage orderStorage, OrderLogic orderLogic)
        {
            _implementerStorage = implementerStorage;
            _orderStorage = orderStorage;
            _orderLogic = orderLogic;

            rnd = new Random(1000);
        }

        public void DoWork()
        {
            var implementers = _implementerStorage.GetFullList();

            var orders = _orderStorage.GetFilteredList(new OrderBindingModel { FreeOrders = true });

            foreach (var implementer in implementers)
            {
                WorkerWorkAsync(implementer, orders);
            }
        }

        private async void WorkerWorkAsync(ImplementerViewModel implementer,
        List<OrderViewModel> orders)
        {
            // ищем заказы, которые уже в работе (вдруг исполнителя прервали)
            var runOrders = await Task.Run(() => _orderStorage.GetFilteredList(new
            OrderBindingModel
            { ImplementerId = implementer.Id }));

            var needComponentOrders = await Task.Run(() => _orderStorage.GetFilteredList(new
            OrderBindingModel
            {
                NeedComponentOrders = true,
            }));

            foreach (var order in runOrders)
            {
                // делаем работу заново
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                _orderLogic.FinishOrder(new ChangeStatusBindingModel
                {
                    OrderId = order.Id,
                    ImplementerId = implementer.Id
                });
                // отдыхаем
                Thread.Sleep(implementer.PauseTime);
            }

            foreach (var order in needComponentOrders)
            {
                RunOrder(order, implementer);
            }
            await Task.Run(() =>
            {
                foreach (var order in orders)
                {
                    RunOrder(order, implementer);
                }
            });
        }

        private void RunOrder(OrderViewModel order, ImplementerViewModel implementer)
        {
            // пытаемся назначить заказ на исполнителя
            try
            {
                _orderLogic.TakeOrderInWork(new ChangeStatusBindingModel
                {
                    OrderId = order.Id,
                    ImplementerId = implementer.Id
                });
                // делаем работу
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) *
                order.Count);
                _orderLogic.FinishOrder(new ChangeStatusBindingModel
                {
                    OrderId = order.Id,
                    ImplementerId = implementer.Id
                });
                // отдыхаем
                Thread.Sleep(implementer.PauseTime);
            }
            catch (Exception) { }
        }
    }
}
