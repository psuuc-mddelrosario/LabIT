using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class EditSystemUnitsAdmin : Form
    {
        private DatabaseHelper dbHelper;
        private string systemUnitId;
        private string userId;

        public EditSystemUnitsAdmin(string systemUnitId, string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            PopulateOperatingSystemDropdown();
            PopulateRoomsDropdown();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.systemUnitId = systemUnitId;

            // Load the system unit data when the form is initialized
            LoadSystemUnitData(systemUnitId);
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

        private void LoadSystemUnitData(string systemUnitId)
        {
            string query = $@"
                SELECT hostname, operating_system, motherboard, cpu, ram, gpu, psu, system_unit_case, storage, location
                FROM system_units
                WHERE system_unit_id = '{systemUnitId}'";

            DataTable systemUnitData = dbHelper.GetData(query);

            if (systemUnitData.Rows.Count > 0)
            {
                DataRow row = systemUnitData.Rows[0];

                hostname.Text = row["hostname"].ToString();
                operatingSystem.SelectedItem = row["operating_system"].ToString();
                motherboard.Text = row["motherboard"].ToString();
                cpu.Text = row["cpu"].ToString();
                ram.Text = row["ram"].ToString();
                gpu.Text = row["gpu"].ToString();
                psu.Text = row["psu"].ToString();
                system_unit_case.Text = row["system_unit_case"].ToString();
                storage.Text = row["storage"].ToString();
                roomDropdown.SelectedValue = row["location"].ToString();
            }
            else
            {
                MessageBox.Show("No data found for the selected system unit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(systemUnitId))
            {
                MessageBox.Show("Please select a system unit to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
            string replacedBy = userId;

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

            string originalLocation = "";
            string originalLocationQuery = $"SELECT location, motherboard, cpu, ram, gpu, psu, system_unit_case, storage, workstation FROM system_units WHERE system_unit_id = '{systemUnitId}'";
            DataTable originalSystemUnitData = dbHelper.GetData(originalLocationQuery);

            string originalWorkstation = "";
            Dictionary<string, string> currentAssets = new Dictionary<string, string>();

            if (originalSystemUnitData.Rows.Count > 0)
            {
                originalLocation = originalSystemUnitData.Rows[0]["location"].ToString();
                originalWorkstation = originalSystemUnitData.Rows[0]["workstation"].ToString();
                currentAssets["motherboard"] = originalSystemUnitData.Rows[0]["motherboard"].ToString();
                currentAssets["cpu"] = originalSystemUnitData.Rows[0]["cpu"].ToString();
                currentAssets["ram"] = originalSystemUnitData.Rows[0]["ram"].ToString();
                currentAssets["gpu"] = originalSystemUnitData.Rows[0]["gpu"].ToString();
                currentAssets["psu"] = originalSystemUnitData.Rows[0]["psu"].ToString();
                currentAssets["system_unit_case"] = originalSystemUnitData.Rows[0]["system_unit_case"].ToString();
                currentAssets["storage"] = originalSystemUnitData.Rows[0]["storage"].ToString();
            }

            if (originalLocation != locationValue)
            {
                string updateAssetsLocationQuery = $"UPDATE assets SET location = '{locationValue}' WHERE system_unit = '{systemUnitId}'";
                dbHelper.ExecuteQuery(updateAssetsLocationQuery);
            }

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
                    else if (assetResult.Rows[0]["location"].ToString() != locationValue && originalLocation == locationValue)
                    {
                        MessageBox.Show($"Asset with ID {assetId} is not in the selected room {locationValue}. All assets must be in the same room.", "Location Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            string unequipAssetsQuery = $"UPDATE assets SET system_unit = 'Unequipped' WHERE system_unit = '{systemUnitId}'";
            dbHelper.ExecuteQuery(unequipAssetsQuery);

            string updateQuery = $@"
    UPDATE system_units 
    SET hostname = '{hostnameValue}', 
        operating_system = '{osValue}', 
        motherboard = '{motherboardValue}', 
        cpu = '{cpuValue}', 
        ram = '{ramValue}', 
        gpu = '{gpuValue}', 
        psu = '{psuValue}', 
        system_unit_case = '{systemUnitCaseValue}', 
        storage = '{storageValue}', 
        location = '{locationValue}', 
        last_updated = NOW()
    WHERE system_unit_id = '{systemUnitId}'";

            try
            {
                dbHelper.ExecuteQuery(updateQuery);

                foreach (var assetType in currentAssets.Keys)
                {
                    string newAssetId = assetIds[currentAssets.Keys.ToList().IndexOf(assetType)];
                    string currentAssetId = currentAssets[assetType];

                    if (!string.IsNullOrEmpty(newAssetId) && currentAssetId != newAssetId)
                    {
                        string updateAssetQuery = $"UPDATE assets SET location = '{locationValue}', system_unit = '{systemUnitId}' WHERE asset_id = '{newAssetId}'";
                        dbHelper.ExecuteQuery(updateAssetQuery);

                        string insertHistoryQuery = $@"
                    INSERT INTO replacement_history 
                    (system_unit_id, workstation_id, room, current_asset, new_asset, requested_by, processed_by)
                    VALUES 
                    ('{systemUnitId}', '{originalWorkstation}', '{locationValue}', '{currentAssetId}', '{newAssetId}', 'N/A', '{replacedBy}')";
                        dbHelper.ExecuteQuery(insertHistoryQuery);

                    }
                    else if (!string.IsNullOrEmpty(newAssetId))
                    {
                        string updateAssetQuery = $"UPDATE assets SET system_unit = '{systemUnitId}' WHERE asset_id = '{newAssetId}'";
                        dbHelper.ExecuteQuery(updateAssetQuery);
                    }
                }

                MessageBox.Show("System Unit updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating system unit: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
