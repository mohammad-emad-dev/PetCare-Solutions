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
    public partial class BookingRequests : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        // This will store the details of the currently selected request
        private DataGridViewRow selectedRequestRow = null;

        public BookingRequests()
        {
            InitializeComponent();
        }

        private void BookingRequests_Load(object sender, EventArgs e)
        {
            LoadVetsComboBox(); // Load vets into the dropdown

            // Set the default filter to "Pending" and load the initial data
            comboBox1.SelectedItem = "Pending";
            LoadBookingRequests("Pending");

        }
        private void LoadBookingRequests(string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM BookingRequests WHERE Status = @status";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@status", status);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load booking requests: " + ex.Message);
                }
            }
        }

        // Method to load veterinarians into comboBox2
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


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem != null)
            {
                LoadBookingRequests(comboBox1.SelectedItem.ToString());
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRequestRow = dataGridView1.Rows[e.RowIndex];
                DisplayRequestDetails();
            }
        }

        private void DisplayRequestDetails()
        {
            if (selectedRequestRow == null) return;

            // Clear previous details
            groupBox1.Controls.Clear();

            // Get IDs from the selected row
            int ownerId = Convert.ToInt32(selectedRequestRow.Cells["ownerIDDataGridViewTextBoxColumn"].Value);
            int petId = Convert.ToInt32(selectedRequestRow.Cells["petIDDataGridViewTextBoxColumn"].Value);

            string ownerName = GetOwnerName(ownerId);
            string petName = GetPetName(petId);

            // Create and add labels dynamically to show the info
            Label lblOwner = new Label { Text = "Owner: " + ownerName, Location = new Point(10, 20), AutoSize = true };
            Label lblPet = new Label { Text = "Pet: " + petName, Location = new Point(10, 40), AutoSize = true };
            Label lblDate = new Label { Text = "Date: " + Convert.ToDateTime(selectedRequestRow.Cells["requestedDateDataGridViewTextBoxColumn"].Value).ToShortDateString(), Location = new Point(10, 60), AutoSize = true };

            groupBox1.Controls.Add(lblOwner);
            groupBox1.Controls.Add(lblPet);
            groupBox1.Controls.Add(lblDate);
        }

        // Helper methods to get names from IDs
        private string GetOwnerName(int id) { /* Database lookup code here */ return "Owner " + id; }
        private string GetPetName(int id) { /* Database lookup code here */ return "Pet " + id; }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (selectedRequestRow == null || comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Please select a request and a veterinarian.", "Validation Error", MessageBoxButtons.OK);
                return;
            }

            // Get all necessary data from the controls
            int requestId = Convert.ToInt32(selectedRequestRow.Cells["requestIDDataGridViewTextBoxColumn"].Value);
            int petId = Convert.ToInt32(selectedRequestRow.Cells["petIDDataGridViewTextBoxColumn"].Value);
            int vetId = Convert.ToInt32(comboBox2.SelectedValue);
            DateTime appDate = Convert.ToDateTime(selectedRequestRow.Cells["requestedDateDataGridViewTextBoxColumn"].Value);
            TimeSpan appTime = (TimeSpan)selectedRequestRow.Cells["requestedTimeDataGridViewTextBoxColumn"].Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Start a transaction

                try
                {
                    // 1. Check for scheduling conflicts before proceeding
                    string checkConflictQuery = "SELECT COUNT(*) FROM appointments WHERE vetId = @vetId AND appointmentDate = @appDate AND appointmentTime = @appTime";
                    using (SqlCommand checkCmd = new SqlCommand(checkConflictQuery, conn, transaction))
                    {
                        checkCmd.Parameters.AddWithValue("@vetId", vetId);
                        checkCmd.Parameters.AddWithValue("@appDate", appDate.Date);
                        checkCmd.Parameters.AddWithValue("@appTime", appTime);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("This veterinarian is already booked at the selected date and time.", "Scheduling Conflict", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            transaction.Rollback(); // Cancel the transaction
                            return;
                        }
                    }

                    // 2. Insert into appointments table
                    string insertAppointmentQuery = "INSERT INTO appointments (appointmentDate, appointmentTime, typeOfService, status, petId, vetId) VALUES (@appDate, @appTime, @service, @status, @petId, @vetId)";
                    using (SqlCommand insertCmd = new SqlCommand(insertAppointmentQuery, conn, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@appDate", appDate.Date);
                        insertCmd.Parameters.AddWithValue("@appTime", appTime);
                        insertCmd.Parameters.AddWithValue("@service", "Consultation"); // Default service type
                        insertCmd.Parameters.AddWithValue("@status", "Scheduled");
                        insertCmd.Parameters.AddWithValue("@petId", petId);
                        insertCmd.Parameters.AddWithValue("@vetId", vetId);
                        insertCmd.ExecuteNonQuery();
                    }

                    // 3. Update the booking request status
                    string updateRequestQuery = "UPDATE BookingRequests SET Status = 'Approved' WHERE RequestID = @reqId";
                    using (SqlCommand updateCmd = new SqlCommand(updateRequestQuery, conn, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@reqId", requestId);
                        updateCmd.ExecuteNonQuery();
                    }

                    // If both commands succeed, commit the transaction
                    transaction.Commit();
                    MessageBox.Show("Request approved and appointment created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // If any error occurs, roll back all changes
                    transaction.Rollback();
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Refresh the grid to show the updated list of pending requests
            LoadBookingRequests("Pending");
            groupBox1.Controls.Clear(); // Clear details
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (selectedRequestRow == null)
            {
                MessageBox.Show("Please select a request to reject.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int requestId = Convert.ToInt32(selectedRequestRow.Cells["requestIDDataGridViewTextBoxColumn"].Value);

            string query = "UPDATE BookingRequests SET Status = 'Rejected' WHERE RequestID = @reqId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@reqId", requestId);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Request has been rejected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error rejecting request: " + ex.Message);
                    }
                }
            }

            // Refresh the grid
            LoadBookingRequests("Pending");
            groupBox1.Controls.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);

        }
    }
}
