using System;
using DinerBusinessLogic.Enums;

namespace DinerBusinessLogic.BindingModels
{
    //заказ
    public class OrderBindingModel
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public int SnackId { get; set; }
        public int? ImplementerId { get; set; }
        public bool? FreeOrders { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
