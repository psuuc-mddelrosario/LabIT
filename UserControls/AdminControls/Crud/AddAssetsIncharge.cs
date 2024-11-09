using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class AddAssetsIncharge : Form
    {
        private DatabaseHelper dbHelper;
        private string assignedRoom;

        public AddAssetsIncharge(string assignedRoom)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            PopulateAssetsDropdown();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.assignedRoom = assignedRoom;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulateAssetsDropdown()
        {
            var assets = new List<string>
            {
                "Monitor",
                "Keyboard",
                "Mouse",
                "AVR",
                "Motherboard",
                "CPU",
                "GPU",
                "RAM",
                "PSU",
                "Storage",
                "System Unit Case",
            };

            assetsDropdown.DataSource = assets;
        }

        private void add_Click(object sender, EventArgs e)
        {
            string assetType = assetsDropdown.SelectedItem.ToString();
            string brandText = brand.Text.Trim();
            string descriptionText = description.Text.Trim();
            int quantityValue = (int)quantity.Value;

            // Validate inputs
            if (string.IsNullOrEmpty(brandText))
            {
                MessageBox.Show("Please enter a brand.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(descriptionText))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (quantityValue <= 0)
            {
                MessageBox.Show("Please enter a valid quantity greater than zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DatabaseHelper dbHelper = new DatabaseHelper();

            int lastAssetIdNumber = GetLastOverallAssetIdNumber();

            for (int i = 0; i < quantityValue; i++)
            {
                string newAssetId = GenerateUniqueAssetId(assetType, lastAssetIdNumber + i + 1);

                while (AssetIdExists(newAssetId))
                {
                    lastAssetIdNumber++;
                    newAssetId = GenerateUniqueAssetId(assetType, lastAssetIdNumber + i + 1);
                }

                string query = $@"
INSERT INTO `assets` (`asset_id`, `asset_type`, `asset_brand`, `asset_description`, `location`) 
VALUES ('{newAssetId}', '{assetType}', '{brandText}', '{descriptionText}', '{assignedRoom}')";

                try
                {
                    dbHelper.ExecuteQuery(query);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inserting asset {newAssetId}: {ex.Message}");
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();

            // MessageBox.Show($"{quantityValue} asset(s) added successfully.");
        }



        private bool AssetIdExists(string assetId)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            string query = $"SELECT COUNT(*) FROM assets WHERE asset_id = '{assetId}'";
            DataTable dt = dbHelper.GetData(query);

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                int count = Convert.ToInt32(dt.Rows[0][0]);
                return count > 0; 
            }
            return false;
        }

        private int GetLastOverallAssetIdNumber()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            string query = "SELECT MAX(asset_id) FROM assets";
            DataTable dt = dbHelper.GetData(query);

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                string lastAssetId = dt.Rows[0][0].ToString();
                string numericPart = new string(lastAssetId.Where(char.IsDigit).ToArray());
                string lastFourDigits = numericPart.Substring(numericPart.Length - 4);

                if (int.TryParse(lastFourDigits, out int lastNumber))
                {
                    return lastNumber;
                }
            }
            return 0;
        }

        private string GenerateUniqueAssetId(string assetType, int idNumber)
        {
            string prefix = GetPrefixByAssetType(assetType);
            return $"{prefix}{idNumber:D4}";
        }

        private string GetPrefixByAssetType(string assetType)
        {
            switch (assetType)
            {
                case "Monitor": return "M";
                case "Motherboard": return "MB";
                case "Keyboard": return "K";
                case "Mouse": return "MO";
                case "PSU": return "P";
                case "AVR": return "A";
                case "RAM": return "R";
                case "CPU": return "C";
                case "GPU": return "G";
                case "Storage": return "S";
                case "System Unit Case": return "SUC";
                default: return "A";
            }
        }       
    }
}
