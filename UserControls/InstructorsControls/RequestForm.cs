using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class RequestForm : Form
    {
        private DatabaseHelper dbHelper;
        private string userId;
        private string scheduledRoom;
        private List<string> assetIds;

        public RequestForm(string userId, string scheduledRoom, List<string> assetIds)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            PopulateRequestType();
            this.userId = userId;
            this.scheduledRoom = scheduledRoom;
            this.assetIds = assetIds;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void add_Click(object sender, EventArgs e)
        {
            string requestType = requestDropdown.SelectedItem.ToString();
            string noteText = note.Text;
            DateTime requestDate = DateTime.Now;
            string status = "Pending";

            foreach (string assetId in assetIds)
            {
                // Insert into asset_requests
                string insertQuery = "INSERT INTO `asset_requests`(`room`, `instructor_id`, `asset_id`, `request_type`, `request_date`, `note`, `status`) " +
                                     "VALUES (@Room, @InstructorId, @AssetId, @RequestType, @RequestDate, @Note, @Status)";

                MySqlParameter[] insertParameters = new MySqlParameter[]
                {
            new MySqlParameter("@Room", scheduledRoom),
            new MySqlParameter("@InstructorId", userId),
            new MySqlParameter("@AssetId", assetId),
            new MySqlParameter("@RequestType", requestType),
            new MySqlParameter("@RequestDate", requestDate),
            new MySqlParameter("@Note", noteText),
            new MySqlParameter("@Status", status)
                };

                dbHelper.ExecuteQuery(insertQuery, insertParameters);

                // Update the asset's status in the assets table
                string updateQuery = "UPDATE `assets` SET `status` = @NewStatus WHERE `asset_id` = @AssetId";

                MySqlParameter[] updateParameters = new MySqlParameter[]
                {
            new MySqlParameter("@NewStatus", requestType),
            new MySqlParameter("@AssetId", assetId)
                };

                dbHelper.ExecuteQuery(updateQuery, updateParameters);
            }

            MessageBox.Show("Request added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void PopulateRequestType()
        {
            var operatingSystem = new List<string>
    {
                "Repair",
                "Replacement",
    };
            requestDropdown.DataSource = operatingSystem;
        }
    }
}
