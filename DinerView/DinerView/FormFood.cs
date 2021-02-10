using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.BusinessLogics;
using Unity;

namespace DinerView
{
    public partial class FormFood : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly FoodLogic logic;

        private int? id;
        public FormFood(FoodLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void FormFood_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new FoodBindingModel
                    {
                        Id = id
                    })?[0];
                    if (view != null)
                    {
                        textBoxProductName.Text = view.FoodName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxProductName.Text))
            {
                MessageBox.Show("Заполните название!", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new FoodBindingModel
                {
                    Id = id,
                    FoodName = textBoxProductName.Text
                });
                MessageBox.Show("Сохранине прошло успешно!", "Сообщение",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK,
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
