using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class AddSystemUnitsIncharge : Form
    {
        private DatabaseHelper dbHelper;
        private string userId;

        public AddSystemUnitsIncharge(string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            PopulateOperatingSystemDropdown();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.userId = userId;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulateOperatingSystemDropdown()
        {
            var operatingSystems = new List<string>
        {
            "Windows",         
            "Linux"
        };

            operatingSystem.DataSource = operatingSystems;
        }

        private void add_Click(object sender, EventArgs e)
        {
            string hostnameValue = hostname.Text;
            string osValue = operatingSystem.SelectedItem.ToString();
            string motherboardValue = motherboard.Text;
            string cpuValue = cpu.Text;
            string ramValue = ram.Text;
            string gpuValue = gpu.Text;
            string psuValue = psu.Text;
            string systemUnitCaseValue = system_unit_case.Text;
            string storageValue = storage.Text;
            string locationValue = GetAssignedRoom(userId);

            DatabaseHelper dbHelper = new DatabaseHelper();

            if (string.IsNullOrEmpty(hostnameValue) || string.IsNullOrEmpty(osValue) ||
                (osValue != "Mac" && (string.IsNullOrEmpty(motherboardValue) || string.IsNullOrEmpty(cpuValue) ||
                string.IsNullOrEmpty(ramValue) || string.IsNullOrEmpty(gpuValue) ||
                string.IsNullOrEmpty(psuValue) || string.IsNullOrEmpty(systemUnitCaseValue) ||
                string.IsNullOrEmpty(storageValue))))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Step 1: Check if the hostname is unique
            string checkHostnameQuery = $"SELECT COUNT(*) FROM system_units WHERE hostname = '{hostnameValue}'";
            DataTable hostnameResult = dbHelper.GetData(checkHostnameQuery);

            if (hostnameResult.Rows.Count > 0 && int.Parse(hostnameResult.Rows[0][0].ToString()) > 0)
            {
                MessageBox.Show($"Hostname '{hostnameValue}' already exists. Please enter a unique hostname.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Step 2: Get the last inserted system_unit_id and generate a new one
            string query = "SELECT system_unit_id FROM system_units ORDER BY system_unit_id DESC LIMIT 1";
            DataTable lastSystemUnit = dbHelper.GetData(query);
            string newSystemUnitId = "SU01";

            if (lastSystemUnit.Rows.Count > 0)
            {
                string lastId = lastSystemUnit.Rows[0]["system_unit_id"].ToString();
                int idNumber = int.Parse(lastId.Substring(2)) + 1;
                newSystemUnitId = "SU" + idNumber.ToString("D2");
            }

            // Step 3: Validate asset IDs and ensure they belong to the same location
            List<(string assetId, string assetType)> assetEntries = new List<(string, string)>
    {
        (motherboardValue, "MB"),
        (cpuValue, "C"),
        (ramValue, "R"),
        (gpuValue, "G"),
        (psuValue, "P"),
        (systemUnitCaseValue, "SUC"),
        (storageValue, "S")
    };

            foreach (var (assetId, expectedPrefix) in assetEntries)
            {
                if (!string.IsNullOrEmpty(assetId))
                {
                    // Check if asset ID exists
                    string checkAssetQuery = $"SELECT workstation, location, system_unit FROM assets WHERE asset_id = '{assetId}'";
                    DataTable assetResult = dbHelper.GetData(checkAssetQuery);

                    if (assetResult.Rows.Count == 0)
                    {
                        MessageBox.Show($"Asset with ID {assetId} does not exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (assetResult.Rows[0]["location"].ToString() != locationValue)
                    {
                        MessageBox.Show($"Asset with ID {assetId} is not in the selected room {locationValue}. All assets must be in the same room.", "Location Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (assetResult.Rows[0]["system_unit"].ToString() != "Unequipped")
                    {
                        MessageBox.Show($"Asset with ID {assetId} is already assigned to a system unit. Please use an unequipped asset.", "Asset Already Equipped", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Check if asset ID has the correct prefix
                    if (!assetId.StartsWith(expectedPrefix))
                    {
                        MessageBox.Show($"Asset ID {assetId} is not valid for the expected type. Expected prefix: {expectedPrefix}.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            // Step 4: Insert new system unit record
            string insertQuery = $@"
    INSERT INTO system_units (system_unit_id, hostname, operating_system, motherboard, cpu, ram, gpu, psu, system_unit_case, storage, location, added_by)
    VALUES ('{newSystemUnitId}', '{hostnameValue}', '{osValue}', '{motherboardValue}', '{cpuValue}', '{ramValue}', '{gpuValue}', '{psuValue}', '{systemUnitCaseValue}', '{storageValue}', '{locationValue}', '{userId}')";

            try
            {
                dbHelper.ExecuteQuery(insertQuery);

                // Step 5: Update the system_unit column for the assets
                foreach (var (assetId, _) in assetEntries)
                {
                    if (!string.IsNullOrEmpty(assetId))
                    {
                        string updateAssetQuery = $"UPDATE assets SET system_unit = '{newSystemUnitId}' WHERE asset_id = '{assetId}'";
                        dbHelper.ExecuteQuery(updateAssetQuery);
                    }
                }

                MessageBox.Show("System Unit added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding system unit: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}
