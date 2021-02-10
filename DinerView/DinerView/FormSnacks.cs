using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;
using Unity;

namespace DinerView
{
    public partial class FormSnacks : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly SnackLogic logic;
        public FormSnacks(SnackLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void FormSnacks_Load(object sender, EventArgs e)
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
                    dataGridViewSnacks.DataSource = list;
                    dataGridViewSnacks.Columns[0].Visible = false;
                    dataGridViewSnacks.Columns[3].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormSnack>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void ButtonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewSnacks.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormSnack>();
                form.Id = Convert.ToInt32(dataGridViewSnacks.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }
        private void ButtonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewSnacks.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridViewSnacks.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        logic.Delete(new SnackBindingModel 
                        { 
                            Id = id 
                        });
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
        private void ButtonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
