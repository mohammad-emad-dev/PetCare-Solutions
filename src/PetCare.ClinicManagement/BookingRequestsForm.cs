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
    public partial class BookingRequestsForm : Form
    {

        // This will store the details of the currently selected request
        private DataGridViewRow selectedRequestRow = null;

        public BookingRequestsForm()
        {
            InitializeComponent();
        }

        private void BookingRequestsForm_Load(object sender, EventArgs e)
        {
            ApplyInterfaceStyle();
            LoadVetsComboBox(); // Load vets into the dropdown

            // Set the default filter to "Pending" and load the initial data
            comboBox1.SelectedItem = "Pending";
            LoadBookingRequests("Pending");

        }
        private void LoadBookingRequests(string status)
        {
            try
            {
                string query = "SELECT * FROM BookingRequests WHERE Status = @status";
                dataGridView1.DataSource = Database.FillDataTable(
                    query,
                    parameters => parameters.AddWithValue("@status", status));
                ConfigureBookingRequestsGrid();
            }
            catch (Exception ex)
            {
                UserMessages.ShowDatabaseError("Failed to load booking requests", ex);
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

            label1.Text = "Status";
            label2.Text = "Request details";
            label3.Text = "Veterinarian";
            StyleLabel(label1);
            StyleLabel(label2);
            StyleLabel(label3);
            comboBox1.Font = new Font("Segoe UI", 10.4F);
            comboBox2.Font = new Font("Segoe UI", 10.4F);
            groupBox1.Font = new Font("Segoe UI", 9.5F);
            groupBox1.BackColor = Color.Transparent;

            StylePrimaryButton(button1, "Approve");
            StyleDangerButton(button2, "Reject");

            dataGridView1.Location = new Point(32, 42);
            dataGridView1.Size = new Size(548, 348);

            label1.Location = new Point(616, 50);
            comboBox1.Location = new Point(616, 78);
            comboBox1.Size = new Size(250, 32);

            label2.Location = new Point(616, 134);
            groupBox1.Location = new Point(616, 162);
            groupBox1.Size = new Size(250, 116);

            label3.Location = new Point(616, 306);
            comboBox2.Location = new Point(616, 334);
            comboBox2.Size = new Size(250, 32);

            button1.Location = new Point(32, 420);
            button2.Location = new Point(212, 420);
            ClientSize = new Size(910, 490);
        }

        private void ConfigureBookingRequestsGrid()
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

            SetColumnHeader(requestIDDataGridViewTextBoxColumn, "Request ID");
            SetColumnHeader(ownerIDDataGridViewTextBoxColumn, "Owner ID");
            SetColumnHeader(petIDDataGridViewTextBoxColumn, "Pet ID");
            SetColumnHeader(requestedDateDataGridViewTextBoxColumn, "Date");
            SetColumnHeader(requestedTimeDataGridViewTextBoxColumn, "Time");
            SetColumnHeader(statusDataGridViewTextBoxColumn, "Status");
        }

        private void StyleLabel(Label label)
        {
            label.BackColor = Color.Transparent;
            label.Font = new Font("Segoe UI", 9.8F, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(34, 58, 94);
        }

        private void StylePrimaryButton(Button button, string text)
        {
            StyleButton(button, text, Color.FromArgb(0, 96, 173));
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
            button.Size = new Size(150, 42);
            button.UseVisualStyleBackColor = false;
        }

        private void SetColumnHeader(DataGridViewColumn column, string headerText)
        {
            column.HeaderText = headerText;
        }

        // Method to load veterinarians into comboBox2
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

            using (SqlConnection conn = Database.CreateConnection())
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
                    UserMessages.ShowDatabaseError("Could not approve booking request", ex);
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
            using (SqlConnection conn = Database.CreateConnection())
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
                        UserMessages.ShowDatabaseError("Could not reject booking request", ex);
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
