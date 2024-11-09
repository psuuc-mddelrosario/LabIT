using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls
{
    public partial class uc_calendar_schedules : UserControl
    {
        private DatabaseHelper dbHelper;
        private DataTable dataTable;
        private DateTime currentMonth;
        private List<Panel> dayPanels;
        private Label lblCurrentMonth;
        private Button btnPreviousMonth, btnNextMonth;

        public uc_calendar_schedules()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            currentMonth = DateTime.Now;

            BlackText();
            InitializeNavigationControls();
            PopulateRoomsDropdown();
            PopulateCourseDropdown();
            PopulateYearDropdown();
            PopulateBlockDropdown();
            PopulateDayDropdown();

            // Create and display calendar for the current month
            CreateCalendar();
            DisplayCurrentMonth();
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

        private void InitializeNavigationControls()
        {
            // Initialize month label
            lblCurrentMonth = new Label
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top
            };
            panel2.Controls.Add(lblCurrentMonth);

            // Initialize previous month button
            btnPreviousMonth = new Button
            {
                Text = "<",
                Width = 40,
                Height = 30,
                Location = new Point(10, lblCurrentMonth.Bottom + 5)
            };
            btnPreviousMonth.Click += btnPreviousMonth_Click;
            panel2.Controls.Add(btnPreviousMonth);

            // Initialize next month button
            btnNextMonth = new Button
            {
                Text = ">",
                Width = 40,
                Height = 30,
                Location = new Point(panel2.Width - 50, lblCurrentMonth.Bottom + 5)
            };
            btnNextMonth.Click += btnNextMonth_Click;
            panel2.Controls.Add(btnNextMonth);

            lblCurrentMonth.Location = new Point((panel2.Width - lblCurrentMonth.Width) / 2, btnPreviousMonth.Top);
            UpdateMonthLabel();
        }

        private void UpdateMonthLabel()
        {
            lblCurrentMonth.Text = $"{currentMonth:MMMM yyyy}";
        }

        private void CreateCalendar()
        {
            panel2.Controls.Clear();
            dayPanels = new List<Panel>();

            // Add the navigation controls to panel2 before creating the calendar grid
            InitializeNavigationControls();

            // Calculate the available height for the calendar
            int gridTop = btnNextMonth.Bottom + 30; // Space between navigation controls and calendar
            int availableHeight = panel2.Height - gridTop; // Remaining height for calendar panels
            int panelWidth = panel2.Width / 7; // Adjust based on the number of columns
            int panelHeight = availableHeight / 6; // Adjust this for the number of rows (max 6 rows)

            // Day names array
            string[] dayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            // Add day labels at the top
            for (int col = 0; col < 7; col++)
            {
                Label dayLabel = new Label
                {
                    Text = dayNames[col],
                    Size = new Size(panelWidth, 30), // Height for the day label
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    BackColor = Color.LightGray
                };

                panel2.Controls.Add(dayLabel);
                dayLabel.Location = new Point(col * panelWidth, gridTop - 30); // Position above day panels
            }

            // Create day panels for the calendar
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    Panel dayPanel = new Panel
                    {
                        Size = new Size(panelWidth, panelHeight),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White,
                        Location = new Point(col * panelWidth, gridTop + row * panelHeight)
                    };

                    Label dayLabel = new Label
                    {
                        Location = new Point(5, 5),
                        AutoSize = true,
                        Font = new Font("Arial", 9, FontStyle.Bold),
                        ForeColor = Color.Black
                    };
                    dayPanel.Controls.Add(dayLabel);

                    panel2.Controls.Add(dayPanel);
                    dayPanels.Add(dayPanel);
                }
            }
        }

        private void DisplayCurrentMonth()
        {
            DateTime firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            int startDayColumn = (int)firstDayOfMonth.DayOfWeek;
            if (startDayColumn == 0) startDayColumn = 7;

            int dayCounter = 1;
            int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);

            foreach (Panel dayPanel in dayPanels)
            {
                dayPanel.Controls[0].Text = "";
                dayPanel.BackColor = Color.White;
            }

            for (int i = startDayColumn - 1; i < dayPanels.Count && dayCounter <= daysInMonth; i++)
            {
                dayPanels[i].Controls[0].Text = dayCounter.ToString();
                dayCounter++;
            }
            UpdateMonthLabel();
        }

        private void ShowSchedulesOnCalendar()
        {
            LoadSchedules();

            /*foreach (Panel dayPanel in dayPanels)
            {
                string dayText = dayPanel.Controls[0].Text;
                if (int.TryParse(dayText, out int dayNumber))
                {
                    DateTime currentDate = new DateTime(currentMonth.Year, currentMonth.Month, dayNumber);

                    var schedulesForDay = dataTable.AsEnumerable()
                        .Where(row => DateTime.TryParse(row["day"].ToString(), out DateTime scheduleDate) &&
                                      scheduleDate.Date == currentDate);

                    if (schedulesForDay.Any())
                    {
                        dayPanel.BackColor = Color.LightBlue;
                        string scheduleDetails = string.Join("\n", schedulesForDay.Select(row =>
                            $"{row["course"]} {row["subject"]} - {row["instructor"]}"));

                        Label scheduleLabel = new Label
                        {
                            Text = scheduleDetails,
                            Location = new Point(5, 20),
                            AutoSize = true,
                            ForeColor = Color.Black
                        };

                        dayPanel.Controls.Add(scheduleLabel);
                    }
                }
            }*/
        }

        private void btnPreviousMonth_Click(object sender, EventArgs e)
        {
            currentMonth = currentMonth.AddMonths(-1);
            DisplayCurrentMonth();
            ShowSchedulesOnCalendar();
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            currentMonth = currentMonth.AddMonths(1);
            DisplayCurrentMonth();
            ShowSchedulesOnCalendar();
        }

        private void LoadSchedules()
        {
            // Load schedules from the database and populate the dataTable
        }

        private void PopulateRoomsDropdown() { /* Your code here */ }
        private void PopulateCourseDropdown() { /* Your code here */ }
        private void PopulateYearDropdown() { /* Your code here */ }
        private void PopulateBlockDropdown() { /* Your code here */ }
        private void PopulateDayDropdown() { /* Your code here */ }

        // Handle resizing of the control
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CreateCalendar();
            DisplayCurrentMonth();
            ShowSchedulesOnCalendar();
        }
    }
}
