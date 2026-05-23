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
            ApplyInterfaceStyle();
            LoadOwners();

        }

        private void LoadOwners()
        {
            try
            {
                string query = "SELECT * FROM owners";
                dataGridView1.DataSource = Database.FillDataTable(query);
                ConfigureOwnersGrid();
            }
            catch (Exception ex)
            {
                UserMessages.ShowDatabaseError("Failed to load owners", ex);
            }
        }

        private void ApplyInterfaceStyle()
        {
            UseWaitCursor = false;
            ClearWaitCursor(this);

            Font = new Font("Segoe UI", 9.8F, FontStyle.Regular);
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.Stretch;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            label1.Text = "First name";
            label6.Text = "Last name";
            label2.Text = "Phone";
            label3.Text = "Email";
            label4.Text = "Street";
            label5.Text = "City";
            label7.Visible = false;
            label8.Visible = false;

            StyleLabel(label1);
            StyleLabel(label2);
            StyleLabel(label3);
            StyleLabel(label4);
            StyleLabel(label5);
            StyleLabel(label6);

            StyleTextBox(textBox1);
            StyleTextBox(textBox2);
            StyleTextBox(textBox3);
            StyleTextBox(textBox4);
            StyleTextBox(textBox5);
            StyleTextBox(textBox6);

            StylePrimaryButton(button2, "Add Owner");
            StyleSecondaryButton(button3, "Update");
            StyleDangerButton(button1, "Delete");

            dataGridView1.Location = new Point(32, 42);
            dataGridView1.Size = new Size(560, 384);

            int labelX = 624;
            int inputX = 624;
            int top = 54;
            int gap = 67;
            PositionField(label1, textBox1, labelX, inputX, top);
            PositionField(label6, textBox5, labelX, inputX, top + gap);
            PositionField(label2, textBox2, labelX, inputX, top + (gap * 2));
            PositionField(label3, textBox4, labelX, inputX, top + (gap * 3));
            PositionField(label4, textBox3, labelX, inputX, top + (gap * 4));
            PositionField(label5, textBox6, labelX, inputX, top + (gap * 5));

            button2.Location = new Point(32, 452);
            button1.Location = new Point(190, 452);
            button3.Location = new Point(348, 452);
            ClientSize = new Size(970, 520);
        }

        private void ConfigureOwnersGrid()
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

            SetColumnHeader(idDataGridViewTextBoxColumn, "ID");
            SetColumnHeader(firstNameDataGridViewTextBoxColumn, "First Name");
            SetColumnHeader(lastNameDataGridViewTextBoxColumn, "Last Name");
            SetColumnHeader(phoneNumberDataGridViewTextBoxColumn, "Phone");
            SetColumnHeader(emailDataGridViewTextBoxColumn, "Email");
            SetColumnHeader(streetDataGridViewTextBoxColumn, "Street");
            SetColumnHeader(cityDataGridViewTextBoxColumn, "City");
        }

        private void ClearWaitCursor(Control parent)
        {
            parent.UseWaitCursor = false;
            foreach (Control child in parent.Controls)
            {
                ClearWaitCursor(child);
            }
        }

        private void PositionField(Label label, TextBox textBox, int labelX, int inputX, int top)
        {
            label.Location = new Point(labelX, top);
            textBox.Location = new Point(inputX, top + 27);
            textBox.Size = new Size(300, 32);
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

        private void StyleDangerButton(Button button, string text)
        {
            StyleButton(button, text, Color.FromArgb(178, 67, 67));
        }

        private void StyleButton(Button button, string text, Color backColor)
        {
            button.Text = text;
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            button.Size = new Size(136, 42);
            button.UseVisualStyleBackColor = false;
        }

        private void SetColumnHeader(DataGridViewColumn column, string headerText)
        {
            column.HeaderText = headerText;
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
                        UserMessages.ShowDatabaseError("Could not add owner", ex);
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
                    UserMessages.ShowDatabaseError("Could not delete owner", ex);
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
                            UserMessages.ShowDatabaseError("Could not update owner", ex);
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
