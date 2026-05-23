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


    public partial class VeterinarianRegistrationForm : Form
    {
        private int? selectedVetId = null;
        public VeterinarianRegistrationForm()
        {
            InitializeComponent();
        }

        private void VeterinarianRegistrationForm_Load(object sender, EventArgs e)
        {
            ApplyInterfaceStyle();
            LoadVeterinarians();

        }
        private void LoadVeterinarians()
        {
            try
            {
                string query = "SELECT * FROM veterinarians";
                dataGridView1.DataSource = Database.FillDataTable(query);
                ConfigureVeterinariansGrid();
            }
            catch (Exception ex)
            {
                UserMessages.ShowDatabaseError("Failed to load veterinarians", ex);
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

            label1.Text = "Name";
            label5.Text = "Specialty";
            label4.Text = "Contact";
            label3.Text = "User ID";

            StyleLabel(label1);
            StyleLabel(label3);
            StyleLabel(label4);
            StyleLabel(label5);
            StyleTextBox(textBox1);
            StyleTextBox(textBox2);
            StyleTextBox(textBox4);
            comboBox1.Font = new Font("Segoe UI", 10.4F);

            StylePrimaryButton(button1, "Add Vet");
            StyleDangerButton(button3, "Delete");
            StyleSecondaryButton(button2, "Update");

            dataGridView1.Location = new Point(32, 42);
            dataGridView1.Size = new Size(548, 384);

            int labelX = 616;
            int inputX = 616;
            int top = 76;
            int gap = 72;
            PositionField(label1, textBox1, labelX, inputX, top);
            PositionField(label5, comboBox1, labelX, inputX, top + gap);
            PositionField(label4, textBox4, labelX, inputX, top + (gap * 2));
            PositionField(label3, textBox2, labelX, inputX, top + (gap * 3));

            button1.Location = new Point(32, 452);
            button3.Location = new Point(190, 452);
            button2.Location = new Point(348, 452);
            ClientSize = new Size(920, 520);
        }

        private void ConfigureVeterinariansGrid()
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
            SetColumnHeader(nameDataGridViewTextBoxColumn, "Name");
            SetColumnHeader(specialtyDataGridViewTextBoxColumn, "Specialty");
            SetColumnHeader(contactInfoDataGridViewTextBoxColumn, "Contact");
            SetColumnHeader(userIdDataGridViewTextBoxColumn, "User ID");
        }

        private void PositionField(Label label, Control control, int labelX, int inputX, int top)
        {
            label.Location = new Point(labelX, top);
            control.Location = new Point(inputX, top + 27);
            control.Size = new Size(276, 32);
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

            using (SqlConnection conn = Database.CreateConnection())
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
                    UserMessages.ShowDatabaseError("Could not add veterinarian", ex);
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

            using (SqlConnection conn = Database.CreateConnection())
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
                    UserMessages.ShowDatabaseError("Could not delete veterinarian", ex);
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

            using (SqlConnection conn = Database.CreateConnection())
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
                        UserMessages.ShowDatabaseError("Could not update veterinarian", ex);
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
