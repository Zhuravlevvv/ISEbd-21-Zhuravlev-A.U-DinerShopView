using System;
using System.Collections.Generic;
using System.Linq;
using DinerViewFileImplement.Models;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.Enums;

namespace DinerViewFileImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly FileDataListSingleton sourse;
        public OrderStorage()
        {
            sourse = FileDataListSingleton.GetInstance();
        }
        public List<OrderViewModel> GetFullList()
        {
            return sourse.Orders.Select(CreateModel).ToList();
        }
        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.ClientId = (int)model.ClientId;
            order.ImplementerId = model.ImplementerId.Value;
            order.SnackId = model.SnackId;
            order.Count = model.Count;
            order.Sum = model.Sum;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;

            return order;
        }
        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                SnackId = order.SnackId,
                ImplementerId = order.ImplementerId,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                SnackName = sourse.Snacks.FirstOrDefault(rec => rec.Id == order.SnackId)?.SnackName,
                ClientFIO = sourse.Clients.FirstOrDefault(rec => rec.Id == order.ClientId)?.ClientFIO,
                ImplementerFIO = sourse.Implementers.FirstOrDefault(rec => rec.Id == order.ImplementerId)?.ImplementerFIO
            };
        }
       
        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return sourse.Orders
                     .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue &&
                     rec.DateCreate.Date == model.DateCreate.Date) ||
                     (model.DateFrom.HasValue && model.DateTo.HasValue &&
                     rec.DateCreate.Date >= model.DateFrom.Value.Date && rec.DateCreate.Date <=
                     model.DateTo.Value.Date) ||
                     (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                     (model.FreeOrders.HasValue && model.FreeOrders.Value && rec.Status == OrderStatus.Принят) ||
                     (model.ImplementerId.HasValue && rec.ImplementerId ==
                     model.ImplementerId && rec.Status == OrderStatus.Выполняется))
                     .Select(CreateModel).ToList();
        }
        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var order = sourse.Orders
                 .FirstOrDefault(rec => rec.SnackId == model.SnackId || rec.Id == model.Id);
            return order != null ? CreateModel(order) : null;
        }
        public void Insert(OrderBindingModel model)
        {
            int maxId = sourse.Orders.Count > 0 ? sourse.Orders.Max(recOder => recOder.Id) : 0;
            var order = new Order { Id = maxId + 1 };
            sourse.Orders.Add(CreateModel(model, order));
        }
        public void Update(OrderBindingModel model)
        {
            var order = sourse.Orders.FirstOrDefault(recOder => recOder.Id == model.Id);
            if (order == null)
            {
                throw new Exception("Заказ не найден");
            }
            CreateModel(model, order);
        }
        public void Delete(OrderBindingModel model)
        {
            var order = sourse.Orders.FirstOrDefault(recOrder => recOrder.Id == model.Id);

            if (order != null)
            {
                sourse.Orders.Remove(order);
            }
            else
            {
                throw new Exception("Заказ не найден");
            }
        }
    }
}
