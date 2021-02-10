using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.BusinessLogics;
using DinerBusinessLogic.ViewModels;
using Unity;

namespace DinerView
{
    public partial class FormSnack : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }

        private readonly SnackLogic logic;

        private int? id;

        private Dictionary<int, (string, int)> snackFoods;
        public FormSnack(SnackLogic service)
        {
            InitializeComponent();
            this.logic = service;
        }
        private void FormSnack_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    SnackViewModel view = logic.Read(new SnackBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.SnackName;
                        textBoxPrice.Text = view.Price.ToString();
                        snackFoods = view.SnackFoods;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            else
            {
                snackFoods = new Dictionary<int, (string, int)>();
            }
        }
        private void LoadData()
        {
            try
            {
                if (snackFoods != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var sf in snackFoods)
                    {
                        dataGridView.Rows.Add(new object[] { sf.Key, sf.Value.Item1, sf.Value.Item2 });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormSnackFood>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (snackFoods.ContainsKey(form.Id))
                {
                    snackFoods[form.Id] = (form.FoodName, form.Count);
                }
                else
                {
                    snackFoods.Add(form.Id, (form.FoodName, form.Count));
                }
                LoadData();
            }
        }
        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormSnackFood>();
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                form.Id = id;
                form.Count = snackFoods[id].Item2;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    snackFoods[form.Id] = (form.FoodName, form.Count);
                    LoadData();
                }
            }
        }
        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        snackFoods.Remove(Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }
        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (snackFoods == null || snackFoods.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new SnackBindingModel
                {
                    Id = id,
                    SnackName = textBoxName.Text,
                    Price = Convert.ToDecimal(textBoxPrice.Text),
                    SnackFoods = snackFoods
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
