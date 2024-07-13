using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GP_Optimization_of_Tourism_Industry
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        #region method to check if admin's email and password is exist in database
        private bool ValidateAdminCredentials(string email, string password)
        {
            string connectionString = @"Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT COUNT(*) FROM ADMIN WHERE Email = @Email AND Password = @Password";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    int matchingRecordsCount = (int)command.ExecuteScalar();

                    return matchingRecordsCount > 0;
                }
            }
        }
        #endregion

        #region button login 
        private void button_login_Click(object sender, EventArgs e)
        {
            string adminEmail = TextBox_email.Text;
            string adminPassword = TextBox_password.Text;
            // Validate the admin credentials against the database
            bool isValidAdmin = ValidateAdminCredentials(adminEmail, adminPassword);

            if (isValidAdmin)
            {
                string connectionString = @"Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT AdminId, Role FROM Admin WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", adminEmail);
                        command.Parameters.AddWithValue("@Password", adminPassword);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int adminId = reader.GetInt32(reader.GetOrdinal("AdminId"));
                                string role = reader.GetString(reader.GetOrdinal("Role"));

                                if (role == "super_admin")
                                {
                                    // Redirect to another page or form (e.g., SuperAdminDashboardForm)
                                    SuperAdminDashboardForm adminDashboardForm = new SuperAdminDashboardForm();
                                    adminDashboardForm.CurrentAdminEmail = adminEmail; // Set the current admin's email
                                    adminDashboardForm.CurrentAdminId = adminId; // Set the current admin's ID

                                    adminDashboardForm.Show();
                                    this.Hide();
                                }
                                else if (role == "manager")
                                {
                                    SuperAdminDashboardForm adminDashboardForm = new SuperAdminDashboardForm();
                                    adminDashboardForm.CurrentAdminEmail = adminEmail;
                                    adminDashboardForm.CurrentAdminId = adminId;

                                    adminDashboardForm.Show();
                                    this.Hide();
                                    adminDashboardForm.subMainButton_updateAdmin.Enabled = false;
                                    adminDashboardForm.subMainButton_addNewAdmin.Enabled = false;
                                }
                                else if (role == "operation_admin")
                                {
                                    // Redirect to another page or form (e.g., OperationAdminDashboardForm)
                                    SuperAdminDashboardForm adminDashboardForm = new SuperAdminDashboardForm();
                                    adminDashboardForm.CurrentAdminEmail = adminEmail;
                                    adminDashboardForm.CurrentAdminId = adminId;

                                    adminDashboardForm.Show();
                                    this.Hide();
                                    adminDashboardForm.subMainButton_addNewAdmin.Enabled = false;
                                    adminDashboardForm.subMainButton_updateAdmin.Enabled = false;
                                    adminDashboardForm.subMainButton_addPackageSeason.Enabled = false;
                                    adminDashboardForm.subMainButton_updatePackageSeason.Enabled = false;
                                    adminDashboardForm.subMainButton_addRole.Enabled = false;
                                    adminDashboardForm.subMainButton_updateRole.Enabled = false;
                                    adminDashboardForm.subMainButton_addBudget.Enabled = false;
                                    adminDashboardForm.subMainButton_updateBudget.Enabled = false;
                                }
                                else if (role == "sales_admin")
                                {
                                    // Redirect to another page or form (e.g., SalesAdminDashboardForm)
                                    SuperAdminDashboardForm adminDashboardForm = new SuperAdminDashboardForm();
                                    adminDashboardForm.CurrentAdminEmail = adminEmail;
                                    adminDashboardForm.CurrentAdminId = adminId;

                                    adminDashboardForm.Show();
                                    this.Hide();
                                    adminDashboardForm.subMainButton_addNewAdmin.Enabled = false;
                                    adminDashboardForm.subMainButton_updateAdmin.Enabled = false;
                                    adminDashboardForm.subMainButton_addResource.Enabled = false;
                                    adminDashboardForm.subMainButton_updateResource.Enabled = false;
                                    adminDashboardForm.subMainButton_allocateResource.Enabled = false;
                                    adminDashboardForm.subMainButton_updateallocation.Enabled = false;
                                    adminDashboardForm.subMainButton_addSeason.Enabled = false;
                                    adminDashboardForm.subMainButton_updateSeason.Enabled = false;
                                    adminDashboardForm.subMainButton_addRole.Enabled = false;
                                    adminDashboardForm.subMainButton_updateRole.Enabled = false;
                                    adminDashboardForm.subMainButton_addBudget.Enabled = false;
                                    adminDashboardForm.subMainButton_updateBudget.Enabled = false;
                                }
                                else if (role == "finance_admin")
                                {
                                    // Redirect to another page or form (e.g., FinanceAdminDashboardForm)
                                    SuperAdminDashboardForm adminDashboardForm = new SuperAdminDashboardForm();
                                    adminDashboardForm.CurrentAdminEmail = adminEmail;
                                    adminDashboardForm.CurrentAdminId = adminId;

                                    adminDashboardForm.Show();
                                    this.Hide();
                                    adminDashboardForm.subMainButton_addNewAdmin.Enabled = false;
                                    adminDashboardForm.subMainButton_updateAdmin.Enabled = false;
                                    adminDashboardForm.subMainButton_addNewPackage.Enabled = false;
                                    adminDashboardForm.subMainButton_updatePackage.Enabled = false;
                                    adminDashboardForm.subMainButton_addPackageSeason.Enabled = false;
                                    adminDashboardForm.subMainButton_updatePackageSeason.Enabled = false;
                                    adminDashboardForm.subMainButton_addResource.Enabled = false;
                                    adminDashboardForm.subMainButton_updateResource.Enabled = false;
                                    adminDashboardForm.subMainButton_allocateResource.Enabled = false;
                                    adminDashboardForm.subMainButton_updateallocation.Enabled = false;
                                    adminDashboardForm.subMainButton_addSeason.Enabled = false;
                                    adminDashboardForm.subMainButton_updateSeason.Enabled = false;
                                    adminDashboardForm.subMainButton_addRole.Enabled = false;
                                    adminDashboardForm.subMainButton_updateRole.Enabled = false;

                                }
                                else
                                {
                                    MessageBox.Show("You are not an Admin now.", "Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Admin Information. Please try again!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            else
            {
                label_information.Text = "Invalid Admin Information. Please try again!";
                label_information.Visible = true;
                picture_wrong.Visible = true;
            }
        }
        #endregion

        #region pictures and textboxes and labels
        private void pictur_hide_eye_Click(object sender, EventArgs e)
        {
            if (TextBox_password.PasswordChar == '●')
            {
                picture_eye.BringToFront();
                TextBox_password.PasswordChar = '\0';
            }
        }
        private void picture_eye_Click(object sender, EventArgs e)
        {
            if (TextBox_password.PasswordChar == '\0')
            {
                picture_hide_eye.BringToFront();
                TextBox_password.PasswordChar = '●';
            }
        }
        private void TextBox_email_TextChanged(object sender, EventArgs e)
        {
            label_information.Visible = false;
            picture_wrong.Visible = false;
        }
        private void TextBox_password_TextChanged(object sender, EventArgs e)
        {
            label_information.Visible = false;
            picture_wrong.Visible = false;
        }
        private void Label_exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you want to exit application ?", "Exit Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void Label_exit_MouseEnter(object sender, EventArgs e)
        {
            Label_exit.ForeColor = Color.Red;
        }
        private void Label_exit_MouseLeave(object sender, EventArgs e)
        {
            Label_exit.ForeColor = SystemColors.HotTrack;
        }
        #endregion
    }
}