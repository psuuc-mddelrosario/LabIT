using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class ReplacementForm : Form
    {
        private DatabaseHelper dbHelper;
        private string replacementId;
        private string currentAssets;
        private string userId;

        public ReplacementForm(string replacementId, string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.replacementId = replacementId;

            this.currentAssets = GetAssetIdByRequestId(replacementId);
            this.userId = userId;   
            LoadAssetDetails();
        }

        private void LoadAssetDetails()
        {
            string query = @"
    SELECT ar.asset_id, a.asset_type
    FROM asset_requests ar
    JOIN assets a ON ar.asset_id = a.asset_id
    WHERE ar.request_id = @RequestId";

            List<MySqlParameter> parameters = new List<MySqlParameter>
    {
        new MySqlParameter("@RequestId", replacementId)
    };

            DataTable assetData = dbHelper.GetData(query, parameters.ToArray());

            if (assetData.Rows.Count > 0)
            {
                string assetId = assetData.Rows[0]["asset_id"].ToString();
                string assetTypeValue = assetData.Rows[0]["asset_type"].ToString();

                currentAsset.Text = assetId;
                assetType.Text = assetTypeValue;
            }
            else
            {
                MessageBox.Show("No asset details found for the given request ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetAssetIdByRequestId(string requestId)
        {
            string query = @"
                SELECT asset_id
                FROM asset_requests
                WHERE request_id = @RequestId";

            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@RequestId", requestId)
            };

            DataTable result = dbHelper.GetData(query, parameters.ToArray());

            if (result.Rows.Count > 0)
            {
                return result.Rows[0]["asset_id"].ToString();
            }
            else
            {
                return null; 
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void replace_Click(object sender, EventArgs e)
        {
            string newAssetId = newAsset.Text.Trim();
            string currentAssetId = currentAssets;

            // Check if new asset textbox is empty
            if (string.IsNullOrEmpty(newAssetId))
            {
                MessageBox.Show("Please enter the ID of the new asset.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Retrieve system unit, workstation, and asset type of current asset
            string queryCurrentAsset = @"
    SELECT system_unit, workstation, asset_type 
    FROM assets 
    WHERE asset_id = @AssetId";

            List<MySqlParameter> currentAssetParams = new List<MySqlParameter>
    {
        new MySqlParameter("@AssetId", currentAssetId)
    };

            DataTable currentAssetData = dbHelper.GetData(queryCurrentAsset, currentAssetParams.ToArray());

            // Retrieve system unit, workstation, and asset type of the new asset
            string queryNewAsset = @"
    SELECT system_unit, workstation, asset_type 
    FROM assets 
    WHERE asset_id = @NewAssetId";

            List<MySqlParameter> newAssetParams = new List<MySqlParameter>
    {
        new MySqlParameter("@NewAssetId", newAssetId)
    };

            DataTable newAssetData = dbHelper.GetData(queryNewAsset, newAssetParams.ToArray());

            // Retrieve the request information for the current asset
            string queryRequestInfo = @"
    SELECT room, instructor_id, request_date 
    FROM asset_requests 
    WHERE asset_id = @CurrentAssetId AND status = 'Pending'";

            List<MySqlParameter> requestParams = new List<MySqlParameter>
    {
        new MySqlParameter("@CurrentAssetId", currentAssetId)
    };

            DataTable requestData = dbHelper.GetData(queryRequestInfo, requestParams.ToArray());

            if (currentAssetData.Rows.Count > 0 && newAssetData.Rows.Count > 0 && requestData.Rows.Count > 0)
            {
                string currentAssetType = currentAssetData.Rows[0]["asset_type"].ToString();
                string newAssetType = newAssetData.Rows[0]["asset_type"].ToString();

                // Ensure asset types match
                if (currentAssetType != newAssetType)
                {
                    MessageBox.Show("The new asset must have the same asset type as the current asset.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if the new asset is already equipped
                string newAssetSystemUnit = newAssetData.Rows[0]["system_unit"].ToString();
                string newAssetWorkstation = newAssetData.Rows[0]["workstation"].ToString();

                if (newAssetSystemUnit != "Unequipped" || newAssetWorkstation != "Unequipped")
                {
                    MessageBox.Show("The new asset is already equipped in a system unit or workstation. Please select an unequipped asset.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Proceed with replacement if types match and the new asset is unequipped
                string systemUnit = currentAssetData.Rows[0]["system_unit"].ToString();
                string workstation = currentAssetData.Rows[0]["workstation"].ToString();

                // Update the current asset to "Unequipped"
                string updateCurrentAssetQuery = @"
        UPDATE assets 
        SET system_unit = 'Unequipped', workstation = 'Unequipped' 
        WHERE asset_id = @CurrentAssetId";

                List<MySqlParameter> updateCurrentParams = new List<MySqlParameter>
        {
            new MySqlParameter("@CurrentAssetId", currentAssetId)
        };

                // Update the new asset with the current asset's system unit and workstation
                string updateNewAssetQuery = @"
        UPDATE assets 
        SET system_unit = @SystemUnit, workstation = @Workstation 
        WHERE asset_id = @NewAssetId";

                List<MySqlParameter> updateNewParams = new List<MySqlParameter>
        {
            new MySqlParameter("@SystemUnit", systemUnit),
            new MySqlParameter("@Workstation", workstation),
            new MySqlParameter("@NewAssetId", newAssetId)
        };

                try
                {
                    dbHelper.ExecuteQuery(updateCurrentAssetQuery, updateCurrentParams.ToArray());
                    dbHelper.ExecuteQuery(updateNewAssetQuery, updateNewParams.ToArray());

                    // Check and update system_units or workstations tables based on asset type
                    if (IsSystemUnitComponent(currentAssetType))
                    {
                        UpdateSystemUnits(newAssetId, currentAssetId, currentAssetType);
                    }
                    else if (IsPeripheral(currentAssetType))
                    {
                        UpdateWorkstations(newAssetId, currentAssetId, currentAssetType);
                    }

                    // Prepare values for inserting into replacement_history
                    string room = requestData.Rows[0]["room"].ToString();
                    string instructorId = requestData.Rows[0]["instructor_id"].ToString();
                    DateTime requestDate = DateTime.Parse(requestData.Rows[0]["request_date"].ToString());

                    string insertReplacementHistoryQuery = @"
            INSERT INTO replacement_history (replacement_history_id, request_id, system_unit_id, workstation_id, room, current_asset, new_asset, requested_by, date_requested, processed_by, date_processed) 
            VALUES (@ReplacementHistoryId, @ReplacementId, @SystemUnitId, @WorkstationId, @Room, @CurrentAssetId, @NewAssetId, @RequestedBy, @DateRequested, @ProcessedBy, @DateProcessed)";

                    List<MySqlParameter> insertParams = new List<MySqlParameter>
            {
                new MySqlParameter("@ReplacementHistoryId", Guid.NewGuid().ToString()), // Generate a unique ID for the history
                new MySqlParameter("@ReplacementId", replacementId), // Assuming replacementId is defined elsewhere
                new MySqlParameter("@SystemUnitId", systemUnit),
                new MySqlParameter("@WorkstationId", workstation),
                new MySqlParameter("@Room", room),
                new MySqlParameter("@CurrentAssetId", currentAssetId),
                new MySqlParameter("@NewAssetId", newAssetId),
                new MySqlParameter("@RequestedBy", instructorId),
                new MySqlParameter("@DateRequested", requestDate),
                new MySqlParameter("@ProcessedBy", userId), // Assuming userId is defined elsewhere
                new MySqlParameter("@DateProcessed", DateTime.Now)
            };

                    dbHelper.ExecuteQuery(insertReplacementHistoryQuery, insertParams.ToArray());

                    // Update the status of the request to "Completed"
                    string updateRequestStatusQuery = @"
            UPDATE asset_requests 
            SET status = 'Completed', processed_date = @ProcessedDate, processed_by = @ProcessedBy
            WHERE request_id = @ReplacementId AND status = 'Pending'";

                    List<MySqlParameter> updateRequestStatusParams = new List<MySqlParameter>
            {
                new MySqlParameter("@ReplacementId", replacementId),
                new MySqlParameter("@ProcessedDate", DateTime.Now),
                new MySqlParameter("@ProcessedBy", userId)
            };

                    dbHelper.ExecuteQuery(updateRequestStatusQuery, updateRequestStatusParams.ToArray());

                    MessageBox.Show("Asset replacement successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during replacement: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Current asset or new asset not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Helper method to check if asset is a system unit component
        private bool IsSystemUnitComponent(string assetType)
        {
            return new[] { "Motherboard", "CPU", "GPU", "Storage", "RAM", "PSU", "System Unit Case" }.Contains(assetType);
        }

        // Helper method to check if asset is a peripheral
        private bool IsPeripheral(string assetType)
        {
            return new[] { "Keyboard", "Mouse", "AVR", "Monitor" }.Contains(assetType);
        }

        // Update the system_units table with the new asset ID
        private void UpdateSystemUnits(string assetId, string currentAsset, string assetType)
        {
            // Determine the field to update based on the asset type
            string updateField = assetType.ToLower().Replace(" ", "_"); // e.g., motherboard, cpu, ram, etc.
            string updateSystemUnitsQuery = $@"
        UPDATE system_units 
        SET {updateField} = @NewAssetId 
        WHERE {updateField} = @CurrentAsset";

            List<MySqlParameter> systemUnitParams = new List<MySqlParameter>
    {
        new MySqlParameter("@NewAssetId", assetId),
        new MySqlParameter("@CurrentAsset", currentAsset),
    };

            dbHelper.ExecuteQuery(updateSystemUnitsQuery, systemUnitParams.ToArray());
        }

        // Update the workstations table with the new asset ID for peripherals
        private void UpdateWorkstations(string assetId, string currentAsset, string assetType)
        {
            string updateField = assetType.ToLower(); // e.g., monitor, keyboard, etc.
            string updateWorkstationsQuery = $@"
        UPDATE workstations 
        SET {updateField} = @NewAssetId 
        WHERE {updateField} = @CurrentAsset";

            List<MySqlParameter> workstationParams = new List<MySqlParameter>
    {
        new MySqlParameter("@NewAssetId", assetId),
        new MySqlParameter("@CurrentAsset", currentAsset)
    };

            dbHelper.ExecuteQuery(updateWorkstationsQuery, workstationParams.ToArray());
        }

    }
}
