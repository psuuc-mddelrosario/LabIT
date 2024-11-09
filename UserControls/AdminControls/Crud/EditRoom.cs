using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class EditRoom : Form
    {
        private DatabaseHelper dbHelper;

        public EditRoom(DatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        private readonly int id;

        public EditRoom(int id)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.id = id;

            // Load room data into the textbox when the form initializes
            LoadRoomData();
        }

        private void LoadRoomData()
        {
            string query = "SELECT laboratory_room FROM rooms WHERE id = @id";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@id", id)
            };

            try
            {
                DataTable result = dbHelper.GetData(query, parameters.ToArray());
                if (result.Rows.Count > 0)
                {
                    room.Text = result.Rows[0]["laboratory_room"].ToString();
                }
                else
                {
                    MessageBox.Show("Room not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the room: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            string newRoomName = room.Text.Trim();

            if (string.IsNullOrEmpty(newRoomName))
            {
                MessageBox.Show("Please enter a room name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string checkQuery = "SELECT COUNT(*) FROM rooms WHERE laboratory_room = @roomName AND id != @id";
            var checkParameters = new List<MySqlParameter>
            {
                new MySqlParameter("@roomName", newRoomName),
                new MySqlParameter("@id", id)
            };

            try
            {
                DataTable checkResult = dbHelper.GetData(checkQuery, checkParameters.ToArray());
                if (checkResult.Rows.Count > 0 && Convert.ToInt32(checkResult.Rows[0][0]) > 0)
                {
                    MessageBox.Show("Room name already exists. Please enter a different name.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string updateQuery = "UPDATE rooms SET laboratory_room = @roomName WHERE id = @id";
                var updateParameters = new List<MySqlParameter>
                {
                    new MySqlParameter("@roomName", newRoomName),
                    new MySqlParameter("@id", id)
                };

                dbHelper.ExecuteQuery(updateQuery, updateParameters.ToArray());
                MessageBox.Show("Room updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the room: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is EditRoom room &&
                   EqualityComparer<DatabaseHelper>.Default.Equals(dbHelper, room.dbHelper);
        }
    }
}
