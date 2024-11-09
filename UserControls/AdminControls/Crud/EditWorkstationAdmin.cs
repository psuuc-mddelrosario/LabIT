using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class EditWorkstationAdmin : Form
    {
        private DatabaseHelper dbHelper;
        private string workstationId;
        private string userId;

        public EditWorkstationAdmin(string workstationId, string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.userId = userId;
            this.workstationId = workstationId;
            LoadWorkstationDetails();
            PopulateSystemUnitDropdown();
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

            string assignedRoom = roomsDropdown.SelectedValue.ToString();

            // Retrieve current assets and system unit associated with this workstation
            string currentAssetsQuery = $@"
    SELECT monitor, keyboard, mouse, avr, system_unit 
    FROM workstations 
    WHERE workstation_id = '{workstationId}'";
            DataTable currentAssetsData = dbHelper.GetData(currentAssetsQuery);

            if (currentAssetsData.Rows.Count == 0)
            {
                MessageBox.Show("Workstation not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow currentAssets = currentAssetsData.Rows[0];
            string oldSystemUnitId = currentAssets["system_unit"].ToString();

            if (!ValidateAsset(dbHelper.GetData($"SELECT * FROM assets WHERE asset_id = '{monitorTextbox.Text}'"), monitorTextbox.Text, "Monitor", assignedRoom, currentAssets["monitor"].ToString()) ||
                !ValidateAsset(dbHelper.GetData($"SELECT * FROM assets WHERE asset_id = '{keyboardTextbox.Text}'"), keyboardTextbox.Text, "Keyboard", assignedRoom, currentAssets["keyboard"].ToString()) ||
                !ValidateAsset(dbHelper.GetData($"SELECT * FROM assets WHERE asset_id = '{mouseTextbox.Text}'"), mouseTextbox.Text, "Mouse", assignedRoom, currentAssets["mouse"].ToString()) ||
                !ValidateAsset(dbHelper.GetData($"SELECT * FROM assets WHERE asset_id = '{avrTextbox.Text}'"), avrTextbox.Text, "AVR", assignedRoom, currentAssets["avr"].ToString()))
            {
                return;
            }

            // Update workstation with new assets
            string updateWorkstationQuery = $@"
    UPDATE workstations 
    SET monitor = '{monitorTextbox.Text}', 
        keyboard = '{keyboardTextbox.Text}', 
        mouse = '{mouseTextbox.Text}', 
        avr = '{avrTextbox.Text}', 
        system_unit = '{systemUnit.SelectedValue}' 
    WHERE workstation_id = '{workstationId}'";
            dbHelper.ExecuteQuery(updateWorkstationQuery);

            // Update the old system unit to "Unequipped" if it has changed
            if (!string.IsNullOrEmpty(oldSystemUnitId) && oldSystemUnitId != systemUnit.SelectedValue.ToString())
            {
                string updateOldSystemUnitQuery = $"UPDATE system_units SET workstation = 'Unequipped' WHERE system_unit_id = '{oldSystemUnitId}'";
                dbHelper.ExecuteQuery(updateOldSystemUnitQuery);
            }

            // Update new system unit's workstation to this workstation ID
            string updateNewSystemUnitQuery = $"UPDATE system_units SET workstation = '{workstationId}' WHERE system_unit_id = '{systemUnit.SelectedValue}'";
            dbHelper.ExecuteQuery(updateNewSystemUnitQuery);

            // Update old assets to "Unequipped" and assign new assets only if they are changed
            string[] oldAssets = { currentAssets["monitor"].ToString(), currentAssets["keyboard"].ToString(), currentAssets["mouse"].ToString(), currentAssets["avr"].ToString() };
            string[] newAssets = { monitorTextbox.Text, keyboardTextbox.Text, mouseTextbox.Text, avrTextbox.Text };

            for (int i = 0; i < oldAssets.Length; i++)
            {
                string oldAsset = oldAssets[i];
                string newAsset = newAssets[i];

                if (!oldAsset.Equals(newAsset, StringComparison.OrdinalIgnoreCase)) 
                {
                    string updateOldAssetQuery = $"UPDATE assets SET workstation = 'Unequipped' WHERE asset_id = '{oldAsset}'";
                    dbHelper.ExecuteQuery(updateOldAssetQuery);

                    string updateNewAssetQuery = $"UPDATE assets SET workstation = '{workstationId}' WHERE asset_id = '{newAsset}'";
                    dbHelper.ExecuteQuery(updateNewAssetQuery);

                    InsertReplacementHistory(oldSystemUnitId, workstationId, assignedRoom, oldAsset, newAsset);
                }
            }

            MessageBox.Show("Workstation updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private bool ValidateAsset(DataTable assetData, string assetId, string expectedType, string assignedRoom, string currentAssetId)
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

            if (!assetId.Equals(currentAssetId, StringComparison.OrdinalIgnoreCase) &&
                !workstationStatus.Equals("Unequipped", StringComparison.OrdinalIgnoreCase))
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


        private void PopulateSystemUnitDropdown(string selectedUnitId = null)
        {
            // Get the assigned room of the current workstation
            string assignedRoom = GetAssignedRoom();

            string currentSystemUnitId = systemUnit.SelectedValue?.ToString() ?? selectedUnitId;

            string query = $@"
SELECT system_unit_id
FROM system_units 
WHERE location = '{assignedRoom}'
AND (workstation IS NULL OR workstation = 'Unequipped' OR system_unit_id = '{currentSystemUnitId}')";

            DataTable dt = dbHelper.GetData(query);
            Console.WriteLine($"Rows returned: {dt.Rows.Count}");

            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(currentSystemUnitId) && !dt.AsEnumerable().Any(r => r.Field<string>("system_unit_id") == currentSystemUnitId))
                {
                    DataRow newRow = dt.NewRow();
                    newRow["system_unit_id"] = currentSystemUnitId;
                    dt.Rows.InsertAt(newRow, 0);
                }

                systemUnit.DataSource = dt;
                systemUnit.DisplayMember = "system_unit_id";
                systemUnit.ValueMember = "system_unit_id";

                if (!string.IsNullOrEmpty(selectedUnitId))
                {
                    var index = dt.AsEnumerable().ToList().FindIndex(r => r.Field<string>("system_unit_id") == selectedUnitId);
                    if (index != -1)
                    {
                        systemUnit.SelectedIndex = index;
                    }
                }
            }
            else
            {
                systemUnit.DataSource = null;
                systemUnit.Items.Clear();
                systemUnit.Items.Add("No available system units");
                systemUnit.SelectedIndex = 0;
            }
        }
        private string GetAssignedRoom()
        {
            string query = $"SELECT room FROM workstations WHERE workstation_id = '{workstationId}'";
            DataTable dt = dbHelper.GetData(query);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["room"].ToString();
            }

            return string.Empty; 
        }


        private void LoadWorkstationDetails()
        {
            string query = $"SELECT * FROM workstations WHERE workstation_id = '{workstationId}'";
            DataTable dt = dbHelper.GetData(query);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                monitorTextbox.Text = row["monitor"].ToString();
                keyboardTextbox.Text = row["keyboard"].ToString();
                mouseTextbox.Text = row["mouse"].ToString();
                avrTextbox.Text = row["avr"].ToString();

                string assignedRoom = row["room"].ToString();
                PopulateRoomsDropdown(); 
                roomsDropdown.SelectedValue = assignedRoom; 

                string systemUnitId = row["system_unit"].ToString();
                PopulateSystemUnitDropdown(systemUnitId);
            }
            else
            {
                MessageBox.Show("Workstation not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        private void InsertReplacementHistory(string systemUnitId, string workstationId, string room, string currentAsset, string newAsset)
        {
            string query = $@"
        INSERT INTO replacement_history 
        (system_unit_id, workstation_id, room, current_asset, new_asset, requested_by, processed_by, date_processed) 
        VALUES 
        ('{systemUnitId}', '{workstationId}', '{room}', '{currentAsset}', '{newAsset}', 'N/A', '{userId}', NOW())";
            dbHelper.ExecuteQuery(query);
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

            roomsDropdown.DataSource = combinedRoomsTable;
            roomsDropdown.DisplayMember = "laboratory_room";
            roomsDropdown.ValueMember = "laboratory_room";
        }
    }
}
