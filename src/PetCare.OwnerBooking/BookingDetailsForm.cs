using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BookingRequests_System
{
    /// <summary>
    /// Form for submitting pet care booking requests.
    /// Implements security best practices and performance optimizations.
    /// </summary>
    public partial class BookingDetailsForm : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        private readonly int currentOwnerId;

        /// <summary>
        /// Initializes the booking details form with the owner's ID.
        /// </summary>
        /// <param name="ownerId">The ID of the logged-in pet owner.</param>
        public BookingDetailsForm(int ownerId)
        {
            InitializeComponent();
            currentOwnerId = ownerId;
        }

        /// <summary>
        /// Handles form load event - configures controls and loads pet data.
        /// </summary>
        private async void BookingDetailsForm_Load(object sender, EventArgs e)
        {
            // Configure the time picker format
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "hh:mm tt"; // e.g., 02:30 PM
            dateTimePicker2.ShowUpDown = true;

            // Set minimum date to today
            dateTimePicker1.MinDate = DateTime.Today;

            // ✅ Improvement 2: Async loading for better UI responsiveness
            await LoadMyPetsAsync();
        }

        /// <summary>
        /// Loads the pet list for the current owner into the combo box asynchronously.
        /// </summary>
        private async System.Threading.Tasks.Task LoadMyPetsAsync()
        {
            try
            {
                // Show loading state
                comboBox1.Enabled = false;
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Loading pets...");
                comboBox1.SelectedIndex = 0;

                await System.Threading.Tasks.Task.Run(() =>
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        // ✅ Improvement 3: Optimized query with explicit column selection
                        string query = "SELECT id, name FROM pets WHERE ownerId = @ownerId ORDER BY name";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // ✅ Improvement 4: Explicit parameter type for SQL injection prevention
                            cmd.Parameters.Add("@ownerId", SqlDbType.Int).Value = currentOwnerId;

                            conn.Open();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                var pets = new DataTable();
                                pets.Load(reader);

                                this.Invoke(new Action(() =>
                                {
                                    comboBox1.DataSource = null;
                                    comboBox1.Items.Clear();

                                    if (pets.Rows.Count > 0)
                                    {
                                        comboBox1.DataSource = pets;
                                        comboBox1.DisplayMember = "name";
                                        comboBox1.ValueMember = "id";
                                    }
                                    else
                                    {
                                        comboBox1.Items.Add("No pets registered");
                                        comboBox1.SelectedIndex = 0;
                                        comboBox1.Enabled = false;
                                    }
                                }));
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // ✅ Improvement 5: Secure error handling
                MessageBox.Show("Failed to load your pets. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Load pets error: {ex.Message}");

                this.Invoke(new Action(() =>
                {
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("Error loading pets");
                    comboBox1.SelectedIndex = 0;
                }));
            }
            finally
            {
                comboBox1.Enabled = true;
            }
        }

        /// <summary>
        /// Validates the booking request before submission
        /// </summary>
        private bool ValidateBookingRequest()
        {
            // ✅ Improvement 6: Comprehensive validation
            if (comboBox1.SelectedValue == null || comboBox1.SelectedItem?.ToString() == "No pets registered" || comboBox1.SelectedItem?.ToString() == "Error loading pets")
            {
                MessageBox.Show("Please select a pet.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            DateTime requestedDate = dateTimePicker1.Value.Date;
            TimeSpan requestedTime = dateTimePicker2.Value.TimeOfDay;

            // Check if date is in the past
            if (requestedDate < DateTime.Today)
            {
                MessageBox.Show("Cannot book appointments in the past.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if time is within business hours (e.g., 8 AM - 8 PM)
            if (requestedTime.TotalHours < 8 || requestedTime.TotalHours >= 20)
            {
                MessageBox.Show("Please select a time between 8:00 AM and 8:00 PM.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if it's a weekend (optional - adjust based on business needs)
            if (requestedDate.DayOfWeek == DayOfWeek.Friday || requestedDate.DayOfWeek == DayOfWeek.Saturday)
            {
                var result = MessageBox.Show("Weekend appointments may have limited availability. Continue?", "Weekend Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks for existing bookings to prevent duplicates
        /// </summary>
        private async System.Threading.Tasks.Task<bool> CheckDuplicateBookingAsync(int petId, DateTime date, TimeSpan time)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // ✅ Improvement 7: Check for duplicate bookings
                    string query = @"SELECT COUNT(*) FROM BookingRequests
                                    WHERE PetID = @petId AND RequestedDate = @reqDate AND RequestedTime = @reqTime
                                    AND Status IN ('Pending', 'Confirmed')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@petId", SqlDbType.Int).Value = petId;
                        cmd.Parameters.Add("@reqDate", SqlDbType.Date).Value = date;
                        cmd.Parameters.Add("@reqTime", SqlDbType.Time).Value = time;

                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            });
        }

        /// <summary>
        /// Handles the submit booking request button click.
        /// </summary>
        private async void button1_Click(object sender, EventArgs e)
        {
            // ✅ Improvement 8: Validate input
            if (!ValidateBookingRequest())
            {
                return;
            }

            button1.Enabled = false;
            button1.Text = "Submitting...";

            try
            {
                int petId = Convert.ToInt32(comboBox1.SelectedValue);
                DateTime requestedDate = dateTimePicker1.Value.Date;
                TimeSpan requestedTime = dateTimePicker2.Value.TimeOfDay;

                // ✅ Improvement 9: Check for duplicate bookings
                bool isDuplicate = await CheckDuplicateBookingAsync(petId, requestedDate, requestedTime);
                if (isDuplicate)
                {
                    MessageBox.Show("You already have a pending or confirmed booking at this date and time.", "Duplicate Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Improvement 10: Async database operation
                await System.Threading.Tasks.Task.Run(() =>
                {
                    string query = @"INSERT INTO BookingRequests (OwnerID, PetID, RequestedDate, RequestedTime, Status)
                                    VALUES (@ownerId, @petId, @reqDate, @reqTime, 'Pending')";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // ✅ Improvement 11: Explicit parameter types
                            cmd.Parameters.Add("@ownerId", SqlDbType.Int).Value = currentOwnerId;
                            cmd.Parameters.Add("@petId", SqlDbType.Int).Value = petId;
                            cmd.Parameters.Add("@reqDate", SqlDbType.Date).Value = requestedDate;
                            cmd.Parameters.Add("@reqTime", SqlDbType.Time).Value = requestedTime;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                });

                MessageBox.Show("Your booking request has been sent successfully!", "Request Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                // ✅ Improvement 12: Secure error handling
                MessageBox.Show("Error sending request. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Booking error: {ex.Message}");
            }
            finally
            {
                button1.Enabled = true;
                button1.Text = "Submit Request";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Selection changed event - reserved for future use
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Date picker value changed - reserved for future use
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            // Time picker value changed - reserved for future use
        }
    }
}
