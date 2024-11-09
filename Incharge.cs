using Laboratory_Management_System.Controls;
using Laboratory_Management_System.UserControls.AdminControls;
using Laboratory_Management_System.UserControls.AdminControls.Crud;
using Laboratory_Management_System.UserControls.InstructorsControls;
using Laboratory_Management_System.UserControls.LaboratoryInchargeControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratory_Management_System
{
    public partial class Incharge : Form
    {
        InstructorInchargeNavigationControl navigationControl;
        public string UserId { get; set; }
        public Incharge(string userId)
        {
            InitializeComponent();
            UserId = userId;
            InitializeNavigationControl();
        }

        private void InitializeNavigationControl()
        {
            uc_asset_requests assetReportsControl = new uc_asset_requests(UserId);
            uc_system_units_incharge systemUnitsControl = new uc_system_units_incharge(UserId);
            uc_workstations_incharge workstationsControl = new uc_workstations_incharge(UserId);
            uc_replacement_history replacementHistoryControl = new uc_replacement_history(UserId);
            uc_repair_history repairHistoryControl = new uc_repair_history(UserId);

            List<UserControl> userControls = new List<UserControl>()
            { assetReportsControl, new uc_assets(UserId), systemUnitsControl, replacementHistoryControl, workstationsControl, repairHistoryControl};

            navigationControl = new InstructorInchargeNavigationControl(userControls, panel3);
            navigationControl.display(1);
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

        private void button6_Click(object sender, EventArgs e)
        {
            navigationControl.display(4);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            navigationControl.display(5);
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
