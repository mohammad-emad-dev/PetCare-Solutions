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
    public partial class OwnerRegistrationForm : Form
    {

        // This variable will hold the ID of the currently selected owner for updates and deletes
        private int? selectedOwnerId = null;

        public OwnerRegistrationForm()
        {
            InitializeComponent();
        }

        private void OwnerRegistrationForm_Load(object sender, EventArgs e)
        {
            LoadOwners();

        }

        private void LoadOwners()
        {
            try
            {
                string query = "SELECT * FROM owners";
                dataGridView1.DataSource = Database.FillDataTable(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load owners' data: " + ex.Message);
            }
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Basic validation to ensure required fields are not empty
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("First Name and Last Name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO owners (firstName, lastName, phoneNumber, email, street, city) VALUES (@firstName, @lastName, @phoneNumber, @email, @street, @city)";

            using (SqlConnection conn = Database.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to prevent SQL Injection
                    cmd.Parameters.AddWithValue("@firstName", textBox1.Text);
                    cmd.Parameters.AddWithValue("@lastName", textBox5.Text);
                    cmd.Parameters.AddWithValue("@phoneNumber", textBox2.Text);
                    cmd.Parameters.AddWithValue("@email", textBox4.Text);
                    cmd.Parameters.AddWithValue("@street", textBox3.Text);
                    cmd.Parameters.AddWithValue("@city", textBox6.Text);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Owner added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding owner: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            LoadOwners(); // Refresh the grid to show the new owner

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if an owner has been selected
            if (selectedOwnerId == null)
            {
                MessageBox.Show("Please select an owner from the list to delete.", "No Owner Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmation dialog
            if (MessageBox.Show("Are you sure you want to delete this owner?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            using (SqlConnection conn = Database.CreateConnection())
            {
                try
                {
                    conn.Open();

                    // IMPORTANT: Check if the owner has any pets before deleting
                    string checkPetsQuery = "SELECT COUNT(*) FROM pets WHERE ownerId = @ownerId";
                    using (SqlCommand checkCmd = new SqlCommand(checkPetsQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ownerId", selectedOwnerId.Value);
                        int petCount = (int)checkCmd.ExecuteScalar();
                        if (petCount > 0)
                        {
                            MessageBox.Show("This owner cannot be deleted because they have associated pets. Please reassign or delete the pets first.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // If no pets are found, proceed with deletion
                    string deleteQuery = "DELETE FROM owners WHERE id = @id";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@id", selectedOwnerId.Value);
                        deleteCmd.ExecuteNonQuery();
                        MessageBox.Show("Owner deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting owner: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Clear the textboxes and refresh the grid
            ClearForm();
            LoadOwners();
        }
        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            selectedOwnerId = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            {
                // Check if an owner has been selected
                if (selectedOwnerId == null)
                {
                    MessageBox.Show("Please select an owner from the list to update.", "No Owner Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "UPDATE owners SET firstName=@firstName, lastName=@lastName, phoneNumber=@phoneNumber, email=@email, street=@street, city=@city WHERE id=@id";

                using (SqlConnection conn = Database.CreateConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@id", selectedOwnerId.Value);
                        cmd.Parameters.AddWithValue("@firstName", textBox1.Text);
                        cmd.Parameters.AddWithValue("@lastName", textBox5.Text);
                        cmd.Parameters.AddWithValue("@phoneNumber", textBox2.Text);
                        cmd.Parameters.AddWithValue("@email", textBox4.Text);
                        cmd.Parameters.AddWithValue("@street", textBox3.Text);
                        cmd.Parameters.AddWithValue("@city", textBox6.Text);

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Owner updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating owner: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                LoadOwners(); // Refresh the grid with the updated data
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure a valid row is clicked (not the header)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Store the selected owner's ID
                selectedOwnerId = Convert.ToInt32(row.Cells["idDataGridViewTextBoxColumn"].Value);

                // Fill the textboxes with the data from the selected row
                textBox1.Text = row.Cells["firstNameDataGridViewTextBoxColumn"].Value.ToString();
                textBox5.Text = row.Cells["lastNameDataGridViewTextBoxColumn"].Value.ToString();
                textBox2.Text = row.Cells["phoneNumberDataGridViewTextBoxColumn"].Value.ToString();
                textBox4.Text = row.Cells["emailDataGridViewTextBoxColumn"].Value.ToString();
                textBox3.Text = row.Cells["streetDataGridViewTextBoxColumn"].Value.ToString();
                textBox6.Text = row.Cells["cityDataGridViewTextBoxColumn"].Value.ToString();
            }

    }
    }
}
