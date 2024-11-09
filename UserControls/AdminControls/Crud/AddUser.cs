using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class AddUser : Form
    {
        private DatabaseHelper dbHelper;

        public AddUser()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            PopulateRoleDropdown();
            PopulateRoomsDropdown();
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            roomDropdown.Enabled = false;

            roleDropdown.SelectedIndexChanged += roleDropdown_SelectedIndexChanged;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulateRoleDropdown()
        {
            roleDropdown.Items.Clear();
            roleDropdown.Items.Add("Instructor");
            roleDropdown.Items.Add("Admin");
            roleDropdown.Items.Add("Incharge");
            roleDropdown.Items.Add("Instructor/Incharge");

            roleDropdown.SelectedIndex = 0;
        }

        private void PopulateRoomsDropdown()
        {
            roomDropdown.Items.Clear();
            roomDropdown.Items.Add("Lab 1");
            roomDropdown.Items.Add("Lab 2");
            roomDropdown.Items.Add("Lab 3");
            roomDropdown.Items.Add("Lab 4");
            roomDropdown.Items.Add("Lab 5");
            roomDropdown.Items.Add("Multimedia");

            roomDropdown.SelectedIndex = 0; 
        }

        private void roleDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (roleDropdown.SelectedItem.ToString() == "Incharge" || roleDropdown.SelectedItem.ToString() == "Instructor/Incharge")
            {
                roomDropdown.Enabled = true;
            }
            else
            {
                roomDropdown.Enabled = false; 
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any required field is empty
                if (string.IsNullOrWhiteSpace(firstname.Text) ||
                    string.IsNullOrWhiteSpace(lastname.Text) ||
                    roleDropdown.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Proceed with user insertion if validation passes
                string lastUserId = GetLastUserId();
                string newUserId = GenerateNewUserId(lastUserId);

                // Generate unique username based on name
                string generatedUsername = GenerateUniqueUsername(firstname.Text, lastname.Text);

                // Handle assigned room
                string assignedRoom = roomDropdown.Enabled && roomDropdown.SelectedItem != null ? roomDropdown.SelectedItem.ToString() : "Unassigned";

                DateTime birthdate = birthdatePicker.Value;
                string tempPassword = birthdate.ToString("MMddyyyy");

                // Insert new user into the database
                string query = @"INSERT INTO `users`(`user_id`, `username`,`password`, `firstname`, `middlename`, `lastname`, `role`, `assigned_room`, `birthdate`) 
                         VALUES (@user_id, @username, @password, @firstname, @middlename, @lastname, @role, @assigned_room, @birthdate)";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
            new MySqlParameter("@user_id", newUserId),
            new MySqlParameter("@username", generatedUsername),
            new MySqlParameter("@password", tempPassword),
            new MySqlParameter("@firstname", firstname.Text),
            new MySqlParameter("@middlename", middlename.Text),
            new MySqlParameter("@lastname", lastname.Text),
            new MySqlParameter("@role", roleDropdown.SelectedItem.ToString()),
            new MySqlParameter("@assigned_room", assignedRoom),
            new MySqlParameter("@birthdate", birthdate)
                };

                dbHelper.ExecuteQuery(query, parameters);

                MessageBox.Show("User added successfully!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private string GetLastUserId()
        {
            string lastUserId = "U000";  

            string query = "SELECT user_id FROM users ORDER BY user_id DESC LIMIT 1";

            DataTable result = dbHelper.GetData(query);

            if (result.Rows.Count > 0)
            {
                lastUserId = result.Rows[0]["user_id"].ToString();
            }

            return lastUserId;
        }

        private string GenerateNewUserId(string lastUserId)
        {
            if (string.IsNullOrEmpty(lastUserId))
            {
                return "U001";
            }

            int lastIdNumber = int.Parse(lastUserId.Substring(1));
            int newIdNumber = lastIdNumber + 1;

            return "U" + newIdNumber.ToString("D3");
        }

        private string GenerateUniqueUsername(string firstname, string lastname)
        {
            string baseUsername = firstname.ToLower() + "." + lastname.ToLower();

            string query = "SELECT COUNT(*) FROM users WHERE username LIKE @usernamePattern";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
        new MySqlParameter("@usernamePattern", baseUsername + "%") 
            };

            DataTable result = dbHelper.GetData(query, parameters);
            int count = 0;

            if (result.Rows.Count > 0)
            {
                count = Convert.ToInt32(result.Rows[0][0]);
            }

            if (count > 0)
            {
                baseUsername += count.ToString();
            }

            return baseUsername;
        }

    }
}
