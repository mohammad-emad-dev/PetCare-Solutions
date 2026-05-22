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
    public partial class Users_Form : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;
        private int? selectedUserId = null;
        private string passwordPlaceholder = "Enter new password to change";
        public Users_Form()
        {
            InitializeComponent();
        }

        private void Users_Form_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox3.PasswordChar = '\0'; // Use normal characters for placeholder
            textBox3.Text = passwordPlaceholder;
            textBox3.ForeColor = Color.Gray;

            LoadUsers();

        }
        private void textBox3_Enter(object sender, EventArgs e)
        {
            // When the user clicks in the textbox...
            if (textBox3.Text == passwordPlaceholder)
            {
                textBox3.Text = ""; // Clear the placeholder
                textBox3.ForeColor = Color.Black; // Change text color to normal
                textBox3.PasswordChar = '*'; // Use the password mask character
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            // When the user clicks out of the textbox...
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.PasswordChar = '\0'; // Remove password mask
                textBox3.Text = passwordPlaceholder; // Put the placeholder back
                textBox3.ForeColor = Color.Gray; // Change color back to gray
            }
        }
        // A reusable method to load or refresh user data
        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT id, username, role, dateCreated FROM users";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    HidePasswordColumn();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load users' data: " + ex.Message);
                }
            }
        }

        private void HidePasswordColumn()
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (string.Equals(column.DataPropertyName, "passwordHash", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(column.Name, "passwordHashDataGridViewTextBoxColumn", StringComparison.OrdinalIgnoreCase))
                {
                    column.Visible = false;
                }
            }
        }




        // Event for clicking a cell in the DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedUserId = Convert.ToInt32(row.Cells["idDataGridViewTextBoxColumn"].Value);
                textBox1.Text = selectedUserId.ToString();
                textBox2.Text = row.Cells["usernameDataGridViewTextBoxColumn"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["roleDataGridViewTextBoxColumn"].Value.ToString();
                textBox4.Text = Convert.ToDateTime(row.Cells["dateCreatedDataGridViewTextBoxColumn"].Value).ToString();

                // **CHANGE**: Set the placeholder text and color for the password field
                textBox3.PasswordChar = '\0';
                textBox3.Text = passwordPlaceholder;
                textBox3.ForeColor = Color.Gray;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            // The password cannot be the placeholder text on insert
            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || textBox3.Text == passwordPlaceholder || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Username, Password, and Role are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ... (rest of the insert code is the same as before)
            string query = "INSERT INTO users (username, passwordHash, role) VALUES (@username, @password, @role)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // ... (try-catch and command execution logic remains the same)
                try
                {
                    conn.Open();
                    string checkUserQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", textBox2.Text);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("This username already exists. Please choose another one.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", textBox2.Text);
                        cmd.Parameters.AddWithValue("@password", PasswordHasher.HashPassword(textBox3.Text));
                        cmd.Parameters.AddWithValue("@role", comboBox1.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadUsers();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (selectedUserId == null)
            {
                MessageBox.Show("Please select a user from the list to delete.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string checkVetsQuery = "SELECT COUNT(*) FROM veterinarians WHERE userId = @userId";
                    using (SqlCommand checkCmd = new SqlCommand(checkVetsQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@userId", selectedUserId.Value);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("This user cannot be deleted because they are linked to a veterinarian record.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string deleteQuery = "DELETE FROM users WHERE id = @id";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@id", selectedUserId.Value);
                        deleteCmd.ExecuteNonQuery();
                        MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadUsers();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (selectedUserId == null)
            {
                MessageBox.Show("Please select a user from the list to update.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query;
            SqlCommand cmd;

            // **CHANGE**: Check if the password is not empty AND not the placeholder text
            if (!string.IsNullOrWhiteSpace(textBox3.Text) && textBox3.Text != passwordPlaceholder)
            {
                // Update password
                query = "UPDATE users SET username=@username, role=@role, passwordHash=@password WHERE id=@id";
                cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@password", PasswordHasher.HashPassword(textBox3.Text));
            }
            else
            {
                // Do not update password
                query = "UPDATE users SET username=@username, role=@role WHERE id=@id";
                cmd = new SqlCommand(query);
            }

            cmd.Parameters.AddWithValue("@id", selectedUserId.Value);
            cmd.Parameters.AddWithValue("@username", textBox2.Text);
            cmd.Parameters.AddWithValue("@role", comboBox1.SelectedItem.ToString());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // ... (rest of update logic is the same)
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadUsers();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);

        }
    }
}
