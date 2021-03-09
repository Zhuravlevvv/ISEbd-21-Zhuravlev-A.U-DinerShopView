using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.BusinessLogics;
using DinerBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace DinerView
{
    public partial class FormReplenishmentStoreHouse : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int ComponentId
        {
            get { return Convert.ToInt32(comboBoxFood.SelectedValue); }
            set { comboBoxFood.SelectedValue = value; }
        }
        public int StoreHouse
        {
            get { return Convert.ToInt32(comboBoxFood.SelectedValue); }
            set { comboBoxFood.SelectedValue = value; }
        }
        public int Count
        {
            get { return Convert.ToInt32(textBoxCount.Text); }
            set
            {
                textBoxCount.Text = value.ToString();
            }
        }
        private readonly StoreHouseLogic storeHouseLogic;
        public FormReplenishmentStoreHouse(FoodLogic logicFood, StoreHouseLogic logicStoreHouse )
        {
            InitializeComponent();
            storeHouseLogic = logicStoreHouse;
            List<FoodViewModel> listFoods = logicFood.Read(null);
            if (listFoods != null)
            {
                comboBoxFood.DisplayMember = "FoodName";
                comboBoxFood.ValueMember = "Id";
                comboBoxFood.DataSource = listFoods;
                comboBoxFood.SelectedItem = null;
            }
            List<StoreHouseViewModel> listStoreHouses = logicStoreHouse.Read(null);
            if (listStoreHouses != null)
            {
                comboBoxStoreHouse.DisplayMember = "StoreHouseName";
                comboBoxStoreHouse.ValueMember = "Id";
                comboBoxStoreHouse.DataSource = listStoreHouses;
                comboBoxStoreHouse.SelectedItem = null;
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxFood.SelectedValue == null)
            {
                MessageBox.Show("Выберите продукт", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (comboBoxStoreHouse.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            storeHouseLogic.Replenishment(new StoreHouseReplenishmentBindingModel
            {
                FoodId = Convert.ToInt32(comboBoxFood.SelectedValue),
                StoreHouseId = Convert.ToInt32(comboBoxStoreHouse.SelectedValue),
                Count = Convert.ToInt32(textBoxCount.Text)
            });
            DialogResult = DialogResult.OK;
            Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}