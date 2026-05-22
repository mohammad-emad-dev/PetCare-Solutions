using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace bitcINTERFACE
{
    public partial class PetRegistrationForm : Form
    {
        private int? selectedPetId = null; // To store the ID of the selected pet


        public PetRegistrationForm()
        {
            InitializeComponent();


        }

        private void PetRegistrationForm_Load(object sender, EventArgs e)
        {
            LoadPets();

        }

        private void LoadPets()
        {
            try
            {
                string query = "SELECT * FROM pets";
                dataGridView1.DataSource = Database.FillDataTable(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load pets' data: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || comboBox1.SelectedItem == null || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please fill in Name, Species, and Owner ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO pets (name, species, breed, dateOfBirth, Gender, medicalNotes, ownerId) VALUES (@name, @species, @breed, @dateOfBirth, @gender, @medicalNotes, @ownerId)";

            using (SqlConnection conn = Database.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@species", comboBox1.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@breed", textBox3.Text);
                    cmd.Parameters.AddWithValue("@dateOfBirth", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@gender", radioButton1.Checked ? "Male" : "Female");
                    cmd.Parameters.AddWithValue("@medicalNotes", textBox4.Text);
                    cmd.Parameters.AddWithValue("@ownerId", Convert.ToInt32(textBox2.Text));
                    // NO @petId here for an INSERT

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Pet added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding pet: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            LoadPets(); // Refresh grid
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedPetId == null)
            {
                MessageBox.Show("Please select a pet from the list to delete.", "No Pet Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this pet?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            using (SqlConnection conn = Database.CreateConnection())
            {
                try
                {
                    conn.Open();
                    // This check is very good!
                    string checkQuery = "SELECT COUNT(*) FROM appointments WHERE petId = @petId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@petId", selectedPetId.Value);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("This pet cannot be deleted because it has appointments.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string deleteQuery = "DELETE FROM pets WHERE id = @petId";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        // The ONLY parameter needed for DELETE is the ID
                        cmd.Parameters.AddWithValue("@petId", selectedPetId.Value);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Pet deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting pet: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadPets(); // Refresh grid
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedPetId == null)
            {
                MessageBox.Show("Please select a pet from the list to update.", "No Pet Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // The UPDATE query now includes medicalNotes
            string query = "UPDATE pets SET name=@name, species=@species, breed=@breed, dateOfBirth=@dateOfBirth, Gender=@gender, medicalNotes=@medicalNotes, ownerId=@ownerId WHERE id=@petId";

            using (SqlConnection conn = Database.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@petId", selectedPetId.Value);
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@species", comboBox1.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@breed", textBox3.Text);
                    cmd.Parameters.AddWithValue("@dateOfBirth", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@gender", radioButton1.Checked ? "Male" : "Female");
                    cmd.Parameters.AddWithValue("@medicalNotes", textBox4.Text); // This now matches the query
                    cmd.Parameters.AddWithValue("@ownerId", Convert.ToInt32(textBox2.Text));

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Pet updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating pet: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            LoadPets(); // Refresh grid
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedPetId = Convert.ToInt32(row.Cells["idDataGridViewTextBoxColumn"].Value);

                // Populate TextBoxes
                textBox1.Text = row.Cells["nameDataGridViewTextBoxColumn"].Value.ToString();
                textBox3.Text = row.Cells["breedDataGridViewTextBoxColumn"].Value.ToString();
                textBox4.Text = row.Cells["medicalNotesDataGridViewTextBoxColumn"].Value.ToString();
                textBox2.Text = row.Cells["ownerIdDataGridViewTextBoxColumn"].Value.ToString();

                // Populate ComboBox
                comboBox1.SelectedItem = row.Cells["speciesDataGridViewTextBoxColumn"].Value.ToString();

                // Populate DateTimePicker
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["dateOfBirthDataGridViewTextBoxColumn"].Value);

                // Populate RadioButtons
                string gender = row.Cells["genderDataGridViewTextBoxColumn"].Value.ToString();
                if (gender.Equals("Male", StringComparison.OrdinalIgnoreCase))
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        // Add this event handler for comboBox2
        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            // Handle the event if needed
        }
    }
}
