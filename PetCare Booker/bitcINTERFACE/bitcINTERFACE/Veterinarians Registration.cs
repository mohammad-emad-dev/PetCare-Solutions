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
   

    public partial class Veterinarians_Registration : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int? selectedVetId = null;
        public Veterinarians_Registration()
        {
            InitializeComponent();
        }

        private void Veterinarians_Registration_Load(object sender, EventArgs e)
        {
            LoadVeterinarians();

        }
        private void LoadVeterinarians()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM veterinarians";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load veterinarians' data: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedVetId = Convert.ToInt32(row.Cells["idDataGridViewTextBoxColumn"].Value);
                textBox1.Text = row.Cells["nameDataGridViewTextBoxColumn"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["specialtyDataGridViewTextBoxColumn"].Value.ToString();
                textBox4.Text = row.Cells["contactInfoDataGridViewTextBoxColumn"].Value.ToString();

                // **CHANGE**: Populate textBox2 with the userId
                textBox2.Text = row.Cells["userIdDataGridViewTextBoxColumn"].Value.ToString();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Name and User ID are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Add validation to ensure userId is a valid number
            if (!int.TryParse(textBox2.Text, out int userId))
            {
                MessageBox.Show("User ID must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO veterinarians (name, specialty, contactInfo, userId) VALUES (@name, @specialty, @contactInfo, @userId)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if the user ID is already assigned
                    string checkUserQuery = "SELECT COUNT(*) FROM veterinarians WHERE userId = @userId";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, conn))
                    {
                        // **CHANGE**: Get userId from the textbox
                        checkCmd.Parameters.AddWithValue("@userId", userId);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("This user account is already assigned to another veterinarian.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Proceed with insert
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBox1.Text);
                        cmd.Parameters.AddWithValue("@specialty", comboBox1.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@contactInfo", textBox4.Text);
                        // **CHANGE**: Get userId from the validated variable
                        cmd.Parameters.AddWithValue("@userId", userId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Veterinarian added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding veterinarian: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadVeterinarians();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (selectedVetId == null)
            {
                MessageBox.Show("Please select a veterinarian to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this veterinarian?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkAppointmentsQuery = "SELECT COUNT(*) FROM appointments WHERE vetId = @vetId";
                    using (SqlCommand checkCmd = new SqlCommand(checkAppointmentsQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@vetId", selectedVetId.Value);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("This veterinarian cannot be deleted because they have associated appointments.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string deleteQuery = "DELETE FROM veterinarians WHERE id = @id";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@id", selectedVetId.Value);
                        deleteCmd.ExecuteNonQuery();
                        MessageBox.Show("Veterinarian deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting veterinarian: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadVeterinarians();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (selectedVetId == null)
            {
                MessageBox.Show("Please select a veterinarian to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Add validation to ensure userId is a valid number
            if (!int.TryParse(textBox2.Text, out int userId))
            {
                MessageBox.Show("User ID must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE veterinarians SET name=@name, specialty=@specialty, contactInfo=@contactInfo, userId=@userId WHERE id=@id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", selectedVetId.Value);
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@specialty", comboBox1.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@contactInfo", textBox4.Text);
                    // **CHANGE**: Get userId from the validated variable
                    cmd.Parameters.AddWithValue("@userId", userId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Veterinarian updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating veterinarian: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            LoadVeterinarians();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
