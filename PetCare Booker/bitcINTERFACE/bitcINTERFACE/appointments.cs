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
    public partial class appointments : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        // This variable will hold the ID of the currently selected appointment
        private int? selectedAppointmentId = null;
        public appointments()
        {
            InitializeComponent();
        }

        private void appointments_Load(object sender, EventArgs e)
        {

            // Set the format for the time picker
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "HH:mm"; // Hours and minutes
            dateTimePicker2.ShowUpDown = true;

            // Load all necessary data when the form opens
            LoadAppointments();
            LoadVetsComboBox();
            LoadPetsComboBox(); // Assuming you will use a ComboBox for pets

        }
        private void LoadAppointments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM appointments";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load appointments: " + ex.Message);
                }
            }
        }

        // Method to load veterinarians into their ComboBox (comboBox2)
        private void LoadVetsComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT id, name FROM veterinarians";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "name";
                    comboBox2.ValueMember = "id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load veterinarians: " + ex.Message);
                }
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

            using (SqlConnection conn = new SqlConnection(connectionString))
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
                    MessageBox.Show("Error creating appointment: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                        MessageBox.Show("Error deleting appointment: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            using (SqlConnection conn = new SqlConnection(connectionString))
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
                        MessageBox.Show("Error updating appointment: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
