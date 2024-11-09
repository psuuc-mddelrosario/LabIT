using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Laboratory_Management_System.AddingUpdatingForms
{
    public partial class Add_Workstation : Form
    {
        private DatabaseHelper dbHelper;
        private string loggedInUserId; // Assuming you store the logged-in user’s ID

        public Add_Workstation(string userId) // Pass the userId of the logged-in user
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            loggedInUserId = "admin123"; // Save the logged-in user's ID
            LoadLabs();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Method to load laboratory rooms into the dropdown
        private void LoadLabs()
        {
            try
            {
                string query = "SELECT laboratory_room FROM rooms";
                DataTable labsData = dbHelper.GetData(query);
                labDropdown.DataSource = labsData;
                labDropdown.DisplayMember = "laboratory_room";
                labDropdown.ValueMember = "laboratory_room";

                // Add a placeholder option for the dropdown
                DataRow placeholderRow = labsData.NewRow();
                placeholderRow["laboratory_room"] = "Select Lab";
                labsData.Rows.InsertAt(placeholderRow, 0);
                labDropdown.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading labs: {ex.Message}");
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (labDropdown.SelectedIndex == 0 || string.IsNullOrEmpty(systemUnit.Text) ||
                string.IsNullOrEmpty(monitor.Text) || string.IsNullOrEmpty(keyboard.Text) ||
                string.IsNullOrEmpty(mouse.Text) || string.IsNullOrEmpty(avr.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Get the values from the form
            string systemUnitValue = systemUnit.Text;
            string room = labDropdown.SelectedValue.ToString();
            string monitorValue = monitor.Text;
            string keyboardValue = keyboard.Text;
            string mouseValue = mouse.Text;
            string avrValue = avr.Text;
            string workstationId = GenerateWorkstationId();
            string addedBy = GetUserNameById(loggedInUserId);
            DateTime dateAdded = DateTime.Now;

            // Construct SQL query to insert the workstation into the database using parameterized query
            string insertQuery = @"
                INSERT INTO workstations 
                (workstation_id, system_unit, room, monitor, keyboard, mouse, avr, added_by, date_added) 
                VALUES 
                (@workstationId, @systemUnit, @room, @monitor, @keyboard, @mouse, @avr, @addedBy, @dateAdded)
            ";

            // Create and configure the MySqlCommand parameters
            MySqlParameter[] parameters = {
                new MySqlParameter("@workstationId", workstationId),
                new MySqlParameter("@systemUnit", systemUnitValue),
                new MySqlParameter("@room", room),
                new MySqlParameter("@monitor", monitorValue),
                new MySqlParameter("@keyboard", keyboardValue),
                new MySqlParameter("@mouse", mouseValue),
                new MySqlParameter("@avr", avrValue),
                new MySqlParameter("@addedBy", addedBy),
                new MySqlParameter("@dateAdded", dateAdded)
            };

            // Execute the query
            try
            {
                dbHelper.ExecuteQuery(insertQuery, parameters);
                MessageBox.Show("Workstation added successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the workstation: {ex.Message}");
            }
        }

        // Method to generate a unique workstation_id
        private string GenerateWorkstationId()
        {
            string query = "SELECT workstation_id FROM workstations ORDER BY workstation_id DESC LIMIT 1";
            DataTable result = dbHelper.GetData(query);
            string lastId = result.Rows.Count > 0 ? result.Rows[0]["workstation_id"].ToString() : "W000";

            // Extract number part from the ID and increment it
            int lastIdNumber = int.Parse(lastId.Substring(1));
            return "W" + (lastIdNumber + 1).ToString("D3");
        }

        // Method to get the user's name by their ID
        private string GetUserNameById(string userId)
        {
            string query = "SELECT name FROM users WHERE user_id = @userId";
            MySqlParameter[] parameters = {
                new MySqlParameter("@userId", userId)
            };
            DataTable result = dbHelper.GetData(query, parameters);
            if (result.Rows.Count > 0)
            {
                return result.Rows[0]["name"].ToString();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }
    }
}
