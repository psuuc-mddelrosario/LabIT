using ExcelDataReader;
using Laboratory_Management_System.UserControls.AdminControls.Crud;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Data.Filtering.Helpers.PropertyDescriptorCriteriaCompilationSupport;

namespace Laboratory_Management_System.UserControls.AdminControls
{
    public partial class uc_users : UserControl
    {
        private DatabaseHelper dbHelper;
        private DataTable dataTable;

        public uc_users()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            CustomizeDataGridView();
            PopulateRoleDropdown();
            PopulateRoomsDropdown();
            BlackText();
            LoadData();
            dataGridView1.Paint += DataGridView1_Paint;
        }
        private void PopulateRoleDropdown()
        {
            roleDropdown.Items.Clear(); 
            roleDropdown.Items.Add("All");
            roleDropdown.Items.Add("Admin");
            roleDropdown.Items.Add("Instructor");
            roleDropdown.Items.Add("Incharge");
            roleDropdown.Items.Add("Instructor/Incharge");

            roleDropdown.SelectedIndex = 0; 
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
            string selectedRole = roleDropdown.SelectedItem?.ToString() ?? "All";  // Ensure roleDropdown has a value
            string selectedRoom = roomsDropdown.SelectedValue?.ToString() ?? "All";  // Ensure roomsDropdown has a value

            string query = @"
    SELECT user_id, password, username, role, assigned_room, date_added
    FROM users";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            // Add role filter
            if (selectedRole != "All")
            {
                query += " WHERE role = @role";
                parameters.Add(new MySqlParameter("@role", selectedRole));
            }

            // Add room filter
            if (selectedRoom != "All")
            {
                if (parameters.Count == 0)
                {
                    query += " WHERE ";
                }
                else
                {
                    query += " AND ";
                }

                query += "assigned_room = @assignedRoom";
                parameters.Add(new MySqlParameter("@assignedRoom", selectedRoom));
            }

            // Add search filter
            if (!string.IsNullOrWhiteSpace(searchFilter.Text))
            {
                string searchValue = searchFilter.Text.Trim();
                if (parameters.Count == 0)
                {
                    query += " WHERE ";
                }
                else
                {
                    query += " AND ";
                }

                query += "(user_id LIKE @searchValue " +
                         "OR username LIKE @searchValue " +
                         "OR firstname LIKE @searchValue " +
                         "OR middlename LIKE @searchValue " +
                         "OR lastname LIKE @searchValue " +
                         "OR role LIKE @searchValue " +
                         "OR assigned_room LIKE @searchValue)";

                parameters.Add(new MySqlParameter("@searchValue", "%" + searchValue + "%"));
            }

            query += " ORDER BY date_added DESC";

            // Execute query with parameters
            dataTable = dbHelper.GetData(query, parameters.ToArray());
            dataGridView1.DataSource = dataTable;

            dataGridView1.Columns["user_id"].HeaderText = "User Id";
            dataGridView1.Columns["password"].HeaderText = "Password";
            dataGridView1.Columns["username"].HeaderText = "Username";
            dataGridView1.Columns["role"].HeaderText = "Role";
            dataGridView1.Columns["assigned_room"].HeaderText = "Assigned Room";
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

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var userIdsToDelete = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    string userId = row.Cells["user_id"].Value.ToString();
                    userIdsToDelete.Add(userId);
                }
            }

            if (userIdsToDelete.Count > 0)
            {
                var confirmResult = MessageBox.Show(
                    "Are you sure you want to delete the selected users?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    string ids = string.Join("','", userIdsToDelete);
                    string query = $"DELETE FROM users WHERE user_id IN ('{ids}')";

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
                MessageBox.Show("No users selected for deletion.");
            }
        }

        private void roleDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(); 
        }

        private void BlackText()
        {
            label4.ForeColor = Color.Black;
            searchText.ForeColor = Color.Black;
            label1.ForeColor = Color.Black; 
        }

        private void searchFilter_TextChanged(object sender, EventArgs e)
        {
            LoadData();
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

        private void roomsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddUser addUserForm = new AddUser();

            DialogResult result = addUserForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            string userId = null;
            int selectedCount = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

                if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
                {
                    userId = row.Cells["user_id"].Value.ToString();
                    selectedCount++;
                    if (selectedCount > 1)
                    {
                        MessageBox.Show("Please select only one user to edit.");
                        return;
                    }
                }
            }

            if (selectedCount == 1)
            {
                EditUser editUserForm = new EditUser(userId);

                DialogResult result = editUserForm.ShowDialog();

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
                bool allRowsProcessed = true;

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
                        try
                        {
                            string firstname = row["Firstname"].ToString();
                            string middlename = row["Middlename"].ToString();
                            string lastname = row["Lastname"].ToString();
                            string role = row["Role"].ToString();
                            string room = row["AssignedRoom"].ToString();

                            Random rnd = new Random();
                            int randomNum = rnd.Next(100, 999); 
                            string username = $"{firstname[0].ToString().ToLower()}{lastname.ToLower()}{randomNum}";

                            string prefix = "U";
                            string idQuery = $"SELECT user_id FROM users WHERE user_id LIKE '{prefix}%' ORDER BY user_id DESC LIMIT 1";
                            DataTable idResult = dbHelper.GetData(idQuery);

                            int lastNumber = 0;
                            if (idResult.Rows.Count > 0)
                            {
                                string lastId = idResult.Rows[0]["user_id"].ToString();
                                lastNumber = int.Parse(lastId.Substring(1));
                            }

                            string userId = $"{prefix}{(lastNumber + 1).ToString("D3")}";

                            string tempPassword = "temp1234";
                            DateTime? birthdate = null;
                            if (row.Table.Columns.Contains("Birthdate") && DateTime.TryParse(row["Birthdate"].ToString(), out DateTime parsedBirthdate))
                            {
                                birthdate = parsedBirthdate;
                                tempPassword = birthdate.Value.ToString("MMddyyyy");
                            }

                            string query = @"
                    INSERT INTO users (user_id, username, password, firstname, middlename, lastname, role, assigned_room, birthdate)
                    VALUES (@user_id, @username, @password, @firstname, @middlename, @lastname, @role, @room, @birthdate)
                    ON DUPLICATE KEY UPDATE
                        firstname = @firstname,
                        middlename = @middlename,
                        lastname = @lastname,
                        role = @role,
                        assigned_room = @room,
                        birthdate = @birthdate";

                            MySqlParameter[] parameters = new MySqlParameter[]
                            {
                        new MySqlParameter("@user_id", userId),
                        new MySqlParameter("@username", username),
                        new MySqlParameter("@password", tempPassword),
                        new MySqlParameter("@firstname", firstname),
                        new MySqlParameter("@middlename", middlename),
                        new MySqlParameter("@lastname", lastname),
                        new MySqlParameter("@role", role),
                        new MySqlParameter("@room", room),
                        new MySqlParameter("@birthdate", birthdate.HasValue ? (object)birthdate.Value : DBNull.Value)
                            };

                            dbHelper.ExecuteQuery(query, parameters);
                        }
                        catch (Exception rowEx)
                        {
                            allRowsProcessed = false;
                            MessageBox.Show($"Error processing row for user {row["Firstname"]} {row["Lastname"]}: {rowEx.Message}");
                        }
                    }

                    if (allRowsProcessed)
                    {
                        MessageBox.Show("Users added successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Some users could not be added due to errors.");
                    }

                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"A fatal error occurred: {ex.Message}");
                }
            }
        }
    }
}
