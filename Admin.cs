using Laboratory_Management_System.Controls;
using Laboratory_Management_System.UserControls.AdminControls;
using Laboratory_Management_System.UserControls.AdminControls.Crud;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laboratory_Management_System
{
    public partial class Admin : Form
    {
        AdminNavigationControl navigationControl;
        public string UserId { get; set; }
        public Admin(string userId)
        {
            InitializeComponent();
            UserId = userId;
            InitializeNavigationControl();
        }

        private void InitializeNavigationControl()
        {
            uc_workstations workstationsControl = new uc_workstations(UserId);
            uc_system_units systemUnitsControl = new uc_system_units(UserId);
            uc_assets_inventory assetsControl = new uc_assets_inventory(UserId);
            List<UserControl> userControls = new List<UserControl>()
            {
                workstationsControl,
                assetsControl,
                new uc_users(),
                new uc_students(),
                systemUnitsControl,
                new uc_schedules(),
                new uc_temporary_schedules(),
                new uc_rooms() 
            };

            navigationControl = new AdminNavigationControl(userControls, panel3);
            navigationControl.display(1); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            navigationControl.display(0); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            navigationControl.display(1); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            navigationControl.display(1); 
        }

        public void ShowWorkstationsControl()
        {
            navigationControl.display(0); 
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Admin_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.BringToFront(); 
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

        private void button3_Click(object sender, EventArgs e)
        {
            navigationControl.display(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            navigationControl.display(3);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            navigationControl.display(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            navigationControl.display(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            navigationControl.display(6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            EditUserDetails editUserDetails = new EditUserDetails(UserId);

            DialogResult result = editUserDetails.ShowDialog();
            if (result == DialogResult.OK)
            {
                
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            navigationControl.display(7);
        }
    }
}
