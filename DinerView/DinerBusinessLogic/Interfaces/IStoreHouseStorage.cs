using System;
using System.Collections.Generic;
using System.Text;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.ViewModels;

namespace DinerBusinessLogic.Interfaces
{
    public interface IStoreHouseStorage
    {
        List<StoreHouseViewModel> GetFullList();
        List<StoreHouseViewModel> GetFilteredList(StoreHouseBindingModel model);
        StoreHouseViewModel GetElement(StoreHouseBindingModel model);
        void Insert(StoreHouseBindingModel model);
        void Update(StoreHouseBindingModel model);
        void Delete(StoreHouseBindingModel model);
        bool CheckAndTake(int count, Dictionary<int, (string, int)> foods);
    }
}
