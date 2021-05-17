using System;
using System.Collections.Generic;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Enums;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;

namespace DinerBusinessLogic.BusinessLogics
{
    public class OrderLogic
    {
        private readonly IOrderStorage _orderStorage;

        private readonly object locker = new object();

        private readonly ISnackStorage _snackStorage;

        private readonly IStoreHouseStorage _storeHouseStorage;

        public OrderLogic(IOrderStorage orderStorage, ISnackStorage snackStorage, IStoreHouseStorage storeHouseStorage)
        {
            _orderStorage = orderStorage;
            _snackStorage = snackStorage;
            _storeHouseStorage = storeHouseStorage;
        }

        public List<OrderViewModel> Read(OrderBindingModel model)
        {
            if (model == null)
            {
                return _orderStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<OrderViewModel> { _orderStorage.GetElement(model) };
            }
            return _orderStorage.GetFilteredList(model);
        }

        public void CreateOrder(CreateOrderBindingModel model)
        {
            _orderStorage.Insert(new OrderBindingModel
            {
                SnackId = model.SnackId,
                ClientId = model.ClientId,
                Count = model.Count,
                Sum = model.Sum,
                DateCreate = DateTime.Now,
                Status = OrderStatus.Принят
            });
        }

        public void TakeOrderInWork(ChangeStatusBindingModel model)
        {
            lock (locker)
            {
                var order = _orderStorage.GetElement(new OrderBindingModel
                {
                    Id = model.OrderId
                });
                if (order == null)
                {
                    throw new Exception("Заказ не найден");
                }
                if (order.Status != OrderStatus.Принят && order.Status != OrderStatus.ТребуютсяMатериалы)
                {
                    throw new Exception("Заказ ещё не принят");
                }
                if (order.ImplementerId.HasValue)
                {
                    throw new Exception("У заказа уже есть исполнитель");
                }

                var foods = _snackStorage.GetElement(new SnackBindingModel { Id = order.SnackId }).SnackFoods;

                if (_storeHouseStorage.CheckAndTake(order.Count, foods))
                {
                    order.DateImplement = DateTime.Now;
                    order.Status = OrderStatus.Выполняется;
                    order.ImplementerId = model.ImplementerId;
                }
                else
                {
                    order.Status = OrderStatus.ТребуютсяMатериалы;
                }

                _orderStorage.Update(new OrderBindingModel
                {
                    Id = order.Id,
                    ClientId = order.ClientId,
                    SnackId = order.SnackId,
                    Count = order.Count,
                    Sum = order.Sum,
                    DateCreate = order.DateCreate,
                    DateImplement = order.DateImplement,
                    Status = order.Status,
                    ImplementerId = order.ImplementerId
                });
            }
        }

        public void FinishOrder(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel
            {
                Id = model.OrderId
            });
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != OrderStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                SnackId = order.SnackId,
                ClientId = order.ClientId,
                ImplementerId = order.ImplementerId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Готов
            });
        }

        public void PayOrder(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel
            {
                Id = model.OrderId
            });
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != OrderStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                SnackId = order.SnackId,
                ClientId = order.ClientId,
                ImplementerId = order.ImplementerId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Оплачен
            });
        }
    }
}
