using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class AddSchedule : Form
    {
        private DatabaseHelper dbHelper; 

        public AddSchedule()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            PopulateBlockDropdown();
            PopulateCourseDropdown();
            PopulateYearDropdown();
            PopulateSubjectDropdown();
            PopulateDayDropdown();
            InitializeDateTimePickers();
            PopulateInstructorDropdown();
            PopulateRoomDropdown();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
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

        private void add_Click(object sender, EventArgs e)
        {

            string instructorId = instructorDropdown.SelectedValue.ToString();
            string course = courseDropdown.SelectedItem.ToString();
            string subject = subjectDropdown.SelectedItem.ToString();
            string year = yearDropdown.SelectedItem.ToString();
            string block = blockDropdown.SelectedItem.ToString();
            string day = dayDropdown.SelectedItem.ToString();
            string classStarts = classStart.Value.ToString("HH:mm");
            string classEnds = classEnd.Value.ToString("HH:mm");

            string room = roomDropdown.SelectedItem.ToString();

            string conflictMessage = CheckForConflicts(room, day, classStarts, classEnds, instructorId);
            if (conflictMessage != null)
            {
                MessageBox.Show(conflictMessage);
                return; 
            }

            string query = "INSERT INTO `schedules`(`instructor`, `course`, `subject`, `year`, `block`, `day`, `class_start`, `class_end`, `room`) " +
                           "VALUES (@Instructor, @Course, @Subject, @Year, @Block, @Day, @ClassStart, @ClassEnd, @Room)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
        new MySqlParameter("@Instructor", instructorId),
        new MySqlParameter("@Course", course),
        new MySqlParameter("@Subject", subject),
        new MySqlParameter("@Year", year),
        new MySqlParameter("@Block", block),
        new MySqlParameter("@Day", day),
        new MySqlParameter("@ClassStart", TimeSpan.Parse(classStarts)),
        new MySqlParameter("@ClassEnd", TimeSpan.Parse(classEnds)),
        new MySqlParameter("@Room", room)
            };

            try
            {
                dbHelper.ExecuteQuery(query, parameters);
                MessageBox.Show("Schedule added successfully!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void classStart_ValueChanged(object sender, EventArgs e)
        {

        }

        private void classEnd_ValueChanged(object sender, EventArgs e)
        {

        }
        private void InitializeDateTimePickers()
        {
            classStart.Format = DateTimePickerFormat.Custom;     
            classStart.CustomFormat = "HH:mm";   
            classStart.ShowUpDown = true;    

            classEnd.Format = DateTimePickerFormat.Custom;
            classEnd.CustomFormat = "HH:mm";
            classEnd.ShowUpDown = true;
        }

        private void instructorDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PopulateInstructorDropdown()
        {
            string query = "SELECT user_id, firstname, lastname FROM users WHERE role IN ('Instructor', 'Instructor/Incharge')";

            try
            {

                DataTable dataTable = dbHelper.GetData(query);

                DataTable instructorTable = new DataTable();
                instructorTable.Columns.Add("FullName", typeof(string));
                instructorTable.Columns.Add("UserId", typeof(string));

                foreach (DataRow row in dataTable.Rows)
                {
                    string userId = row["user_id"].ToString(); 
                    string firstName = row["firstname"].ToString();
                    string lastName = row["lastname"].ToString();
              
                    instructorTable.Rows.Add($"{firstName} {lastName}", userId);
                }

                instructorDropdown.DataSource = instructorTable;
                instructorDropdown.DisplayMember = "FullName"; 
                instructorDropdown.ValueMember = "UserId";   
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void courseDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void subjectDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void yearDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void blockDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dayDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PopulateRoomDropdown()
        {
            var rooms = new List<string>
    {
        "Lab 1",
        "Lab 2",
        "Lab 3",
        "Lab 4",
        "Lab 5",
        "Multimedia",
    };

            roomDropdown.DataSource = rooms;
        }

        private string CheckForConflicts(string room, string day, string classStarts, string classEnds, string instructorId)
        {
            // Check for room conflicts
            string roomQuery = "SELECT COUNT(*) as ConflictCount FROM `schedules` WHERE `room` = @Room AND `day` = @Day " +
                               "AND ((`class_start` < @ClassEnd AND `class_end` > @ClassStart))";

            MySqlParameter[] roomParameters = new MySqlParameter[]
            {
        new MySqlParameter("@Room", room),
        new MySqlParameter("@Day", day),
        new MySqlParameter("@ClassStart", TimeSpan.Parse(classStarts)),
        new MySqlParameter("@ClassEnd", TimeSpan.Parse(classEnds))
            };

            // Check for instructor conflicts
            string instructorQuery = "SELECT COUNT(*) as ConflictCount FROM `schedules` WHERE `instructor` = @Instructor " +
                                      "AND `day` = @Day " +
                                      "AND ((`class_start` < @ClassEnd AND `class_end` > @ClassStart))";

            MySqlParameter[] instructorParameters = new MySqlParameter[]
            {
        new MySqlParameter("@Instructor", instructorId),
        new MySqlParameter("@Day", day),
        new MySqlParameter("@ClassStart", TimeSpan.Parse(classStarts)),
        new MySqlParameter("@ClassEnd", TimeSpan.Parse(classEnds))
            };

            try
            {
                // Check for room conflicts
                DataTable roomResultTable = dbHelper.GetData(roomQuery, roomParameters);
                if (roomResultTable != null && roomResultTable.Rows.Count > 0)
                {
                    int roomConflictCount = Convert.ToInt32(roomResultTable.Rows[0]["ConflictCount"]);
                    if (roomConflictCount > 0)
                    {
                        return "A schedule conflict was detected for the selected room.";
                    }
                }

                // Check for instructor conflicts
                DataTable instructorResultTable = dbHelper.GetData(instructorQuery, instructorParameters);
                if (instructorResultTable != null && instructorResultTable.Rows.Count > 0)
                {
                    int instructorConflictCount = Convert.ToInt32(instructorResultTable.Rows[0]["ConflictCount"]);
                    if (instructorConflictCount > 0)
                    {
                        return "A schedule conflict was detected for the selected instructor.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error checking for conflicts: {ex.Message}";
            }

            return null; // No conflicts
        }


    }
}
