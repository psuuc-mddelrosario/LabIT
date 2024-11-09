using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ExcelDataReader;
using static DevExpress.Data.Filtering.Helpers.PropertyDescriptorCriteriaCompilationSupport;
using MySql.Data.MySqlClient;
using Laboratory_Management_System.UserControls.AdminControls.Crud;

namespace Laboratory_Management_System.UserControls.AdminControls
{
    public partial class uc_students : UserControl
    {
        private DatabaseHelper dbHelper;
        private DataTable dataTable;
        public uc_students()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            CustomizeDataGridView();
            BlackText();
            PopulateBlockDropdown();
            PopulateCourseDropdown();
            PopulateYearDropdown();
            LoadData();
            dataGridView1.Paint += DataGridView1_Paint;
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
            // Start with the base query
            string query = @"
SELECT student_id, password, firstname, middlename, lastname, course, year, block, date_added
FROM students
WHERE 1=1"; // Always true, simplifies appending additional conditions.

            // Add filters based on the selected values in the dropdowns
            if (courseDropdown.SelectedIndex > 0) // Index 0 is "All"
            {
                string selectedCourse = courseDropdown.SelectedItem.ToString();
                query += $" AND course = '{selectedCourse}'";
            }

            if (yearDropdown.SelectedIndex > 0) // Index 0 is "All"
            {
                string selectedYear = yearDropdown.SelectedItem.ToString();
                query += $" AND year = '{selectedYear}'";
            }

            if (blockDropdown.SelectedIndex > 0) // Index 0 is "All"
            {
                string selectedBlock = blockDropdown.SelectedItem.ToString();
                query += $" AND block = '{selectedBlock}'";
            }

            // Add search filter if searchFilter is not empty
            if (!string.IsNullOrWhiteSpace(searchFilter.Text))
            {
                string searchValue = searchFilter.Text.Trim();
                query += $" AND (student_id LIKE '%{searchValue}%' " +
                         $"OR firstname LIKE '%{searchValue}%' " +
                         $"OR middlename LIKE '%{searchValue}%' " +
                         $"OR lastname LIKE '%{searchValue}%' " +
                         $"OR course LIKE '%{searchValue}%')";
            }

            // Order the results
            query += " ORDER BY date_added DESC";

            // Execute the query and populate the DataGridView
            dataTable = dbHelper.GetData(query);
            dataGridView1.DataSource = dataTable;

            // Set the headers for each column
            dataGridView1.Columns["student_id"].HeaderText = "Student Id";
            dataGridView1.Columns["password"].HeaderText = "Password";
            dataGridView1.Columns["firstname"].HeaderText = "Firstname";
            dataGridView1.Columns["middlename"].HeaderText = "Middlename";
            dataGridView1.Columns["lastname"].HeaderText = "Lastname";
            dataGridView1.Columns["course"].HeaderText = "Course";
            dataGridView1.Columns["year"].HeaderText = "Year";
            dataGridView1.Columns["block"].HeaderText = "Block";
            dataGridView1.Columns["date_added"].HeaderText = "Date Added";

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

        private void deleteButton_Click_1(object sender, EventArgs e)
        {
                var studentIdsToDelete = new List<string>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Select"].Value))
                    {
                        string studentId = row.Cells["student_id"].Value.ToString();
                    studentIdsToDelete.Add(studentId);
                    }
                }

                if (studentIdsToDelete.Count > 0)
                {
                    var confirmResult = MessageBox.Show(
                        "Are you sure you want to delete the selected students?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmResult == DialogResult.Yes)
                    {
                        string ids = string.Join("','", studentIdsToDelete);
                        string query = $"DELETE FROM students WHERE student_id IN ('{ids}')";

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
                    MessageBox.Show("No students selected for deletion.");
                }
            
        }

        private void BlackText()
        {
            label1.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;
            label6.ForeColor = Color.Black;
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
        "E",
    };

            blockDropdown.DataSource = block;
        }

        private void PopulateCourseDropdown()
        {
            var course = new List<string>
    {
        "All",
        "BSIT",
        "BSCOE",
        "BSCE",
    };

            courseDropdown.DataSource = course;
        }

        private void excelButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.csv",
                Title = "Select an Excel or CSV File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DataTable dataTable = new DataTable();

                try
                {
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });

                            dataTable = result.Tables[0];
                        }
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string studentId = row["StudentID"].ToString(); 
                        string firstname = row["Firstname"].ToString();
                        string middlename = row["Middlename"].ToString();
                        string lastname = row["Lastname"].ToString();
                        string course = row["Course"].ToString();
                        string year = row["Year"].ToString();
                        string block = row["Block"].ToString();
                   
                        string tempPassword = "temp1234"; 
                        if (row.Table.Columns.Contains("Birthdate") && DateTime.TryParse(row["Birthdate"].ToString(), out DateTime birthdate))
                        {
                            tempPassword = birthdate.ToString("MMddyyyy"); 
                        }

                        string query = $@"
                INSERT INTO students (student_id, password, firstname, middlename, lastname, course, year, block)
                VALUES ('{studentId}', '{tempPassword}', '{firstname}', '{middlename}', '{lastname}', '{course}', '{year}', '{block}')
                ON DUPLICATE KEY UPDATE
                    firstname = '{firstname}',
                    middlename = '{middlename}',
                    lastname = '{lastname}',
                    course = '{course}',
                    year = '{year}',
                    block = '{block}'";

                        dbHelper.ExecuteQuery(query);
                    }

                    MessageBox.Show("Students added successfully.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }



        private void searchFilter_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddStudent addStudentForm = new AddStudent();

            DialogResult result = addStudentForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            string studentId = null;
            int selectedCount = 0; 

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

                if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
                {
                    studentId = row.Cells["student_id"].Value.ToString();
                    selectedCount++; 
                    if (selectedCount > 1)
                    {
                        MessageBox.Show("Please select only one student to edit.");
                        return; 
                    }
                }
            }

            if (selectedCount == 1)
            {
                EditStudent editStudentForm = new EditStudent(studentId);

                DialogResult result = editStudentForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Please select a student to edit.");
            }
        }

    }
}
