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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace bitcINTERFACE
{
    public partial class MedicalRecordsForm : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private Tuple<int, int> selectedRecordId = null; // (petId, recordId)

        public MedicalRecordsForm()
        {
            InitializeComponent();
        }

        private void MedicalRecordsForm_Load(object sender, EventArgs e)
        {
            LoadMedicalRecords();
            LoadPetsComboBox();

        }

        private void LoadMedicalRecords()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM MedicalRecordsForm";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load medical records: " + ex.Message);
                }
            }
        }

        // Method to load pet names into comboBox1
        private void LoadPetsComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT id, name FROM pets";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "name";
                    comboBox1.ValueMember = "id";
                    comboBox1.SelectedIndex = -1; // Start with no pet selected
                    groupBox1.Controls.Clear(); // Clear the info box initially
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load pets: " + ex.Message);
                }
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
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                            MessageBox.Show("Error fetching pet details: " + ex.Message);
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
                string query = "INSERT INTO MedicalRecordsForm (petId, diagnosis, prescriptions, vaccinationsGiven, followupInstructions, vetId) VALUES (@petId, @diagnosis, @prescriptions, @vaccinations, @instructions, @vetId)";
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                            MessageBox.Show("Error saving record: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                // UPDATE LOGIC
                string query = "UPDATE MedicalRecordsForm SET petId=@newPetId, diagnosis=@diagnosis, prescriptions=@prescriptions, vaccinationsGiven=@vaccinations, followupInstructions=@instructions, vetId=@vetId WHERE petId=@oldPetId AND id=@recordId";
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                            MessageBox.Show("Error updating record: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
