namespace DinerView
{
    partial class FormSnackFood
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelFood = new System.Windows.Forms.Label();
            this.labelCountFood = new System.Windows.Forms.Label();
            this.comboBoxFood = new System.Windows.Forms.ComboBox();
            this.textBoxCountFood = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelFood
            // 
            this.labelFood.AutoSize = true;
            this.labelFood.Location = new System.Drawing.Point(23, 21);
            this.labelFood.Name = "labelFood";
            this.labelFood.Size = new System.Drawing.Size(52, 13);
            this.labelFood.TabIndex = 0;
            this.labelFood.Text = "Продукт:";
            // 
            // labelCountFood
            // 
            this.labelCountFood.AutoSize = true;
            this.labelCountFood.Location = new System.Drawing.Point(23, 51);
            this.labelCountFood.Name = "labelCountFood";
            this.labelCountFood.Size = new System.Drawing.Size(69, 13);
            this.labelCountFood.TabIndex = 1;
            this.labelCountFood.Text = "Количество:";
            // 
            // comboBoxFood
            // 
            this.comboBoxFood.FormattingEnabled = true;
            this.comboBoxFood.Location = new System.Drawing.Point(82, 21);
            this.comboBoxFood.Name = "comboBoxFood";
            this.comboBoxFood.Size = new System.Drawing.Size(257, 21);
            this.comboBoxFood.TabIndex = 2;
            // 
            // textBoxCountFood
            // 
            this.textBoxCountFood.Location = new System.Drawing.Point(99, 51);
            this.textBoxCountFood.Name = "textBoxCountFood";
            this.textBoxCountFood.Size = new System.Drawing.Size(240, 20);
            this.textBoxCountFood.TabIndex = 3;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(247, 82);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(92, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(149, 82);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(92, 23);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormSnackFood
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 117);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxCountFood);
            this.Controls.Add(this.comboBoxFood);
            this.Controls.Add(this.labelCountFood);
            this.Controls.Add(this.labelFood);
            this.Name = "FormSnackFood";
            this.Text = "Продукты закуски";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFood;
        private System.Windows.Forms.Label labelCountFood;
        private System.Windows.Forms.ComboBox comboBoxFood;
        private System.Windows.Forms.TextBox textBoxCountFood;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
    }
}