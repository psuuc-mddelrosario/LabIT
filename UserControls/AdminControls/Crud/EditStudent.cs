using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class EditStudent : Form
    {
        private string studentId;
        private DatabaseHelper dbHelper; // Assume you have a DatabaseHelper class for database operations
        public EditStudent(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            dbHelper = new DatabaseHelper(); // Initialize your database helper
            PopulateBlockDropdown();
            PopulateCourseDropdown();
            PopulateYearDropdown();
            LoadStudentData(); // Load and populate data
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void PopulateCourseDropdown()
        {
            var course = new List<string>
            {
                "BSIT",
                "BSCOE",
                "BSCE",
            };

            courseDropdown.DataSource = course;
        }

        private void PopulateBlockDropdown()
        {
            var block = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E",
            };

            blockDropdown.DataSource = block;
        }

        private void PopulateYearDropdown()
        {
            var year = new List<string>
            {
                "1",
                "2",
                "3",
                "4",
            };

            yearDropdown.DataSource = year;
        }

        private void LoadStudentData()
        {
            // Query to fetch the student data based on the student_id
            string query = "SELECT password, firstname, middlename, lastname, course, year, block FROM students WHERE student_id = @studentId";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@studentId", studentId)
            };

            DataTable studentData = dbHelper.GetData(query, parameters);

            if (studentData.Rows.Count > 0)
            {
                DataRow row = studentData.Rows[0];

                // Populate the textboxes and dropdowns
                firstname.Text = row["firstname"].ToString();
                middlename.Text = row["middlename"].ToString();
                lastname.Text = row["lastname"].ToString();
                password.Text = row["password"].ToString();
                courseDropdown.SelectedItem = row["course"].ToString();
                yearDropdown.SelectedItem = row["year"].ToString();
                blockDropdown.SelectedItem = row["block"].ToString();
            }
            else
            {
                MessageBox.Show("Student not found.");
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            string query = @"UPDATE students 
                             SET password = @password, firstname = @firstName, middlename = @middlename, lastname = @lastName, course = @course, year = @year, block = @block 
                             WHERE student_id = @studentId";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@firstName", firstname.Text),
                new MySqlParameter("@middlename", middlename.Text),
                new MySqlParameter("@lastName", lastname.Text),
                new MySqlParameter("@password", password.Text),
                new MySqlParameter("@course", courseDropdown.SelectedItem.ToString()),
                new MySqlParameter("@year", yearDropdown.SelectedItem.ToString()),
                new MySqlParameter("@block", blockDropdown.SelectedItem.ToString()),
                new MySqlParameter("@studentId", studentId)
            };

            try
            {
                dbHelper.ExecuteQuery(query, parameters);
                MessageBox.Show("Student updated successfully.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating student: " + ex.Message);
            }
        }
    }
}
