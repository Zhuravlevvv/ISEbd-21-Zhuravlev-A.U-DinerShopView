using System;
using System.Collections.Generic;
using System.Text;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;

namespace DinerBusinessLogic.BusinessLogics
{
    public class SnackLogic
    {
        private readonly ISnackStorage _snackStorage;
        public SnackLogic(ISnackStorage snackStorage)
        {
            _snackStorage = snackStorage;
        }

        public List<SnackViewModel> Read(SnackBindingModel model)
        {
            if (model == null)
            {
                return _snackStorage.GetFullList();
            }

            if (model.Id.HasValue)
            {
                return new List<SnackViewModel> { _snackStorage.GetElement(model) };
            }
            return _snackStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(SnackBindingModel model)
        {
            var element = _snackStorage.GetElement(new SnackBindingModel { 
            SnackName = model.SnackName });

            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Закуска с таким названием уже есть!");
            }

            if (model.Id.HasValue)
            {
                _snackStorage.Update(model);
            }
            else
            {
                _snackStorage.Insert(model);
            }
        }

        public void Delete(SnackBindingModel model)
        {
            var element = _snackStorage.GetElement(new SnackBindingModel { 
            Id = model.Id });

            if (element == null)
            {
                throw new Exception("Закуска не найдена!");
            }
            _snackStorage.Delete(model);
        }
    }
}
