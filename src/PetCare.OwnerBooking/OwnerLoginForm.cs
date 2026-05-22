using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace BookingRequests_System
{
    /// <summary>
    /// Login form for pet owners using phone number authentication.
    /// Implements security best practices and performance optimizations.
    /// </summary>
    public partial class OwnerLoginForm : Form
    {

        // ✅ Improvement 2: Rate limiting to prevent brute force attacks
        private int _failedLoginAttempts = 0;
        private DateTime _lastFailedAttempt = DateTime.MinValue;
        private const int MaxFailedAttempts = 5;
        private const int LockoutDurationMinutes = 15;

        public OwnerLoginForm()
        {
            InitializeComponent();
            SetupPhoneValidation();
        }

        /// <summary>
        /// Configures phone number input validation
        /// </summary>
        private void SetupPhoneValidation()
        {
            if (textBox1 != null)
            {
                textBox1.MaxLength = 15;
                textBox1.KeyPress += TextBox1_KeyPress;
            }
        }

        /// <summary>
        /// Handles phone number input validation - only digits allowed
        /// </summary>
        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Validates phone number format
        /// </summary>
        private bool IsValidPhoneNumber(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) &&
                   phone.Length >= 10 &&
                   phone.Length <= 15 &&
                   long.TryParse(phone, out _);
        }

        /// <summary>
        /// Checks if account is locked due to multiple failed attempts
        /// </summary>
        private bool IsAccountLocked()
        {
            if (_failedLoginAttempts >= MaxFailedAttempts)
            {
                var timeSinceLastAttempt = DateTime.Now - _lastFailedAttempt;
                if (timeSinceLastAttempt.TotalMinutes < LockoutDurationMinutes)
                {
                    return true;
                }
                // Reset after lockout period
                _failedLoginAttempts = 0;
            }
            return false;
        }

        /// <summary>
        /// Handles the login button click event.
        /// Validates the phone number and navigates to booking details if found.
        /// </summary>
        private async void button1_Click(object sender, EventArgs e)
        {
            // ✅ Improvement 3: Input validation
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter your phone number.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidPhoneNumber(textBox1.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid phone number (10-15 digits).", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Improvement 4: Check for account lockout
            if (IsAccountLocked())
            {
                var remainingMinutes = Math.Ceiling(LockoutDurationMinutes - (DateTime.Now - _lastFailedAttempt).TotalMinutes);
                MessageBox.Show($"Account temporarily locked. Please try again in {remainingMinutes} minutes.", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button1.Enabled = false;
            button1.Text = "Logging in...";

            try
            {
                // ✅ Improvement 5: Async database operation for better UI responsiveness
                await System.Threading.Tasks.Task.Run(() =>
                {
                    using (SqlConnection conn = Database.CreateConnection())
                    {
                        string query = "SELECT id FROM owners WHERE phoneNumber = @phone";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // ✅ Improvement 6: Explicit parameter type for SQL injection prevention
                            cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 20).Value = textBox1.Text.Trim();

                            conn.Open();
                            object result = cmd.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                // Owner found - open the booking details form
                                int ownerId = Convert.ToInt32(result);
                                this.Invoke(new Action(() =>
                                {
                                    BookingDetailsForm detailsForm = new BookingDetailsForm(ownerId);
                                    detailsForm.Show();
                                    this.Hide();
                                    _failedLoginAttempts = 0; // Reset on success
                                }));
                            }
                            else
                            {
                                // ✅ Improvement 7: Track failed attempts
                                _failedLoginAttempts++;
                                _lastFailedAttempt = DateTime.Now;

                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show("Phone number not found. Please check your number and try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // ✅ Improvement 8: Secure error handling - don't expose internal details
                MessageBox.Show("An error occurred during login. Please try again later.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                // Log error internally (implement logging mechanism)
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                button1.Enabled = true;
                button1.Text = "Login";
            }
        }

        private void OwnerLoginForm_Load(object sender, EventArgs e)
        {
            // Form load event - reserved for future use
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Text changed event - reserved for future use
        }
    }
}
