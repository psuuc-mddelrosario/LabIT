using Laboratory_Management_System.Controls;
using Laboratory_Management_System.UserControls.AdminControls;
using Laboratory_Management_System.UserControls.InstructorsControls;
using Laboratory_Management_System.UserControls.LaboratoryInchargeControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Laboratory_Management_System
{
    public partial class InstructorIncharge : Form
    {
        InstructorInchargeNavigationControl navigationControl;
        public string UserId { get; set; }
        public InstructorIncharge(string userId)
        {
            InitializeComponent();
            UserId = userId; 
            InitializeNavigationControl();
        }

        private void InitializeNavigationControl()
        {
            uc_asset_requests assetReportsControl = new uc_asset_requests(UserId);
            uc_attendance attendanceControl = new uc_attendance(UserId);
            uc_assets assetsControl = new uc_assets(UserId);
            uc_remoteview remoteviewControl = new uc_remoteview(UserId);
            uc_system_units_incharge systemUnitsControl = new uc_system_units_incharge(UserId);
            uc_workstations_incharge workstationsControl = new uc_workstations_incharge(UserId);
            uc_replacement_history replacementHistoryControl = new uc_replacement_history(UserId);

            uc_repair_history repairHistoryControl = new uc_repair_history(UserId);


            List<UserControl> userControls = new List<UserControl>()
            { assetReportsControl, assetsControl, attendanceControl, remoteviewControl, systemUnitsControl, workstationsControl, replacementHistoryControl, repairHistoryControl};

            navigationControl = new InstructorInchargeNavigationControl(userControls, panel3);
            navigationControl.display(2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            navigationControl.display(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            navigationControl.display(0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            navigationControl.display(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            navigationControl.display(3);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InstructorIncharge_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            navigationControl.display(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            navigationControl.display(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            navigationControl.display(5);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            navigationControl.display(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            navigationControl.display(7);
        }
    }
}
