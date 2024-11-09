using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class EditUser : Form
    {
        private DatabaseHelper dbHelper;
        private string userId;

        public EditUser(string userId)
        {
            InitializeComponent();
            this.userId = userId;
            dbHelper = new DatabaseHelper();
            PopulateRoleDropdown();
            PopulateRoomsDropdown();
            LoadUserDetails(); // Load user details when the form initializes
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

        private void LoadUserDetails()
        {
            string query = "SELECT username, password, firstname, middlename, lastname, role, assigned_room FROM users WHERE user_id = @user_id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@user_id", userId)
            };

            DataTable userData = dbHelper.GetData(query, parameters);
            if (userData.Rows.Count > 0)
            {
                DataRow row = userData.Rows[0];
                username.Text = row["username"].ToString();
                password.Text = row["password"].ToString();
                firstname.Text = row["firstname"].ToString();
                middlename.Text = row["middlename"].ToString();
                lastname.Text = row["lastname"].ToString();
                roleDropdown.SelectedItem = row["role"].ToString();
                string assignedRoom = row["assigned_room"].ToString();

                // Set the room dropdown based on the assigned room
                if (assignedRoom != "Unassigned")
                {
                    roomDropdown.SelectedItem = assignedRoom;
                    roomDropdown.Enabled = true; // Enable the room dropdown if a room is assigned
                }
                else
                {
                    roomDropdown.SelectedIndex = 0; // Set to the default value
                    roomDropdown.Enabled = false; // Disable the room dropdown
                }
            }
            else
            {
                MessageBox.Show("User not found.");
                this.Close(); // Close the form if the user is not found
            }
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

        private void update_Click(object sender, EventArgs e)
        {
            string usernames = username.Text;
            string passwords = password.Text;
            string firstnames = firstname.Text;
            string middlenames = middlename.Text;
            string lastnames = lastname.Text;
            string role = roleDropdown.SelectedItem.ToString();
            string assignedRoom = roomDropdown.Enabled ? roomDropdown.SelectedItem.ToString() : "Unassigned";

            string query = "UPDATE users SET username = @username, password = @password, firstname = @firstname, middlename = @middlename, lastname = @lastname, role = @role, assigned_room = @assigned_room WHERE user_id = @user_id";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", usernames),
                new MySqlParameter("@password", passwords),
                new MySqlParameter("@firstname", firstnames),
                new MySqlParameter("@middlename", middlenames),
                new MySqlParameter("@lastname", lastnames),
                new MySqlParameter("@role", role),
                new MySqlParameter("@assigned_room", assignedRoom),
                new MySqlParameter("@user_id", userId)
            };

            dbHelper.ExecuteQuery(query, parameters);

            MessageBox.Show("User updated successfully!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
