using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class AddStudent : Form
    {
        private DatabaseHelper dbHelper; // Class-level dbHelper

        public AddStudent()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper(); // Initialize the database helper
            PopulateBlockDropdown();
            PopulateCourseDropdown();
            PopulateYearDropdown();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
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

        private void add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(studentId.Text) ||
                string.IsNullOrWhiteSpace(middlename.Text) ||
                string.IsNullOrWhiteSpace(lastname.Text) ||
                courseDropdown.SelectedItem == null ||
                yearDropdown.SelectedItem == null ||
                blockDropdown.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            string checkIdQuery = "SELECT COUNT(*) FROM students WHERE student_id = @student_id";
            MySqlParameter[] checkIdParameters = new MySqlParameter[]
            {
        new MySqlParameter("@student_id", studentId.Text.Trim())
            };

            int existingIdCount = Convert.ToInt32(dbHelper.GetData(checkIdQuery, checkIdParameters).Rows[0][0]);

            if (existingIdCount > 0)
            {
                MessageBox.Show("The student ID already exists");
                return; 
            }

            string checkQuery = "SELECT COUNT(*) FROM students WHERE firstname = @firstname AND middlename = @middlename AND lastname = @lastname";
            MySqlParameter[] checkParameters = new MySqlParameter[]
            {
        new MySqlParameter("@firstname", firstname.Text),
        new MySqlParameter("@middlename", middlename.Text),
        new MySqlParameter("@lastname", lastname.Text)
            };

            int existingStudentCount = Convert.ToInt32(dbHelper.GetData(checkQuery, checkParameters).Rows[0][0]);

            if (existingStudentCount > 0)
            {
                DialogResult dialogResult = MessageBox.Show("A student with the same name already exists. Do you want to add this student anyway?", "Duplicate Student", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return; 
                }
            }

            string newStudentId = studentId.Text.Trim();

            DateTime birthdate = birthdatePicker.Value;
            string tempPassword = birthdate.ToString("MMddyyyy"); 

            string insertQuery = "INSERT INTO students (student_id, password, firstname, middlename, lastname, course, year, block, date_added, birthdate) " +
                                 "VALUES (@student_id, @password, @firstname, @middlename, @lastname, @course, @year, @block, @date_added, @birthdate)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
        new MySqlParameter("@student_id", newStudentId),
        new MySqlParameter("@password", tempPassword),
        new MySqlParameter("@firstname", firstname.Text),
        new MySqlParameter("@middlename", middlename.Text),
        new MySqlParameter("@lastname", lastname.Text),
        new MySqlParameter("@course", courseDropdown.SelectedItem.ToString()),
        new MySqlParameter("@year", yearDropdown.SelectedItem.ToString()),
        new MySqlParameter("@block", blockDropdown.SelectedItem.ToString()),
        new MySqlParameter("@date_added", DateTime.Now),
        new MySqlParameter("@birthdate", birthdate) 
            };

            dbHelper.ExecuteQuery(insertQuery, parameters);

            MessageBox.Show("Student added successfully!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


    }
}
