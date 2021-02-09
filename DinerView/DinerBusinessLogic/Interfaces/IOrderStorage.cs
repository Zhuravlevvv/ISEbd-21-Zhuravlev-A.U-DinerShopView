using System;
using System.Collections.Generic;
using System.Text;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.BindingModels;

namespace DinerBusinessLogic.Interfaces
{
    public interface IOrderStorage
    {
        List<OrderViewModel> GetFullList();
        List<OrderViewModel> GetFilteredList(OrderBindingModel model);
        OrderViewModel GetElement(OrderBindingModel model);
        void Insert(OrderBindingModel model);
        void Update(OrderBindingModel model);
        void Delete(OrderBindingModel model);
    }
}
