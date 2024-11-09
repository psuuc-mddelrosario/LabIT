using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class EditAssetsIncharge : Form
    {
        private DatabaseHelper dbHelper;
        private string assetId;

        public EditAssetsIncharge(string assetId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            PopulateAssetsDropdown();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.assetId = assetId;
            LoadAssetDetails(assetId);
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
                "System Unit Case",
            };

            assetsDropdown.DataSource = assets;
        }

        private void LoadAssetDetails(string assetId)
        {
            try
            {
                string query = $"SELECT asset_type, asset_brand, asset_description FROM assets WHERE asset_id = '{assetId}'";
                DataTable dt = dbHelper.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    string assetType = row["asset_type"].ToString();
                    string assetBrand = row["asset_brand"].ToString();
                    string assetDescription = row["asset_description"].ToString();

                    assetsDropdown.SelectedItem = assetType;

                    brand.Text = assetBrand;
                    description.Text = assetDescription;
                }
                else
                {
                    MessageBox.Show("Asset not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving asset details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            string assetType = assetsDropdown.SelectedItem.ToString();
            string assetBrand = brand.Text;
            string assetDescription = description.Text;

            if (string.IsNullOrEmpty(assetBrand) || string.IsNullOrEmpty(assetDescription))
            {
                MessageBox.Show("Please fill in all the fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = $@"
UPDATE assets 
SET asset_type = '{assetType}', asset_brand = '{assetBrand}', asset_description = '{assetDescription}'
WHERE asset_id = '{assetId}'";

                dbHelper.ExecuteQuery(query);
                //MessageBox.Show("Asset updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating asset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
