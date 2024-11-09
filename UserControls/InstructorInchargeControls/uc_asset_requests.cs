using Laboratory_Management_System.UserControls.AdminControls.Crud;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.LaboratoryInchargeControls
{
    public partial class uc_asset_requests : UserControl
    {
        private string userId;
        private DatabaseHelper dbHelper;
        private DataTable dataTable;

        public uc_asset_requests(string userId)
        {
            InitializeComponent();
            this.userId = userId;
            dbHelper = new DatabaseHelper();
            CustomizeDataGridView();
            LoadData("All", "All");
            PopulateAssetsDropdown();
            ReportTypeDropdown();
            dataGridView1.Paint += DataGridView1_Paint;
            string assignedRoom = GetAssignedRoom(userId);
            titleLabel.Text = $"Asset Requests - {assignedRoom}";
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

            // Add checkbox column for selection
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Select",
                HeaderText = "",
                Width = 50,
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

            // Set up custom cell painting for enhanced visuals
            dataGridView1.CellPainting += DataGridView1_CellPainting;
        }

        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, true);

                using (Brush gridBrush = new SolidBrush(dataGridView1.GridColor),
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

        private void LoadData(string assetFilter = "All", string reportTypeFilter = "All")
        {
            string assignedRoom = GetAssignedRoom(userId);

            // Define the query to fetch only pending asset requests for the assigned room
            string query = @"
SELECT ar.request_id, ar.asset_id, a.system_unit AS system_unit, a.workstation AS workstation,
       CONCAT(u.firstname, ' ', u.lastname) AS instructor_name,
       ar.request_type, ar.note, ar.request_date, ar.status
FROM asset_requests ar
JOIN users u ON ar.instructor_id = u.user_id
JOIN assets a ON ar.asset_id = a.asset_id
WHERE ar.room = @AssignedRoom AND ar.status = 'Pending'";

            List<MySqlParameter> parameters = new List<MySqlParameter> { new MySqlParameter("@AssignedRoom", assignedRoom) };

            // Apply additional filters if specified
            if (assetFilter != "All")
            {
                query += " AND a.asset_type = @AssetFilter";
                parameters.Add(new MySqlParameter("@AssetFilter", assetFilter));
            }

            if (reportTypeFilter != "All")
            {
                query += " AND ar.request_type = @ReportTypeFilter";
                parameters.Add(new MySqlParameter("@ReportTypeFilter", reportTypeFilter));
            }

            // Fetch data from the database
            DataTable requestsData = dbHelper.GetData(query, parameters.ToArray());

            // Set data source
            dataGridView1.DataSource = requestsData;

            // Hide request_id from display
            if (dataGridView1.Columns["request_id"] != null)
            {
                dataGridView1.Columns["request_id"].Visible = false;
            }

            // Add a checkbox column for selection, using request_id as the value
            if (dataGridView1.Columns["Select"] == null)
            {
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "Select",
                    HeaderText = "Select",
                    DataPropertyName = "request_id"
                };
                dataGridView1.Columns.Insert(0, checkBoxColumn);
            }

            // Set custom headers
            dataGridView1.Columns["asset_id"].HeaderText = "Asset ID";
            dataGridView1.Columns["system_unit"].HeaderText = "System Unit";
            dataGridView1.Columns["workstation"].HeaderText = "Workstation";
            dataGridView1.Columns["instructor_name"].HeaderText = "Instructor";
            dataGridView1.Columns["request_type"].HeaderText = "Request Type";
            dataGridView1.Columns["note"].HeaderText = "Note";
            dataGridView1.Columns["request_date"].HeaderText = "Request Date";           
            //dataGridView1.Columns["status"].HeaderText = "Status";

            if (dataGridView1.Columns["status"] != null)
            {
                dataGridView1.Columns["status"].Visible = false;
            }

            AdjustDataGridViewHeight();
        }


        private void AdjustDataGridViewHeight()
        {
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
        "Storage",
        "System Unit Case",
        "iMac",
    };

            assetsDropdown.DataSource = assets;
        }

        private void ReportTypeDropdown()
        {
            var reportType = new List<string>
    {
        "All",
        "Repair",
        "Replacement",
    };

            reportDropdown.DataSource = reportType;
        }
      
        private void assetsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAsset = assetsDropdown.SelectedValue?.ToString() ?? "All";
            LoadData(selectedAsset, reportDropdown.SelectedValue?.ToString() ?? "All");
        }

        private void reportDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedReportType = reportDropdown.SelectedValue?.ToString() ?? "All";
            LoadData( assetsDropdown.SelectedValue?.ToString() ?? "All", selectedReportType);
        }

        private void approveButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                {
                    string requestId = row.Cells["request_id"].Value.ToString();
                    string requestType = row.Cells["request_type"].Value.ToString();

                    if (requestType == "Replacement")
                    {
                        ReplacementForm replacementForm = new ReplacementForm(requestId, userId);
                        replacementForm.ShowDialog();

                        if (replacementForm.DialogResult == DialogResult.OK)
                        {
                            LoadData("All", "All");
                        }
                    }
                    else if (requestType == "Repair")
                    {
                        DialogResult dialogResult = MessageBox.Show(
                            "Do you want to approve this request and mark it as repaired?",
                            "Approve Repair Request",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (dialogResult == DialogResult.Yes)
                        {
                            // Retrieve the asset_id from the asset_requests table
                            string selectRequestQuery = $@"
                    SELECT instructor_id, request_date, asset_id 
                    FROM asset_requests 
                    WHERE request_id = '{requestId}'";

                            var requestData = dbHelper.GetData(selectRequestQuery);

                            if (requestData.Rows.Count > 0)
                            {
                                var requestedBy = requestData.Rows[0]["instructor_id"].ToString();
                                var dateRequested = Convert.ToDateTime(requestData.Rows[0]["request_date"]).ToString("yyyy-MM-dd HH:mm:ss"); // Format date
                                var assetId = requestData.Rows[0]["asset_id"].ToString();

                                // Retrieve system_unit_id and workstation_id from the assets table
                                string assetDetailsQuery = $@"
                        SELECT system_unit, workstation, location 
                        FROM assets 
                        WHERE asset_id = '{assetId}'";

                                var assetDetailsData = dbHelper.GetData(assetDetailsQuery);

                                if (assetDetailsData.Rows.Count > 0)
                                {
                                    var systemUnitId = assetDetailsData.Rows[0]["system_unit"].ToString();
                                    var workstationId = assetDetailsData.Rows[0]["workstation"].ToString();
                                    var room = assetDetailsData.Rows[0]["location"].ToString();

                                    // Mark the repair request as completed in the asset_requests table
                                    string updateRequestQuery = $@"
                            UPDATE asset_requests 
                            SET status = 'Completed', 
                                processed_date = NOW(), 
                                processed_by = '{userId}' 
                            WHERE request_id = '{requestId}'";
                                    dbHelper.ExecuteQuery(updateRequestQuery);

                                    // Update the asset status to "Working"
                                    string updateAssetQuery = $@"
                            UPDATE assets 
                            SET status = 'Working' 
                            WHERE asset_id = '{assetId}'";
                                    dbHelper.ExecuteQuery(updateAssetQuery);

                                    // Insert into the repair_history table
                                    string insertRepairHistoryQuery = $@"
                            INSERT INTO repair_history(request_id, system_unit_id, workstation_id, room, asset, requested_by, date_requested, processed_by, date_processed) 
                            VALUES ('{requestId}', '{systemUnitId}', '{workstationId}', '{room}', '{assetId}', '{requestedBy}', '{dateRequested}', '{userId}', NOW())";
                                    dbHelper.ExecuteQuery(insertRepairHistoryQuery);

                                    MessageBox.Show("The repair request has been approved and marked as completed, the asset status is now 'Working', and the repair history has been updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadData("All", "All");
                                }
                                else
                                {
                                    MessageBox.Show("Asset details could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void searchFilter_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
