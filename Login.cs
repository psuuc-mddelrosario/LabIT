using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Data.Filtering.Helpers.PropertyDescriptorCriteriaCompilationSupport;

namespace Laboratory_Management_System
{
    public partial class Login : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string userId = idTextbox.Text.Trim();
            string password = passwordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both User ID and Password.");
                return;
            }

            AuthenticateUser(userId, password);
        }

        private void showPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (showPassword.Checked)
            {
                passwordTextBox.PasswordChar = '\0';
            }
            else
            {
                passwordTextBox.PasswordChar = '●';
            }
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void idTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void AuthenticateUser(string userId, string password)
        {
            string query = "SELECT * FROM users WHERE user_id = @userId AND password = @password";

            MySqlParameter[] parameters =
            {
        new MySqlParameter("@userId", userId),
        new MySqlParameter("@password", password)
    };

            DataTable userTable = dbHelper.GetData(query, parameters);

            if (userTable.Rows.Count > 0)
            {
                DataRow userRow = userTable.Rows[0];

                string role = userRow["role"].ToString();
                if (role == "Admin")
                {
                    Admin adminForm = new Admin(userId);
                    adminForm.Show();
                    this.Hide(); 
                }
                else if (role == "Instructor")
                {
                    Instructor instructorsForm = new Instructor(userId);
                    instructorsForm.Show();
                    this.Hide();
                }
                else if (role == "Instructor/Incharge")
                {
                    InstructorIncharge labinchargeForm = new InstructorIncharge(userId);
                    labinchargeForm.Show();
                    this.Hide();
                }
                else if (role == "Incharge")
                {
                    Incharge labinchargeForm = new Incharge(userId);
                    labinchargeForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Access denied. You do not have the required role.");
                }
            }
            else
            {
                MessageBox.Show("Invalid User ID or Password.");
            }
        }

    }
}
