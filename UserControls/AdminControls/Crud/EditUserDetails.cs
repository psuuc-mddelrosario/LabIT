using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    public partial class EditUserDetails : Form
    {
        private DatabaseHelper dbHelper;
        private string userId;

        public EditUserDetails(string userId)
        {
            InitializeComponent();
            this.userId = userId;
            dbHelper = new DatabaseHelper();
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            LoadUserData(); 
        }

        private void LoadUserData()
        {
            try
            {
                string query = "SELECT firstname, middlename, lastname, username FROM users WHERE user_id = @userId";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@userId", MySqlDbType.VarChar) { Value = userId }
                };

                DataTable userTable = dbHelper.GetData(query, parameters);

                if (userTable.Rows.Count > 0)
                {
                    DataRow row = userTable.Rows[0];
                    firstname.Text = row["firstname"].ToString();
                    middlename.Text = row["middlename"].ToString();
                    lastname.Text = row["lastname"].ToString();
                    username.Text = row["username"].ToString();
                }
                else
                {
                    MessageBox.Show("User not found.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading user details: {ex.Message}");
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPassword_CheckedChanged_1(object sender, EventArgs e)
        {
            if (showPassword.Checked)
            {
                password.PasswordChar = '\0';
                newPassword.PasswordChar = '\0';
                confirmPassword.PasswordChar = '\0';
            }
            else
            {
                password.PasswordChar = '●';
                newPassword.PasswordChar = '●';
                confirmPassword.PasswordChar = '●';
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            try
            {
                // Check that required fields are filled out
                if (string.IsNullOrWhiteSpace(firstname.Text) ||
                    string.IsNullOrWhiteSpace(lastname.Text) ||
                    string.IsNullOrWhiteSpace(username.Text))
                {
                    MessageBox.Show("Please fill out all required fields.");
                    return;
                }

                // If the user provided a new password, validate it
                if (!string.IsNullOrWhiteSpace(password.Text) ||
                    !string.IsNullOrWhiteSpace(newPassword.Text) ||
                    !string.IsNullOrWhiteSpace(confirmPassword.Text))
                {
                    // Check that all password fields are filled
                    if (string.IsNullOrWhiteSpace(password.Text) ||
                        string.IsNullOrWhiteSpace(newPassword.Text) ||
                        string.IsNullOrWhiteSpace(confirmPassword.Text))
                    {
                        MessageBox.Show("Please fill out all password fields.");
                        return;
                    }

                    // Verify the current password matches the database
                    string currentPasswordQuery = "SELECT password FROM users WHERE user_id = @userId";
                    MySqlParameter[] passwordParameters = new MySqlParameter[]
                    {
                new MySqlParameter("@userId", MySqlDbType.VarChar) { Value = userId }
                    };

                    DataTable passwordTable = dbHelper.GetData(currentPasswordQuery, passwordParameters);
                    if (passwordTable.Rows.Count > 0)
                    {
                        string currentPasswordInDb = passwordTable.Rows[0]["password"].ToString();
                        if (password.Text != currentPasswordInDb)
                        {
                            MessageBox.Show("Current password is incorrect.");
                            return;
                        }
                    }

                    if (newPassword.Text != confirmPassword.Text)
                    {
                        MessageBox.Show("New password and confirm password do not match.");
                        return;
                    }
                }

                string updateQuery = "UPDATE users SET firstname = @firstname, middlename = @middlename, lastname = @lastname, username = @username";

                if (!string.IsNullOrWhiteSpace(newPassword.Text))
                {
                    updateQuery += ", password = @newPassword";
                }

                updateQuery += " WHERE user_id = @userId";

                var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@firstname", MySqlDbType.VarChar) { Value = firstname.Text },
            new MySqlParameter("@middlename", MySqlDbType.VarChar) { Value = middlename.Text },
            new MySqlParameter("@lastname", MySqlDbType.VarChar) { Value = lastname.Text },
            new MySqlParameter("@username", MySqlDbType.VarChar) { Value = username.Text },
            new MySqlParameter("@userId", MySqlDbType.VarChar) { Value = userId }
        };

                if (!string.IsNullOrWhiteSpace(newPassword.Text))
                {
                    parameters.Add(new MySqlParameter("@newPassword", MySqlDbType.VarChar) { Value = newPassword.Text });
                }

                dbHelper.ExecuteQuery(updateQuery, parameters.ToArray());

                MessageBox.Show("User details updated successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating user details: {ex.Message}");
            }
        }

    }
}
