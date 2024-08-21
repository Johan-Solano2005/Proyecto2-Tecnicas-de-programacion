namespace PruebaDeMigrar
{
    partial class Reporte
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
            button1 = new Button();
            dataGridView1 = new DataGridView();
            dataGridView2 = new DataGridView();
            dataGridView3 = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            dataGridView4 = new DataGridView();
            dataGridView5 = new DataGridView();
            label4 = new Label();
            label5 = new Label();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView5).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(42, 672);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 0;
            button1.Text = "Volver";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(42, 84);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(418, 225);
            dataGridView1.TabIndex = 1;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(528, 84);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(382, 225);
            dataGridView2.TabIndex = 2;
            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Location = new Point(965, 84);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(370, 225);
            dataGridView3.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(182, 33);
            label1.Name = "label1";
            label1.Size = new Size(90, 20);
            label1.TabIndex = 4;
            label1.Text = "Venta Diaria";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(657, 33);
            label2.Name = "label2";
            label2.Size = new Size(105, 20);
            label2.TabIndex = 5;
            label2.Text = "Venta Mensual";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1094, 33);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 6;
            label3.Text = "Venta Anual";
            // 
            // dataGridView4
            // 
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Location = new Point(42, 383);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.RowHeadersWidth = 51;
            dataGridView4.Size = new Size(418, 215);
            dataGridView4.TabIndex = 7;
            dataGridView4.CellContentClick += dataGridView4_CellContentClick;
            // 
            // dataGridView5
            // 
            dataGridView5.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView5.Location = new Point(528, 383);
            dataGridView5.Name = "dataGridView5";
            dataGridView5.RowHeadersWidth = 51;
            dataGridView5.Size = new Size(382, 215);
            dataGridView5.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(155, 333);
            label4.Name = "label4";
            label4.Size = new Size(80, 20);
            label4.TabIndex = 9;
            label4.Text = "TopCliente";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(676, 330);
            label5.Name = "label5";
            label5.Size = new Size(75, 20);
            label5.TabIndex = 10;
            label5.Text = "Inventario";
            // 
            // button2
            // 
            button2.Location = new Point(1032, 417);
            button2.Name = "button2";
            button2.Size = new Size(150, 29);
            button2.TabIndex = 11;
            button2.Text = "Descargar diario";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click_1;
            // 
            // button3
            // 
            button3.Location = new Point(1032, 477);
            button3.Name = "button3";
            button3.Size = new Size(150, 29);
            button3.TabIndex = 12;
            button3.Text = "Descargar Mensual";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // button4
            // 
            button4.Location = new Point(1032, 535);
            button4.Name = "button4";
            button4.Size = new Size(150, 29);
            button4.TabIndex = 13;
            button4.Text = "Descargar Anual";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click_1;
            // 
            // Reporte
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1357, 713);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(dataGridView5);
            Controls.Add(dataGridView4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView3);
            Controls.Add(dataGridView2);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Name = "Reporte";
            Text = "Reporte";
            Load += Reporte_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private DataGridView dataGridView3;
        private Label label1;
        private Label label2;
        private Label label3;
        private DataGridView dataGridView4;
        private DataGridView dataGridView5;
        private Label label4;
        private Label label5;
        private Button button2;
        private Button button3;
        private Button button4;
    }
}