using Microsoft.Reporting.WebForms;
using DinerBusinessLogic.BindingModels;
using DinerBusinessLogic.ViewModels;
using DinerBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;
using Unity;
using System.Collections.Generic;

namespace DinerView
{
    public partial class FormMain : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly OrderLogic _orderLogic;
        private readonly WorkModeling workModeling;
        private ReportLogic report;
        private BackUpAbstractLogic _backUpAbstractLogic;
        public FormMain(OrderLogic orderLogic, ReportLogic Report, WorkModeling modeling,
            BackUpAbstractLogic backUp)
        {
            InitializeComponent();
            _orderLogic = orderLogic;
            workModeling = modeling;
            report = Report;
            _backUpAbstractLogic = backUp;
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                Program.ConfigGrid(_orderLogic.Read(null), dataGridView);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }
        private void продуктыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormFoods>();
            form.ShowDialog();
        }
        private void закускиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormSnacks>();
            form.ShowDialog();
        }
        private void ButtonCreateOrder_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateOrder>();
            form.ShowDialog();
            LoadData();
        }     
        private void ButtonPayOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    _orderLogic.PayOrder(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }
        private void ButtonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void списокЗакусокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "docx|*.docx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    report.SaveSnacksToWordFile(new ReportBindingModel
                    {
                        FileName = dialog.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
            }
        }

        private void продуктыПоЗакускамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportFoodSnack>();
            form.ShowDialog();
        }

        private void списокЗаказовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportOrders>();
            form.ShowDialog();
        }

        private void запускРаботToolStripMenuItem_Click(object sender, EventArgs e)
        {
            workModeling.DoWork();
            LoadData();
        }

        private void исполнителиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormImplementers>();
            form.ShowDialog();
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormClients>();
            form.ShowDialog();
        }

        private void buttonMails_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormMails>();
            form.ShowDialog();
        }

        private void создатьБэкапToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_backUpAbstractLogic != null)
                {
                    var fbd = new FolderBrowserDialog();
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        _backUpAbstractLogic.CreateArchive(fbd.SelectedPath);
                        MessageBox.Show("Бекап создан", "Сообщение",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }
    }
}
