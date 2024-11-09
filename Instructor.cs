using Laboratory_Management_System.Controls;
using Laboratory_Management_System.UserControls.AdminControls.Crud;
using Laboratory_Management_System.UserControls.InstructorsControls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laboratory_Management_System
{
    public partial class Instructor : Form
    {
        InstructorNavigationControl navigationControl;
        public string UserId { get; set; }

        public Instructor(string userId)
        {
            InitializeComponent();
            UserId = userId; 
            InitializeNavigationControl();
        }

        private void InitializeNavigationControl()
        {
            Console.WriteLine($"UserId in Instructor: {UserId}"); 
            uc_attendance attendanceControl = new uc_attendance(UserId);
            uc_assets_instructor assetsControl = new uc_assets_instructor(UserId);
            uc_remoteview remoteviewControl = new uc_remoteview(UserId);
            List<UserControl> userControls = new List<UserControl>()
            {
                remoteviewControl,
                attendanceControl,
                assetsControl
            };

            navigationControl = new InstructorNavigationControl(userControls, panel3);
            navigationControl.display(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            navigationControl.display(0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            navigationControl.display(1);
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                UserId = null;
                Login loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            navigationControl.display(2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            EditUserDetails editUserDetails = new EditUserDetails(UserId);

            DialogResult result = editUserDetails.ShowDialog();
            if (result == DialogResult.OK)
            {

            }
        }
    }
}
