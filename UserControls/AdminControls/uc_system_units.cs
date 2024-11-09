using Laboratory_Management_System.UserControls.AdminControls.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace Laboratory_Management_System.UserControls.AdminControls
{
    public partial class uc_system_units : UserControl
    {
        private string userId;

        private DatabaseHelper dbHelper;
        private DataTable dataTable;
        public uc_system_units(string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.userId = userId;
            CustomizeDataGridView();
            BlackText();
            PopulateOSDropdown();
            PopulateRoomsDropdown();
            PopulateEquippedDropdown();
            ApplyFilters();
            dataGridView1.Paint += DataGridView1_Paint;
            modifyTitleLabel();
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
                Padding = new Padding(5, 0, 0, 0)
            };
            dataGridView1.DefaultCellStyle = rowStyle;

            DataGridViewCellStyle alternatingRowStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 245, 245)
            };
            dataGridView1.AlternatingRowsDefaultCellStyle = alternatingRowStyle;

            dataGridView1.RowTemplate.Height = 45;
            dataGridView1.RowTemplate.Resizable = DataGridViewTriState.False;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Name != "Select")
                {
                    column.ReadOnly = true;
                }
            }

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

        private void LoadData(string osFilter = null, string roomFilter = null, string equippedFilter = null, string searchText = "")
        {
            StringBuilder query = new StringBuilder(@"
SELECT 
    system_units.system_unit_id AS 'System Unit ID', 
    system_units.hostname AS 'Hostname', 
    system_units.operating_system AS 'Operating System', 
    system_units.location AS 'Location', 
    system_units.workstation AS 'Workstation',
    system_units.date_built AS 'Date Built'
FROM system_units
WHERE 1=1");

            if (!string.IsNullOrEmpty(osFilter) && osFilter != "All")
            {
                if (osFilter == "Windows")
                {
                    query.Append(" AND system_units.operating_system LIKE 'Windows%'");
                }
                else
                {
                    query.Append($" AND system_units.operating_system = '{osFilter}'");
                }
            }

            if (!string.IsNullOrEmpty(roomFilter) && roomFilter != "All")
            {
                query.Append($" AND system_units.location = '{roomFilter}'");
            }

            if (!string.IsNullOrEmpty(equippedFilter) && equippedFilter != "All")
            {
                if (equippedFilter == "Equipped")
                {
                    query.Append(" AND system_units.workstation IS NOT NULL AND system_units.workstation <> 'Unequipped'");
                }
                else if (equippedFilter == "Unequipped")
                {
                    query.Append(" AND (system_units.workstation IS NULL OR system_units.workstation = 'Unequipped')");
                }
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query.Append($@"
        AND (
            system_units.system_unit_id LIKE '%{searchText}%' 
            OR system_units.hostname LIKE '%{searchText}%' 
            OR system_units.operating_system LIKE '%{searchText}%'
            OR system_units.location LIKE '%{searchText}%' 
            OR system_units.workstation LIKE '%{searchText}%'
        )");
            }

            query.Append(" ORDER BY system_units.date_built DESC");

            dataTable = dbHelper.GetData(query.ToString());
            dataGridView1.DataSource = dataTable;

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

        private void BlackText()
        {
            label6.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;
            label7.ForeColor = Color.Black; 
            label1.ForeColor = Color.Black;
        }

        private void roomsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            modifyTitleLabel();
            ApplyFilters();
        }

        private void osDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void equippedDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void PopulateOSDropdown()
        {
            var operatingSystem = new List<string>
    {
        "All",
        "Windows",
        "Linux",
        "MAC",
      
    };

            osDropdown.DataSource = operatingSystem;
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

        private void ApplyFilters()
        {
            string osFilter = osDropdown.SelectedItem?.ToString() ?? "All";
            string roomFilter = roomsDropdown.SelectedValue?.ToString() ?? "All";
            string equippedFilter = equippedDropdown.SelectedItem?.ToString() ?? "All";

            LoadData(osFilter, roomFilter, equippedFilter);
        }

        private void addButton_Click(object sender, EventArgs e)
        {          
            AddSystemUnits addForm = new AddSystemUnits(userId);

            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
                //MessageBox.Show("System unit added successfully!");
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            List<string> systemUnitsToDelete = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Select"].Value != null && (bool)row.Cells["Select"].Value)
                {
                    string systemUnitId = row.Cells["System Unit ID"].Value.ToString();
                    systemUnitsToDelete.Add(systemUnitId);
                }
            }

            if (systemUnitsToDelete.Count == 0)
            {
                MessageBox.Show("Please select at least one system unit to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected system unit(s)? This will unequip all related assets.",
                                                  "Confirm Deletion",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper dbHelper = new DatabaseHelper();

                    foreach (string systemUnitId in systemUnitsToDelete)
                    { 
                        string updateAssetsQuery = $"UPDATE assets SET system_unit = 'Unequipped' WHERE system_unit = '{systemUnitId}'";
                        dbHelper.ExecuteQuery(updateAssetsQuery);

                        string deleteSystemUnitQuery = $"DELETE FROM system_units WHERE system_unit_id = '{systemUnitId}'";
                        dbHelper.ExecuteQuery(deleteSystemUnitQuery);
                    }

                    MessageBox.Show("System unit(s) and related assets updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData("All", "All", "All"); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting system unit(s): {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void searchFilter_TextChanged(object sender, EventArgs e)
        {
            string osFilter = osDropdown.SelectedItem?.ToString() ?? "All";
            string roomFilter = roomsDropdown.SelectedValue?.ToString() ?? "All";
            string equippedFilter = equippedDropdown.SelectedItem?.ToString() ?? "All";

            string searchText = searchFilter.Text;
            LoadData(osFilter, roomFilter, equippedFilter, searchText);
        }

        private void modifyTitleLabel()
        {
            string selectedRoom = roomsDropdown.SelectedValue?.ToString() ?? "All";
            titleLabel.Text = $"System Units - {selectedRoom}";
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            string systemUnitId = null;
            int selectedCount = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

                if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
                {                  
                    systemUnitId = row.Cells[1].Value.ToString();
                    selectedCount++;
                    if (selectedCount > 1)
                    {
                        MessageBox.Show("Please select only one system unit to edit.");
                        return;
                    }
                }
            }

            if (selectedCount == 1)
            {
                EditSystemUnitsAdmin editAssetForm = new EditSystemUnitsAdmin(systemUnitId, userId);

                DialogResult result = editAssetForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    LoadData("All", "All");
                }
            }
            else
            {
                MessageBox.Show("Please select a system unit to edit.");
            }
        }
    }
}
