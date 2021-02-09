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
    public partial class FormFoods : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly FoodLogic logic;
        public FormFoods(FoodLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormFoods_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = logic.Read(null);
                if (list != null)
                {
                    dataGridViewFoods.DataSource = list;
                    dataGridViewFoods.Columns[0].Visible = false;
                    dataGridViewFoods.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormFood>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewFoods.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormFood>();
                form.Id = Convert.ToInt32(dataGridViewFoods.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
           if (dataGridViewFoods.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridViewFoods.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        logic.Delete(new FoodBindingModel
                        {
                            Id = id
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
