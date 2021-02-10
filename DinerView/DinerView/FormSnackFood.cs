using System;
using System.Windows.Forms;
using System.Collections.Generic;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.BusinessLogics;
using Unity;

namespace DinerView
{
    public partial class FormSnackFood : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id { get { return Convert.ToInt32(comboBoxFood.SelectedValue); }
        set { comboBoxFood.SelectedValue = value; } }
        public string FoodName { get { return comboBoxFood.Text; } }
        public int Count { get { return Convert.ToInt32(textBoxCountFood.Text); } set {
        textBoxCountFood.Text = value.ToString(); } }
        public FormSnackFood(FoodLogic logic)
        {
            InitializeComponent();

            List<FoodViewModel> list = logic.Read(null);
            if (list != null)
            {
                comboBoxFood.DisplayMember = "FoodName";
                comboBoxFood.ValueMember = "Id";
                comboBoxFood.DataSource = list;
                comboBoxFood.SelectedItem = null;
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCountFood.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
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
