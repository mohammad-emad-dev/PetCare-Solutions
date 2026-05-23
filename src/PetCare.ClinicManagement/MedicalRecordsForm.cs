using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bitcINTERFACE
{
    public partial class MedicalRecordsForm : Form
    {
        private Tuple<int, int> selectedRecordId = null; // (petId, recordId)

        public MedicalRecordsForm()
        {
            InitializeComponent();
        }

        private void MedicalRecordsForm_Load(object sender, EventArgs e)
        {
            ApplyInterfaceStyle();
            LoadMedicalRecords();
            LoadPetsComboBox();

        }

        private void LoadMedicalRecords()
        {
            try
            {
                string query = "SELECT * FROM medical_records";
                dataGridView1.DataSource = Database.FillDataTable(query);
                ConfigureMedicalRecordsGrid();
            }
            catch (Exception ex)
            {
                UserMessages.ShowDatabaseError("Failed to load medical records", ex);
            }
        }

        private void ApplyInterfaceStyle()
        {
            Font = new Font("Segoe UI", 9.8F, FontStyle.Regular);
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.Stretch;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            label1.Text = "Pet";
            label2.Text = "Pet details";
            label4.Text = "Diagnosis";
            label6.Text = "Prescriptions";
            label3.Text = "Vaccinations";
            label5.Text = "Follow-up";
            label7.Text = "Vet ID";

            StyleLabel(label1);
            StyleLabel(label2);
            StyleLabel(label3);
            StyleLabel(label4);
            StyleLabel(label5);
            StyleLabel(label6);
            StyleLabel(label7);

            StyleTextBox(textBox1);
            StyleTextBox(textBox2);
            StyleTextBox(textBox3);
            StyleTextBox(textBox4);
            StyleTextBox(textBox5);
            comboBox1.Font = new Font("Segoe UI", 10.4F);
            groupBox1.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            groupBox1.BackColor = Color.Transparent;

            StylePrimaryButton(button1, "Save Record");
            StyleSecondaryButton(button2, "Clear");

            dataGridView1.Location = new Point(32, 42);
            dataGridView1.Size = new Size(548, 384);

            int labelX = 616;
            int inputX = 616;
            int top = 42;
            int gap = 57;
            PositionField(label1, comboBox1, labelX, inputX, top);

            label2.Location = new Point(labelX, top + gap);
            groupBox1.Location = new Point(inputX, top + gap + 27);
            groupBox1.Size = new Size(278, 92);

            PositionField(label4, textBox3, labelX, inputX, top + (gap * 3));
            PositionField(label6, textBox4, labelX, inputX, top + (gap * 4));
            PositionField(label3, textBox1, labelX, inputX, top + (gap * 5));
            PositionField(label5, textBox5, labelX, inputX, top + (gap * 6));
            PositionField(label7, textBox2, labelX, inputX, top + (gap * 7));

            button1.Location = new Point(32, 452);
            button2.Location = new Point(214, 452);
            ClientSize = new Size(930, 520);
        }

        private void ConfigureMedicalRecordsGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.FixedSingle;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 96, 173);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 9.2F);
            dataGridView1.RowHeadersVisible = false;

            SetColumnHeader(petIdDataGridViewTextBoxColumn, "Pet ID");
            SetColumnHeader(idDataGridViewTextBoxColumn, "Record ID");
            SetColumnHeader(diagnosisDataGridViewTextBoxColumn, "Diagnosis");
            SetColumnHeader(prescriptionsDataGridViewTextBoxColumn, "Prescriptions");
            SetColumnHeader(vaccinationsGivenDataGridViewTextBoxColumn, "Vaccinations");
            SetColumnHeader(followupInstructionsDataGridViewTextBoxColumn, "Follow-up");
            SetColumnHeader(vetIdDataGridViewTextBoxColumn, "Vet ID");
        }

        private void PositionField(Label label, Control control, int labelX, int inputX, int top)
        {
            label.Location = new Point(labelX, top);
            control.Location = new Point(inputX, top + 27);
            control.Size = new Size(278, 32);
        }

        private void StyleLabel(Label label)
        {
            label.BackColor = Color.Transparent;
            label.Font = new Font("Segoe UI", 9.8F, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(34, 58, 94);
        }

        private void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Segoe UI", 10.4F);
            textBox.Multiline = false;
        }

        private void StylePrimaryButton(Button button, string text)
        {
            StyleButton(button, text, Color.FromArgb(0, 96, 173));
        }

        private void StyleSecondaryButton(Button button, string text)
        {
            StyleButton(button, text, Color.FromArgb(40, 47, 56));
        }

        private void StyleButton(Button button, string text, Color backColor)
        {
            button.Text = text;
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            button.Size = new Size(158, 42);
            button.UseVisualStyleBackColor = false;
        }

        private void SetColumnHeader(DataGridViewColumn column, string headerText)
        {
            column.HeaderText = headerText;
        }

        // Method to load pet names into comboBox1
        private void LoadPetsComboBox()
        {
            try
            {
                string query = "SELECT id, name FROM pets";
                DataTable dt = Database.FillDataTable(query);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "name";
                comboBox1.ValueMember = "id";
                comboBox1.SelectedIndex = -1; // Start with no pet selected
                groupBox1.Controls.Clear(); // Clear the info box initially
            }
            catch (Exception ex)
            {
                UserMessages.ShowDatabaseError("Failed to load pets", ex);
            }
        }

        // Event for clicking a cell in the DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int petId = Convert.ToInt32(row.Cells["petIdDataGridViewTextBoxColumn"].Value);
                int recordId = Convert.ToInt32(row.Cells["idDataGridViewTextBoxColumn"].Value);
                selectedRecordId = new Tuple<int, int>(petId, recordId);

                // Populate the form controls
                comboBox1.SelectedValue = petId;
                textBox2.Text = row.Cells["vetIdDataGridViewTextBoxColumn"].Value.ToString();
                textBox3.Text = row.Cells["diagnosisDataGridViewTextBoxColumn"].Value.ToString();
                textBox4.Text = row.Cells["prescriptionsDataGridViewTextBoxColumn"].Value.ToString();
                textBox1.Text = row.Cells["vaccinationsGivenDataGridViewTextBoxColumn"].Value.ToString();
                textBox5.Text = row.Cells["followupInstructionsDataGridViewTextBoxColumn"].Value.ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Clear the GroupBox first
            groupBox1.Controls.Clear();

            // Check if a valid pet is selected
            if (comboBox1.SelectedValue != null && comboBox1.SelectedValue is int)
            {
                int selectedPetId = (int)comboBox1.SelectedValue;

                // Fetch details for the selected pet from the database
                using (SqlConnection conn = Database.CreateConnection())
                {
                    string query = "SELECT species, breed, Gender, dateOfBirth FROM pets WHERE id = @petId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@petId", selectedPetId);
                        try
                        {
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                // Create and add labels dynamically to show the info
                                string species = reader["species"].ToString();
                                string breed = reader["breed"].ToString();
                                string gender = reader["Gender"].ToString();
                                string dob = Convert.ToDateTime(reader["dateOfBirth"]).ToShortDateString();

                                Label lblSpecies = new Label { Text = "Species: " + species, Location = new Point(10, 20), AutoSize = true };
                                Label lblBreed = new Label { Text = "Breed: " + breed, Location = new Point(10, 40), AutoSize = true };
                                Label lblGender = new Label { Text = "Gender: " + gender, Location = new Point(10, 60), AutoSize = true };
                                // Note: GroupBox size in your designer might be small. Adjust label locations if needed.

                                groupBox1.Controls.Add(lblSpecies);
                                groupBox1.Controls.Add(lblBreed);
                                groupBox1.Controls.Add(lblGender);
                            }
                        }
                        catch (Exception ex)
                        {
                            UserMessages.ShowDatabaseError("Could not load pet details", ex);
                        }
                    }
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Pet, Vet ID, and Diagnosis are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox2.Text, out int vetId))
            {
                MessageBox.Show("Vet ID must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedRecordId == null)
            {
                // INSERT LOGIC
                string query = "INSERT INTO medical_records (petId, diagnosis, prescriptions, vaccinationsGiven, followupInstructions, vetId) VALUES (@petId, @diagnosis, @prescriptions, @vaccinations, @instructions, @vetId)";
                using (SqlConnection conn = Database.CreateConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@petId", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@vetId", vetId);
                        cmd.Parameters.AddWithValue("@diagnosis", textBox3.Text);
                        cmd.Parameters.AddWithValue("@prescriptions", textBox4.Text);
                        cmd.Parameters.AddWithValue("@vaccinations", textBox1.Text);
                        cmd.Parameters.AddWithValue("@instructions", textBox5.Text);

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Medical record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            UserMessages.ShowDatabaseError("Could not save medical record", ex);
                        }
                    }
                }
            }
            else
            {
                // UPDATE LOGIC
                string query = "UPDATE medical_records SET petId=@newPetId, diagnosis=@diagnosis, prescriptions=@prescriptions, vaccinationsGiven=@vaccinations, followupInstructions=@instructions, vetId=@vetId WHERE petId=@oldPetId AND id=@recordId";
                using (SqlConnection conn = Database.CreateConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@oldPetId", selectedRecordId.Item1);
                        cmd.Parameters.AddWithValue("@recordId", selectedRecordId.Item2);
                        cmd.Parameters.AddWithValue("@newPetId", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@vetId", vetId);
                        cmd.Parameters.AddWithValue("@diagnosis", textBox3.Text);
                        cmd.Parameters.AddWithValue("@prescriptions", textBox4.Text);
                        cmd.Parameters.AddWithValue("@vaccinations", textBox1.Text);
                        cmd.Parameters.AddWithValue("@instructions", textBox5.Text);

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Medical record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            UserMessages.ShowDatabaseError("Could not update medical record", ex);
                        }
                    }
                }
            }

            LoadMedicalRecords();
            ClearForm();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            selectedRecordId = null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);

        }
    }
}
