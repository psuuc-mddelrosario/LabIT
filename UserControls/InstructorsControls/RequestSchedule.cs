using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class RequestSchedule : Form
    {
        private DatabaseHelper dbHelper;
        private string userId;

        public RequestSchedule(string userId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            PopulateBlockDropdown();
            PopulateCourseDropdown();
            PopulateYearDropdown();
            PopulateSubjectDropdown();
            PopulateDayDropdown();
            PopulateRoomsDropdown();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.userId = userId;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulateDayDropdown()
        {
            var day = new List<string>
    {
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

        private void PopulateSubjectDropdown()
        {
            var subject = new List<string>
    {
        "Fundamentals of Programming",
        "Introduction to Computing",
    };

            subjectDropdown.DataSource = subject;
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

        private void PopulateRoomsDropdown()
        {
            string query = "SELECT laboratory_room FROM rooms";
            DataTable roomsTable = dbHelper.GetData(query);

            DataTable combinedRoomsTable = new DataTable();
            combinedRoomsTable.Columns.Add("laboratory_room");

            foreach (DataRow row in roomsTable.Rows)
            {
                combinedRoomsTable.ImportRow(row);
            }

            roomDropdown.DataSource = combinedRoomsTable;
            roomDropdown.DisplayMember = "laboratory_room";
            roomDropdown.ValueMember = "laboratory_room";
        }

        private void request_Click(object sender, EventArgs e)
        {
            string instructorId = userId;
            string course = courseDropdown.SelectedItem.ToString();
            string subject = subjectDropdown.SelectedItem.ToString();
            string year = yearDropdown.SelectedItem.ToString();
            string block = blockDropdown.SelectedItem.ToString();
            string day = dayDropdown.SelectedItem.ToString();
            string classStarts = classStart.Value.ToString("HH:mm");
            string classEnds = classEnd.Value.ToString("HH:mm");
            string room = roomDropdown.SelectedValue.ToString();

            // Insert query for the temporary_schedules table
            string query = "INSERT INTO `temporary_schedules`(`instructor`, `room`, `course`, `subject`, `year`, `block`, `day`, `class_start`, `class_end`) " +
                           "VALUES (@Instructor, @Room, @Course, @Subject, @Year, @Block, @Day, @ClassStart, @ClassEnd)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
        new MySqlParameter("@Instructor", instructorId),
        new MySqlParameter("@Room", room),
        new MySqlParameter("@Course", course),
        new MySqlParameter("@Subject", subject),
        new MySqlParameter("@Year", year),
        new MySqlParameter("@Block", block),
        new MySqlParameter("@Day", day),
        new MySqlParameter("@ClassStart", TimeSpan.Parse(classStarts)),
        new MySqlParameter("@ClassEnd", TimeSpan.Parse(classEnds))
            };

            try
            {
                dbHelper.ExecuteQuery(query, parameters);
                MessageBox.Show("Temporary schedule requested successfully!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
