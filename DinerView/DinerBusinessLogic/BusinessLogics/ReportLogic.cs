using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.HelperModels;
using DinerBusinessLogic.Interfaces;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.Enums;

namespace DinerBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly ISnackStorage _snackStorage;

        private readonly IOrderStorage _orderStorage;

        private readonly IStoreHouseStorage _storeHouseStorage;

        public ReportLogic(ISnackStorage snackStorage, IStoreHouseStorage
        storeHouseStorage, IOrderStorage orderStorage)
        {
            _snackStorage = snackStorage;
            _storeHouseStorage = storeHouseStorage;
            _orderStorage = orderStorage;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>
        public List<ReportFoodSnackViewModel> GetFoodSnack()
        {
            var snacks = _snackStorage.GetFullList();
            var list = new List<ReportFoodSnackViewModel>();
            foreach (var snack in snacks)
            {
                var record = new ReportFoodSnackViewModel
                {
                    SnackName = snack.SnackName,
                    Foods = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var food in snack.SnackFoods)
                {  
                    record.Foods.Add(new Tuple<string, int>(food.Value.Item1,
                    food.Value.Item2));
                    record.TotalCount += food.Value.Item2;       
                }
                list.Add(record);
            }
            return list;
        }
        public List<ReportStoreHouseFoodsViewModel> GetStoreHouseFood()
        {
            var storeHouses = _storeHouseStorage.GetFullList();

            var list = new List<ReportStoreHouseFoodsViewModel>();

            foreach (var storeHouse in storeHouses)
            {
                var record = new ReportStoreHouseFoodsViewModel
                {
                    StoreHouseName = storeHouse.StoreHouseName,
                    Foods = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var component in storeHouse.StoreHouseFoods)
                {
                    record.Foods.Add(new Tuple<string, int>(component.Value.Item1, component.Value.Item2));
                    record.TotalCount += component.Value.Item2;
                }
                list.Add(record);
            }
            return list;
        }
        /// <summary>
        /// Получение списка заказов за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.DateCreate,
                SnackName = x.SnackName,
                Count = x.Count,
                Sum = x.Sum,
                Status = ((OrderStatus)Enum.Parse(typeof(OrderStatus), x.Status.ToString())).ToString()
            })
            .ToList();
        }
        public List<ReportOrderByDateViewModel> GetOrdersInfo()
        {
            return _orderStorage.GetFullList()
                .GroupBy(order => order.DateCreate
                .ToShortDateString())
                .Select(rec => new ReportOrderByDateViewModel
                {
                    Date = Convert.ToDateTime(rec.Key),
                    Count = rec.Count(),
                    Sum = rec.Sum(order => order.Sum)
                })
                .ToList();
        }
        /// <summary>
        /// Сохранение изделия в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SaveSnacksToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список закусок",
                Snacks = _snackStorage.GetFullList()
            });
        }
        /// <summary>
        /// Сохранение компонент с указаеним продуктов в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SaveFoodSnackToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список закусок",
                FoodSnacks = GetFoodSnack()
            });
        }
        /// <summary>
        /// Сохранение заказов в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }
        public void SaveStoreHouseFoodsToExcel(ReportBindingModel model)
        {
            SaveToExcel.CreateDocForStoreHouse(new ExcelInfoForStoreHouse
            {
                FileName = model.FileName,
                Title = "Список складов",
                StoreHouseFoods = GetStoreHouseFood()
            });
        }
        public void SaveOrdersInfoToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDocForStoreHouse(new PdfInfoForOrder
            {
                FileName = model.FileName,
                Title = "Список заказов",
                Orders = GetOrdersInfo()
            });
        }
        public void SaveStoreHousesToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDocForStoreHouse(new WordInfoForStoreHouse
            {
                FileName = model.FileName,
                Title = "Список складов",
                StoreHouses = _storeHouseStorage.GetFullList()
            });
        }
    }
}
