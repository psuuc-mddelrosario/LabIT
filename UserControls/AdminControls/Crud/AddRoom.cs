using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class AddRoom : Form
    {
        private DatabaseHelper dbHelper;

        public AddRoom()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void add_Click(object sender, EventArgs e)
        {
            string roomName = room.Text.Trim();

            if (string.IsNullOrEmpty(roomName))
            {
                MessageBox.Show("Please enter a room name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string checkQuery = "SELECT COUNT(*) FROM rooms WHERE laboratory_room = @roomName";
            List<MySqlParameter> checkParameters = new List<MySqlParameter>
    {
        new MySqlParameter("@roomName", roomName)
    };

            try
            {
                DataTable result = dbHelper.GetData(checkQuery, checkParameters.ToArray());
                if (result.Rows[0][0].ToString() != "0")
                {
                    MessageBox.Show("The room name already exists. Please enter a different name.", "Duplicate Room", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string insertQuery = "INSERT INTO rooms (laboratory_room, date_added) VALUES (@roomName, @dateAdded)";
                List<MySqlParameter> insertParameters = new List<MySqlParameter>
        {
            new MySqlParameter("@roomName", roomName),
            new MySqlParameter("@dateAdded", DateTime.Now)
        };

                dbHelper.ExecuteQuery(insertQuery, insertParameters.ToArray());

                MessageBox.Show("Room added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
