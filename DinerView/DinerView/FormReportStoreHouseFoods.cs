using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;
using Unity;

namespace DinerView
{
    public partial class FormReportStoreHouseFoods : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic logic;

        public FormReportStoreHouseFoods(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormReportStoreHouseComponents_Load(object sender, EventArgs e)
        {
            try
            {
                var dict = logic.GetStoreHouseFood();
                if (dict != null)
                {
                    dataGridViewReportStoreHouseFoods.Rows.Clear();
                    foreach (var elem in dict)
                    {
                        dataGridViewReportStoreHouseFoods.Rows.Add(new object[]
                        {
                            elem.StoreHouseName, "", ""
                        });
                        foreach (var listElem in elem.Foods)
                        {
                            dataGridViewReportStoreHouseFoods.Rows.Add(new object[]
                            {
                                "", listElem.Item1, listElem.Item2
                            });
                        }
                        dataGridViewReportStoreHouseFoods.Rows.Add(new object[]
                        {
                            "Итого", "", elem.TotalCount
                        });
                        dataGridViewReportStoreHouseFoods.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
MessageBoxIcon.Error);
            }
        }

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.SaveStoreHouseFoodsToExcel(new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
