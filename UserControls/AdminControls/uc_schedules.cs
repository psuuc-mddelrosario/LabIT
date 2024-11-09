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
using static DevExpress.Data.Filtering.Helpers.PropertyDescriptorCriteriaCompilationSupport;

namespace Laboratory_Management_System.UserControls.AdminControls
{
    public partial class uc_schedules : UserControl
    {
        private DatabaseHelper dbHelper;
        private DataTable dataTable;
        public uc_schedules()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            BlackText();
            CustomizeDataGridView();
            PopulateRoomsDropdown();
            PopulateCourseDropdown();
            PopulateYearDropdown();
            PopulateBlockDropdown();
            PopulateDayDropdown();
            LoadData();
            dataGridView1.Paint += DataGridView1_Paint;
        }

        private void BlackText()
        {
            label1.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label7.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label8.ForeColor = Color.Black;
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
        SELECT s.schedule_id, 
               CONCAT(u.firstname, ' ', u.lastname) AS instructor,
               s.room,
               s.subject, 
               s.course, 
               s.year, 
               s.block, 
               s.day,
               s.class_start, 
               s.class_end,       
               s.date_added
        FROM schedules s
        INNER JOIN users u ON s.instructor = u.user_id 
        {filter}
        ORDER BY s.date_added DESC;";

                dataTable = dbHelper.GetData(query);
                dataGridView1.DataSource = dataTable;

                dataGridView1.Columns["schedule_id"].Visible = false;

                // Customize headers
                dataGridView1.Columns["instructor"].HeaderText = "Instructor";
                dataGridView1.Columns["room"].HeaderText = "Room";
                dataGridView1.Columns["subject"].HeaderText = "Subject";
                dataGridView1.Columns["course"].HeaderText = "Course";
                dataGridView1.Columns["year"].HeaderText = "Year";
                dataGridView1.Columns["block"].HeaderText = "Block";
                dataGridView1.Columns["day"].HeaderText = "Day";
                dataGridView1.Columns["class_start"].HeaderText = "Class Start";
                dataGridView1.Columns["class_end"].HeaderText = "Class End";
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
            // Initialize empty filter
            List<string> conditions = new List<string>();

            if (roomsDropdown.SelectedValue != null && roomsDropdown.SelectedValue.ToString() != "All")
            {
                conditions.Add($"s.room = '{roomsDropdown.SelectedValue}'");
            }

            if (courseDropdown.SelectedValue != null && courseDropdown.SelectedValue.ToString() != "All")
            {
                conditions.Add($"s.course = '{courseDropdown.SelectedValue}'");
            }

            if (yearDropdown.SelectedValue != null && yearDropdown.SelectedValue.ToString() != "All")
            {
                conditions.Add($"s.year = '{yearDropdown.SelectedValue}'");
            }

            if (blockDropdown.SelectedValue != null && blockDropdown.SelectedValue.ToString() != "All")
            {
                conditions.Add($"s.block = '{blockDropdown.SelectedValue}'");
            }

            if (dayDropdown.SelectedValue != null && dayDropdown.SelectedValue.ToString() != "All")
            {
                conditions.Add($"s.day = '{dayDropdown.SelectedValue}'");
            }

            if (!string.IsNullOrEmpty(searchFilter.Text))
            {

                conditions.Add($"(u.firstname LIKE '%{searchFilter.Text}%' OR u.lastname LIKE '%{searchFilter.Text}%' OR s.subject LIKE '%{searchFilter.Text}%' OR s.room LIKE '%{searchFilter.Text}%' OR s.course LIKE '%{searchFilter.Text}%' OR s.subject LIKE '%{searchFilter.Text}%' OR s.year LIKE '%{searchFilter.Text}%' OR s.block LIKE '%{searchFilter.Text}%' OR s.day LIKE '%{searchFilter.Text}%')");
            }

            if (conditions.Count > 0)
            {
                return "WHERE " + string.Join(" AND ", conditions);
            }
            return "";
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

        private void PopulateCourseDropdown()
        {
            var course = new List<string>
    {
        "All",
        "BSIT",
        "BSCE",
    };

            courseDropdown.DataSource = course;
        }

        private void PopulateYearDropdown()
        {
            var year = new List<string>
    {
        "All",
        "1",
        "2",
        "3",
        "4",
    };

            yearDropdown.DataSource = year;
        }
        private void PopulateBlockDropdown()
        {
            var block = new List<string>
    {
        "All",
        "A",
        "B",
        "C",
        "D",
    };

            blockDropdown.DataSource = block;
        }

        private void PopulateDayDropdown()
        {
            var day = new List<string>
    {
        "All",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday",
        "Sunday",
    };

            dayDropdown.DataSource = day;
        }

        private void roomsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void courseDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void yearDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void blockDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dayDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var scheduleIdsToDelete = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    string studentId = row.Cells["schedule_id"].Value.ToString();
                    scheduleIdsToDelete.Add(studentId);
                }
            }

            if (scheduleIdsToDelete.Count > 0)
            {
                var confirmResult = MessageBox.Show(
                    "Are you sure you want to delete the selected schedules?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    string ids = string.Join("','", scheduleIdsToDelete);
                    string query = $"DELETE FROM schedules WHERE schedule_id IN ('{ids}')";

                    try
                    {
                        dbHelper.ExecuteQuery(query);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("No schedules selected for deletion.");
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            int selectedCount = 0;
            string selectedScheduleId = null;
   
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    selectedCount++;
                    if (selectedCount == 1)
                    {
                        selectedScheduleId = row.Cells["schedule_id"].Value.ToString();
                    }
                }
            }

            if (selectedCount > 1)
            {
                MessageBox.Show("Please select only one schedule to edit.");
                return;
            }

            if (selectedCount == 0)
            {
                MessageBox.Show("Please select a schedule to edit.");
                return;
            }

            EditSchedule editScheduleForm = new EditSchedule(selectedScheduleId);
            DialogResult result = editScheduleForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                LoadData(); 
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddSchedule addScheduleForm = new AddSchedule();

            DialogResult result = addScheduleForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void searchFilter_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

    }
}
