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
            ApplyInterfaceStyle();
            LoadPets();

        }

        private void LoadPets()
        {
            try
            {
                string query = "SELECT * FROM pets";
                dataGridView1.DataSource = Database.FillDataTable(query);
                ConfigurePetsGrid();
            }
            catch (Exception ex)
            {
                UserMessages.ShowDatabaseError("Failed to load pets", ex);
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
            label4.Text = "Species";
            label5.Text = "Breed";
            label2.Text = "Birth date";
            label3.Text = "Gender";
            label6.Text = "Medical notes";
            label7.Text = "Owner ID";

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
            comboBox1.Font = new Font("Segoe UI", 10.4F);
            dateTimePicker1.Font = new Font("Segoe UI", 10.4F);
            radioButton1.Font = new Font("Segoe UI", 10.2F);
            radioButton2.Font = new Font("Segoe UI", 10.2F);
            radioButton1.BackColor = Color.Transparent;
            radioButton2.BackColor = Color.Transparent;

            StylePrimaryButton(button1, "Add Pet");
            StyleDangerButton(button2, "Delete");
            StyleSecondaryButton(button3, "Update");

            dataGridView1.Location = new Point(32, 42);
            dataGridView1.Size = new Size(548, 384);

            int labelX = 616;
            int inputX = 616;
            int top = 48;
            int gap = 59;
            PositionField(label1, textBox1, labelX, inputX, top);
            PositionField(label4, comboBox1, labelX, inputX, top + gap);
            PositionField(label5, textBox3, labelX, inputX, top + (gap * 2));
            PositionField(label2, dateTimePicker1, labelX, inputX, top + (gap * 3));

            label3.Location = new Point(labelX, top + (gap * 4));
            radioButton1.Location = new Point(inputX, top + (gap * 4) + 28);
            radioButton2.Location = new Point(inputX + 110, top + (gap * 4) + 28);

            PositionField(label6, textBox4, labelX, inputX, top + (gap * 5));
            PositionField(label7, textBox2, labelX, inputX, top + (gap * 6));

            button1.Location = new Point(32, 452);
            button2.Location = new Point(190, 452);
            button3.Location = new Point(348, 452);
            ClientSize = new Size(930, 520);
        }

        private void ConfigurePetsGrid()
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
            SetColumnHeader(speciesDataGridViewTextBoxColumn, "Species");
            SetColumnHeader(breedDataGridViewTextBoxColumn, "Breed");
            SetColumnHeader(dateOfBirthDataGridViewTextBoxColumn, "Birth Date");
            SetColumnHeader(medicalNotesDataGridViewTextBoxColumn, "Medical Notes");
            SetColumnHeader(ownerIdDataGridViewTextBoxColumn, "Owner ID");
            SetColumnHeader(genderDataGridViewTextBoxColumn, "Gender");
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
                        UserMessages.ShowDatabaseError("Could not add pet", ex);
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
                    UserMessages.ShowDatabaseError("Could not delete pet", ex);
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
                        UserMessages.ShowDatabaseError("Could not update pet", ex);
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
