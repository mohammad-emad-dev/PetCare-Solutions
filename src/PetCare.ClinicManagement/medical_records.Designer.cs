namespace bitcINTERFACE
{
    partial class medical_records
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.medicalRecordsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.medicalRecordsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.petIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.diagnosisDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prescriptionsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vaccinationsGivenDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.followupInstructionsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.medicalrecordsBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.petCareSolutionsDataSet = new bitcINTERFACE.PetCareSolutionsDataSet();
            this.medical_recordsTableAdapter = new bitcINTERFACE.PetCareSolutionsDataSetTableAdapters.medical_recordsTableAdapter();
            this.textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.medicalRecordsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.medicalRecordsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.medicalrecordsBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.petCareSolutionsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 444);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 34);
            this.button1.TabIndex = 27;
            this.button1.Text = "Save Record";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(685, 372);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(266, 27);
            this.textBox5.TabIndex = 26;
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(685, 267);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(266, 27);
            this.textBox4.TabIndex = 25;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(685, 205);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(266, 27);
            this.textBox3.TabIndex = 24;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(685, 320);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(266, 27);
            this.textBox1.TabIndex = 22;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(548, 376);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "Follow UpInstructions";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(575, 272);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "Prescriptions ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(557, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Vaccinations Given ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(583, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "Diagnosis ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(589, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "Pet Info";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(583, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Select Pet";
            // 
            // medicalRecordsBindingSource1
            // 
            this.medicalRecordsBindingSource1.DataMember = "MedicalRecords";
            // 
            // medicalRecordsBindingSource
            // 
            this.medicalRecordsBindingSource.DataMember = "MedicalRecords";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(683, 40);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(268, 24);
            this.comboBox1.TabIndex = 28;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(685, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 83);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(592, 426);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 16);
            this.label7.TabIndex = 30;
            this.label7.Text = "Vet Id";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(280, 444);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(230, 34);
            this.button2.TabIndex = 32;
            this.button2.Text = "Clear Record";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.petIdDataGridViewTextBoxColumn,
            this.idDataGridViewTextBoxColumn,
            this.diagnosisDataGridViewTextBoxColumn,
            this.prescriptionsDataGridViewTextBoxColumn,
            this.vaccinationsGivenDataGridViewTextBoxColumn,
            this.followupInstructionsDataGridViewTextBoxColumn,
            this.vetIdDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.medicalrecordsBindingSource2;
            this.dataGridView1.Location = new System.Drawing.Point(24, 45);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(486, 381);
            this.dataGridView1.TabIndex = 33;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // petIdDataGridViewTextBoxColumn
            // 
            this.petIdDataGridViewTextBoxColumn.DataPropertyName = "petId";
            this.petIdDataGridViewTextBoxColumn.HeaderText = "petId";
            this.petIdDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.petIdDataGridViewTextBoxColumn.Name = "petIdDataGridViewTextBoxColumn";
            this.petIdDataGridViewTextBoxColumn.Width = 125;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "id";
            this.idDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 125;
            // 
            // diagnosisDataGridViewTextBoxColumn
            // 
            this.diagnosisDataGridViewTextBoxColumn.DataPropertyName = "diagnosis";
            this.diagnosisDataGridViewTextBoxColumn.HeaderText = "diagnosis";
            this.diagnosisDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.diagnosisDataGridViewTextBoxColumn.Name = "diagnosisDataGridViewTextBoxColumn";
            this.diagnosisDataGridViewTextBoxColumn.Width = 125;
            // 
            // prescriptionsDataGridViewTextBoxColumn
            // 
            this.prescriptionsDataGridViewTextBoxColumn.DataPropertyName = "prescriptions";
            this.prescriptionsDataGridViewTextBoxColumn.HeaderText = "prescriptions";
            this.prescriptionsDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.prescriptionsDataGridViewTextBoxColumn.Name = "prescriptionsDataGridViewTextBoxColumn";
            this.prescriptionsDataGridViewTextBoxColumn.Width = 125;
            // 
            // vaccinationsGivenDataGridViewTextBoxColumn
            // 
            this.vaccinationsGivenDataGridViewTextBoxColumn.DataPropertyName = "vaccinationsGiven";
            this.vaccinationsGivenDataGridViewTextBoxColumn.HeaderText = "vaccinationsGiven";
            this.vaccinationsGivenDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.vaccinationsGivenDataGridViewTextBoxColumn.Name = "vaccinationsGivenDataGridViewTextBoxColumn";
            this.vaccinationsGivenDataGridViewTextBoxColumn.Width = 125;
            // 
            // followupInstructionsDataGridViewTextBoxColumn
            // 
            this.followupInstructionsDataGridViewTextBoxColumn.DataPropertyName = "followupInstructions";
            this.followupInstructionsDataGridViewTextBoxColumn.HeaderText = "followupInstructions";
            this.followupInstructionsDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.followupInstructionsDataGridViewTextBoxColumn.Name = "followupInstructionsDataGridViewTextBoxColumn";
            this.followupInstructionsDataGridViewTextBoxColumn.Width = 125;
            // 
            // vetIdDataGridViewTextBoxColumn
            // 
            this.vetIdDataGridViewTextBoxColumn.DataPropertyName = "vetId";
            this.vetIdDataGridViewTextBoxColumn.HeaderText = "vetId";
            this.vetIdDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.vetIdDataGridViewTextBoxColumn.Name = "vetIdDataGridViewTextBoxColumn";
            this.vetIdDataGridViewTextBoxColumn.Width = 125;
            // 
            // medicalrecordsBindingSource2
            // 
            this.medicalrecordsBindingSource2.DataMember = "medical_records";
            this.medicalrecordsBindingSource2.DataSource = this.petCareSolutionsDataSet;
            // 
            // petCareSolutionsDataSet
            // 
            this.petCareSolutionsDataSet.DataSetName = "PetCareSolutionsDataSet";
            this.petCareSolutionsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // medical_recordsTableAdapter
            // 
            this.medical_recordsTableAdapter.ClearBeforeFill = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(685, 423);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(266, 27);
            this.textBox2.TabIndex = 34;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // medical_records
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::bitcINTERFACE.Properties.Resources.healthcare_and_medical_innovation_background_with_dots_and_lines_molecular_structure_and_hospital_cross_illustration_vector;
            this.ClientSize = new System.Drawing.Size(979, 496);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "medical_records";
            this.Text = "medical_records";
            this.Load += new System.EventHandler(this.medical_records_Load);
            ((System.ComponentModel.ISupportInitialize)(this.medicalRecordsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.medicalRecordsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.medicalrecordsBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.petCareSolutionsDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource medicalRecordsBindingSource1;
        private System.Windows.Forms.BindingSource medicalRecordsBindingSource;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private PetCareSolutionsDataSet petCareSolutionsDataSet;
        private System.Windows.Forms.BindingSource medicalrecordsBindingSource2;
        private PetCareSolutionsDataSetTableAdapters.medical_recordsTableAdapter medical_recordsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn petIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn diagnosisDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prescriptionsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vaccinationsGivenDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn followupInstructionsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox textBox2;
    }
}