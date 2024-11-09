using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class AddWorkstationIncharge : Form
    {
        private DatabaseHelper dbHelper;
        private string userId;

        public AddWorkstationIncharge(string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.userId = userId;
            PopulateSystemUnitDropdown();

            string assignedRoom = GetAssignedRoom(userId);
        }

        private string GetAssignedRoom(string userId)
        {
            string query = $"SELECT assigned_room FROM users WHERE user_id = '{userId}'";
            DataTable dt = dbHelper.GetData(query);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["assigned_room"].ToString();
            }
            else
            {
                return "Unassigned";
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(monitorTextbox.Text) ||
                string.IsNullOrWhiteSpace(keyboardTextbox.Text) ||
                string.IsNullOrWhiteSpace(mouseTextbox.Text) ||
                string.IsNullOrWhiteSpace(avrTextbox.Text))
            {
                MessageBox.Show("Please fill in all fields for Monitor, Keyboard, Mouse, and AVR.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string assignedRoom = GetAssignedRoom(userId);

            string monitorQuery = $"SELECT * FROM assets WHERE asset_id = '{monitorTextbox.Text}'";
            string keyboardQuery = $"SELECT * FROM assets WHERE asset_id = '{keyboardTextbox.Text}'";
            string mouseQuery = $"SELECT * FROM assets WHERE asset_id = '{mouseTextbox.Text}'";
            string avrQuery = $"SELECT * FROM assets WHERE asset_id = '{avrTextbox.Text}'";

            DataTable monitorData = dbHelper.GetData(monitorQuery);
            DataTable keyboardData = dbHelper.GetData(keyboardQuery);
            DataTable mouseData = dbHelper.GetData(mouseQuery);
            DataTable avrData = dbHelper.GetData(avrQuery);

            if (!ValidateAsset(monitorData, monitorTextbox.Text, "Monitor", assignedRoom) ||
                !ValidateAsset(keyboardData, keyboardTextbox.Text, "Keyboard", assignedRoom) ||
                !ValidateAsset(mouseData, mouseTextbox.Text, "Mouse", assignedRoom) ||
                !ValidateAsset(avrData, avrTextbox.Text, "AVR", assignedRoom))
            {
                return;
            }

            string newWorkstationId = GenerateNewWorkstationId();

            string insertQuery = $@"
        INSERT INTO workstations 
        (workstation_id, system_unit, room, monitor, keyboard, mouse, avr, added_by, date_added) 
        VALUES 
        ('{newWorkstationId}', '{systemUnit.SelectedValue}', '{assignedRoom}', '{monitorTextbox.Text}', 
        '{keyboardTextbox.Text}', '{mouseTextbox.Text}', '{avrTextbox.Text}', '{userId}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";

            dbHelper.ExecuteQuery(insertQuery);

            // Update the system_units table to set workstation for the selected system unit
            string updateSystemUnitQuery = $@"
        UPDATE system_units 
        SET workstation = '{newWorkstationId}' 
        WHERE system_unit_id = '{systemUnit.SelectedValue}'";

            dbHelper.ExecuteQuery(updateSystemUnitQuery);

            // Update the assets table to set workstation for each selected asset
            string updateMonitorQuery = $"UPDATE assets SET workstation = '{newWorkstationId}' WHERE asset_id = '{monitorTextbox.Text}'";
            string updateKeyboardQuery = $"UPDATE assets SET workstation = '{newWorkstationId}' WHERE asset_id = '{keyboardTextbox.Text}'";
            string updateMouseQuery = $"UPDATE assets SET workstation = '{newWorkstationId}' WHERE asset_id = '{mouseTextbox.Text}'";
            string updateAvrQuery = $"UPDATE assets SET workstation = '{newWorkstationId}' WHERE asset_id = '{avrTextbox.Text}'";

            dbHelper.ExecuteQuery(updateMonitorQuery);
            dbHelper.ExecuteQuery(updateKeyboardQuery);
            dbHelper.ExecuteQuery(updateMouseQuery);
            dbHelper.ExecuteQuery(updateAvrQuery);

            MessageBox.Show("Workstation added successfully and system unit/assets updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK; 
            this.Close(); 
        }

        private string GenerateNewWorkstationId()
        {
            string lastIdQuery = "SELECT workstation_id FROM workstations ORDER BY workstation_id DESC LIMIT 1";
            DataTable dt = dbHelper.GetData(lastIdQuery);

            if (dt.Rows.Count > 0)
            {
                string lastId = dt.Rows[0]["workstation_id"].ToString();
                int lastNumber = int.Parse(lastId.Substring(1)); 
                return $"W{(lastNumber + 1):D3}"; 
            }
            return "W001"; 
        }


        private bool ValidateAsset(DataTable assetData, string assetId, string expectedType, string assignedRoom)
        {
            if (assetData.Rows.Count == 0)
            {
                MessageBox.Show($"The asset '{assetId}' was not found in the assets as a {expectedType} asset.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            DataRow assetRow = assetData.Rows[0];
            string assetType = assetRow["asset_type"].ToString();
            string workstationStatus = assetRow["workstation"].ToString();
            string assetStatus = assetRow["status"].ToString();
            string assetLocation = assetRow["location"].ToString();

            if (!assetType.Equals(expectedType, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"The asset '{assetId}' is not a {expectedType} asset. It is listed as a {assetType} asset.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!workstationStatus.Equals("Unequipped", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"The asset '{assetId}' is already assigned to a workstation.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!assetStatus.Equals("Working", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"The asset '{assetId}' is not available for use. It is marked as '{assetStatus}' and may need replacement or repair.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!assetLocation.Equals(assignedRoom, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"The asset '{assetId}' is located in '{assetLocation}', which does not match the assigned room '{assignedRoom}' for this workstation.", "Location Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void PopulateSystemUnitDropdown()
        {
            string assignedRoom = GetAssignedRoom(userId);

            string query = $@"
        SELECT system_unit_id
        FROM system_units 
        WHERE workstation = 'Unequipped' 
        AND location = '{assignedRoom}'";

            DataTable dt = dbHelper.GetData(query);

            if (dt.Rows.Count > 0)
            {
                systemUnit.DataSource = dt;
                systemUnit.DisplayMember = "system_unit_id"; 
                systemUnit.ValueMember = "system_unit_id";  

                systemUnit.SelectedIndex = 0;
            }
            else
            {
                systemUnit.DataSource = null;
                systemUnit.Items.Clear();
                systemUnit.Items.Add("No available system units");
                systemUnit.SelectedIndex = 0;
            }
        }

    }
}
