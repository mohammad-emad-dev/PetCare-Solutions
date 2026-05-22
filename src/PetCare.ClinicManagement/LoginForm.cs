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
    /// <summary>
    /// Login form with enhanced security features.
    /// Implements rate limiting, input validation, and secure password handling.
    /// </summary>
    public partial class LoginForm : Form
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        // ✅ Improvement 2: Rate limiting to prevent brute force attacks
        private int _failedLoginAttempts = 0;
        private DateTime _lastFailedAttempt = DateTime.MinValue;
        private const int MaxFailedAttempts = 5;
        private const int LockoutDurationMinutes = 15;

        public LoginForm()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            SetupInputValidation();
        }

        /// <summary>
        /// Configures input validation for username and password fields
        /// </summary>
        private void SetupInputValidation()
        {
            if (textBox1 != null)
            {
                textBox1.MaxLength = 50;
            }
            if (textBox2 != null)
            {
                textBox2.MaxLength = 100;
            }
        }

        /// <summary>
        /// Validates username format
        /// </summary>
        private bool IsValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username) &&
                   username.Length >= 3 &&
                   username.Length <= 50;
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

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Form load event
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Text changed event
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Text changed event
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            // ✅ Improvement 3: Input validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidUsername(username))
            {
                MessageBox.Show("Please enter a valid username (3-50 characters).", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Improvement 4: Check for account lockout
            if (IsAccountLocked())
            {
                var remainingMinutes = Math.Ceiling(LockoutDurationMinutes - (DateTime.Now - _lastFailedAttempt).TotalMinutes);
                MessageBox.Show($"Account temporarily locked. Please try again in {remainingMinutes} minutes.", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Disable button during login attempt
            button1.Enabled = false;
            button1.Text = "Logging in...";

            try
            {
                // ✅ Improvement 5: Async database operation for better UI responsiveness
                await Task.Run(() =>
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        // SQL query to find the user
                        string query = "SELECT id, passwordHash, role FROM users WHERE username = @username";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // ✅ Improvement 6: Explicit parameter type for SQL injection prevention
                            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username;

                            conn.Open();
                            int? userId = null;
                            string storedPasswordHash = null;
                            string retrievedUserRole = null;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userId = Convert.ToInt32(reader["id"]);
                                    storedPasswordHash = reader["passwordHash"].ToString();
                                    retrievedUserRole = reader["role"].ToString();
                                }
                            }

                            if (userId.HasValue)
                            {
                                bool isPasswordValid = PasswordHasher.VerifyPassword(password, storedPasswordHash, out bool needsRehash);
                                if (isPasswordValid)
                                {
                                    if (needsRehash)
                                    {
                                        UpdatePasswordHash(conn, userId.Value, PasswordHasher.HashPassword(password));
                                    }

                                    // Login successful
                                    string userRole = retrievedUserRole;

                                    this.Invoke(new Action(() =>
                                    {
                                        MessageBox.Show($"Login successful. Welcome, {retrievedUserRole}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        DashboardForm dashboardForm = new DashboardForm(userRole, username);
                                        dashboardForm.Show();
                                        this.Hide();

                                        // Reset failed attempts on success
                                        _failedLoginAttempts = 0;
                                    }));
                                }
                                else
                                {
                                    // Incorrect password
                                    _failedLoginAttempts++;
                                    _lastFailedAttempt = DateTime.Now;

                                    this.Invoke(new Action(() =>
                                    {
                                        MessageBox.Show("Incorrect password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }));
                                }
                            }
                            else
                            {
                                // Username not found
                                _failedLoginAttempts++;
                                _lastFailedAttempt = DateTime.Now;

                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show("Username not found.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // ✅ Improvement 8: Secure error handling - don't expose internal details
                MessageBox.Show("An error occurred during login. Please try again later.", "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                button1.Enabled = true;
                button1.Text = "Login";
            }
        }

        private void UpdatePasswordHash(SqlConnection conn, int userId, string passwordHash)
        {
            string query = "UPDATE users SET passwordHash = @passwordHash WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@passwordHash", SqlDbType.NVarChar, 255).Value = passwordHash;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
