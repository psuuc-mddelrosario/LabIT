using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class AddSystemUnits : Form
    {
        private DatabaseHelper dbHelper;
        private string userId;

        public AddSystemUnits(string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            PopulateOperatingSystemDropdown();
            PopulateRoomsDropdown();
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
            // Get the values from the input fields
            string hostnameValue = hostname.Text;
            string osValue = operatingSystem.SelectedItem.ToString();
            string motherboardValue = motherboard.Text;
            string cpuValue = cpu.Text;
            string ramValue = ram.Text;
            string gpuValue = gpu.Text;
            string psuValue = psu.Text;
            string systemUnitCaseValue = system_unit_case.Text;
            string storageValue = storage.Text;
            string locationValue = roomDropdown.SelectedValue.ToString();

            // Database helper instance
            DatabaseHelper dbHelper = new DatabaseHelper();

            // Check if all required fields are filled
            if (string.IsNullOrEmpty(hostnameValue) || string.IsNullOrEmpty(osValue) ||
                (osValue != "Mac" && (string.IsNullOrEmpty(motherboardValue) || string.IsNullOrEmpty(cpuValue) ||
                string.IsNullOrEmpty(ramValue) || string.IsNullOrEmpty(gpuValue) ||
                string.IsNullOrEmpty(psuValue) || string.IsNullOrEmpty(systemUnitCaseValue) ||
                string.IsNullOrEmpty(storageValue))))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Step 1: Get the last inserted system_unit_id and generate a new one
            string query = "SELECT system_unit_id FROM system_units ORDER BY system_unit_id DESC LIMIT 1";
            DataTable lastSystemUnit = dbHelper.GetData(query);
            string newSystemUnitId = "SU01";

            if (lastSystemUnit.Rows.Count > 0)
            {
                string lastId = lastSystemUnit.Rows[0]["system_unit_id"].ToString();
                int idNumber = int.Parse(lastId.Substring(2)) + 1;
                newSystemUnitId = "SU" + idNumber.ToString("D2");
            }

            // Step 2: Validate asset IDs and ensure they belong to the same location
            List<string> assetIds = new List<string> { motherboardValue, cpuValue, ramValue, gpuValue, psuValue, systemUnitCaseValue, storageValue };

            foreach (var assetId in assetIds)
            {
                if (!string.IsNullOrEmpty(assetId))
                {
                    string checkAssetQuery = $"SELECT workstation, location FROM assets WHERE asset_id = '{assetId}'";
                    DataTable assetResult = dbHelper.GetData(checkAssetQuery);

                    if (assetResult.Rows.Count == 0)
                    {
                        MessageBox.Show($"Asset with ID {assetId} does not exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (assetResult.Rows[0]["workstation"].ToString() != "Unequipped")
                    {
                        MessageBox.Show($"Asset with ID {assetId} is already equipped in another system.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (assetResult.Rows[0]["location"].ToString() != locationValue)
                    {
                        MessageBox.Show($"Asset with ID {assetId} is not in the selected room {locationValue}. All assets must be in the same room.", "Location Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            // Step 3: Insert new system unit record
            string insertQuery = $@"
        INSERT INTO system_units (system_unit_id, hostname, operating_system, motherboard, cpu, ram, gpu, psu, system_unit_case, storage, location, added_by)
        VALUES ('{newSystemUnitId}', '{hostnameValue}', '{osValue}', '{motherboardValue}', '{cpuValue}', '{ramValue}', '{gpuValue}', '{psuValue}', '{systemUnitCaseValue}', '{storageValue}', '{locationValue}', '{userId}')
    ";

            try
            {
                dbHelper.ExecuteQuery(insertQuery);

                // Step 4: Update the system_unit column for the assets
                foreach (var assetId in assetIds)
                {
                    if (!string.IsNullOrEmpty(assetId))
                    {
                        string updateAssetQuery = $"UPDATE assets SET system_unit = '{newSystemUnitId}' WHERE asset_id = '{assetId}'";
                        dbHelper.ExecuteQuery(updateAssetQuery);
                    }
                }

                //MessageBox.Show("System Unit added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding system unit: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PopulateRoomsDropdown()
        {
            string query = "SELECT laboratory_room FROM rooms";
            DataTable roomsTable = dbHelper.GetData(query);

            DataTable combinedRoomsTable = new DataTable();
            combinedRoomsTable.Columns.Add("laboratory_room");

            foreach (DataRow row in roomsTable.Rows)
            {
                combinedRoomsTable.ImportRow(row);
            }

            roomDropdown.DataSource = combinedRoomsTable;
            roomDropdown.DisplayMember = "laboratory_room";
            roomDropdown.ValueMember = "laboratory_room";
        }
    }
}
