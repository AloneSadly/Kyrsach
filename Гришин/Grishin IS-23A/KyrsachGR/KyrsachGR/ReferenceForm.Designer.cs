namespace KyrsachGR
{
    partial class ReferenceForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReferenceForm));
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpCountry = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.lblCountryName = new System.Windows.Forms.Label();
            this.txtCountryName = new System.Windows.Forms.TextBox();
            this.dgvCountry = new System.Windows.Forms.DataGridView();
            this.tpMaterial = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tpCategory = new System.Windows.Forms.TabPage();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tpColor = new System.Windows.Forms.TabPage();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.tpPattern = new System.Windows.Forms.TabPage();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.button14 = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tpCountry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCountry)).BeginInit();
            this.tpMaterial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tpCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tpColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.tpPattern.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(158, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Справочники";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpCountry);
            this.tabControl.Controls.Add(this.tpMaterial);
            this.tabControl.Controls.Add(this.tpCategory);
            this.tabControl.Controls.Add(this.tpColor);
            this.tabControl.Controls.Add(this.tpPattern);
            this.tabControl.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.tabControl.Location = new System.Drawing.Point(12, 45);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 500);
            this.tabControl.TabIndex = 1;
            // 
            // tpCountry
            // 
            this.tpCountry.Controls.Add(this.button1);
            this.tpCountry.Controls.Add(this.button2);
            this.tpCountry.Controls.Add(this.button15);
            this.tpCountry.Controls.Add(this.lblCountryName);
            this.tpCountry.Controls.Add(this.txtCountryName);
            this.tpCountry.Controls.Add(this.dgvCountry);
            this.tpCountry.Location = new System.Drawing.Point(4, 25);
            this.tpCountry.Name = "tpCountry";
            this.tpCountry.Padding = new System.Windows.Forms.Padding(3);
            this.tpCountry.Size = new System.Drawing.Size(752, 471);
            this.tpCountry.TabIndex = 0;
            this.tpCountry.Text = "Страны";
            this.tpCountry.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(619, 422);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 32);
            this.button1.TabIndex = 22;
            this.button1.Text = "Отменить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(367, 422);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 32);
            this.button2.TabIndex = 21;
            this.button2.Text = "Добавить";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(493, 422);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(120, 32);
            this.button15.TabIndex = 20;
            this.button15.Text = "Изменить";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // lblCountryName
            // 
            this.lblCountryName.AutoSize = true;
            this.lblCountryName.Location = new System.Drawing.Point(14, 430);
            this.lblCountryName.Name = "lblCountryName";
            this.lblCountryName.Size = new System.Drawing.Size(77, 16);
            this.lblCountryName.TabIndex = 0;
            this.lblCountryName.Text = "Название:";
            // 
            // txtCountryName
            // 
            this.txtCountryName.Location = new System.Drawing.Point(109, 427);
            this.txtCountryName.Name = "txtCountryName";
            this.txtCountryName.Size = new System.Drawing.Size(244, 23);
            this.txtCountryName.TabIndex = 1;
            // 
            // dgvCountry
            // 
            this.dgvCountry.AllowUserToAddRows = false;
            this.dgvCountry.AllowUserToDeleteRows = false;
            this.dgvCountry.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCountry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCountry.Location = new System.Drawing.Point(15, 15);
            this.dgvCountry.Name = "dgvCountry";
            this.dgvCountry.ReadOnly = true;
            this.dgvCountry.RowHeadersVisible = false;
            this.dgvCountry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCountry.Size = new System.Drawing.Size(725, 379);
            this.dgvCountry.TabIndex = 4;
            // 
            // tpMaterial
            // 
            this.tpMaterial.Controls.Add(this.button3);
            this.tpMaterial.Controls.Add(this.button4);
            this.tpMaterial.Controls.Add(this.button5);
            this.tpMaterial.Controls.Add(this.label1);
            this.tpMaterial.Controls.Add(this.textBox1);
            this.tpMaterial.Controls.Add(this.dataGridView1);
            this.tpMaterial.Location = new System.Drawing.Point(4, 25);
            this.tpMaterial.Name = "tpMaterial";
            this.tpMaterial.Padding = new System.Windows.Forms.Padding(3);
            this.tpMaterial.Size = new System.Drawing.Size(752, 471);
            this.tpMaterial.TabIndex = 1;
            this.tpMaterial.Text = "Материалы";
            this.tpMaterial.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(619, 423);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 32);
            this.button3.TabIndex = 22;
            this.button3.Text = "Отменить";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(367, 423);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 32);
            this.button4.TabIndex = 21;
            this.button4.Text = "Добавить";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(493, 423);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 32);
            this.button5.TabIndex = 20;
            this.button5.Text = "Изменить";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 431);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Название:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(108, 428);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(244, 23);
            this.textBox1.TabIndex = 9;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(14, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(725, 379);
            this.dataGridView1.TabIndex = 10;
            // 
            // tpCategory
            // 
            this.tpCategory.Controls.Add(this.button6);
            this.tpCategory.Controls.Add(this.button7);
            this.tpCategory.Controls.Add(this.button8);
            this.tpCategory.Controls.Add(this.label3);
            this.tpCategory.Controls.Add(this.textBox3);
            this.tpCategory.Controls.Add(this.label2);
            this.tpCategory.Controls.Add(this.textBox2);
            this.tpCategory.Controls.Add(this.dataGridView2);
            this.tpCategory.Location = new System.Drawing.Point(4, 25);
            this.tpCategory.Name = "tpCategory";
            this.tpCategory.Padding = new System.Windows.Forms.Padding(3);
            this.tpCategory.Size = new System.Drawing.Size(752, 471);
            this.tpCategory.TabIndex = 2;
            this.tpCategory.Text = "Категории";
            this.tpCategory.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(619, 423);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(120, 32);
            this.button6.TabIndex = 22;
            this.button6.Text = "Отменить";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(367, 423);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(120, 32);
            this.button7.TabIndex = 21;
            this.button7.Text = "Добавить";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(493, 423);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(120, 32);
            this.button8.TabIndex = 20;
            this.button8.Text = "Изменить";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 404);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Название:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(108, 401);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(244, 23);
            this.textBox3.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 431);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Описание:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(108, 428);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(244, 23);
            this.textBox2.TabIndex = 9;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(14, 16);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(725, 379);
            this.dataGridView2.TabIndex = 10;
            // 
            // tpColor
            // 
            this.tpColor.Controls.Add(this.button9);
            this.tpColor.Controls.Add(this.button10);
            this.tpColor.Controls.Add(this.button11);
            this.tpColor.Controls.Add(this.label4);
            this.tpColor.Controls.Add(this.textBox4);
            this.tpColor.Controls.Add(this.dataGridView3);
            this.tpColor.Location = new System.Drawing.Point(4, 25);
            this.tpColor.Name = "tpColor";
            this.tpColor.Padding = new System.Windows.Forms.Padding(3);
            this.tpColor.Size = new System.Drawing.Size(752, 471);
            this.tpColor.TabIndex = 3;
            this.tpColor.Text = "Цвета";
            this.tpColor.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(619, 423);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(120, 32);
            this.button9.TabIndex = 22;
            this.button9.Text = "Отменить";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(367, 423);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(120, 32);
            this.button10.TabIndex = 21;
            this.button10.Text = "Добавить";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(493, 423);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(120, 32);
            this.button11.TabIndex = 20;
            this.button11.Text = "Изменить";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 431);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Название:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(108, 428);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(244, 23);
            this.textBox4.TabIndex = 15;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(14, 16);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.RowHeadersVisible = false;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(725, 379);
            this.dataGridView3.TabIndex = 16;
            // 
            // tpPattern
            // 
            this.tpPattern.Controls.Add(this.button12);
            this.tpPattern.Controls.Add(this.button13);
            this.tpPattern.Controls.Add(this.label5);
            this.tpPattern.Controls.Add(this.textBox5);
            this.tpPattern.Controls.Add(this.dataGridView4);
            this.tpPattern.Controls.Add(this.button14);
            this.tpPattern.Location = new System.Drawing.Point(4, 25);
            this.tpPattern.Name = "tpPattern";
            this.tpPattern.Padding = new System.Windows.Forms.Padding(3);
            this.tpPattern.Size = new System.Drawing.Size(752, 471);
            this.tpPattern.TabIndex = 4;
            this.tpPattern.Text = "Шаблоны";
            this.tpPattern.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(619, 423);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(120, 32);
            this.button12.TabIndex = 19;
            this.button12.Text = "Отменить";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(367, 423);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(120, 32);
            this.button13.TabIndex = 18;
            this.button13.Text = "Добавить";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 431);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "Название:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(108, 428);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(244, 23);
            this.textBox5.TabIndex = 15;
            // 
            // dataGridView4
            // 
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView4.AllowUserToDeleteRows = false;
            this.dataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(14, 16);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.ReadOnly = true;
            this.dataGridView4.RowHeadersVisible = false;
            this.dataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView4.Size = new System.Drawing.Size(725, 379);
            this.dataGridView4.TabIndex = 16;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(493, 423);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(120, 32);
            this.button14.TabIndex = 17;
            this.button14.Text = "Изменить";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(652, 556);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 32);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ReferenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 600);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReferenceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ы";
            this.tabControl.ResumeLayout(false);
            this.tpCountry.ResumeLayout(false);
            this.tpCountry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCountry)).EndInit();
            this.tpMaterial.ResumeLayout(false);
            this.tpMaterial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tpCategory.ResumeLayout(false);
            this.tpCategory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tpColor.ResumeLayout(false);
            this.tpColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.tpPattern.ResumeLayout(false);
            this.tpPattern.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Load += new System.EventHandler(this.ReferenceForm_Load);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TabControl tabControl;

        private System.Windows.Forms.TabPage tpCountry;
        private System.Windows.Forms.TabPage tpMaterial;
        private System.Windows.Forms.TabPage tpCategory;
        private System.Windows.Forms.TabPage tpColor;
        private System.Windows.Forms.TabPage tpPattern;

        // Страны
        private System.Windows.Forms.Label lblCountryName;
        private System.Windows.Forms.TextBox txtCountryName;
        private System.Windows.Forms.DataGridView dgvCountry;

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
    }
}