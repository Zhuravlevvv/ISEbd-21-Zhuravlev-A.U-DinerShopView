using DinerBusinessLogic.BusinessLogics;
using DinerBusinessLogic.Interfaces;
using DinerViewDatabaseImplement.Implements;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;


namespace DinerView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();

            currentContainer.RegisterType<IFoodStorage, FoodStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<ISnackStorage, SnackStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<IStoreHouseStorage, StoreHouseStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<FoodLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<OrderLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<SnackLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<StoreHouseLogic>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}

