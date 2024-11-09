using DevExpress.Xpo.DB.Helpers;
using Laboratory_Management_System.UserControls.AdminControls.Crud;
using Laboratory_Management_System.UserControls.InstructorsControls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Laboratory_Management_System.UserControls.AdminControls
{
    public partial class uc_assets_inventory : UserControl
    {
        private string userId;

        private DatabaseHelper dbHelper;
        private DataTable dataTable;
        public uc_assets_inventory(string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            CustomizeDataGridView();
            LoadData("All", "All", "All", "All");
            PopulateRoomsDropdown();
            PopulateAssetsDropdown();
            PopulateStatusDropdown();
            PopulateEquippedDropdown();
            BlackText();
            CreateDashboardPanels();
            dataGridView1.Paint += DataGridView1_Paint;
            this.userId = userId;
            assetsDropdown.SelectedIndexChanged += assetsDropdown_SelectedIndexChanged;

            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
        }

        private void DataGridView1_Paint(object sender, PaintEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                string message = "No data available";
                Font font = new Font("Montserrat", 14, FontStyle.Regular);
                Color textColor = Color.Gray;

                SizeF textSize = e.Graphics.MeasureString(message, font);
                float textX = (dataGridView1.Width - textSize.Width) / 2;
                float textY = (dataGridView1.Height - textSize.Height) / 2;

                e.Graphics.DrawString(message, font, new SolidBrush(textColor), textX, textY);
            }
        }

        private void CustomizeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.GridColor = Color.LightGray;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Dock = DockStyle.Fill;

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Select",
                HeaderText = "",
                Width = 20,
                ReadOnly = false,
                TrueValue = true,
                FalseValue = false
            };
            dataGridView1.Columns.Insert(0, checkBoxColumn);

            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(29, 60, 170),
                ForeColor = Color.White,
                Font = new Font("Montserrat", 11, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Montserrat", 11, FontStyle.Regular),
                SelectionBackColor = Color.FromArgb(225, 235, 245),
                SelectionForeColor = Color.Black,
                Padding = new Padding(5, 10, 0, 10),
                WrapMode = DataGridViewTriState.True 
            };
            dataGridView1.DefaultCellStyle = rowStyle;

            DataGridViewCellStyle alternatingRowStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 245, 245)
            };
            dataGridView1.AlternatingRowsDefaultCellStyle = alternatingRowStyle;

            if (dataGridView1.Columns["note"] != null)
            {
                dataGridView1.Columns["note"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            dataGridView1.RowTemplate.Height = 45;
            dataGridView1.RowTemplate.Resizable = DataGridViewTriState.True; 
            dataGridView1.CellPainting += dataGridView1_CellPainting;
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, true);

                using (Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                            e.CellBounds.Top, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);

                        e.PaintContent(e.CellBounds);
                        e.Handled = true;
                    }
                }
            }
        }

        private void LoadData(string assetFilter, string roomFilter, string statusFilter, string equippedFilter, string searchText = "")
        {
            string equippedCondition = equippedFilter == "Equipped" ? "workstation <> 'Unequipped'" : "1=1";
            string searchCondition = string.IsNullOrEmpty(searchText)
                ? "1=1" // If search text is empty, no filter
                : $@"(asset_id LIKE '%{searchText}%' OR asset_type LIKE '%{searchText}%' 
            OR asset_brand LIKE '%{searchText}%' OR asset_description LIKE '%{searchText}%' 
            OR location LIKE '%{searchText}%' OR workstation LIKE '%{searchText}%' 
            OR status LIKE '%{searchText}%')";

            string query = $@"
        SELECT asset_id AS ID, asset_type, asset_brand, asset_description, location, workstation, status, date_added 
        FROM assets 
        WHERE location = @roomFilter 
          AND status = @statusFilter 
          AND asset_type = @assetFilter 
          AND {equippedCondition} 
          AND {searchCondition}
        ORDER BY date_added DESC";

            // Replace filter parameters for query customization
            query = query.Replace("@roomFilter", roomFilter == "All" ? "location" : $"'{roomFilter}'");
            query = query.Replace("@statusFilter", statusFilter == "All" ? "status" : $"'{statusFilter}'");
            query = query.Replace("@assetFilter", assetFilter == "All" ? "asset_type" : $"'{assetFilter}'");

            dataTable = dbHelper.GetData(query);
            dataGridView1.DataSource = dataTable;

            dataGridView1.Columns[1].HeaderText = "Asset ID";
            dataGridView1.Columns[2].HeaderText = "Asset Type";
            dataGridView1.Columns[3].HeaderText = "Brand";
            dataGridView1.Columns[4].HeaderText = "Description";
            dataGridView1.Columns[5].HeaderText = "Location";
            dataGridView1.Columns[6].HeaderText = "Workstation";
            dataGridView1.Columns[7].HeaderText = "Status";

            dataGridView1.Columns["date_added"].Visible = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["Select"].Value = false;
                row.Tag = row.Cells["ID"].Value.ToString();
            }

            AdjustDataGridViewHeight();
        }

        private void AdjustDataGridViewHeight()
        {
            int rowCount = dataGridView1.Rows.Count;

            int rowHeight = dataGridView1.RowTemplate.Height;
            int headerHeight = dataGridView1.ColumnHeadersHeight;
            int totalHeight = (rowHeight * rowCount) + headerHeight;

            int maxHeight = 500;
            if (totalHeight > maxHeight)
            {
                totalHeight = maxHeight;
            }

            dataGridView1.Height = totalHeight;
        }

        private void roomsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRoom = roomsDropdown.SelectedValue.ToString();
            string selectedAsset = assetsDropdown.SelectedItem?.ToString() ?? "All";
            string selectedStatus = statusDropdown.SelectedItem?.ToString() ?? "All";
            string selectedEquipped = equippedDropdown.SelectedItem?.ToString() ?? "All";


            LoadData(selectedAsset, selectedRoom, selectedStatus, selectedEquipped);
            CreateDashboardPanels();
        }


        private void assetsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAsset = assetsDropdown.SelectedItem.ToString();
            string selectedRoom = roomsDropdown.SelectedValue.ToString();
            string selectedStatus = statusDropdown.SelectedItem?.ToString() ?? "All";
            string selectedEquipped = equippedDropdown.SelectedItem?.ToString() ?? "All";

            LoadData(selectedAsset, selectedRoom, selectedStatus, selectedEquipped);
            CreateDashboardPanels();
        }

        private void PopulateRoomsDropdown()
        {
            string query = "SELECT laboratory_room FROM rooms"; 
            DataTable roomsTable = dbHelper.GetData(query);

            DataTable combinedRoomsTable = new DataTable();
            combinedRoomsTable.Columns.Add("laboratory_room"); 

            DataRow allRow = combinedRoomsTable.NewRow();
            allRow["laboratory_room"] = "All";
            combinedRoomsTable.Rows.Add(allRow);


            foreach (DataRow row in roomsTable.Rows)
            {
                combinedRoomsTable.ImportRow(row);
            }

            roomsDropdown.DataSource = combinedRoomsTable;
            roomsDropdown.DisplayMember = "laboratory_room"; 
            roomsDropdown.ValueMember = "laboratory_room";
        }

        private void PopulateAssetsDropdown()
        {
            var assets = new List<string>
    {
        "All",
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
        "iMac",
    };

            assetsDropdown.DataSource = assets;
        }

        private void PopulateStatusDropdown()
        {
            var status = new List<string>
    {
        "All",
        "Working",
        "Repair",
        "Replacement",
    };

            statusDropdown.DataSource = status;
        }

        private void PopulateEquippedDropdown()
        {
            var equipped = new List<string>
    {
        "All",
        "Equipped",
        "Unequipped",
    };

            equippedDropdown.DataSource = equipped;
        }      

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string status = row.Cells["status"].Value?.ToString();

                if (status == "Repair")
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (status == "Replacement")
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void searchFilter_TextChanged(object sender, EventArgs e)
        {
            string selectedAsset = assetsDropdown.SelectedItem.ToString();
            string selectedRoom = roomsDropdown.SelectedValue.ToString();
            string selectedStatus = statusDropdown.SelectedItem.ToString();
            string selectedEquipped = equippedDropdown.SelectedItem?.ToString() ?? "All";
            string searchText = searchFilter.Text;
            LoadData(selectedAsset, selectedRoom, selectedStatus, selectedEquipped, searchText);
        }

        private void statusDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAsset = assetsDropdown.SelectedItem.ToString();
            string selectedRoom = roomsDropdown.SelectedValue.ToString();
            string selectedStatus = statusDropdown.SelectedItem.ToString();
            string selectedEquipped = equippedDropdown.SelectedItem?.ToString() ?? "All";

            LoadData(selectedAsset, selectedRoom, selectedStatus, selectedEquipped);
            CreateDashboardPanels();
        }

        private void equippedDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAsset = assetsDropdown.SelectedItem.ToString();
            string selectedRoom = roomsDropdown.SelectedValue.ToString();
            string selectedStatus = statusDropdown.SelectedItem?.ToString() ?? "All";
            string selectedEquipped = equippedDropdown.SelectedItem?.ToString() ?? "All";

            LoadData(selectedAsset, selectedRoom, selectedStatus, selectedEquipped);
            CreateDashboardPanels();
        }

        private void BlackText()
        {
            label1.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;
            label7.ForeColor = Color.Black;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddAssets addAssetForm = new AddAssets();

            DialogResult result = addAssetForm.ShowDialog();
            if (result == DialogResult.OK) 
            {
                string selectedAsset = assetsDropdown.SelectedItem.ToString();
                string selectedRoom = roomsDropdown.SelectedValue.ToString();
                string selectedStatus = statusDropdown.SelectedItem.ToString();
                string selectedEquipped = equippedDropdown.SelectedItem?.ToString() ?? "All";

                LoadData(selectedAsset, selectedRoom, selectedStatus, selectedEquipped);
                CreateDashboardPanels();
            }
        }

        private void CreateDashboardPanels()
        {
            panelBlocks.Controls.Clear();

            string selectedRoom = roomsDropdown.SelectedValue?.ToString() ?? "All";
            string selectedAssetType = assetsDropdown.SelectedItem?.ToString() ?? "All";
            string selectedEquipped = equippedDropdown.SelectedItem?.ToString() ?? "All";
            string selectedStatus = statusDropdown.SelectedItem?.ToString() ?? "All";

            titleLabel.Text = $"Computer Assets Inventory - {selectedRoom}";

            string[] categories = { "Total Assets", "Equipped", "Unequipped", "Working", "Repair", "Replacement" };
            Color[] colors =
            {
        Color.FromArgb(52, 73, 94),
        Color.FromArgb(39, 174, 96),
        Color.FromArgb(192, 57, 43),
        Color.FromArgb(41, 128, 185),
        Color.FromArgb(241, 196, 15),
        Color.FromArgb(127, 140, 141)
    };

            int panelWidth = panelBlocks.Width;
            int panelHeight = panelBlocks.Height;
            int padding = 5;

            int panelCount = categories.Length;
            int totalPaddingWidth = (padding * (panelCount - 1));
            int availableWidth = panelWidth - totalPaddingWidth;
            int panelWidthEach = availableWidth / panelCount;
            int panelHeightEach = panelHeight - (padding * 2);

            DatabaseHelper dbHelper = new DatabaseHelper();

            int totalCount = GetAssetCount(dbHelper, selectedRoom, selectedAssetType, selectedEquipped, selectedStatus);

            int equippedCount = (selectedEquipped == "Equipped" || selectedEquipped == "All")
       ? GetAssetCount(dbHelper, selectedRoom, selectedAssetType, "Equipped", selectedStatus)
       : 0;

            int unequippedCount = (selectedEquipped == "Unequipped" || selectedEquipped == "All")
                ? GetAssetCount(dbHelper, selectedRoom, selectedAssetType, "Unequipped", selectedStatus)
                : 0;

            int workingCount = (selectedStatus == "Working" || selectedStatus == "All")
                    ? GetAssetCount(dbHelper, selectedRoom, selectedAssetType, selectedEquipped, "Working")
                    : 0;

            int repairCount = (selectedStatus == "Repair" || selectedStatus == "All")
                               ? GetAssetCount(dbHelper, selectedRoom, selectedAssetType, selectedEquipped, "Repair")
                               : 0;

            int replacementCount = (selectedStatus == "Replacement" || selectedStatus == "All")
                                   ? GetAssetCount(dbHelper, selectedRoom, selectedAssetType, selectedEquipped, "Replacement")
                                   : 0;


            int[] counts = { totalCount, equippedCount, unequippedCount, workingCount, repairCount, replacementCount };

            for (int i = 0; i < panelCount; i++)
            {
                Panel smallPanel = new Panel
                {
                    Size = new Size(panelWidthEach, panelHeightEach),
                    Location = new Point((i == 0) ? 0 : (i * (panelWidthEach + padding)), padding),
                    BorderStyle = BorderStyle.None,
                    BackColor = colors[i],
                    Padding = new Padding(10)
                };

                Label categoryLabel = new Label
                {
                    Text = categories[i],
                    TextAlign = ContentAlignment.TopLeft,
                    Dock = DockStyle.Top,
                    Font = new Font("Montserrat", 11, FontStyle.Regular),
                    ForeColor = Color.White
                };

                Label countLabel = new Label
                {
                    Text = counts[i].ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Montserrat", 22, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Fill
                };

                smallPanel.Controls.Add(countLabel);
                smallPanel.Controls.Add(categoryLabel);

                panelBlocks.Controls.Add(smallPanel);
            }

            panelBlocks.Resize += (s, e) =>
            {
                panelWidth = panelBlocks.Width;
                panelHeight = panelBlocks.Height;
                totalPaddingWidth = (padding * (panelCount - 1));
                availableWidth = panelWidth - totalPaddingWidth;
                panelWidthEach = availableWidth / panelCount;
                panelHeightEach = panelHeight - (padding * 2);

                for (int i = 0; i < panelBlocks.Controls.Count; i++)
                {
                    Panel smallPanel = (Panel)panelBlocks.Controls[i];
                    smallPanel.Size = new Size(panelWidthEach, panelHeightEach);
                    smallPanel.Location = new Point((i == 0) ? 0 : (i * (panelWidthEach + padding)), padding);
                }
            };
        }

        private int GetAssetCount(DatabaseHelper dbHelper, string room, string assetType, string equippedStatus, string assetStatus)
        {
            string roomCondition = room == "All" ? "1=1" : $"location = '{room}'";
            string assetCondition = assetType == "All" ? "1=1" : $"asset_type = '{assetType}'";

            string equippedCondition;
            if (equippedStatus == "Unequipped")
            {
                equippedCondition = "workstation = 'Unequipped'";
            }
            else if (equippedStatus == "Equipped")
            {               
                equippedCondition = "workstation <> 'Unequipped'";
            }
            else
            {
                equippedCondition = "1=1";
            }

            string statusCondition = assetStatus == "All" ? "1=1" : $"status = '{assetStatus}'";

            string query = $@"
    SELECT COUNT(*) AS AssetCount 
    FROM assets 
    WHERE {roomCondition} 
    AND {assetCondition} 
    AND {equippedCondition} 
    AND {statusCondition}";

            // Log the query for debugging purposes
            Console.WriteLine("Query Executed: " + query);
            Console.WriteLine($"Room: {room}, AssetType: {assetType}, EquippedStatus: {equippedStatus}, AssetStatus: {assetStatus}");

            return GetRowCount(query);
        }


        private int GetRowCount(string query)
        {
            DataTable data = dbHelper.GetData(query);

            if (data.Rows.Count > 0)
            {
                Console.WriteLine("Count Result: " + data.Rows[0]["AssetCount"]);
                return Convert.ToInt32(data.Rows[0]["AssetCount"]);
            }
            else
            {
                Console.WriteLine("No Rows Returned");
                return 0;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var assetsIdsToDelete = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    string assetId = row.Cells["ID"].Value.ToString();
                    assetsIdsToDelete.Add(assetId);
                }
            }

            if (assetsIdsToDelete.Count > 0)
            {
                var confirmResult = MessageBox.Show(
                    "Are you sure you want to delete the selected assets?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    string ids = string.Join("','", assetsIdsToDelete);
                    string query = $"DELETE FROM assets WHERE asset_id IN ('{ids}')";

                    try
                    {
                        dbHelper.ExecuteQuery(query);
                        LoadData("All", "All", "All", "All");
                        CreateDashboardPanels();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("No assets selected for deletion.");
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            string assetId = null;
            int selectedCount = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

                if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
                {
                    assetId = row.Cells["ID"].Value.ToString();
                    selectedCount++;
                    if (selectedCount > 1)
                    {
                        MessageBox.Show("Please select only one asset to edit.");
                        return;
                    }
                }
            }

            if (selectedCount == 1)
            {
                EditAssetsIncharge editAssetForm = new EditAssetsIncharge(assetId);

                DialogResult result = editAssetForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    LoadData("All", "All", "All", "All");
                }
            }
            else
            {
                MessageBox.Show("Please select an asset to edit.");
            }
        }
    }
}
