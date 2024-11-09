using Laboratory_Management_System.UserControls.AdminControls.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls
{
    public partial class uc_temporary_schedules : UserControl
    {
        private DatabaseHelper dbHelper;
        private DataTable dataTable;
        private System.Windows.Forms.Timer scheduleTimer;

        public uc_temporary_schedules()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            BlackText();
            CustomizeDataGridView();
            PopulateRoomsDropdown();
            PopulateStatusDropdown();
            LoadData();
            dataGridView1.Paint += DataGridView1_Paint;

            // Initialize and start the timer to check schedules every second
            scheduleTimer = new System.Windows.Forms.Timer();
            scheduleTimer.Interval = 1000; // 1 second
            scheduleTimer.Tick += ScheduleTimer_Tick;
            scheduleTimer.Start();
        }

        private void BlackText()
        {
            label1.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
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

        private void LoadData()
        {
            try
            {
                string filter = BuildFilterQuery();

                string query = $@"
                    SELECT ts.schedule_id, 
                           CONCAT(u.firstname, ' ', u.lastname) AS instructor,
                           ts.room,
                           ts.subject, 
                           ts.course, 
                           ts.year, 
                           ts.block, 
                           ts.day,
                           ts.class_start, 
                           ts.class_end,  
                           ts.status,
                           ts.date_added
                    FROM temporary_schedules ts
                    INNER JOIN users u ON ts.instructor = u.user_id 
                    {filter}
                    ORDER BY ts.date_added DESC;";

                dataTable = dbHelper.GetData(query);
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns["schedule_id"].Visible = false;

                dataGridView1.Columns["instructor"].HeaderText = "Instructor";
                dataGridView1.Columns["room"].HeaderText = "Room";
                dataGridView1.Columns["subject"].HeaderText = "Subject";
                dataGridView1.Columns["course"].HeaderText = "Course";
                dataGridView1.Columns["year"].HeaderText = "Year";
                dataGridView1.Columns["block"].HeaderText = "Block";
                dataGridView1.Columns["day"].HeaderText = "Day";
                dataGridView1.Columns["class_start"].HeaderText = "Class Start";
                dataGridView1.Columns["class_end"].HeaderText = "Class End";
                dataGridView1.Columns["status"].HeaderText = "Status";
                dataGridView1.Columns["date_added"].HeaderText = "Date Added";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells["Select"].Value = false;
                    row.Tag = row.Cells["schedule_id"].Value;
                }

                AdjustDataGridViewHeight();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading data: {ex.Message}");
            }
        }

        private string BuildFilterQuery()
        {
            List<string> conditions = new List<string>();

            if (roomsDropdown.SelectedValue != null && roomsDropdown.SelectedValue.ToString() != "All")
            {
                conditions.Add($"ts.room = '{roomsDropdown.SelectedValue}'");
            }

            if (statusDropdown.SelectedValue != null && statusDropdown.SelectedValue.ToString() != "All")
            {
                conditions.Add($"ts.status = '{statusDropdown.SelectedValue}'");
            }

            if (!string.IsNullOrEmpty(searchFilter.Text))
            {
                conditions.Add($"(u.firstname LIKE '%{searchFilter.Text}%' OR u.lastname LIKE '%{searchFilter.Text}%' OR ts.subject LIKE '%{searchFilter.Text}%' OR ts.room LIKE '%{searchFilter.Text}%' OR ts.course LIKE '%{searchFilter.Text}%' OR ts.year LIKE '%{searchFilter.Text}%' OR ts.block LIKE '%{searchFilter.Text}%' OR ts.day LIKE '%{searchFilter.Text}%')");
            }

            return conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";
        }

        private void AdjustDataGridViewHeight()
        {
            int rowCount = dataGridView1.Rows.Count;
            int rowHeight = dataGridView1.RowTemplate.Height;
            int headerHeight = dataGridView1.ColumnHeadersHeight;
            int totalHeight = (rowHeight * rowCount) + headerHeight;

            int maxHeight = 500;
            dataGridView1.Height = Math.Min(totalHeight, maxHeight);
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
            roomsDropdown.SelectedIndex = 0;
        }

        private void PopulateStatusDropdown()
        {
            DataTable statusTable = new DataTable();
            statusTable.Columns.Add("status");

            statusTable.Rows.Add("All");
            statusTable.Rows.Add("Pending");
            statusTable.Rows.Add("Approved");

            statusDropdown.DataSource = statusTable;
            statusDropdown.DisplayMember = "status";
            statusDropdown.ValueMember = "status";
            statusDropdown.SelectedIndex = 0;
        }

        private void ScheduleTimer_Tick(object sender, EventArgs e)
        {
            DeleteExpiredSchedules();
        }

        private void DeleteExpiredSchedules()
        {
            string todayDay = DateTime.Now.DayOfWeek.ToString();
            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            try
            {
                // Fetch the expired schedules
                DataTable expiredSchedules = new DataTable();
                string selectExpiredQuery = $@"
            SELECT * FROM temporary_schedules 
            WHERE day = '{todayDay}' 
            AND class_end <= '{currentTime}'";

                expiredSchedules = dbHelper.GetData(selectExpiredQuery);

                // If there are expired schedules, delete them
                if (expiredSchedules.Rows.Count > 0)
                {
                    // Delete the expired schedules
                    string deleteQuery = $@"
                DELETE FROM temporary_schedules 
                WHERE day = '{todayDay}' 
                AND class_end <= '{currentTime}'";

                    dbHelper.ExecuteQuery(deleteQuery);

                    foreach (DataRow expiredRow in expiredSchedules.Rows)
                    {
                        foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
                        {
                            if (dgvRow.Cells["schedule_id"].Value.ToString() == expiredRow["schedule_id"].ToString())
                            {
                                dataGridView1.Rows.Remove(dgvRow);
                                break; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting expired schedules: {ex.Message}");
            }
        }


        private void roomsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void statusDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void searchFilter_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void approveButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Select"].Value) && row.Cells["status"].Value.ToString() == "Pending")
                    {
                        string scheduleId = row.Cells["schedule_id"].Value.ToString();

                        string updateQuery = $@"
                    UPDATE temporary_schedules 
                    SET status = 'Approved' 
                    WHERE schedule_id = '{scheduleId}'";

                        dbHelper.ExecuteQuery(updateQuery);

                        row.Cells["status"].Value = "Approved";
                    }
                }

                MessageBox.Show("Selected schedules have been approved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while approving schedules: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
