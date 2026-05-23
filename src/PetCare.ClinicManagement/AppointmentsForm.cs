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
    public partial class AppointmentsForm : Form
    {

        // This variable will hold the ID of the currently selected appointment
        private int? selectedAppointmentId = null;
        public AppointmentsForm()
        {
            InitializeComponent();
        }

        private void AppointmentsForm_Load(object sender, EventArgs e)
        {
            ApplyInterfaceStyle();
            // Set the format for the time picker
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "HH:mm"; // Hours and minutes
            dateTimePicker2.ShowUpDown = true;
            dateTimePicker2.Visible = true;
            dateTimePicker2.MinDate = DateTime.Today;
            dateTimePicker2.MaxDate = DateTime.Today.AddDays(1);
            dateTimePicker2.Value = DateTime.Today.AddHours(9);

            // Load all necessary data when the form opens
            LoadAppointments();
            LoadVetsComboBox();
            LoadPetsComboBox(); // Assuming you will use a ComboBox for pets

        }
        private void LoadAppointments()
        {
            try
            {
                string query = "SELECT * FROM appointments";
                dataGridView1.DataSource = Database.FillDataTable(query);
                ConfigureAppointmentsGrid();
            }
            catch (Exception ex)
            {
                UserMessages.ShowDatabaseError("Failed to load appointments", ex);
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

            label7.Text = "Appointment ID";
            label1.Text = "Date";
            label2.Text = "Time";
            label6.Text = "Service";
            label5.Text = "Status";
            label4.Text = "Pet ID";
            label3.Text = "Veterinarian";
            label8.Visible = false;

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
            comboBox2.Font = new Font("Segoe UI", 10.4F);
            comboBox4.Font = new Font("Segoe UI", 10.4F);
            dateTimePicker1.Font = new Font("Segoe UI", 10.4F);
            dateTimePicker2.Font = new Font("Segoe UI", 10.4F);

            StylePrimaryButton(button1, "Add Appointment");
            StyleDangerButton(button2, "Delete");
            StyleSecondaryButton(button3, "Update");

            dataGridView1.Location = new Point(32, 42);
            dataGridView1.Size = new Size(548, 384);

            int labelX = 616;
            int inputX = 616;
            int top = 46;
            int gap = 59;
            PositionField(label7, textBox1, labelX, inputX, top);
            PositionField(label1, dateTimePicker1, labelX, inputX, top + gap);
            PositionField(label2, dateTimePicker2, labelX, inputX, top + (gap * 2));
            PositionField(label6, textBox3, labelX, inputX, top + (gap * 3));
            PositionField(label5, comboBox4, labelX, inputX, top + (gap * 4));
            PositionField(label4, textBox2, labelX, inputX, top + (gap * 5));
            PositionField(label3, comboBox2, labelX, inputX, top + (gap * 6));

            button1.Location = new Point(32, 452);
            button2.Location = new Point(214, 452);
            button3.Location = new Point(396, 452);
            ClientSize = new Size(910, 520);
        }

        private void ConfigureAppointmentsGrid()
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
            SetColumnHeader(appointmentDateDataGridViewTextBoxColumn, "Date");
            SetColumnHeader(appointmentTimeDataGridViewTextBoxColumn, "Time");
            SetColumnHeader(typeOfServiceDataGridViewTextBoxColumn, "Service");
            SetColumnHeader(statusDataGridViewTextBoxColumn, "Status");
            SetColumnHeader(petIdDataGridViewTextBoxColumn, "Pet ID");
            SetColumnHeader(vetIdDataGridViewTextBoxColumn, "Vet ID");
        }

        private void PositionField(Label label, Control control, int labelX, int inputX, int top)
        {
            label.Location = new Point(labelX, top);
            control.Location = new Point(inputX, top + 27);
            control.Size = new Size(260, 32);
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
            button.Size = new Size(158, 42);
            button.UseVisualStyleBackColor = false;
        }

        private void SetColumnHeader(DataGridViewColumn column, string headerText)
        {
            column.HeaderText = headerText;
        }

        // Method to load veterinarians into their ComboBox (comboBox2)
        private void LoadVetsComboBox()
        {
            try
            {
                string query = "SELECT id, name FROM veterinarians";
                DataTable dt = Database.FillDataTable(query);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "name";
                comboBox2.ValueMember = "id";
            }
            catch (Exception ex)
            {
                UserMessages.ShowDatabaseError("Failed to load veterinarians", ex);
            }
        }
        private void LoadPetsComboBox()
        {
            // Note: Your designer uses textBox2 for Pet ID.
            // For a better UX, a ComboBox is recommended. I am providing the code for it.
            // If you want to use a ComboBox for pets, add one and name it 'comboBox1'.
            // For now, this method is not called, but is here for your future use.
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the user clicked on a valid row (not the header)
            if (e.RowIndex >= 0)
            {
                // Get the row that was clicked
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Store the selected appointment's ID for later use in Update and Delete operations
                selectedAppointmentId = Convert.ToInt32(row.Cells["idDataGridViewTextBoxColumn"].Value);

                // Display the appointment ID in the first textbox
                textBox1.Text = selectedAppointmentId.ToString();

                // Check if the date field is not null before populating it
                if (row.Cells["appointmentDateDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(row.Cells["appointmentDateDataGridViewTextBoxColumn"].Value);
                }

                // Check if the time field is not null before populating it
                if (row.Cells["appointmentTimeDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    // The value comes from the database as a TimeSpan, so it must be added to today's date to be displayed correctly
                    DateTime computedValue = DateTime.Today + (TimeSpan)row.Cells["appointmentTimeDataGridViewTextBoxColumn"].Value;
                    if (computedValue >= dateTimePicker2.MinDate && computedValue <= dateTimePicker2.MaxDate)
                    {
                        dateTimePicker2.Value = computedValue;
                    }
                    else
                    {
                        MessageBox.Show("The selected time is out of range.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Check if the service type field is not null
                object serviceValue = row.Cells["typeOfServiceDataGridViewTextBoxColumn"].Value;
                textBox3.Text = (serviceValue == DBNull.Value) ? "" : serviceValue.ToString();

                // Check if the status field is not null
                object statusValue = row.Cells["statusDataGridViewTextBoxColumn"].Value;
                if (statusValue != DBNull.Value)
                {
                    comboBox4.SelectedItem = statusValue.ToString();
                }
                else
                {
                    comboBox4.SelectedIndex = -1; // If the value is null, select nothing
                }

                // Check if the Pet ID field is not null
                object petIdValue = row.Cells["petIdDataGridViewTextBoxColumn"].Value;
                textBox2.Text = (petIdValue == DBNull.Value) ? "" : petIdValue.ToString();

                // Check if the Vet ID field is not null
                object vetIdValue = row.Cells["vetIdDataGridViewTextBoxColumn"].Value;
                if (vetIdValue != DBNull.Value)
                {
                    comboBox2.SelectedValue = vetIdValue;
                }
                else
                {
                    comboBox2.SelectedIndex = -1; // If the value is null, select nothing
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO appointments (appointmentDate, appointmentTime, typeOfService, status, petId, vetId) VALUES (@appDate, @appTime, @service, @status, @petId, @vetId)";

            using (SqlConnection conn = Database.CreateConnection())
            {
                try
                {
                    conn.Open();

                    // Check for scheduling conflicts
                    string checkConflictQuery = "SELECT COUNT(*) FROM appointments WHERE vetId = @vetId AND appointmentDate = @appDate AND appointmentTime = @appTime";
                    using (SqlCommand checkCmd = new SqlCommand(checkConflictQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@vetId", comboBox2.SelectedValue);
                        checkCmd.Parameters.AddWithValue("@appDate", dateTimePicker1.Value.Date);
                        checkCmd.Parameters.AddWithValue("@appTime", dateTimePicker2.Value.TimeOfDay);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("This veterinarian is already booked at the selected date and time.", "Scheduling Conflict", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // If no conflict, proceed with insertion
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@appDate", dateTimePicker1.Value.Date);
                        cmd.Parameters.AddWithValue("@appTime", dateTimePicker2.Value.TimeOfDay);
                        cmd.Parameters.AddWithValue("@service", textBox3.Text);
                        cmd.Parameters.AddWithValue("@status", comboBox4.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@petId", Convert.ToInt32(textBox2.Text));
                        cmd.Parameters.AddWithValue("@vetId", comboBox2.SelectedValue);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Appointment created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    UserMessages.ShowDatabaseError("Could not create appointment", ex);
                }
            }
            LoadAppointments();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (selectedAppointmentId == null)
            {
                MessageBox.Show("Please select an appointment to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            string query = "DELETE FROM appointments WHERE id = @id";
            using (SqlConnection conn = Database.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", selectedAppointmentId.Value);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Appointment deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        UserMessages.ShowDatabaseError("Could not delete appointment", ex);
                    }
                }
            }
            LoadAppointments();
        }


        private void button3_Click(object sender, EventArgs e)
        {

            if (selectedAppointmentId == null)
            {
                MessageBox.Show("Please select an appointment to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE appointments SET appointmentDate=@appDate, appointmentTime=@appTime, typeOfService=@service, status=@status, petId=@petId, vetId=@vetId WHERE id=@id";

            using (SqlConnection conn = Database.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", selectedAppointmentId.Value);
                    cmd.Parameters.AddWithValue("@appDate", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@appTime", dateTimePicker2.Value.TimeOfDay);
                    cmd.Parameters.AddWithValue("@service", textBox3.Text);
                    cmd.Parameters.AddWithValue("@status", comboBox4.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@petId", Convert.ToInt32(textBox2.Text));
                    cmd.Parameters.AddWithValue("@vetId", comboBox2.SelectedValue);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Appointment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        UserMessages.ShowDatabaseError("Could not update appointment", ex);
                    }
                }
            }
            LoadAppointments();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);

        }
    }
}
