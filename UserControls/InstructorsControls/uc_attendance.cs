using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Windows.Input;

namespace Laboratory_Management_System.UserControls.InstructorsControls
{
    public partial class uc_attendance : UserControl
    {
        private DatabaseHelper dbHelper;
        private DataTable dataTable;
        private string instructorId;
        private PrintDocument printDocument1;
        private int totalWidth = 0;
        private int rowPos = 0;
        private int currentPage = 1;

        public uc_attendance(string instructorId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.instructorId = instructorId;
            printDocument1 = new PrintDocument();
            CustomizeDataGridView();
            LoadData(instructorId, date: DateTime.Today);
            LoadSubjects(instructorId);
            dataGridView1.Paint += DataGridView1_Paint;
            LoadFilteredData();
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

        private void LoadData(string instructorId, string subject = "All", string year = "All", string block = "All", string course = "All", DateTime? date = null, string searchTerm = "")
        {
            string query = @"
    SELECT attendance.workstation, attendance.student_id, attendance.schedule_id, 
           attendance.time_in, attendance.time_out, attendance.duration, attendance.attendance_date,
           CONCAT(students.firstname, ' ', students.middlename, ' ', students.lastname) AS student_name,
           schedules.subject, schedules.year, schedules.block, schedules.course, schedules.instructor
    FROM attendance
    INNER JOIN students ON attendance.student_id = students.student_id
    INNER JOIN schedules ON attendance.schedule_id = schedules.schedule_id
    WHERE schedules.instructor = @instructorId";

            if (subject != "All")
            {
                query += " AND schedules.subject = @subject";
            }
            if (course != "All")
            {
                query += " AND schedules.course = @course";
            }
            if (year != "All" && block != "All")
            {
                query += " AND schedules.year = @year AND schedules.block = @block";
            }
            if (date.HasValue)
            {
                query += " AND attendance.attendance_date = @date";
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += @" AND (students.firstname LIKE @searchTerm
                     OR students.middlename LIKE @searchTerm
                     OR students.lastname LIKE @searchTerm
                     OR attendance.workstation LIKE @searchTerm)";
            }

            query += " ORDER BY attendance.attendance_date DESC";

            MySqlParameter[] parameters = {
        new MySqlParameter("@instructorId", instructorId),
        new MySqlParameter("@subject", subject),
        new MySqlParameter("@course", course),
        new MySqlParameter("@year", year),
        new MySqlParameter("@block", block),
        new MySqlParameter("@date", date?.Date ?? (object)DBNull.Value), 
        new MySqlParameter("@searchTerm", $"%{searchTerm}%") 
    };

            dataTable = dbHelper.GetData(query, parameters);
            dataGridView1.DataSource = dataTable;

            dataGridView1.Columns["workstation"].HeaderText = "Workstation";
            dataGridView1.Columns["student_name"].HeaderText = "Student Name";
            dataGridView1.Columns["time_in"].HeaderText = "Time In";
            dataGridView1.Columns["time_out"].HeaderText = "Time Out";
            dataGridView1.Columns["duration"].HeaderText = "Duration";
            dataGridView1.Columns["attendance_date"].HeaderText = "Attendance Date";

            dataGridView1.Columns["student_name"].DisplayIndex = 0;
            dataGridView1.Columns["workstation"].DisplayIndex = 1;
            dataGridView1.Columns["time_in"].DisplayIndex = 2;
            dataGridView1.Columns["time_out"].DisplayIndex = 3;
            dataGridView1.Columns["duration"].DisplayIndex = 4;
            dataGridView1.Columns["attendance_date"].DisplayIndex = 5;

            dataGridView1.Columns["student_id"].Visible = false;
            dataGridView1.Columns["schedule_id"].Visible = false;
            dataGridView1.Columns["subject"].Visible = false;
            dataGridView1.Columns["year"].Visible = false;
            dataGridView1.Columns["block"].Visible = false;
            dataGridView1.Columns["instructor"].Visible = false;
            dataGridView1.Columns["course"].Visible = false;

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

        private void printButton_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument1.PrintPage += new PrintPageEventHandler(PrintDocument1_PrintPage);
                printDocument1.Print();
            }
        }

        private void PrintDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                int rowCount = dataGridView1.Rows.Count;
                bool morePagesToPrint = false;

                string selectedSubject = subjectsDropdown.SelectedValue.ToString();
                string selectedCourse = courseDropdown.SelectedValue.ToString();
                string selectedYearBlock = yearblockDropdown.SelectedValue.ToString().Replace(" ", "");

                Font titleFont = new Font("Montserrat", 16, FontStyle.Bold);
                Font subtitleFont = new Font("Montserrat", 10, FontStyle.Regular);

                string title = "Attendance Report";
                SizeF titleSize = e.Graphics.MeasureString(title, titleFont);
                e.Graphics.DrawString(title, titleFont, Brushes.Black,
                    new PointF((e.PageBounds.Width - titleSize.Width) / 2, e.MarginBounds.Top - 40));

                string reportDetails = $"{selectedSubject} | {selectedCourse} - {selectedYearBlock}";
                SizeF detailsSize = e.Graphics.MeasureString(reportDetails, subtitleFont);
                e.Graphics.DrawString(reportDetails, subtitleFont, Brushes.Black,
                    new PointF((e.PageBounds.Width - detailsSize.Width) / 2, e.MarginBounds.Top + titleSize.Height - 25));

                DateTime attendance = dateFilter.Value;
                string attendanceDate = "Attendance Date: " + attendance.ToString("MMMM d, yyyy");
                SizeF attendanceDateSize = e.Graphics.MeasureString(attendanceDate, subtitleFont);
                e.Graphics.DrawString(attendanceDate, subtitleFont, Brushes.Black,
                    new PointF((e.PageBounds.Width - attendanceDateSize.Width) / 2, e.MarginBounds.Top + titleSize.Height + detailsSize.Height - 5));

                DateTime date = DateTime.Now;
                string dateSubtitle = $"Generated on: {date:MMMM d, yyyy}";
                SizeF dateSubtitleSize = e.Graphics.MeasureString(dateSubtitle, subtitleFont);
                e.Graphics.DrawString(dateSubtitle, subtitleFont, Brushes.Black,
                    new PointF((e.PageBounds.Width - dateSubtitleSize.Width) / 2, e.MarginBounds.Top + titleSize.Height + detailsSize.Height + attendanceDateSize.Height + 5));


                float tableTopPos = e.MarginBounds.Top + titleSize.Height + dateSubtitleSize.Height + detailsSize.Height + 30;

                float totalWidth = 0;
                string[] columnOrder = { "student_name", "workstation", "time_in", "time_out", "duration" };
                float[] columnWidths = new float[columnOrder.Length];

                for (int i = 0; i < columnOrder.Length; i++)
                {
                    string colName = columnOrder[i];
                    if (dataGridView1.Columns[colName].Visible)
                    {
                        columnWidths[i] = dataGridView1.Columns[colName].Width;
                        totalWidth += columnWidths[i];
                    }
                }

                float maxWidth = e.MarginBounds.Width - 20;
                float scaleFactor = totalWidth > maxWidth ? maxWidth / totalWidth : 1;

                for (int i = 0; i < columnOrder.Length; i++)
                {
                    if (dataGridView1.Columns[columnOrder[i]].Visible)
                    {
                        columnWidths[i] *= scaleFactor;
                    }
                }

                float leftMargin = e.MarginBounds.Left;
                Font headerFont = new Font("Montserrat", 10, FontStyle.Bold);
                float headerHeight = 40;

                StringFormat headerFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                for (int i = 0; i < columnOrder.Length; i++)
                {
                    string colName = columnOrder[i];
                    if (!dataGridView1.Columns[colName].Visible)
                        continue;

                    e.Graphics.DrawString(dataGridView1.Columns[colName].HeaderText, headerFont, Brushes.Black,
                        new RectangleF(leftMargin, tableTopPos, columnWidths[i], headerHeight), headerFormat);

                    leftMargin += columnWidths[i];
                }

                int startRow = rowPos;
                for (int i = startRow; i < rowCount; i++)
                {
                    leftMargin = e.MarginBounds.Left;

                    StringFormat dataFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    for (int j = 0; j < columnOrder.Length; j++)
                    {
                        string colName = columnOrder[j];
                        if (!dataGridView1.Columns[colName].Visible)
                            continue;

                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[colName].Value?.ToString() ?? string.Empty,
                            dataGridView1.Font, Brushes.Black,
                            new RectangleF(leftMargin, tableTopPos + headerHeight + dataGridView1.RowTemplate.Height * (i - startRow), columnWidths[j], dataGridView1.RowTemplate.Height), dataFormat);

                        leftMargin += columnWidths[j];
                    }

                    if ((tableTopPos + headerHeight + dataGridView1.RowTemplate.Height * (i - startRow + 1)) >= e.MarginBounds.Bottom)
                    {
                        morePagesToPrint = true;
                        rowPos = i + 1;
                        break;
                    }
                }

                e.HasMorePages = morePagesToPrint;
                if (!morePagesToPrint)
                {
                    rowPos = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSubjects(string instructorId)
        {
            string query = "SELECT DISTINCT subject FROM schedules WHERE instructor = @instructorId";
            MySqlParameter[] parameters = {
        new MySqlParameter("@instructorId", instructorId)
    };

            DataTable subjectsTable = dbHelper.GetData(query, parameters);

            DataTable combinedSubjectsTable = new DataTable();
            combinedSubjectsTable.Columns.Add("subject", typeof(string));

            combinedSubjectsTable.Rows.Add("All");

            foreach (DataRow row in subjectsTable.Rows)
            {
                combinedSubjectsTable.Rows.Add(row["subject"]);
            }

            subjectsDropdown.DataSource = combinedSubjectsTable;
            subjectsDropdown.DisplayMember = "subject";
            subjectsDropdown.ValueMember = "subject";

            LoadCourses("All");
        }

        private void LoadCourses(string subject)
        {
            string query = @"
        SELECT DISTINCT course 
        FROM schedules 
        WHERE subject = @subject";

            MySqlParameter[] parameters = {
        new MySqlParameter("@subject", subject)
    };

            DataTable coursesTable = dbHelper.GetData(query, parameters);

            DataTable combinedCoursesTable = new DataTable();
            combinedCoursesTable.Columns.Add("course", typeof(string));

            combinedCoursesTable.Rows.Add("All");

            foreach (DataRow row in coursesTable.Rows)
            {
                combinedCoursesTable.Rows.Add(row["course"]);
            }

            courseDropdown.DataSource = combinedCoursesTable;
            courseDropdown.DisplayMember = "course";
            courseDropdown.ValueMember = "course";
        }

        private void subjectsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subjectsDropdown.SelectedValue != null)
            {
                string selectedSubject = subjectsDropdown.SelectedValue.ToString();
                LoadYearAndBlock(selectedSubject);
                LoadCourses(selectedSubject); 
            }
            LoadFilteredData();
        }

        private void LoadYearAndBlock(string subject)
        {
            string query = @"
        SELECT DISTINCT year, block 
        FROM schedules 
        WHERE subject = @subject";

            MySqlParameter[] parameters = {
        new MySqlParameter("@subject", subject)
        };

            DataTable yearBlockTable = dbHelper.GetData(query, parameters);

            DataTable combinedTable = new DataTable();
            combinedTable.Columns.Add("YearBlock", typeof(string));

            combinedTable.Rows.Add("All");

            foreach (DataRow row in yearBlockTable.Rows)
            {
                string year = row["year"].ToString();
                string block = row["block"].ToString();
                combinedTable.Rows.Add($"{year} {block}");
            }

            yearblockDropdown.DataSource = combinedTable;
            yearblockDropdown.DisplayMember = "YearBlock";
            yearblockDropdown.ValueMember = "YearBlock";
        }



        private void yearblockDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }

        private void searchFilter_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }

        private void dateFilter_ValueChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }

        private void LoadFilteredData()
        {
            string selectedSubject = subjectsDropdown.SelectedValue?.ToString() ?? "All";

            string yearblock = yearblockDropdown.SelectedValue?.ToString() ?? "All";
            string selectedYear = "All";
            string selectedBlock = "All";

            if (yearblock != "All")
            {
                var parts = yearblock.Split(' ');
                if (parts.Length == 2)
                {
                    selectedYear = parts[0];
                    selectedBlock = parts[1];
                }
            }

            DateTime? selectedDate = dateFilter.Checked ? (DateTime?)dateFilter.Value : null;

            string searchTerm = searchFilter.Text.Trim();

            string selectedCourse = courseDropdown.SelectedValue?.ToString() ?? "All";

            LoadData(instructorId, selectedSubject, selectedYear, selectedBlock, selectedCourse, selectedDate, searchTerm);
        }

        private void courseDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }
    }
}
