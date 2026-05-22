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
    public partial class Dashboard : Form
    {
        private string currentUserRole;
        private string currentUsername;

        public Dashboard(string userRole, string username) // Added a parameter to accept username  
        {
            InitializeComponent();
            this.currentUserRole = userRole;
            this.currentUsername = username; // Fixed the error by using the passed parameter  
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Owner_registration ownersForm = new Owner_registration();
            ownersForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Veterinarians_Registration vetsForm = new Veterinarians_Registration();
            vetsForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Pets_registration petsForm = new Pets_registration();
            petsForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            appointments appointmentsForm = new appointments();
            appointmentsForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            medical_records medicalForm = new medical_records();
            medicalForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BookingRequests bookingForm = new BookingRequests();
            bookingForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Users_Form usersForm = new Users_Form();
            usersForm.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new login().Show();
            this.Close();
        }

        private void ApplyRolePermissions()
        {
            button7.Visible = false; // owners  
            button1.Visible = false; // veterinarians  
            button2.Visible = false; // pets  
            button3.Visible = false; // appointments  
            button6.Visible = false; // medical_records  
            button5.Visible = false; // BookingRequests  
            button4.Visible = false; // users  

            switch (currentUserRole)
            {
                case "Administrator":
                    button7.Visible = true; // owners  
                    button1.Visible = true; // veterinarians  
                    button2.Visible = true; // pets  
                    button3.Visible = true; // appointments  
                    button6.Visible = true; // medical_records  
                    button5.Visible = true; // BookingRequests  
                    button4.Visible = true; // users  
                    break;

                case "Veterinarian":
                    button6.Visible = true; // medical_records  
                    button3.Visible = true; // appointments  
                    button2.Visible = true; // Pets registration  
                    break;

                case "Nurse":
                    button2.Visible = true; // Pets registration  
                    button7.Visible = true; // owners registration  
                    button6.Visible = true; // medical_records  
                    button5.Visible = true; // BookingRequests  
                    break;

                default:
                    break;
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            ApplyRolePermissions();
            lblWelcome.Text = "Welcome, " + currentUsername;
        }
    }
}
