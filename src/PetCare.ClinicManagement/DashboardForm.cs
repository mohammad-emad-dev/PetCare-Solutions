using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace bitcINTERFACE
{
    public partial class DashboardForm : Form
    {
        private string currentUserRole;
        private string currentUsername;

        public DashboardForm(string userRole, string username) // Added a parameter to accept username
        {
            InitializeComponent();
            this.currentUserRole = userRole;
            this.currentUsername = username; // Fixed the error by using the passed parameter
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OwnerRegistrationForm ownersForm = new OwnerRegistrationForm();
            ownersForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VeterinarianRegistrationForm vetsForm = new VeterinarianRegistrationForm();
            vetsForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PetRegistrationForm petsForm = new PetRegistrationForm();
            petsForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AppointmentsForm appointmentsForm = new AppointmentsForm();
            appointmentsForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MedicalRecordsForm medicalForm = new MedicalRecordsForm();
            medicalForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BookingRequestsForm bookingForm = new BookingRequestsForm();
            bookingForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UsersForm usersForm = new UsersForm();
            usersForm.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Close();
        }

        private void ApplyRolePermissions()
        {
            button7.Visible = false; // owners
            button1.Visible = false; // veterinarians
            button2.Visible = false; // pets
            button3.Visible = false; // appointments
            button6.Visible = false; // MedicalRecordsForm
            button5.Visible = false; // BookingRequests
            button4.Visible = false; // users

            switch (currentUserRole)
            {
                case "Administrator":
                    button7.Visible = true; // owners
                    button1.Visible = true; // veterinarians
                    button2.Visible = true; // pets
                    button3.Visible = true; // appointments
                    button6.Visible = true; // MedicalRecordsForm
                    button5.Visible = true; // BookingRequests
                    button4.Visible = true; // users
                    break;

                case "Veterinarian":
                    button6.Visible = true; // MedicalRecordsForm
                    button3.Visible = true; // appointments
                    button2.Visible = true; // Pets registration
                    break;

                case "Nurse":
                    button2.Visible = true; // Pets registration
                    button7.Visible = true; // owners registration
                    button6.Visible = true; // MedicalRecordsForm
                    button5.Visible = true; // BookingRequests
                    break;

                default:
                    break;
            }
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            ApplyRolePermissions();
            lblWelcome.Text = "Welcome, " + currentUsername;
            ArrangeVisibleButtons();
        }

        private void ArrangeVisibleButtons()
        {
            Button[] actionButtons = { button7, button1, button2, button3, button6, button5, button4 };
            int index = 0;

            foreach (Button button in actionButtons)
            {
                if (!button.Visible)
                {
                    continue;
                }

                int column = index % 2;
                int row = index / 2;
                button.Location = new Point(62 + (column * 246), 178 + (row * 74));
                index++;
            }
        }
    }
}
