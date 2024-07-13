using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GP_Optimization_of_Tourism_Industry
{
    public partial class SuperAdminDashboardForm : Form
    {

        #region connection to database
        private string connectionString = "Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True";
        #endregion
        public SuperAdminDashboardForm()
        {
            InitializeComponent();
            PopulateComboBox_Role();
            uPopulateComboBox_Role();
            customizeDesign();
            LoadResourceAllocationData();
            LoadPackageSeasonData();
            LoadPackageSeasonDataUpdate();
            LoadResourceNames();
            LoadResourceData();
            LoadSeasonNames();
            viewSeasonData_inDataGridView();
            LoadSeasonNamesIntoComboBox();
            LoadSeasonsIntoDataGridView();
            LoadSeasonNamesUpdate();
            LoadDataIntoDataGridView();
            viewResourcesData_inDataGridView();
            LoadDataIntoDataGridView_viewAllocation();
            ViewPackageSeason();
            LoadResourceTypes();
            PopulateResourceBudgetData();
            viewBudgetData_inDataGridView();
            LoadSeasons();
            LoadRole();
            LoadRole_update();
            viewRoleData_inDataGridView();
            viewinfoOfPackages_DataGridView();
            DataGridView_allocateResource.CellClick += DataGridView_allocateResource_CellClick;
            // Initialize DateTimePickers if a season is selected
            if (ComboBox_season_name.SelectedItem != null)
            {
                int seasonId = (int)ComboBox_season_name.SelectedValue;
                PopulateSeasonDates(seasonId);
            }
        }
        private void SuperAdminDashboardForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'gPTourismDataSet2.TourPackage' table. You can move, or remove it, as needed.
            this.tourPackageTableAdapter.Fill(this.gPTourismDataSet2.TourPackage);
            // TODO: This line of code loads data into the 'gPTourismDataSet1.Admin' table. You can move, or remove it, as needed.
            this.adminTableAdapter1.Fill(this.gPTourismDataSet1.Admin);
            // TODO: This line of code loads data into the 'gPTourismDataSet.Admin' table. You can move, or remove it, as needed.
            this.adminTableAdapter.Fill(this.gPTourismDataSet.Admin);

        }
        public string CurrentAdminEmail { get; set; }
        public int CurrentAdminId { get; set; }

        #region Admin

        #region add new admin panel

        #region methods for add new admin panel
        private bool IsEmailExists_admin(string email)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True");
            cn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ADMIN WHERE Email = @Email", cn);
            cmd.Parameters.AddWithValue("@Email", email);
            int count = (int)cmd.ExecuteScalar();
            cn.Close();
            return count > 0;
        }
        private void UpdateTextBoxBorders_admin()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(text_username_admin.Text))
                text_username_admin.BorderColor = Color.Red;
            else
                text_username_admin.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(text_email_admin.Text))
                text_email_admin.BorderColor = Color.Red;
            else
                text_email_admin.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(text_password_admin.Text))
                text_password_admin.BorderColor = Color.Red;
            else
                text_password_admin.BorderColor = Color.Silver;
            if (ComboBox_Role.SelectedItem == null)
                ComboBox_Role.BorderColor = Color.Red;
            else
                ComboBox_Role.BorderColor = Color.Silver;
        }
        private bool AreAllFieldsFilled_admin()
        {
            return !string.IsNullOrWhiteSpace(text_username_admin.Text)
                && !string.IsNullOrWhiteSpace(text_email_admin.Text)
                && !string.IsNullOrWhiteSpace(text_password_admin.Text)
                && ComboBox_Role.SelectedItem != null;
        }
        private void PopulateComboBox_Role()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string categoryQuery = "SELECT RoleName FROM Role";

                using (SqlCommand categoryCommand = new SqlCommand(categoryQuery, connection))
                {
                    using (SqlDataReader categoryReader = categoryCommand.ExecuteReader())
                    {
                        ComboBox_Role.Items.Clear();
                        while (categoryReader.Read())
                        {
                            ComboBox_Role.Items.Add(categoryReader["RoleName"].ToString());
                        }
                    }
                }
            }
        }
        #endregion

        #region textBoxes and pictures of add new admin panel
        private void AddAdmin_button_Click(object sender, EventArgs e)
        {
            string adminEmail = text_email_admin.Text;
            if (AreAllFieldsFilled_admin())
            {
                if (IsEmailExists_admin(text_email_admin.Text))
                {
                    MessageBox.Show("Email already exists. Please use a different email.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection cn = new SqlConnection(@"Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True");

                cn.Open();
                // Create the SQL query
                SqlCommand MyCommand = new SqlCommand("insert into Admin (Username,Email,Password,Role) values('" + text_username_admin.Text.ToString() + "','" + text_email_admin.Text.ToString() + "','" + text_password_admin.Text.ToString() + "','" + ComboBox_Role.SelectedItem.ToString() + "')", cn);
                MyCommand.ExecuteNonQuery();
                MessageBox.Show("Admin Added Successfully!", "Add Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                viewAdminData_inDataGridView();
            }
            else
            {
                Label_information_admin.Visible = true;
                Picture_information_admin.Visible = true;
                UpdateTextBoxBorders_admin();
                viewinfoOfPackages_DataGridView();
            }
        }
        private void picture_hide_eye_admin_Click(object sender, EventArgs e)
        {
            if (text_password_admin.PasswordChar == '●')
            {
                picture_eye_admin.BringToFront();
                text_password_admin.PasswordChar = '\0';
            }
        }

        private void picture_eye_admin_Click(object sender, EventArgs e)
        {
            if (text_password_admin.PasswordChar == '\0')
            {
                picture_hide_eye_admin.BringToFront();
                text_password_admin.PasswordChar = '●';
            }
        }

        private void text_username_admin_TextChanged(object sender, EventArgs e)
        {
            Label_information_admin.Visible = false;
            Picture_information_admin.Visible = false;
            text_username_admin.BorderColor = Color.Silver;
            ComboBox_Role.BorderColor = Color.Silver;
        }

        private void text_email_admin_TextChanged(object sender, EventArgs e)
        {
            Label_information_admin.Visible = false;
            Picture_information_admin.Visible = false;
            text_email_admin.BorderColor = Color.Silver;
            ComboBox_Role.BorderColor = Color.Silver;
        }

        private void text_password_admin_TextChanged(object sender, EventArgs e)
        {
            Label_information_admin.Visible = false;
            Picture_information_admin.Visible = false;
            text_password_admin.BorderColor = Color.Silver;
            ComboBox_Role.BorderColor = Color.Silver;
        }
        #endregion

        #endregion

        #region update admin panel

        #region search and update buttons 

        private void SearchTextBox_update_admin_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_update_admin.Text == "Search...")
            {
                searchTextBox_update_admin.Text = "";
                searchTextBox_update_admin.ForeColor = Color.Black;
            }
        }
        private void SearchTextBox_update_admin_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_update_admin.Text))
            {
                searchTextBox_update_admin.Text = "Search...";
                searchTextBox_update_admin.ForeColor = Color.Gray;
            }
        }
        private void SearchTextBox_update_admin_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_update_admin.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                loadAdminData_inDataGridView();
            }
            else
            {
                loadAdminData_inDataGridView(searchQuery);
            }
        }
        private void UpdateTextBoxBorders_uadmin()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(textBox_Username_admin.Text))
                textBox_Username_admin.BorderColor = Color.Red;
            else
                textBox_Username_admin.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(textBox_password_admin.Text))
                textBox_password_admin.BorderColor = Color.Red;
            else
                textBox_password_admin.BorderColor = Color.Silver;
            if (uComboBox_Role.SelectedItem == null)
                uComboBox_Role.BorderColor = Color.Red;
            else
                uComboBox_Role.BorderColor = Color.Silver;
        }
        private bool AreAllFieldsFilled_uadmin()
        {
            return uComboBox_Role.SelectedItem != null;
        }
        private void Button_update_profile_admin_Click(object sender, EventArgs e)
        {
            if (sindex == -1)
            {
                MessageBox.Show("Please select an Admin to update its Information.", "Update Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedID = Convert.ToInt32(dataGridView_updateAdmin.CurrentRow.Cells["adminIdDataGridView"].Value);
            string adminRole = uComboBox_Role.SelectedItem.ToString();
            string currentAdminEmail = this.CurrentAdminEmail; // Replace with the actual current admin email
            int currentAdminId = this.CurrentAdminId;

            if (AreAllFieldsFilled_uadmin())
            {
                bool isUpdatingOwnRole = IsUpdatingOwnRole(selectedID, currentAdminEmail);

                if (isUpdatingOwnRole && IsOnlySuperAdmin())
                {
                    MessageBox.Show("You are the only Super Admin. Please assign another Super Admin before changing your role.", "Role Change Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string query = "UPDATE Admin SET Role = @admin_role WHERE AdminId = @selectedID";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            cmd.Parameters.AddWithValue("@admin_role", adminRole);
                            cmd.Parameters.AddWithValue("@selectedID", selectedID);
                            int RowsAffected = cmd.ExecuteNonQuery();

                            if (RowsAffected > 0)
                            {
                                MessageBox.Show("Admin information updated successfully.", "Update Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Reload the data grid view with updated data
                                loadAdminData_inDataGridView();
                                viewAdminData_inDataGridView();
                                // Clear the input fields
                                textBox_Id_admin.Text = null;
                                textBox_password_admin.Text = null;
                                textBox_Username_admin.Text = null;
                                textBox_email_admin.Text = null;
                                uComboBox_Role.SelectedItem = null;

                                // Reset index
                                sindex = -1;

                                // Check if the current admin updated their own role
                                if (isUpdatingOwnRole && adminRole != "super_admin")
                                {
                                    MessageBox.Show("You have changed your role. Please log in again.", "Role Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    // Redirect to the login form
                                    LoginForm loginForm = new LoginForm();
                                    loginForm.Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Update failed. Admin not found in the database.", "Update Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Update Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                Label_information_admin2.Visible = true;
                Picture_information_admin2.Visible = true;
                UpdateTextBoxBorders_uadmin();
            }
        }

        #endregion

        #region DataGridView Update admin

        private int sindex = -1;
        private void LoadAdminInformation_inTheTextBoxes()
        {
            int selectedID = Convert.ToInt32(dataGridView_updateAdmin.CurrentRow.Cells["adminIdDataGridView"].Value);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Username, Email, Password , Role FROM Admin WHERE AdminId = @selectedID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@selectedID", selectedID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox_Username_admin.Text = reader["Username"].ToString();
                        textBox_email_admin.Text = reader["Email"].ToString();
                        textBox_password_admin.Text = reader["Password"].ToString();
                        string role = reader["Role"].ToString();
                        if (uComboBox_Role.Items.Contains(role))
                        {
                            uComboBox_Role.SelectedItem = role;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Admin not found in the database.", "Update Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void dataGridView_updateAdmin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                sindex = e.RowIndex;
                DataGridViewRow row = dataGridView_updateAdmin.Rows[sindex];  // Change 'index' to 'sindex'
                uComboBox_Role.Text = row.Cells[1].Value.ToString();
                textBox_Id_admin.Text = row.Cells[0].Value.ToString();
                LoadAdminInformation_inTheTextBoxes();
                textBox_Id_admin.ReadOnly = true;
                textBox_Username_admin.ReadOnly = true;
                textBox_email_admin.ReadOnly = true;
                textBox_password_admin.ReadOnly = true;
            }
        }
        private void uPopulateComboBox_Role()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string categoryQuery = "SELECT RoleName FROM Role";

                using (SqlCommand categoryCommand = new SqlCommand(categoryQuery, connection))
                {
                    using (SqlDataReader categoryReader = categoryCommand.ExecuteReader())
                    {
                        uComboBox_Role.Items.Clear();
                        while (categoryReader.Read())
                        {
                            uComboBox_Role.Items.Add(categoryReader["RoleName"].ToString());
                        }
                    }
                }
            }
        }
        private void loadAdminData_inDataGridView(string searchQuery = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT AdminId, Role FROM Admin";
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                sql += " WHERE AdminId LIKE @searchQuery OR Role LIKE @searchQuery";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            // Add parameters for the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            dataGridView_updateAdmin.Columns[0].DataPropertyName = "AdminId";
            dataGridView_updateAdmin.Columns[1].DataPropertyName = "Role";

            // Set the new data source
            dataGridView_updateAdmin.DataSource = dataTable;
        }
        private bool IsOnlySuperAdmin()
        {
            string query = "SELECT COUNT(*) FROM Admin WHERE Role = 'super_admin'";
            int superAdminCount;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                superAdminCount = (int)command.ExecuteScalar();
            }

            return superAdminCount == 1;
        }
        private bool IsUpdatingOwnRole(int adminId, string currentAdminEmail)
        {
            string query = "SELECT Email FROM Admin WHERE AdminId = @adminId";
            string email;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@adminId", adminId);
                connection.Open();
                email = (string)command.ExecuteScalar();
            }

            return email == currentAdminEmail;
        }

        #endregion

        #endregion

        #region view admin panel
        private void SearchTextBox_view_admin_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_view_admin.Text == "Search...")
            {
                searchTextBox_view_admin.Text = "";
                searchTextBox_view_admin.ForeColor = Color.Black;
            }
        }
        private void SearchTextBox_view_admin_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_view_admin.Text))
            {
                searchTextBox_view_admin.Text = "Search...";
                searchTextBox_view_admin.ForeColor = Color.Gray;
            }
        }
        private void SearchTextBox_view_admin_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_view_admin.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                viewAdminData_inDataGridView();
            }
            else
            {
                viewAdminData_inDataGridView(searchQuery);
            }
        }
        private void viewAdminData_inDataGridView(string searchQuery = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT * FROM Admin";
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                sql += " WHERE AdminId LIKE @searchQuery OR Role LIKE @searchQuery OR Email LIKE @searchQuery OR Username LIKE @searchQuery";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            // Add parameters for the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            DataGridView_viewAdmin.Columns[0].DataPropertyName = "AdminId";
            DataGridView_viewAdmin.Columns[1].DataPropertyName = "Username";
            DataGridView_viewAdmin.Columns[2].DataPropertyName = "Email";
            DataGridView_viewAdmin.Columns[3].DataPropertyName = "Password";
            DataGridView_viewAdmin.Columns[4].DataPropertyName = "Role";

            // Set the new data source
            DataGridView_viewAdmin.DataSource = dataTable;
        }
        #endregion

        #endregion

        #region Package

        #region add new package

        #region textboxes
        private void text_package_name_TextChanged(object sender, EventArgs e)
        {
            Label_information_admin3.Visible = false;
            Picture_information_admin3.Visible = false;
            text_package_name.BorderColor = Color.Silver;
        }

        private void text_package_price_TextChanged(object sender, EventArgs e)
        {
            Label_information_admin3.Visible = false;
            Picture_information_admin3.Visible = false;
            text_package_price.BorderColor = Color.Silver;
        }

        private void text_package_cost_TextChanged(object sender, EventArgs e)
        {
            Label_information_admin3.Visible = false;
            Picture_information_admin3.Visible = false;
            text_package_cost.BorderColor = Color.Silver;
        }

        private void text_package_dmin_TextChanged(object sender, EventArgs e)
        {
            Label_information_admin3.Visible = false;
            Picture_information_admin3.Visible = false;
            text_package_dmin.BorderColor = Color.Silver;
        }
        #endregion

        #region methods to add package
        private void text_package_dmax_TextChanged(object sender, EventArgs e)
        {
            Label_information_admin3.Visible = false;
            Picture_information_admin3.Visible = false;
            text_package_dmax.BorderColor = Color.Silver;
        }
        private int GetAdminId(string adminEmail)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string adminIdQuery = "SELECT AdminId FROM Admin WHERE email = @AdminEmail";

                using (SqlCommand adminIdCommand = new SqlCommand(adminIdQuery, connection))
                {
                    adminIdCommand.Parameters.AddWithValue("@AdminEmail", adminEmail);
                    object result = adminIdCommand.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int adminId))
                    {
                        return adminId;
                    }
                }
                return -1;
            }
        }
        private void add_package_button_Click(object sender, EventArgs e)
        {
            if (AreAllFieldsFilled_admin3())
            {
                try
                {
                    // Retrieve the selected values from the ComboBoxes
                    string packageName = text_package_name.Text;

                    if (!float.TryParse(text_package_price.Text, out float price))
                    {
                        MessageBox.Show("Price must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!float.TryParse(text_package_cost.Text, out float cost))
                    {
                        MessageBox.Show("Cost must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(text_package_dmin.Text, out int dmin))
                    {
                        MessageBox.Show("Minimum demand must be a valid integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(text_package_dmax.Text, out int dmax))
                    {
                        MessageBox.Show("Maximum demand must be a valid integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validate demand values
                    if (dmin <= 0 || dmax <= 0)
                    {
                        MessageBox.Show("Minimum and maximum demand must be positive numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cost <= 0 || price <= 0)
                    {
                        MessageBox.Show("Price and Cost must be positive numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (dmax < dmin)
                    {
                        MessageBox.Show("Maximum demand must be greater than or equal to minimum demand.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Get the admin_id associated with the current admin's email
                    int adminId = GetAdminId(CurrentAdminEmail);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check for duplicate package
                        string checkDuplicateQuery = "SELECT COUNT(*) FROM TourPackage WHERE Name = @PackageName AND Price = @Price AND Cost = @Cost AND Dmin = @Dmin AND Dmax = @Dmax";
                        using (SqlCommand checkDuplicateCommand = new SqlCommand(checkDuplicateQuery, connection))
                        {
                            checkDuplicateCommand.Parameters.AddWithValue("@PackageName", packageName);
                            checkDuplicateCommand.Parameters.AddWithValue("@Price", price);
                            checkDuplicateCommand.Parameters.AddWithValue("@Cost", cost);
                            checkDuplicateCommand.Parameters.AddWithValue("@Dmin", dmin);
                            checkDuplicateCommand.Parameters.AddWithValue("@Dmax", dmax);

                            int duplicateCount = (int)checkDuplicateCommand.ExecuteScalar();
                            if (duplicateCount > 0)
                            {
                                MessageBox.Show("Package with the same details already exists in the database.", "Duplicate Package", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Insert the new product into the PRODUCT table
                        string insertProductQuery = "INSERT INTO TourPackage (Name, Price, Cost, Dmin, Dmax, AdminId) " +
                                                    "VALUES (@PackageName, @Price, @Cost, @Dmin, @Dmax, @AdminId)";
                        using (SqlCommand insertProductCommand = new SqlCommand(insertProductQuery, connection))
                        {
                            insertProductCommand.Parameters.AddWithValue("@PackageName", packageName);
                            insertProductCommand.Parameters.AddWithValue("@Price", price);
                            insertProductCommand.Parameters.AddWithValue("@Cost", cost);
                            insertProductCommand.Parameters.AddWithValue("@Dmin", dmin);
                            insertProductCommand.Parameters.AddWithValue("@Dmax", dmax);
                            insertProductCommand.Parameters.AddWithValue("@AdminId", adminId);

                            int rowsAffected = insertProductCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Package added successfully.");
                                LoadDataIntoDataGridView();
                                LoadResourceAllocationData();
                                LoadPackageSeasonDataUpdate();
                                loadPackageData_inDataGridView();
                                viewPackageData_inDataGridView();
                                viewinfoOfPackages_DataGridView();
                                LoadPackageSeasonData();
                                LoadPackageSeasonDataUpdate();
                            }
                            else
                            {
                                MessageBox.Show("Failed to add the package.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                Label_information_admin3.Visible = true;
                Picture_information_admin3.Visible = true;
                UpdateTextBoxBorders_admin3();
            }
        }
        private void UpdateTextBoxBorders_admin3()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(text_package_name.Text))
                text_package_name.BorderColor = Color.Red;
            else
                text_package_name.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(text_package_price.Text))
                text_package_price.BorderColor = Color.Red;
            else
                text_package_price.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(text_package_cost.Text))
                text_package_cost.BorderColor = Color.Red;
            else
                text_package_cost.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(text_package_dmin.Text))
                text_package_dmin.BorderColor = Color.Red;
            else
                text_package_dmin.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(text_package_dmax.Text))
                text_package_dmax.BorderColor = Color.Red;
            else
                text_package_dmax.BorderColor = Color.Silver;
        }
        private bool AreAllFieldsFilled_admin3()
        {
            return !string.IsNullOrWhiteSpace(text_package_name.Text)
                && !string.IsNullOrWhiteSpace(text_package_price.Text)
                && !string.IsNullOrWhiteSpace(text_package_cost.Text)
                && !string.IsNullOrWhiteSpace(text_package_dmin.Text)
                && !string.IsNullOrWhiteSpace(text_package_dmax.Text);
        }

        #endregion

        #endregion

        #region update package

        #region search and textboxes
        private void searchTextBox_update_package_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_update_package.Text == "Search...")
            {
                searchTextBox_update_package.Text = "";
                searchTextBox_update_package.ForeColor = Color.Black;
            }
        }
        private void searchTextBox_update_package_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_update_package.Text))
            {
                searchTextBox_update_package.Text = "Search...";
                searchTextBox_update_package.ForeColor = Color.Gray;
            }
        }
        private void searchTextBox_update_package_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_update_package.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                loadPackageData_inDataGridView();
            }
            else
            {
                loadPackageData_inDataGridView(searchQuery);
            }
        }
        private void textBox_package_name_TextChanged(object sender, EventArgs e)
        {
            Label_information_updatePackage.Visible = false;
            Picture_information_updatePackage.Visible = false;
            textBox_package_name.BorderColor = Color.Silver;
        }
        private void textBox_package_cost_TextChanged(object sender, EventArgs e)
        {
            Label_information_updatePackage.Visible = false;
            Picture_information_updatePackage.Visible = false;
            textBox_package_cost.BorderColor = Color.Silver;
        }
        private void textBox_package_price_TextChanged(object sender, EventArgs e)
        {
            Label_information_updatePackage.Visible = false;
            Picture_information_updatePackage.Visible = false;
            textBox_package_price.BorderColor = Color.Silver;
        }
        private void textBox_package_dmin_TextChanged(object sender, EventArgs e)
        {
            Label_information_updatePackage.Visible = false;
            Picture_information_updatePackage.Visible = false;
            textBox_package_dmin.BorderColor = Color.Silver;
        }
        private void textBox_package_dmax_TextChanged(object sender, EventArgs e)
        {
            Label_information_updatePackage.Visible = false;
            Picture_information_updatePackage.Visible = false;
            textBox_package_dmax.BorderColor = Color.Silver;
        }
        #endregion

        #region methods to update package
        private int pindex = -1;
        private void loadPackageData_inDataGridView(string searchQuery = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT PackageId ,Name, Price , Cost , Dmin,Dmax,AdminId FROM TourPackage";
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                sql += " WHERE PackageId LIKE @searchQuery OR Name LIKE @searchQuery OR Price LIKE @searchQuery OR Cost LIKE @searchQuery OR Dmin LIKE @searchQuery OR Dmax LIKE @searchQuery OR AdminId LIKE @searchQuery";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            // Add parameters for the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            DataGridView_updatePackage.DataSource = dataTable;
        }
        private void LoadPackageInformation_inTheTextBoxes()
        {
            int selectedID = Convert.ToInt32(DataGridView_updatePackage.CurrentRow.Cells["packageIdDataGridView"].Value);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name, Price, Cost, Dmin,Dmax FROM TourPackage WHERE PackageId = @selectedID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@selectedID", selectedID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox_package_name.Text = reader["Name"].ToString();
                        textBox_package_price.Text = reader["Price"].ToString();
                        textBox_package_cost.Text = reader["Cost"].ToString();
                        textBox_package_dmin.Text = reader["dmin"].ToString();
                        textBox_package_dmax.Text = reader["dmax"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Package not found in the database.", "Update Package", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void DataGridView_updatePackage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                pindex = e.RowIndex;
                DataGridViewRow row = DataGridView_updatePackage.Rows[pindex];  // Change 'index' to 'pindex'
                LoadPackageInformation_inTheTextBoxes();
            }
        }
        private void UpdateTextBoxBorders_upackage()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(textBox_package_name.Text))
                textBox_package_name.BorderColor = Color.Red;
            else
                textBox_package_name.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(textBox_package_price.Text))
                textBox_package_price.BorderColor = Color.Red;
            else
                textBox_package_price.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(textBox_package_cost.Text))
                textBox_package_cost.BorderColor = Color.Red;
            else
                textBox_package_cost.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(textBox_package_dmin.Text))
                textBox_package_dmin.BorderColor = Color.Red;
            else
                textBox_package_dmin.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(textBox_package_dmax.Text))
                textBox_package_dmax.BorderColor = Color.Red;
            else
                textBox_package_dmax.BorderColor = Color.Silver;

        }
        private bool AreAllFieldsFilled_upackage()
        {
            return !string.IsNullOrWhiteSpace(textBox_package_name.Text)
                && !string.IsNullOrWhiteSpace(textBox_package_price.Text)
                && !string.IsNullOrWhiteSpace(textBox_package_cost.Text)
                && !string.IsNullOrWhiteSpace(textBox_package_dmin.Text)
                && !string.IsNullOrWhiteSpace(textBox_package_dmax.Text);
        }
        private void UpdatePackage_button_Click(object sender, EventArgs e)
        {
            if (pindex == -1)
            {
                MessageBox.Show("Please select an Package to update its Information.", "Update Package", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (AreAllFieldsFilled_upackage())
            {
                try
                    {
                    int selectedID = Convert.ToInt32(DataGridView_updatePackage.CurrentRow.Cells["packageIdDataGridView"].Value);
                    string packageName = textBox_package_name.Text;
                    float price = float.Parse(textBox_package_price.Text);
                    float cost = float.Parse(textBox_package_cost.Text);
                    int dmin = int.Parse(textBox_package_dmin.Text);
                    int dmax = int.Parse(textBox_package_dmax.Text);
                    int currentAdminId = this.CurrentAdminId;
                    if (!float.TryParse(textBox_package_price.Text, out price))
                    {
                        MessageBox.Show("Price must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!float.TryParse(textBox_package_cost.Text, out cost))
                    {
                        MessageBox.Show("Cost must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(textBox_package_dmin.Text, out dmin))
                    {
                        MessageBox.Show("Minimum demand must be a valid integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(textBox_package_dmax.Text, out dmax))
                    {
                        MessageBox.Show("Maximum demand must be a valid integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validate demand values
                    if (dmin <= 0 || dmax <= 0)
                    {
                        MessageBox.Show("Minimum and maximum demand must be positive numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cost <= 0 || price <= 0)
                    {
                        MessageBox.Show("Price and Cost must be positive numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (dmax < dmin)
                    {
                        MessageBox.Show("Maximum demand must be greater than or equal to minimum demand.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string query = "UPDATE TourPackage SET Name = @name , Price = @price , Cost = @cost , Dmin = @dmin , Dmax = @dmax ,AdminId = @adminId WHERE PackageId = @selectedID";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            cmd.Parameters.AddWithValue("@name", packageName);
                            cmd.Parameters.AddWithValue("@price", price);
                            cmd.Parameters.AddWithValue("@cost", cost);
                            cmd.Parameters.AddWithValue("@dmin", dmin);
                            cmd.Parameters.AddWithValue("@dmax", dmax);
                            cmd.Parameters.AddWithValue("@adminId", currentAdminId);
                            cmd.Parameters.AddWithValue("@selectedID", selectedID);
                            int RowsAffected = cmd.ExecuteNonQuery();

                            if (RowsAffected > 0)
                            {
                                MessageBox.Show("Package information updated successfully.", "Update Package", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Reload the data grid view with updated data
                                loadPackageData_inDataGridView();
                                LoadDataIntoDataGridView();
                                LoadResourceAllocationData();
                                LoadPackageSeasonDataUpdate();
                                viewPackageData_inDataGridView();
                                LoadPackageSeasonData();
                                viewinfoOfPackages_DataGridView();

                            // Clear the input fields
                            textBox_package_name.Text = null;
                                textBox_package_price.Text = null;
                                textBox_package_cost.Text = null;
                                textBox_package_dmin.Text = null;
                                textBox_package_dmax.Text = null;

                                // Reset index
                                pindex = -1;
                            }
                            else
                            {
                                MessageBox.Show("Update failed. Package not found in the database.", "Update Package", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Update Package", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
            else
            {
                Label_information_updatePackage.Visible = true;
                Picture_information_updatePackage.Visible = true;
                UpdateTextBoxBorders_upackage();
            }
        }
        #endregion

        #endregion

        #region view package

        private void SearchTextBox_view_package_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_view_package.Text == "Search...")
            {
                searchTextBox_view_package.Text = "";
                searchTextBox_view_package.ForeColor = Color.Black;
            }
        }
        private void SearchTextBox_view_package_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_view_package.Text))
            {
                searchTextBox_view_package.Text = "Search...";
                searchTextBox_view_package.ForeColor = Color.Gray;
            }
        }
        private void SearchTextBox_view_package_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_view_package.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                viewPackageData_inDataGridView();
            }
            else
            {
                viewPackageData_inDataGridView(searchQuery);
            }
        }
        private void viewPackageData_inDataGridView(string searchQuery = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT * FROM TourPackage";
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                sql += " WHERE PackageId LIKE @searchQuery OR Name LIKE @searchQuery OR Price LIKE @searchQuery OR Cost LIKE @searchQuery OR Dmin LIKE @searchQuery OR Dmax LIKE @searchQuery OR AdminId LIKE @searchQuery";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            // Add parameters for the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            DataGridView_viewPackage.Columns[0].DataPropertyName = "PackageId";
            DataGridView_viewPackage.Columns[1].DataPropertyName = "Name";
            DataGridView_viewPackage.Columns[2].DataPropertyName = "Price";
            DataGridView_viewPackage.Columns[3].DataPropertyName = "Cost";
            DataGridView_viewPackage.Columns[4].DataPropertyName = "Dmin";
            DataGridView_viewPackage.Columns[5].DataPropertyName = "Dmax";
            DataGridView_viewPackage.Columns[6].DataPropertyName = "AdminId";

            // Set the new data source
            DataGridView_viewPackage.DataSource = dataTable;
        }

        #endregion

        #endregion

        #region Package Season

        #region add package season
        private void DataGridView_PackageSeason_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridView_PackageSeason.Rows[e.RowIndex];

                TextBox_PackageName.Text = row.Cells["Package Name"].Value.ToString();
                TextBox_MinDemand.Text = row.Cells["Minimum Demand"].Value.ToString();
                TextBox_MaxDemand.Text = row.Cells["Maximum Demand"].Value.ToString();
            }
        }
        private void LoadSeasonNames()
        {
            string query = "SELECT Name FROM Season";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComboBox_Season.Items.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading resources: " + ex.Message);
            }
        }
        private void PackageSeasonAdd_button_Click(object sender, EventArgs e)
        {
            string packageName = TextBox_PackageName.Text;
            int minDemand;
            int maxDemand;
            string seasonName = ComboBox_Season.SelectedItem?.ToString();
            int actualDemand;

            if (string.IsNullOrEmpty(packageName))
            {
                MessageBox.Show("Please select a package and enter all required details.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Min Demand
            if (!int.TryParse(TextBox_MinDemand.Text, out minDemand) || minDemand <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer value for Minimum Demand.", "Add Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Max Demand
            if (!int.TryParse(TextBox_MaxDemand.Text, out maxDemand) || maxDemand <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer value for Maximum Demand.", "Add Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (minDemand > maxDemand)
            {
                MessageBox.Show("Minimum Demand cannot be greater than Maximum Demand.", "Add Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AreAllFieldsFilled_packageSeason())
            {
                if (!int.TryParse(TextBox_ActualDemand.Text, out actualDemand) || actualDemand <= 0)
                {
                    MessageBox.Show("Please enter a valid positive integer value for Actual Demand.", "Add Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (actualDemand < minDemand || actualDemand > maxDemand)
                {
                    MessageBox.Show("Actual Demand must be between Minimum Demand and Maximum Demand.", "Add Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Get the PackageId
                        string getPackageIdQuery = @"
                    SELECT PackageId
                    FROM TourPackage
                    WHERE Name = @PackageName";

                        SqlCommand getPackageIdCommand = new SqlCommand(getPackageIdQuery, connection);
                        getPackageIdCommand.Parameters.AddWithValue("@PackageName", packageName);
                        int packageId = (int)getPackageIdCommand.ExecuteScalar();

                        // Get the SeasonId
                        string getSeasonIdQuery = @"
                    SELECT SeasonId
                    FROM Season
                    WHERE Name = @SeasonName";

                        SqlCommand getSeasonIdCommand = new SqlCommand(getSeasonIdQuery, connection);
                        getSeasonIdCommand.Parameters.AddWithValue("@SeasonName", seasonName);
                        int seasonId = (int)getSeasonIdCommand.ExecuteScalar();

                        // Check if the record already exists
                        string checkExistingRecordQuery = @"
                SELECT COUNT(*)
                FROM PackageSeason
                WHERE PackageId = @PackageId AND SeasonId = @SeasonId AND Demand = @ActualDemand";

                        SqlCommand checkExistingRecordCommand = new SqlCommand(checkExistingRecordQuery, connection);
                        checkExistingRecordCommand.Parameters.AddWithValue("@PackageId", packageId);
                        checkExistingRecordCommand.Parameters.AddWithValue("@SeasonId", seasonId);
                        checkExistingRecordCommand.Parameters.AddWithValue("@ActualDemand", actualDemand);

                        int existingRecordCount = (int)checkExistingRecordCommand.ExecuteScalar();

                        if (existingRecordCount > 0)
                        {
                            MessageBox.Show("This package with the same actual demand and season already exists.", "Add Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Insert into PackageSeason
                        string insertPackageSeasonQuery = @"
                    INSERT INTO PackageSeason (PackageId, SeasonId,Demand)
                    VALUES (@PackageId, @SeasonId, @ActualDemand)";

                        SqlCommand insertPackageSeasonCommand = new SqlCommand(insertPackageSeasonQuery, connection);
                        insertPackageSeasonCommand.Parameters.AddWithValue("@PackageId", packageId);
                        insertPackageSeasonCommand.Parameters.AddWithValue("@SeasonId", seasonId);
                        insertPackageSeasonCommand.Parameters.AddWithValue("@ActualDemand", actualDemand);

                        int rowsAffected = insertPackageSeasonCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Package Season Added successfully.", "Add Package Season", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            TextBox_PackageName.Text = null;
                            TextBox_ActualDemand.Text = null;
                            TextBox_MinDemand.Text = null;
                            TextBox_MaxDemand.Text = null;
                            ComboBox_Season.SelectedItem = null;
                            LoadDataIntoDataGridView();
                            LoadResourceAllocationData();
                            LoadPackageSeasonDataUpdate();
                            ViewPackageSeason();
                            viewinfoOfPackages_DataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the package season.", "Add Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Label_information_PackageSeason.Visible = true;
                Picture_information_PackageSeason.Visible = true;
                UpdateTextBoxBorders_packageSeason();
            }
        }
        private bool AreAllFieldsFilled_packageSeason()
        {
            return !string.IsNullOrWhiteSpace(TextBox_ActualDemand.Text)
                && ComboBox_Season.SelectedItem != null;
        }
        private void UpdateTextBoxBorders_packageSeason()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(TextBox_ActualDemand.Text))
                TextBox_ActualDemand.BorderColor = Color.Red;
            else
                TextBox_ActualDemand.BorderColor = Color.Silver;
            if (ComboBox_Season.SelectedItem == null)
                ComboBox_Season.BorderColor = Color.Red;
            else
                ComboBox_Season.BorderColor = Color.Silver;
        }
        private void TextBox_ActualDemand_TextChanged(object sender, EventArgs e)
        {
            Label_information_PackageSeason.Visible = false;
            Picture_information_PackageSeason.Visible = false;
            TextBox_ActualDemand.BorderColor = Color.Silver;
            ComboBox_Season.BorderColor = Color.Silver;
        }
        private void LoadPackageSeasonData()
        {
            string query = @"
        SELECT 
            tp.Name AS 'Package Name', 
            tp.Dmin AS 'Minimum Demand', 
            tp.Dmax AS 'Maximum Demand'
        FROM 
            TourPackage tp";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        DataGridView_PackageSeason.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data: " + ex.Message);
            }
        }

        #endregion

        #region update package season
        private void UpdatePackageSeason_button_Click(object sender, EventArgs e)
        {
            string packageName = TextBox_uPackageSeasonName.Text;
            int minDemand;
            int maxDemand;
            string seasonName = ComboBox_uPackageSeasonSeason.SelectedItem?.ToString();
            int actualDemand;

            if (string.IsNullOrEmpty(packageName))
            {
                MessageBox.Show("Please select a package and enter all required details.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Min Demand
            if (!int.TryParse(TextBox_uPackageSeasonDmin.Text, out minDemand) || minDemand <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer value for Minimum Demand.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Max Demand
            if (!int.TryParse(TextBox_uPackageSeasonDmax.Text, out maxDemand) || maxDemand <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer value for Maximum Demand.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (minDemand > maxDemand)
            {
                MessageBox.Show("Minimum Demand cannot be greater than Maximum Demand.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AreAllFieldsFilled_upackageSeason())
            {
                if (!int.TryParse(TextBox_uPackageSeasonDemand.Text, out actualDemand) || actualDemand <= 0)
                {
                    MessageBox.Show("Please enter a valid positive integer value for Actual Demand.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (actualDemand < minDemand || actualDemand > maxDemand)
                {
                    MessageBox.Show("Actual Demand must be between Minimum Demand and Maximum Demand.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Get the PackageId
                        string getPackageIdQuery = @"
                    SELECT PackageId
                    FROM TourPackage
                    WHERE Name = @PackageName";

                        SqlCommand getPackageIdCommand = new SqlCommand(getPackageIdQuery, connection);
                        getPackageIdCommand.Parameters.AddWithValue("@PackageName", packageName);
                        int packageId = (int)getPackageIdCommand.ExecuteScalar();


                        // Get the SeasonId
                        string getSeasonIdQuery = @"
                    SELECT SeasonId
                    FROM Season
                    WHERE Name = @SeasonName";

                        SqlCommand getSeasonIdCommand = new SqlCommand(getSeasonIdQuery, connection);
                        getSeasonIdCommand.Parameters.AddWithValue("@SeasonName", seasonName);
                        int seasonId = (int)getSeasonIdCommand.ExecuteScalar();


                        // Check if a row is selected
                        if (DataGridView_UpdatePackageSeason.CurrentRow == null)
                        {
                            MessageBox.Show("Please select a package season to update.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Get the original demand and season from the selected row in the DataGridView
                        var originalDemandValue = DataGridView_UpdatePackageSeason.CurrentRow.Cells["Actual Demand"].Value;
                        var originalSeasonValue = DataGridView_UpdatePackageSeason.CurrentRow.Cells["Season"].Value;

                        if (originalDemandValue == null || !int.TryParse(originalDemandValue.ToString(), out int originalDemand) ||
                            originalSeasonValue == null)
                        {
                            MessageBox.Show("Could not retrieve the original demand or season from the selected package season.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Get the original SeasonId
                        string getOriginalSeasonIdQuery = @"
                SELECT SeasonId
                FROM Season
                WHERE Name = @OriginalSeasonName";

                        SqlCommand getOriginalSeasonIdCommand = new SqlCommand(getOriginalSeasonIdQuery, connection);
                        getOriginalSeasonIdCommand.Parameters.AddWithValue("@OriginalSeasonName", originalSeasonValue.ToString());
                        int originalSeasonId = (int)getOriginalSeasonIdCommand.ExecuteScalar();

                        // Get the PackageSeasonId based on PackageId, original SeasonId, and original demand
                        string getPackageSeasonIdQuery = @"
                SELECT PackageSeasonId
                FROM PackageSeason
                WHERE PackageId = @PackageId AND SeasonId = @OriginalSeasonId AND Demand = @OriginalDemand";

                        SqlCommand getPackageSeasonIdCommand = new SqlCommand(getPackageSeasonIdQuery, connection);
                        getPackageSeasonIdCommand.Parameters.AddWithValue("@PackageId", packageId);
                        getPackageSeasonIdCommand.Parameters.AddWithValue("@OriginalSeasonId", originalSeasonId);
                        getPackageSeasonIdCommand.Parameters.AddWithValue("@OriginalDemand", originalDemand);
                        object packageSeasonIdObj = getPackageSeasonIdCommand.ExecuteScalar();

                        if (packageSeasonIdObj == null)
                        {
                            MessageBox.Show("Could not retrieve the PackageSeasonId.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        int packageSeasonId = (int)packageSeasonIdObj;


                        // Check if the record already exists
                        string checkExistingRecordQuery = @"
                SELECT COUNT(*)
                FROM PackageSeason
                WHERE PackageId = @PackageId AND SeasonId = @SeasonId AND Demand = @ActualDemand";

                        SqlCommand checkExistingRecordCommand = new SqlCommand(checkExistingRecordQuery, connection);
                        checkExistingRecordCommand.Parameters.AddWithValue("@PackageId", packageId);
                        checkExistingRecordCommand.Parameters.AddWithValue("@SeasonId", seasonId);
                        checkExistingRecordCommand.Parameters.AddWithValue("@ActualDemand", actualDemand);

                        int existingRecordCount = (int)checkExistingRecordCommand.ExecuteScalar();

                        if (existingRecordCount > 0)
                        {
                            MessageBox.Show("This package with the same actual demand and season already exists.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Update the PackageSeason with the new Actual Demand
                        string updatePackageSeasonQuery = @"
                    UPDATE PackageSeason
                    SET Demand = @ActualDemand, SeasonId = @SeasonId
                    WHERE PackageSeasonId = @PackageSeasonId";

                        SqlCommand updatePackageSeasonCommand = new SqlCommand(updatePackageSeasonQuery, connection);
                        updatePackageSeasonCommand.Parameters.AddWithValue("@ActualDemand", actualDemand);
                        updatePackageSeasonCommand.Parameters.AddWithValue("@PackageSeasonId", packageSeasonId);
                        updatePackageSeasonCommand.Parameters.AddWithValue("@SeasonId", seasonId);

                        int rowsAffected = updatePackageSeasonCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Package Season Updated successfully.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            TextBox_uPackageSeasonName.Text = null;
                            TextBox_uPackageSeasonDemand.Text = null;
                            TextBox_uPackageSeasonDmin.Text = null;
                            TextBox_uPackageSeasonDmax.Text = null;
                            ComboBox_uPackageSeasonSeason.SelectedItem = null;
                            LoadDataIntoDataGridView();
                            LoadResourceAllocationData();
                            LoadPackageSeasonDataUpdate();
                            ViewPackageSeason();
                            viewinfoOfPackages_DataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update the package season.", "Update Package Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Label_information_upackageSeason.Visible = true;
                Picture_information_upackageSeason.Visible = true;
                UpdateTextBoxBorders_upackageSeason();
            }
        }
        private void LoadSeasonNamesUpdate()
        {
            string query = "SELECT Name FROM Season";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComboBox_uPackageSeasonSeason.Items.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading resources: " + ex.Message);
            }
        }
        private void DataGridView_UpdatePackageSeason_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridView_UpdatePackageSeason.Rows[e.RowIndex];

                TextBox_uPackageSeasonName.Text = row.Cells["Package"].Value.ToString();
                TextBox_uPackageSeasonDmin.Text = row.Cells["Min Demand"].Value.ToString();
                TextBox_uPackageSeasonDmax.Text = row.Cells["Max Demand"].Value.ToString();
                TextBox_uPackageSeasonDemand.Text = row.Cells["Actual Demand"].Value.ToString();
                ComboBox_uPackageSeasonSeason.SelectedItem = row.Cells["Season"].Value.ToString();
            }
        }
        private void LoadPackageSeasonDataUpdate()
        {
            string query = @"
         SELECT 
    tp.Name AS Package, 
    tp.Dmin AS 'Min Demand', 
    tp.Dmax AS 'Max Demand', 
    ps.Demand AS 'Actual Demand', 
    s.Name AS Season
FROM 
    TourPackage tp
INNER JOIN 
    PackageSeason ps ON tp.PackageId = ps.PackageId
INNER JOIN 
    Season s ON ps.SeasonId = s.SeasonId;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        DataGridView_UpdatePackageSeason.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data: " + ex.Message);
            }
        }
        private void TextBox_uPackageSeasonDemand_TextChanged(object sender, EventArgs e)
        {
            Label_information_upackageSeason.Visible = false;
            Picture_information_upackageSeason.Visible = false;
            TextBox_uPackageSeasonDemand.BorderColor = Color.Silver;
            ComboBox_uPackageSeasonSeason.BorderColor = Color.Silver;

        }
        private bool AreAllFieldsFilled_upackageSeason()
        {
            return !string.IsNullOrWhiteSpace(TextBox_uPackageSeasonDemand.Text)
            && ComboBox_uPackageSeasonSeason.SelectedItem != null;
        }
        private void UpdateTextBoxBorders_upackageSeason()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(TextBox_uPackageSeasonDemand.Text))
                TextBox_uPackageSeasonDemand.BorderColor = Color.Red;
            else
                TextBox_uPackageSeasonDemand.BorderColor = Color.Silver;
            if (ComboBox_uPackageSeasonSeason.SelectedItem == null)
                ComboBox_uPackageSeasonSeason.BorderColor = Color.Red;
            else
                ComboBox_uPackageSeasonSeason.BorderColor = Color.Silver;
        }

        #endregion

        #region view package season
        private void searchTextBox_view_packageSeason_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_view_packageSeason.Text == "Search...")
            {
                searchTextBox_view_packageSeason.Text = "";
                searchTextBox_view_packageSeason.ForeColor = Color.Black;
            }
        }
        private void searchTextBox_view_packageSeason_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_view_packageSeason.Text))
            {
                searchTextBox_view_packageSeason.Text = "Search...";
                searchTextBox_view_packageSeason.ForeColor = Color.Gray;
            }
        }
        private void searchTextBox_view_packageSeason_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_view_packageSeason.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                ViewPackageSeason();
            }
            else
            {
                ViewPackageSeason(searchQuery);
            }
        }
        private void ViewPackageSeason(string searchQuery = "")
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL query to fetch data
                string sql = @"
            SELECT ps.PackageSeasonId AS ID,
                   tp.Name AS Package, 
                   tp.Dmin AS 'Min Demand', 
                   tp.Dmax AS 'Max Demand', 
                   ps.Demand AS 'Actual Demand', 
                   s.Name AS Season
            FROM TourPackage tp
            INNER JOIN PackageSeason ps ON tp.PackageId = ps.PackageId
            INNER JOIN Season s ON ps.SeasonId = s.SeasonId";

                // Append WHERE clause if searchQuery is provided
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    sql += " WHERE ps.PackageSeasonId LIKE @searchQuery " +
                           "OR tp.Name LIKE @searchQuery " +
                           "OR tp.Dmin LIKE @searchQuery " +
                           "OR tp.Dmax LIKE @searchQuery " +
                           "OR ps.Demand LIKE @searchQuery " +
                           "OR s.Name LIKE @searchQuery";
                }

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Add parameter for searchQuery
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
                }

                // Create data adapter and fill data into DataTable
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Bind DataTable to DataGridView
                DataGridView_ViewPackageSeason.DataSource = dataTable;
            }
        }
        #endregion


        #endregion

        #region Allocation Resources

        #region allocate resources

        private void DataGridView_allocateResource_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridView_allocateResource.Rows[e.RowIndex];

                text_PackageName.Text = row.Cells["Package Name"].Value.ToString();
                text_PackageDemand.Text = row.Cells["Demand"].Value.ToString();
                text_PackageSeason.Text = row.Cells["Season"].Value.ToString();
            }
        }
        private void LoadResourceNames()
        {
            string query = "SELECT Name FROM Resource";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComboBox_PackageTypeOfResource.Items.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading resources: " + ex.Message);
            }
        }
        private void AllocateResource_button_Click(object sender, EventArgs e)
        {
            string packageName = text_PackageName.Text;
            string seasonName = text_PackageSeason.Text;
            string resourceName = ComboBox_PackageTypeOfResource.SelectedItem?.ToString();
            int resourceAllocation;

            if (string.IsNullOrEmpty(packageName) || string.IsNullOrEmpty(seasonName))
            {
                MessageBox.Show("Please select a package and enter all required details.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AreAllFieldsFilled_resource())
            {
                if (!int.TryParse(text_PackageResource.Text, out resourceAllocation) || resourceAllocation <= 0)
                {
                    MessageBox.Show("Please enter a valid positive integer value for resource allocation.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Get the PackageSeasonId
                        string getPackageSeasonQuery = @"
                SELECT ps.PackageSeasonId
                FROM PackageSeason ps
                INNER JOIN TourPackage tp ON ps.PackageId = tp.PackageId
                INNER JOIN Season s ON ps.SeasonId = s.SeasonId
                WHERE tp.Name = @PackageName AND s.Name = @SeasonName";

                        SqlCommand getPackageSeasonCommand = new SqlCommand(getPackageSeasonQuery, connection);
                        getPackageSeasonCommand.Parameters.AddWithValue("@PackageName", packageName);
                        getPackageSeasonCommand.Parameters.AddWithValue("@SeasonName", seasonName);

                        int packageSeasonId = (int)getPackageSeasonCommand.ExecuteScalar();

                        // Get the ResourceId and AvailableAmount
                        string getResourceDetailsQuery = "SELECT ResourceId, AvailableAmount FROM Resource WHERE Name = @ResourceName";
                        SqlCommand getResourceDetailsCommand = new SqlCommand(getResourceDetailsQuery, connection);
                        getResourceDetailsCommand.Parameters.AddWithValue("@ResourceName", resourceName);

                        int resourceId = 0;
                        int availableAmount = 0;

                        using (SqlDataReader reader = getResourceDetailsCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                resourceId = reader.GetInt32(0);
                                availableAmount = reader.GetInt32(1);
                            }
                            else
                            {
                                MessageBox.Show("Resource not found.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        if (resourceAllocation > availableAmount)
                        {
                            MessageBox.Show("The allocation amount exceeds the available amount of resources.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Check if the resource is already allocated to the package in the same season
                        string checkResourceAllocationQuery = @"
                SELECT COUNT(*)
                FROM ResourceAllocation
                WHERE PackageSeasonId = @PackageSeasonId AND ResourceId = @ResourceId";

                        SqlCommand checkResourceAllocationCommand = new SqlCommand(checkResourceAllocationQuery, connection);
                        checkResourceAllocationCommand.Parameters.AddWithValue("@PackageSeasonId", packageSeasonId);
                        checkResourceAllocationCommand.Parameters.AddWithValue("@ResourceId", resourceId);

                        int allocationCount = (int)checkResourceAllocationCommand.ExecuteScalar();

                        if (allocationCount > 0)
                        {
                            MessageBox.Show("This package already has this resource allocated for the selected season.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Insert into ResourceAllocation
                        string insertResourceAllocationQuery = @"
                INSERT INTO ResourceAllocation (AllocationAmount, PackageSeasonId, ResourceId)
                VALUES (@AllocationAmount, @PackageSeasonId, @ResourceId)";

                        SqlCommand insertResourceAllocationCommand = new SqlCommand(insertResourceAllocationQuery, connection);
                        insertResourceAllocationCommand.Parameters.AddWithValue("@AllocationAmount", resourceAllocation);
                        insertResourceAllocationCommand.Parameters.AddWithValue("@PackageSeasonId", packageSeasonId);
                        insertResourceAllocationCommand.Parameters.AddWithValue("@ResourceId", resourceId);

                        int rowsAffected = insertResourceAllocationCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Resource allocated successfully.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            text_PackageName.Text = null;
                            text_PackageDemand.Text = null;
                            text_PackageSeason.Text = null;
                            text_PackageResource.Text = null;
                            ComboBox_PackageTypeOfResource.SelectedItem = null;
                            LoadDataIntoDataGridView();
                            LoadDataIntoDataGridView_viewAllocation();
                            viewinfoOfPackages_DataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Failed to allocate the resource.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                Label_information_allocateResource.Visible = true;
                Picture_information_allocateResource.Visible = true;
                UpdateTextBoxBorders_resource();
            }
        }
        private bool AreAllFieldsFilled_resource()
        {
            return !string.IsNullOrWhiteSpace(text_PackageResource.Text)
                && ComboBox_PackageTypeOfResource.SelectedItem != null;
        }
        private void UpdateTextBoxBorders_resource()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(text_PackageResource.Text))
                text_PackageResource.BorderColor = Color.Red;
            else
                text_PackageResource.BorderColor = Color.Silver;
            if (ComboBox_PackageTypeOfResource.SelectedItem == null)
                ComboBox_PackageTypeOfResource.BorderColor = Color.Red;
            else
                ComboBox_PackageTypeOfResource.BorderColor = Color.Silver;
        }
        private void text_PackageResource_TextChanged(object sender, EventArgs e)
        {
            Label_information_allocateResource.Visible = false;
            Picture_information_allocateResource.Visible = false;
            text_PackageResource.BorderColor = Color.Silver;
            ComboBox_PackageTypeOfResource.BorderColor = Color.Silver;
        }
        private void LoadResourceAllocationData()
        {
            string query = @"
        SELECT 
            tp.Name AS 'Package Name', 
            ps.Demand, 
            s.Name AS Season
        FROM 
            PackageSeason ps
        INNER JOIN 
            TourPackage tp ON ps.PackageId = tp.PackageId
        INNER JOIN 
            Season s ON ps.SeasonId = s.SeasonId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        DataGridView_allocateResource.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data: " + ex.Message);
            }
        }

        #endregion

        #region update allocation
        private void LoadDataIntoDataGridView()
        {
            string query = @"
                SELECT tp.Name AS Package, ps.Demand, s.Name AS Season, r.Name AS Resource, ra.AllocationAmount AS 'Amount Of Resources'
                FROM ResourceAllocation ra
                INNER JOIN PackageSeason ps ON ra.PackageSeasonId = ps.PackageSeasonId
                INNER JOIN TourPackage tp ON ps.PackageId = tp.PackageId
                INNER JOIN Season s ON ps.SeasonId = s.SeasonId
                INNER JOIN Resource r ON ra.ResourceId = r.ResourceId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        DataGridView_UpdateallocateResource.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data: " + ex.Message);
            }
        }
        private void UpdateAllocateResource_button_Click(object sender, EventArgs e)
        {
            string packageName = text_uPackageName.Text;
            string seasonName = text_uPackageSeason.Text;
            string resourceName = text_uPackageTypeResource.Text;
            int newAllocationAmount;

            if (string.IsNullOrEmpty(packageName) || string.IsNullOrEmpty(seasonName))
            {
                MessageBox.Show("Please select a package and enter all required details.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AreAllFieldsFilled_uresource())
            {

                if (!int.TryParse(text_uPackageResource.Text, out newAllocationAmount) || newAllocationAmount <= 0)
                {
                    MessageBox.Show("Please enter a valid positive integer value for resource allocation.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Get PackageSeasonId
                        string getPackageSeasonQuery = @"
                SELECT ps.PackageSeasonId
                FROM PackageSeason ps
                INNER JOIN TourPackage tp ON ps.PackageId = tp.PackageId
                INNER JOIN Season s ON ps.SeasonId = s.SeasonId
                WHERE tp.Name = @PackageName AND s.Name = @SeasonName";

                        SqlCommand getPackageSeasonCommand = new SqlCommand(getPackageSeasonQuery, connection);
                        getPackageSeasonCommand.Parameters.AddWithValue("@PackageName", packageName);
                        getPackageSeasonCommand.Parameters.AddWithValue("@SeasonName", seasonName);

                        int packageSeasonId = (int)getPackageSeasonCommand.ExecuteScalar();

                        // Get ResourceId and AvailableAmount
                        string getResourceDetailsQuery = "SELECT ResourceId, AvailableAmount FROM Resource WHERE Name = @ResourceName";
                        SqlCommand getResourceDetailsCommand = new SqlCommand(getResourceDetailsQuery, connection);
                        getResourceDetailsCommand.Parameters.AddWithValue("@ResourceName", resourceName);

                        using (SqlDataReader reader = getResourceDetailsCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int resourceId = reader.GetInt32(0);
                                int availableAmount = reader.GetInt32(1);

                                if (newAllocationAmount > availableAmount)
                                {
                                    MessageBox.Show("The allocation amount exceeds the available amount of resources.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                reader.Close(); // Close the reader before executing another command

                                // Update ResourceAllocation
                                string updateResourceAllocationQuery = @"
                        UPDATE ResourceAllocation
                        SET AllocationAmount = @AllocationAmount
                        WHERE PackageSeasonId = @PackageSeasonId AND ResourceId = @ResourceId";

                                SqlCommand updateResourceAllocationCommand = new SqlCommand(updateResourceAllocationQuery, connection);
                                updateResourceAllocationCommand.Parameters.AddWithValue("@AllocationAmount", newAllocationAmount);
                                updateResourceAllocationCommand.Parameters.AddWithValue("@PackageSeasonId", packageSeasonId);
                                updateResourceAllocationCommand.Parameters.AddWithValue("@ResourceId", resourceId);

                                int rowsAffected = updateResourceAllocationCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Resource allocation updated successfully.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    // Refresh DataGridView
                                    LoadDataIntoDataGridView();
                                    LoadDataIntoDataGridView_viewAllocation();
                                    viewinfoOfPackages_DataGridView();

                                    // Clear text boxes
                                    text_uPackageName.Text = null;
                                    text_uPackageDemand.Text = null;
                                    text_uPackageSeason.Text = null;
                                    text_uPackageTypeResource.Text = null;
                                    text_uPackageResource.Text = null;
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update resource allocation.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Resource not found.", "Resource Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Label_information_uResource.Visible = true;
                Picture_information_uResource.Visible = true;
                UpdateTextBoxBorders_uresource();
            }
        }
        private void DataGridView_UpdateallocateResource_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridView_UpdateallocateResource.Rows[e.RowIndex];

                text_uPackageName.Text = row.Cells["Package"].Value.ToString();
                text_uPackageDemand.Text = row.Cells["Demand"].Value.ToString();
                text_uPackageSeason.Text = row.Cells["Season"].Value.ToString();
                text_uPackageTypeResource.Text = row.Cells["Resource"].Value.ToString();
                text_uPackageResource.Text = row.Cells["Amount Of Resources"].Value.ToString();
            }
        }
        private void text_uPackageResource_TextChanged(object sender, EventArgs e)
        {
            Label_information_uResource.Visible = false;
            Picture_information_uResource.Visible = false;
            text_uPackageResource.BorderColor = Color.Silver;
        }
        private bool AreAllFieldsFilled_uresource()
        {
            return !string.IsNullOrWhiteSpace(text_uPackageResource.Text);
        }
        private void UpdateTextBoxBorders_uresource()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(text_uPackageResource.Text))
                text_uPackageResource.BorderColor = Color.Red;
            else
                text_uPackageResource.BorderColor = Color.Silver;
        }

        #endregion

        #region view allocation
        private void searchTextBox_view_allocatedResourcesn_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_view_allocatedResources.Text == "Search...")
            {
                searchTextBox_view_allocatedResources.Text = "";
                searchTextBox_view_allocatedResources.ForeColor = Color.Black;
            }
        }
        private void searchTextBox_view_allocatedResources_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_view_allocatedResources.Text))
            {
                searchTextBox_view_allocatedResources.Text = "Search...";
                searchTextBox_view_allocatedResources.ForeColor = Color.Gray;
            }
        }
        private void searchTextBox_view_allocatedResourcesn_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_view_allocatedResources.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                LoadDataIntoDataGridView_viewAllocation();
            }
            else
            {
                LoadDataIntoDataGridView_viewAllocation(searchQuery);
            }
        }
        private void LoadDataIntoDataGridView_viewAllocation(string searchQuery = "")
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
            SELECT tp.Name AS Package, ps.Demand, s.Name AS Season, r.Name AS Resource, ra.AllocationAmount AS AmountOfResources
            FROM ResourceAllocation ra
            INNER JOIN PackageSeason ps ON ra.PackageSeasonId = ps.PackageSeasonId
            INNER JOIN TourPackage tp ON ps.PackageId = tp.PackageId
            INNER JOIN Season s ON ps.SeasonId = s.SeasonId
            INNER JOIN Resource r ON ra.ResourceId = r.ResourceId";

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    sql += " WHERE tp.Name LIKE @searchQuery OR ps.Demand LIKE @searchQuery OR s.Name LIKE @searchQuery OR r.Name LIKE @searchQuery OR ra.AllocationAmount LIKE @searchQuery";
                }

                SqlCommand cmd = new SqlCommand(sql, connection);

                // Add parameters for the search query
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
                }

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                DataGridView_viewallocateResource.AutoGenerateColumns = false;

                // Clear existing columns
                DataGridView_viewallocateResource.Columns.Clear();

                // Define and add columns
                DataGridView_viewallocateResource.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Package", DataPropertyName = "Package" });
                DataGridView_viewallocateResource.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Demand", DataPropertyName = "Demand" });
                DataGridView_viewallocateResource.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Season", DataPropertyName = "Season" });
                DataGridView_viewallocateResource.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Resource", DataPropertyName = "Resource" });
                DataGridView_viewallocateResource.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Amount Of Resources", DataPropertyName = "AmountOfResources" });

                // Set the new data source
                DataGridView_viewallocateResource.DataSource = dataTable;
            }
        }

        #endregion

        #endregion

        #region Resources

        #region add new resource

        #region methods for add new resource panel
        private void UpdateTextBoxBorders_resources()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(TextBox_ResourceCost.Text))
                TextBox_ResourceCost.BorderColor = Color.Red;
            else
                TextBox_ResourceCost.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(TextBox_ResourceAmount.Text))
                TextBox_ResourceAmount.BorderColor = Color.Red;
            else
                TextBox_ResourceAmount.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(TextBox_ResourceType.Text))
                TextBox_ResourceType.BorderColor = Color.Red;
            else
                TextBox_ResourceType.BorderColor = Color.Silver;
        }
        private bool AreAllFieldsFilled_resources()
        {
            return !string.IsNullOrWhiteSpace(TextBox_ResourceCost.Text)
                && !string.IsNullOrWhiteSpace(TextBox_ResourceAmount.Text)
                && !string.IsNullOrWhiteSpace(TextBox_ResourceType.Text);
        }
        #endregion

        #region textBoxes and pictures of add new resource panel
        private void AddResource_button_Click(object sender, EventArgs e)
        {
            int adminId = this.CurrentAdminId;

            // Validate if all fields are filled
            if (AreAllFieldsFilled_resources())
            {
                string resourceName = TextBox_ResourceType.Text.Trim();
                string resourceCostStr = TextBox_ResourceCost.Text.Trim();
                string resourceAmountStr = TextBox_ResourceAmount.Text.Trim();

                // Validate resource cost
                if (!decimal.TryParse(resourceCostStr, out decimal resourceCost) || resourceCost < 0)
                {
                    MessageBox.Show("Please enter a valid non-negative number for Resource Cost.", "Add Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TextBox_ResourceCost.Focus();
                    TextBox_ResourceCost.SelectAll();
                    return;
                }

                // Validate resource amount
                if (!int.TryParse(resourceAmountStr, out int resourceAmount) || resourceAmount <= 0)
                {
                    MessageBox.Show("Please enter a valid positive integer for Available Amount.", "Add Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TextBox_ResourceAmount.Focus();
                    TextBox_ResourceAmount.SelectAll();
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if the resource name already exists
                        string checkResourceQuery = "SELECT COUNT(*) FROM Resource WHERE Name = @ResourceName";
                        SqlCommand checkResourceCommand = new SqlCommand(checkResourceQuery, connection);
                        checkResourceCommand.Parameters.AddWithValue("@ResourceName", resourceName);

                        int existingResourceCount = (int)checkResourceCommand.ExecuteScalar();

                        if (existingResourceCount > 0)
                        {
                            MessageBox.Show("This resource name already exists in the database.", "Add Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Insert the resource into the database
                        string insertResourceQuery = @"
                    INSERT INTO Resource (Name, Cost, AvailableAmount, AdminId)
                    VALUES (@ResourceName, @ResourceCost, @ResourceAmount, @AdminId)";

                        SqlCommand insertResourceCommand = new SqlCommand(insertResourceQuery, connection);
                        insertResourceCommand.Parameters.AddWithValue("@ResourceName", resourceName);
                        insertResourceCommand.Parameters.AddWithValue("@ResourceCost", resourceCost);
                        insertResourceCommand.Parameters.AddWithValue("@ResourceAmount", resourceAmount);
                        insertResourceCommand.Parameters.AddWithValue("@AdminId", adminId);

                        int rowsAffected = insertResourceCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Resource Added Successfully!", "Add Resource", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Clear input fields or reset form state as needed
                            TextBox_ResourceType.Clear();
                            TextBox_ResourceCost.Clear();
                            TextBox_ResourceAmount.Clear();
                            LoadDataIntoDataGridView();  // Optional: Refresh your data grid view if needed
                            viewResourcesData_inDataGridView();
                            LoadResourceNames();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the resource.", "Add Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Label_information_AddResource.Visible = true;
                Picture_information_AddResource.Visible = true;
                UpdateTextBoxBorders_resources();
            }
        }

        private void TextBox_ResourceCost_TextChanged(object sender, EventArgs e)
        {
            Label_information_AddResource.Visible = false;
            Picture_information_AddResource.Visible = false;
            TextBox_ResourceCost.BorderColor = Color.Silver;
            TextBox_ResourceAmount.BorderColor = Color.Silver;
            TextBox_ResourceType.BorderColor = Color.Silver;
        }

        private void TextBox_ResourceAmount_TextChanged(object sender, EventArgs e)
        {
            Label_information_AddResource.Visible = false;
            Picture_information_AddResource.Visible = false;
            TextBox_ResourceAmount.BorderColor = Color.Silver;
            TextBox_ResourceCost.BorderColor = Color.Silver;
            TextBox_ResourceType.BorderColor = Color.Silver;
        }
        private void TextBox_ResourceType_TextChanged(object sender, EventArgs e)
        {
            Label_information_AddResource.Visible = false;
            Picture_information_AddResource.Visible = false;
            TextBox_ResourceAmount.BorderColor = Color.Silver;
            TextBox_ResourceCost.BorderColor = Color.Silver;
            TextBox_ResourceType.BorderColor = Color.Silver;
        }





        #endregion

        #endregion

        #region update resource
        private int uindex = -1;
        private void LoadResourceData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Name, Cost, AvailableAmount, AdminId FROM Resource";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    DataGridView_uResources.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading resource data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DataGridView_uResources_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                uindex = e.RowIndex;
                DataGridViewRow row = DataGridView_uResources.Rows[e.RowIndex];
                TextBox_uResourceType.Text = row.Cells["Name"].Value.ToString();
                TextBox_uResourceCost.Text = row.Cells["Cost"].Value.ToString();
                TextBox_uResourceAmount.Text = row.Cells["AvailableAmount"].Value.ToString();
            }
        }
        private void UpdateResources_Button_Click(object sender, EventArgs e)
        {
            if (uindex == -1)
            {
                MessageBox.Show("Please select an Resource to update its Information.", "Update Resources", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (AreAllFieldsFilled_uResources())
            {
                string currentResourceName = TextBox_uResourceType.Text.Trim();
            string resourceCostStr = TextBox_uResourceCost.Text.Trim();
            string resourceAmountStr = TextBox_uResourceAmount.Text.Trim();

            // Validate resource cost
            if (!decimal.TryParse(resourceCostStr, out decimal resourceCost) || resourceCost < 0)
            {
                MessageBox.Show("Please enter a valid non-negative number for Resource Cost.", "Update Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_uResourceCost.Focus();
                TextBox_uResourceCost.SelectAll();
                return;
            }

            // Validate resource amount
            if (!int.TryParse(resourceAmountStr, out int resourceAmount) || resourceAmount <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer for Available Amount.", "Update Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_uResourceAmount.Focus();
                TextBox_uResourceAmount.SelectAll();
                return;
            }

            int adminId = this.CurrentAdminId;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the new resource name already exists in the database
                    string checkExistingResourceQuery = "SELECT COUNT(*) FROM Resource WHERE Name = @NewResourceName AND Name != @CurrentResourceName";
                    SqlCommand checkExistingResourceCommand = new SqlCommand(checkExistingResourceQuery, connection);
                    checkExistingResourceCommand.Parameters.AddWithValue("@NewResourceName", currentResourceName);
                    checkExistingResourceCommand.Parameters.AddWithValue("@CurrentResourceName", DataGridView_uResources.CurrentRow.Cells["Name"].Value.ToString());

                    int existingResourceCount = (int)checkExistingResourceCommand.ExecuteScalar();

                    if (existingResourceCount > 0)
                    {
                        MessageBox.Show("This resource name already exists in the database.", "Update Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Update the resource details
                    string updateResourceQuery = @"
                UPDATE Resource
                SET Name = @NewResourceName, Cost = @ResourceCost, AvailableAmount = @ResourceAmount, AdminId = @AdminId
                WHERE Name = @CurrentResourceName";

                    SqlCommand updateResourceCommand = new SqlCommand(updateResourceQuery, connection);
                    updateResourceCommand.Parameters.AddWithValue("@NewResourceName", currentResourceName);
                    updateResourceCommand.Parameters.AddWithValue("@ResourceCost", resourceCost);
                    updateResourceCommand.Parameters.AddWithValue("@ResourceAmount", resourceAmount);
                    updateResourceCommand.Parameters.AddWithValue("@AdminId", adminId);
                    updateResourceCommand.Parameters.AddWithValue("@CurrentResourceName", DataGridView_uResources.CurrentRow.Cells["Name"].Value.ToString());

                    int rowsAffected = updateResourceCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Resource Updated Successfully!", "Update Resource", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadResourceData(); // Refresh the DataGridView with updated data
                        viewResourcesData_inDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update the resource.", "Update Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
                        else
            {
                Label_information_updateResources.Visible = true;
                Picture_information_updateResources.Visible = true;
                UpdateTextBoxBorders_uResources();
            }
        }
        private void TextBox_uResourceType_TextChanged(object sender, EventArgs e)
        {
            Label_information_updateResources.Visible = false;
            Picture_information_updateResources.Visible = false;
            TextBox_uResourceType.BorderColor = Color.Silver;
        }
        private void TextBox_uResourceCost_TextChanged(object sender, EventArgs e)
        {
            Label_information_updateResources.Visible = false;
            Picture_information_updateResources.Visible = false;
            TextBox_uResourceCost.BorderColor = Color.Silver;
        }
        private void TextBox_uResourceAmount_TextChanged(object sender, EventArgs e)
        {
            Label_information_updateResources.Visible = false;
            Picture_information_updateResources.Visible = false;
            TextBox_uResourceAmount.BorderColor = Color.Silver;
        }
        private void UpdateTextBoxBorders_uResources()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(TextBox_uResourceType.Text))
                TextBox_uResourceType.BorderColor = Color.Red;
            else
                TextBox_uResourceType.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(TextBox_uResourceCost.Text))
                TextBox_uResourceCost.BorderColor = Color.Red;
            else
                TextBox_uResourceCost.BorderColor = Color.Silver;
            if (string.IsNullOrWhiteSpace(TextBox_uResourceAmount.Text))
                TextBox_uResourceAmount.BorderColor = Color.Red;
            else
                TextBox_uResourceAmount.BorderColor = Color.Silver;


        }
        private bool AreAllFieldsFilled_uResources()
        {
            return !string.IsNullOrWhiteSpace(TextBox_uResourceType.Text)
                && !string.IsNullOrWhiteSpace(TextBox_uResourceCost.Text)
                && !string.IsNullOrWhiteSpace(TextBox_uResourceAmount.Text);
        }


        #endregion

        #region view resources

        private void searchTextBox_ViewResources_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_ViewResources.Text == "Search...")
            {
                searchTextBox_ViewResources.Text = "";
                searchTextBox_ViewResources.ForeColor = Color.Black;
            }
        }
        private void searchTextBox_ViewResources_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_ViewResources.Text))
            {
                searchTextBox_ViewResources.Text = "Search...";
                searchTextBox_ViewResources.ForeColor = Color.Gray;
            }
        }
        private void searchTextBox_ViewResources_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_ViewResources.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all resources.
                viewResourcesData_inDataGridView();
            }
            else
            {
                viewResourcesData_inDataGridView(searchQuery);
            }
        }
        private void viewResourcesData_inDataGridView(string searchQuery = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT * FROM Resource";
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                sql += " WHERE ResourceId LIKE @searchQuery OR Name LIKE @searchQuery OR Cost LIKE @searchQuery OR AvailableAmount LIKE @searchQuery OR AdminId LIKE @searchQuery";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            // Add parameters for the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);


            // Set the new data source
            DataGridView_ViewResources.DataSource = dataTable;
        }

        #endregion

        #endregion

        #region Season

        #region add season
        private void AddSeason_button_Click(object sender, EventArgs e)
        {
            string seasonName = ComboBox_SeasonName.SelectedItem?.ToString();
            DateTime startDate = DateTimePicker_startDate.Value;
            DateTime endDate = DateTimePicker_endDate.Value;
            int adminId = this.CurrentAdminId; // Assuming CurrentAdminId is a property that holds the admin's ID

            // Validation: Check if season name is selected
            if (string.IsNullOrEmpty(seasonName))
            {
                MessageBox.Show("Please select a season name from the combo box.", "Add Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation: Check if start date is the same as end date
            if (startDate.Date == endDate.Date)
            {
                MessageBox.Show("Start date and end date cannot be the same.", "Add Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation: Check if start date is after end date
            if (startDate > endDate)
            {
                MessageBox.Show("Start date cannot be after the end date.", "Add Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation: Check if the duration is at least 3 months
            if ((endDate - startDate).TotalDays < 90)
            {
                MessageBox.Show("The season duration must be at least 3 months.", "Add Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Validation: Check if a season with the same name and start date already exists
                    string checkSeasonQuery = @"
            SELECT COUNT(*)
            FROM Season
            WHERE Name = @Name AND StartDate = @StartDate";

                    SqlCommand checkSeasonCommand = new SqlCommand(checkSeasonQuery, connection);
                    checkSeasonCommand.Parameters.AddWithValue("@Name", seasonName);
                    checkSeasonCommand.Parameters.AddWithValue("@StartDate", startDate);
                    int existingSeasonCount = (int)checkSeasonCommand.ExecuteScalar();

                    if (existingSeasonCount > 0)
                    {
                        MessageBox.Show("A season with the same name and start date already exists.", "Add Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert the new season into the database
                    string insertSeasonQuery = @"
            INSERT INTO Season (Name, StartDate, EndDate, AdminId)
            VALUES (@Name, @StartDate, @EndDate, @AdminId)";

                    SqlCommand insertSeasonCommand = new SqlCommand(insertSeasonQuery, connection);
                    insertSeasonCommand.Parameters.AddWithValue("@Name", seasonName);
                    insertSeasonCommand.Parameters.AddWithValue("@StartDate", startDate);
                    insertSeasonCommand.Parameters.AddWithValue("@EndDate", endDate);
                    insertSeasonCommand.Parameters.AddWithValue("@AdminId", adminId);

                    int rowsAffected = insertSeasonCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Season added successfully.", "Add Season", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ComboBox_SeasonName.SelectedItem = null;
                        DateTimePicker_startDate.Value = DateTime.Today;
                        DateTimePicker_endDate.Value = DateTime.Today.AddDays(90);
                        LoadSeasons();
                        LoadSeasonsIntoDataGridView();
                        viewSeasonData_inDataGridView();
                        // Reload the data grid view or other necessary components
                    }
                    else
                    {
                        MessageBox.Show("Failed to add the season.", "Add Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadSeasonNamesIntoComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT DISTINCT Name FROM Season";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    ComboBox_SeasonName.Items.Clear();

                    while (reader.Read())
                    {
                        ComboBox_SeasonName.Items.Add(reader["Name"].ToString());
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading season names: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region update season

        private void LoadSeasonsIntoDataGridView()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SeasonId, Name, StartDate, EndDate FROM Season";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                DataGridView_updateSeason.DataSource = dataTable;
            }
        }
        private int selectedSeasonId;
        private void DataGridView_updateSeason_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridView_updateSeason.Rows[e.RowIndex];
                selectedSeasonId = Convert.ToInt32(row.Cells["SeasonId"].Value); // Store the SeasonId
                text_SeasonName_update.Text = row.Cells["Name"].Value.ToString();
                DateTimePicker_startDate_update.Value = Convert.ToDateTime(row.Cells["StartDate"].Value);
                DateTimePicker_endDate_update.Value = Convert.ToDateTime(row.Cells["EndDate"].Value);
            }
        }
        private void UpdateSeason_button_Click(object sender, EventArgs e)
        {
            string seasonName = text_SeasonName_update.Text;
            DateTime startDate = DateTimePicker_startDate_update.Value;
            DateTime endDate = DateTimePicker_endDate_update.Value;
            int adminId = this.CurrentAdminId; // Assuming CurrentAdminId is a property that holds the admin's ID

            // Validation: Check if a season is selected
            if (selectedSeasonId == 0)
            {
                MessageBox.Show("Please select a season from the data grid view.", "Update Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation: Check if start date is the same as end date
            if (startDate.Date == endDate.Date)
            {
                MessageBox.Show("Start date and end date cannot be the same.", "Update Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation: Check if start date is after end date
            if (startDate > endDate)
            {
                MessageBox.Show("Start date cannot be after the end date.", "Update Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation: Check if the duration is at least 3 months
            if ((endDate - startDate).TotalDays < 90)
            {
                MessageBox.Show("The season duration must be at least 3 months.", "Update Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Validation: Check if a season with the same start date already exists
                    string checkStartDateQuery = @"
            SELECT COUNT(*)
            FROM Season
            WHERE StartDate = @StartDate AND SeasonId != @SeasonId";

                    SqlCommand checkStartDateCommand = new SqlCommand(checkStartDateQuery, connection);
                    checkStartDateCommand.Parameters.AddWithValue("@StartDate", startDate);
                    checkStartDateCommand.Parameters.AddWithValue("@SeasonId", selectedSeasonId);
                    int existingStartDateCount = (int)checkStartDateCommand.ExecuteScalar();

                    if (existingStartDateCount > 0)
                    {
                        MessageBox.Show("A season with the same start date already exists.", "Update Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Update the season in the database
                    string updateSeasonQuery = @"
            UPDATE Season
            SET StartDate = @StartDate, EndDate = @EndDate, AdminId = @AdminId
            WHERE SeasonId = @SeasonId";

                    SqlCommand updateSeasonCommand = new SqlCommand(updateSeasonQuery, connection);
                    updateSeasonCommand.Parameters.AddWithValue("@StartDate", startDate);
                    updateSeasonCommand.Parameters.AddWithValue("@EndDate", endDate);
                    updateSeasonCommand.Parameters.AddWithValue("@AdminId", adminId);
                    updateSeasonCommand.Parameters.AddWithValue("@SeasonId", selectedSeasonId);

                    int rowsAffected = updateSeasonCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Season updated successfully.", "Update Season", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSeasonsIntoDataGridView();
                        LoadSeasons();
                        viewSeasonData_inDataGridView();
                        selectedSeasonId = 0; // Reset the selectedSeasonId
                    }
                    else
                    {
                        MessageBox.Show("Failed to update the season.", "Update Season", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region view season

        private void searchTextBox_view_seasonEnter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_view_season.Text == "Search...")
            {
                searchTextBox_view_season.Text = "";
                searchTextBox_view_season.ForeColor = Color.Black;
            }
        }
        private void searchTextBox_view_season_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_view_season.Text))
            {
                searchTextBox_view_season.Text = "Search...";
                searchTextBox_view_season.ForeColor = Color.Gray;
            }
        }
        private void searchTextBox_view_season_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_view_season.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                viewSeasonData_inDataGridView();
            }
            else
            {
                viewSeasonData_inDataGridView(searchQuery);
            }
        }
        private void viewSeasonData_inDataGridView(string searchQuery = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT * FROM Season";
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                sql += " WHERE SeasonId LIKE @searchQuery OR Name LIKE @searchQuery ";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            // Add parameters for the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            // Set the new data source
            DataGridView_ViewSeason.DataSource = dataTable;
        }

        #endregion

        #endregion

        #region Budget

        #region add budget

        private void LoadResourceTypes()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ResourceId, Name FROM Resource";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                ComboBox_resourceType.DisplayMember = "Name";
                ComboBox_resourceType.ValueMember = "ResourceId";
                ComboBox_resourceType.DataSource = dataTable;
            }
        }
        private void ComboBox_season_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox_season_name.SelectedItem != null)
            {
                int seasonId = (int)ComboBox_season_name.SelectedValue;
                PopulateSeasonDates(seasonId);
            }
        }
        private void PopulateSeasonDates(int seasonId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT StartDate, EndDate FROM Season WHERE SeasonId = @SeasonId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SeasonId", seasonId);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        DateTime startDate = reader.GetDateTime(0);
                        DateTime endDate = reader.GetDateTime(1);

                        DateTimePicker_season_startDate_budget.Value = startDate;
                        DateTimePicker_season_endDate_budget.Value = endDate;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving season dates: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadSeasons()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SeasonId, Name FROM Season";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                ComboBox_season_name.DisplayMember = "Name";
                ComboBox_season_name.ValueMember = "SeasonId";
                ComboBox_season_name.DataSource = dataTable;
            }
        }
        private void AddBudget_button_Click(object sender, EventArgs e)
        {
            if (ComboBox_resourceType.SelectedItem == null)
            {
                MessageBox.Show("Please select a resource type.", "Add Budget", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ComboBox_season_name.SelectedItem == null)
            {
                MessageBox.Show("Please select a season.", "Add Budget", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(text_resource_budget.Text) || !int.TryParse(text_resource_budget.Text, out int resourceBudget) || resourceBudget <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer for resource budget.", "Add Budget", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int resourceId = (int)ComboBox_resourceType.SelectedValue;
            int seasonId = (int)ComboBox_season_name.SelectedValue;
            int adminId = this.CurrentAdminId; // Assuming CurrentAdminId is a property that holds the admin's ID

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Validation: Check if the same resource type and season already have a budget
                    string checkBudgetQuery = @"
            SELECT COUNT(*)
            FROM Budget
            WHERE ResourceId = @ResourceId AND SeasonId = @SeasonId";

                    SqlCommand checkBudgetCommand = new SqlCommand(checkBudgetQuery, connection);
                    checkBudgetCommand.Parameters.AddWithValue("@ResourceId", resourceId);
                    checkBudgetCommand.Parameters.AddWithValue("@SeasonId", seasonId);
                    int existingBudgetCount = (int)checkBudgetCommand.ExecuteScalar();

                    if (existingBudgetCount > 0)
                    {
                        MessageBox.Show("A budget for the selected resource type and season already exists.", "Add Budget", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert the new budget into the database
                    string insertBudgetQuery = @"
            INSERT INTO Budget (ResourceBudget, ResourceId, SeasonId, AdminId)
            VALUES (@ResourceBudget, @ResourceId, @SeasonId, @AdminId)";

                    SqlCommand insertBudgetCommand = new SqlCommand(insertBudgetQuery, connection);
                    insertBudgetCommand.Parameters.AddWithValue("@ResourceBudget", resourceBudget);
                    insertBudgetCommand.Parameters.AddWithValue("@ResourceId", resourceId);
                    insertBudgetCommand.Parameters.AddWithValue("@SeasonId", seasonId);
                    insertBudgetCommand.Parameters.AddWithValue("@AdminId", adminId);

                    int rowsAffected = insertBudgetCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Budget added successfully.", "Add Budget", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ComboBox_resourceType.SelectedItem = null;
                        ComboBox_season_name.SelectedItem = null;
                        text_resource_budget.Clear();
                        PopulateResourceBudgetData();
                        viewBudgetData_inDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add the budget.", "Add Budget", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region update budget

        private void PopulateResourceBudgetData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT b.BudgetId, r.Name AS ResourceName, s.Name AS SeasonName, b.ResourceBudget
                FROM Budget b
                INNER JOIN Resource r ON b.ResourceId = r.ResourceId
                INNER JOIN Season s ON b.SeasonId = s.SeasonId";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    DataGridView_ResourceBudget_update.DataSource = dataTable;

                    // Set columns for read-only display
                    DataGridView_ResourceBudget_update.Columns["BudgetId"].ReadOnly = true;
                    DataGridView_ResourceBudget_update.Columns["ResourceName"].ReadOnly = true;
                    DataGridView_ResourceBudget_update.Columns["SeasonName"].ReadOnly = true;
                    DataGridView_ResourceBudget_update.Columns["ResourceBudget"].ReadOnly = true;

                    // Optionally hide BudgetId column if it's not needed for display
                    DataGridView_ResourceBudget_update.Columns["BudgetId"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while populating resource budget data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DataGridView_ResourceBudget_update_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGridView_ResourceBudget_update.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = DataGridView_ResourceBudget_update.SelectedRows[0];

                // Populate read-only fields
                text_ResourceType_update.Text = selectedRow.Cells["ResourceName"].Value.ToString();
                text_Season_update.Text = selectedRow.Cells["SeasonName"].Value.ToString();
                text_ResourceBudget_update.Text = selectedRow.Cells["ResourceBudget"].Value.ToString();

                // Retrieve and display season start and end dates
                int budgetId = Convert.ToInt32(selectedRow.Cells["BudgetId"].Value);
                PopulateSeasonDatesForBudget(budgetId);
            }
        }
        private void PopulateSeasonDatesForBudget(int budgetId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT s.StartDate, s.EndDate
                FROM Season s
                INNER JOIN Budget b ON s.SeasonId = b.SeasonId
                WHERE b.BudgetId = @BudgetId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@BudgetId", budgetId);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        DateTime startDate = reader.GetDateTime(0);
                        DateTime endDate = reader.GetDateTime(1);

                        DateTimePicker_StartDate_updateBudget.Value = startDate;
                        DateTimePicker_EndDate_updateBudget.Value = endDate;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving season dates: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateBudget_button_Click(object sender, EventArgs e)
        {
            if (DataGridView_ResourceBudget_update.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a resource budget from the grid to update.", "Update Budget", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int budgetId = Convert.ToInt32(DataGridView_ResourceBudget_update.SelectedRows[0].Cells["BudgetId"].Value);
            int newResourceBudget;

            if (!int.TryParse(text_ResourceBudget_update.Text, out newResourceBudget) || newResourceBudget <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer for the resource budget.", "Update Budget", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = @"
                UPDATE Budget
                SET ResourceBudget = @ResourceBudget , AdminId = @AdminId
                WHERE BudgetId = @BudgetId";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@ResourceBudget", newResourceBudget);
                    updateCommand.Parameters.AddWithValue("@AdminId", CurrentAdminId); // Set AdminId parameter
                    updateCommand.Parameters.AddWithValue("@BudgetId", budgetId);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Resource budget updated successfully.", "Update Budget", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Refresh data in DataGridView
                        PopulateResourceBudgetData();
                        viewBudgetData_inDataGridView();
                        // Clear/update any necessary UI fields
                        text_ResourceType_update.Text = "";
                        text_Season_update.Text = "";
                        text_ResourceBudget_update.Text = "";
                        DateTimePicker_StartDate_updateBudget.Value = DateTime.Now;
                        DateTimePicker_EndDate_updateBudget.Value = DateTime.Now;
                    }
                    else
                    {
                        MessageBox.Show("Failed to update resource budget.", "Update Budget", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region view budget

        private void searchTextBox_view_budget_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_view_budget.Text == "Search...")
            {
                searchTextBox_view_budget.Text = "";
                searchTextBox_view_budget.ForeColor = Color.Black;
            }
        }
        private void searchTextBox_view_budget_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_view_budget.Text))
            {
                searchTextBox_view_budget.Text = "Search...";
                searchTextBox_view_budget.ForeColor = Color.Gray;
            }
        }
        private void searchTextBox_view_budget_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_view_budget.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                viewBudgetData_inDataGridView();
            }
            else
            {
                viewBudgetData_inDataGridView(searchQuery);
            }
        }
        private void viewBudgetData_inDataGridView(string searchQuery = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = @"SELECT 
        b.BudgetId,
        r.Name AS Resource,
        s.Name AS Season,
        s.StartDate,
        s.EndDate,
        b.ResourceBudget
    FROM 
        Budget b
    JOIN 
        Resource r ON b.ResourceId = r.ResourceId
    JOIN 
        Season s ON b.SeasonId = s.SeasonId";

            // Check if there's a search query to append a WHERE clause
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                sql += " WHERE b.BudgetId LIKE @searchQuery OR b.ResourceBudget LIKE @searchQuery OR s.Name LIKE @searchQuery OR r.Name LIKE @searchQuery";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            // Add parameters for the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
            }

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            // Set the new data source
            DataGridView_viewBudget.DataSource = dataTable;

            connection.Close(); // Ensure to close the connection when done
        }

        #endregion

        #endregion

        #region Role

        #region add role

        #region method to load role in the datagridview

        private void LoadRole()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT RoleId, RoleName FROM Role";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    DataGridView_addRole.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        #endregion

        #region methods for add new brand
        private bool IsRoleExists(string role_name)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True");
            cn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ROLE WHERE RoleName = @role_name", cn);
            cmd.Parameters.AddWithValue("@role_name", role_name);
            int count = (int)cmd.ExecuteScalar();
            cn.Close();
            return count > 0;
        }
        private void UpdateTextBoxBorders_role()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(text_role_name.Text))
                text_role_name.BorderColor = Color.Red;
            else
                text_role_name.BorderColor = Color.Silver;
        }
        private bool AreAllFieldsFilled_role()
        {
            return !string.IsNullOrWhiteSpace(text_role_name.Text);
        }
        #endregion

        #region textBoxes and pictures of add new brand panel
        private void AddRole_button_Click(object sender, EventArgs e)
        {
            string roleName = text_role_name.Text;
            if (AreAllFieldsFilled_role())
            {
                if (IsRoleExists(roleName))
                {
                    MessageBox.Show("Role already exists. Please Enter a different role.", "Role Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection cn = new SqlConnection(@"Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True");

                cn.Open();
                // Create the SQL query
                SqlCommand MyCommand = new SqlCommand("insert into Role (RoleName) values('" + text_role_name.Text.ToString() + "')", cn);
                MyCommand.ExecuteNonQuery();
                MessageBox.Show("Role Added Successfully!", "Add Role", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRole();
                LoadRole_update();
                PopulateComboBox_Role();
                uPopulateComboBox_Role();
                viewRoleData_inDataGridView();
                text_role_name.Text = null;
            }
            else
            {
                Label_information_role.Visible = true;
                Picture_information_role.Visible = true;
                UpdateTextBoxBorders_role();
            }
        }
        private void text_role_name_TextChanged(object sender, EventArgs e)
        {
            Label_information_role.Visible = false;
            Picture_information_role.Visible = false;
            text_role_name.BorderColor = Color.Silver;
        }

        #endregion

        #endregion

        #region update role

        #region method to load brand in the datagridview
        private void LoadRole_update()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT RoleId, RoleName FROM Role";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Assuming you have a DataGridView named "dataGridViewProducts"
                    DataGridView_updateRole.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        #endregion

        #region methods for update brand
        int rindex = -1;
        private bool IsRoleExists_update(string role_name_upd)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True");
            cn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Role WHERE RoleName = @role_name_upd", cn);
            cmd.Parameters.AddWithValue("@role_name_upd", role_name_upd);
            int count = (int)cmd.ExecuteScalar();
            cn.Close();
            return count > 0;
        }
        private void UpdateTextBoxBorders_role_update()
        {
            // Set the border color to red for empty textboxes
            if (string.IsNullOrWhiteSpace(text_RoleName_update.Text))
                text_RoleName_update.BorderColor = Color.Red;
            else
                text_RoleName_update.BorderColor = Color.Silver;
        }
        private bool AreAllFieldsFilled_role_update()
        {
            return !string.IsNullOrWhiteSpace(text_RoleName_update.Text);
        }
        private void DataGridView_updateRole_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                rindex = e.RowIndex;
                DataGridViewRow row = DataGridView_updateRole.Rows[rindex];
                text_RoleName_update.Text = row.Cells[1].Value.ToString();
                TextBox_RoleID.Text = row.Cells[0].Value.ToString();
                TextBox_RoleID.ReadOnly = true;
            }

        }

        #endregion

        #region textBoxes and pictures of update brand panel
        private void UpdateRole_button_Click(object sender, EventArgs e)
        {
            if (rindex == -1)
            {
                MessageBox.Show("Please select a role to update.", "Update Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string roleName = text_RoleName_update.Text;
            int roleId = Convert.ToInt32(DataGridView_updateRole.Rows[rindex].Cells[0].Value);
            if (AreAllFieldsFilled_role_update())
            {
                if (IsRoleExists_update(roleName))
                {
                    MessageBox.Show("Role already exists. Please Enter a different role.", "Role Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SqlConnection cn = new SqlConnection(@"Data Source=USER;Initial Catalog=GPTourism;Integrated Security=True");

                cn.Open();
                // Create the SQL query
                string query = "UPDATE Role SET RoleName = @roleName WHERE RoleId = @roleID";
                SqlCommand orderStatusCmd = new SqlCommand(query, cn);
                orderStatusCmd.Parameters.AddWithValue("@roleName", roleName);
                orderStatusCmd.Parameters.AddWithValue("@roleID", roleId);
                int orderStatusRowsAffected = orderStatusCmd.ExecuteNonQuery();
                MessageBox.Show("Role Updated Successfully!", "Update Role", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rindex = -1;
                text_RoleName_update.Text = null;
                TextBox_RoleID.Text = null;
                LoadRole_update();
                LoadRole();
                PopulateComboBox_Role();
                uPopulateComboBox_Role();
                viewRoleData_inDataGridView();
            }
            else
            {
                Label_information_role_update.Visible = true;
                Picture_information_role_update.Visible = true;
                UpdateTextBoxBorders_role_update();
            }
        }
        private void text_role_name_update_TextChanged(object sender, EventArgs e)
        {
            Label_information_role_update.Visible = false;
            Picture_information_role_update.Visible = false;
            text_RoleName_update.BorderColor = Color.Silver;
        }

        #endregion

        #endregion

        #region view role

        private void SearchTextBox_view_role_Enter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (searchTextBox_view_role.Text == "Search...")
            {
                searchTextBox_view_role.Text = "";
                searchTextBox_view_role.ForeColor = Color.Black;
            }
        }
        private void SearchTextBox_view_role_Leave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(searchTextBox_view_role.Text))
            {
                searchTextBox_view_role.Text = "Search...";
                searchTextBox_view_role.ForeColor = Color.Gray;
            }
        }
        private void SearchTextBox_view_role_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = searchTextBox_view_role.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                viewRoleData_inDataGridView();
            }
            else
            {
                viewRoleData_inDataGridView(searchQuery);
            }
        }
        private void viewRoleData_inDataGridView(string searchQuery = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT * FROM Role";
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                sql += " WHERE RoleId LIKE @searchQuery OR RoleName LIKE @searchQuery";
            }

            SqlCommand cmd = new SqlCommand(sql, connection);

            // Add parameters for the search query
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            // Set the new data source
            DataGridView_viewRole.DataSource = dataTable;
        }

        #endregion

        #endregion

        #region View Packages Information

        private void SearchTextBox_infoPackagesEnter(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (SearchTextBox_infoPackages.Text == "Search...")
            {
                SearchTextBox_infoPackages.Text = "";
                SearchTextBox_infoPackages.ForeColor = Color.Black;
            }
        }
        private void SearchTextBox_infoPackagesLeave(object sender, EventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(SearchTextBox_infoPackages.Text))
            {
                SearchTextBox_infoPackages.Text = "Search...";
                SearchTextBox_infoPackages.ForeColor = Color.Gray;
            }
        }
        private void SearchTextBox_infoPackages_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = SearchTextBox_infoPackages.Text.Trim(); // Remove leading and trailing spaces
            if (searchQuery == "Search...")
            {
                // If the search query is empty or contains only the placeholder text, show all admins.
                viewinfoOfPackages_DataGridView();
            }
            else
            {
                viewinfoOfPackages_DataGridView(searchQuery);
            }
        }
        private void viewinfoOfPackages_DataGridView(string searchQuery = "")
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
            SELECT 
    ps.PackageSeasonId,
    tp.Name AS Package,
    tp.Price,
    tp.Cost,
    tp.Dmin AS Dmin, 
    tp.Dmax AS Dmax, 
    ps.Demand AS ActualDemand,
    r.Name AS Resources,
    ra.AllocationAmount AS Amount,
    s.Name AS Season,
    s.StartDate AS 'S.Date',
    s.EndDate AS 'E.Date'
FROM 
    TourPackage tp
INNER JOIN 
    PackageSeason ps ON tp.PackageId = ps.PackageId
INNER JOIN 
    ResourceAllocation ra ON ps.PackageSeasonId = ra.PackageSeasonId
INNER JOIN 
    Resource r ON ra.ResourceId = r.ResourceId
INNER JOIN 
    Season s ON ps.SeasonId = s.SeasonId
        ";

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    sql += " WHERE tp.Name LIKE @searchQuery OR tp.Price LIKE @searchQuery";
                }

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    // Add parameters for the search query
                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
                    }

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Set the new data source
                    infoOfPackages_DataGridView.DataSource = dataTable;
                }
            }
        }

        #endregion

        #region show and hide submenus methods
        private void customizeDesign()
        {
            Panel_admin.Visible = false;
            Panel_Packages.Visible = false;
            Panel_PackageSeason.Visible = false;
            Panel_Resources.Visible = false;
            Panel_season.Visible = false;
            Panel_budget.Visible = false;
            Panel_Role.Visible = false;
        }

        private void hideSubMenu()
        {
            if (Panel_admin.Visible == true)
                Panel_admin.Visible = false;
            if (Panel_Packages.Visible == true)
                Panel_Packages.Visible = false;
            if (Panel_PackageSeason.Visible == true)
                Panel_PackageSeason.Visible = false;
            if (Panel_Resources.Visible == true)
                Panel_Resources.Visible = false;
            if (Panel_season.Visible == true)
                Panel_season.Visible = false;
            if (Panel_budget.Visible == true)
                Panel_budget.Visible = false;
            if (Panel_Role.Visible == true)
                Panel_Role.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;

        }
        #endregion

        #region sidemenu panels buttons

        #region main buttons
        private void MainButton_admin_Click(object sender, EventArgs e)
        {
            showSubMenu(Panel_admin);
        }
        private void MainButton_TourPackage_Click(object sender, EventArgs e)
        {
            showSubMenu(Panel_Packages);
        }
        private void MainButton_Resources_Click(object sender, EventArgs e)
        {
            showSubMenu(Panel_Resources);
        }
        private void MainButton_packageSeason_Click(object sender, EventArgs e)
        {
            showSubMenu(Panel_PackageSeason);
        }
        private void MainButton_season_Click(object sender, EventArgs e)
        {
            showSubMenu(Panel_season);
        }
        private void MainButton_budget_Click(object sender, EventArgs e)
        {
            showSubMenu(Panel_budget);
        }
        private void MainButton_Role_Click(object sender, EventArgs e)
        {
            showSubMenu(Panel_Role);
        }
        private void Info_of_Packages_Button_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = true;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        #endregion

        #region submain admin buttons

        private void subMainButton_addNewAdmin_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = true;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        private void subMainButton_updateAdmin_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = true;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        private void subMainButton_viewAdmin_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = true;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }



        #endregion

        #region submain package buttons

        private void subMainButton_addNewPackage_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = true;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        private void subMainButton_updatePackage_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = true;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        private void subMainButton_viewPackage_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = true;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        #endregion

        #region submain package season buttons
        private void subMainButton_addPackageSeason_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = true;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }
        private void subMainButton_updatePackageSeason_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = true;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }
        private void subMainButton_viewPackageSeason_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = true;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        #endregion

        #region submain Resources buttons

        private void subMainButton_allocateResource_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = true;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        private void subMainButton_updateallocation_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = true;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        private void subMainButton_viewAllocated_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = true;
            hideSubMenu();
        }

        private void subMainButton_addResource_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = true;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        private void subMainButton_updateResource_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = true;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        private void subMainButton_viewResource_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = true;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }


        #endregion

        #region submain Season buttons
        private void subMainButton_addSeason_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = true;
            DateTimePicker_startDate.Value = DateTime.Today;
            DateTimePicker_endDate.Value = DateTime.Today.AddDays(90);
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            viewRole_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }
        private void subMainButton_updateSeason_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = true;
            DateTimePicker_startDate_update.Value = DateTime.Today;
            DateTimePicker_endDate_update.Value = DateTime.Today.AddDays(90);
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }
        private void subMainButton_viewSeason_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewRole_panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = true;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }


        #endregion

        #region submain Budget buttons
        private void subMainButton_addBudget_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = true;
            DateTimePicker_season_startDate_budget.Enabled = false;
            DateTimePicker_season_endDate_budget.Enabled = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            viewRole_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }
        private void subMainButton_updateBudget_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = true;
            DateTimePicker_StartDate_updateBudget.Enabled = false;
            DateTimePicker_EndDate_updateBudget.Enabled = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            viewRole_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }
        private void subMainButton_viewBudget_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = true;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            viewRole_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }



        #endregion

        #region submain Role buttons

        private void subMainButton_addRole_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = true;
            update_role_panel.Visible = false;
            viewRole_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }
        private void subMainButton_updateRole_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = true;
            viewRole_panel.Visible = false;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }
        private void subMainButton_viewRole_Click(object sender, EventArgs e)
        {
            info_of_packages_Panel.Visible = false;
            viewResources_Panel.Visible = false;
            add_season_panel.Visible = false;
            update_season_panel.Visible = false;
            view_season_panel.Visible = false;
            add_budget_panel.Visible = false;
            update_budget_panel.Visible = false;
            view_budget_panel.Visible = false;
            add_role_panel.Visible = false;
            update_role_panel.Visible = false;
            viewRole_panel.Visible = true;
            UpdateResource_Panel.Visible = false;
            add_admin_panel.Visible = false;
            AddResource_Panel.Visible = false;
            viewPackageSeason_Panel.Visible = false;
            PackageSeason_Panel.Visible = false;
            update_admin_panel.Visible = false;
            UpdatePackageSeason_Panel.Visible = false;
            view_admin_panel.Visible = false;
            update_packages_panel.Visible = false;
            view_package_panel.Visible = false;
            add_package_panel.Visible = false;
            allocateResource_panel.Visible = false;
            update_allocationResource_panel.Visible = false;
            view_allocatedResource_panel.Visible = false;
            hideSubMenu();
        }

        #endregion

        #region exit and logout buttons
        private void Label_exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you want to exit application ?", "Exit Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void Lable_logout_Click(object sender, EventArgs e)
        {
            LoginForm logAdmin = new LoginForm();
            logAdmin.Show();
            this.Hide();
        }
        private void Label_logout_MouseEnter(object sender, EventArgs e)
        {
            Label_logout.ForeColor = Color.Gold;
        }
        private void Label_logout_MouseLeave(object sender, EventArgs e)
        {
            Label_logout.ForeColor = Color.White;
        }
        private void Label_exit_MouseEnter(object sender, EventArgs e)
        {
            Label_exit.ForeColor = Color.Red;
        }
        private void Label_exit_MouseLeave(object sender, EventArgs e)
        {
            Label_exit.ForeColor = Color.White;
        }
        #endregion

        #endregion
    }
}
