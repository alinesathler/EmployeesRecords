using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

//Aline Sathler Delfino,  2024/03/03
//Name of Project: Employees' Record
//Purpose: C# Windows Form for employees' record
//Revision History:
//REV00 - 2024/03/03 - Initial version
//REV01 - 2024/03/04 - Adding text file and methods search by id and view all records 
//REV02 - 2024/03/05 - Methods search by name, delete, delete all and edit
//REV03 - 2024/03/05 - Bugs and comments

namespace Example {
    public partial class Employees : Form {
        public string error = string.Empty;

        //Instanciate employess.
        EmployeeRecords employeesRecords = new EmployeeRecords();

        public Employees() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            //Costumize the format of the date time picker.
            dtpDateOfBirth.Format = DateTimePickerFormat.Custom;
            dtpDateOfBirth.CustomFormat = "MM/dd/yyyy";

            employeesRecords.GetEmployees();

            rtxtOutput.Text = employeesRecords.ToString();

            ChangeToInitialState();
        }

        //Check if inputs are valid.
        private bool ValidateInputs(int employeeId, bool isEmployeeId, string firstName, string lastName, DateTime dateOfBirth, string phone, double salary, bool isSalary) {
            bool isFirstName = false;

            bool isLastName = true;

            bool isDate = true;

            string pattern = @"^\(\d{3}\)\s\d{3}\-\d{4}$";
            Regex regexPhone = new Regex(pattern);
            bool isPhone = true;

            //Validate id.
            ValidateId(employeeId, ref isEmployeeId);

            //Validade first name.
            ValidateName(firstName, ref isFirstName, txtFirstName, "first");

            //Validade last name.
            ValidateName(lastName, ref isLastName, txtLastName, "last");

            //Change backgroud color and display error message for date of birth input.
            if (dateOfBirth > DateTime.Now) {
                dtpDateOfBirth.BackColor = Color.LightPink;
                error += "Please enter a valid date.\n";

                isDate = false;
            }

            //Change backgroud color and display error message for phone input.
            if (!regexPhone.IsMatch(phone)) {
                mtxtPhone.BackColor = Color.LightPink;
                error += "Please enter a valid phone number.\n" ;

                isPhone = false;
            }

            //Change backgroud color and display error message for employee id input.
            if (!isSalary || salary <= 0) {
                txtSalary.BackColor = Color.LightPink;
                error += "Please enter a valid salary.\n";

                isSalary = false;
            }

            //Return true if all inputs are valid, otherwise, false.
            if(isEmployeeId && isFirstName && isLastName && isDate && isPhone && isSalary) {
                return true;
            } else {
                return false;
            }
        }

        //Check if id is valid.
        private void ValidateId(int employeeId, ref bool isEmployeeId) {
            //Change backgroud color and display error message for employee id input.
            if (!isEmployeeId || employeeId <= 0 || employeeId >= 99999) {
                txtId.BackColor = Color.LightPink;
                error += "Please enter a valid id.\n";

                isEmployeeId = false;
            }
        }

        //Check if name is valid.
        private void ValidateName(string name, ref bool isName, TextBox txtName, string label) {
            //Change backgroud color and display error message for employee first/last name input.
            if (string.IsNullOrEmpty(name)) {
                txtName.BackColor = Color.LightPink;
                error += $"Please enter a valid {label} name.\n";
            } else {
                isName = true;
            }
        }

        //Clean error message and background colors for the default.
        private void ChangeToInitialState() {
            //Clear error and status messages.
            lblError.Text = String.Empty;
            lblError.Visible = false;
            error = String.Empty;
            tsslStatus.Text = String.Empty;

            //Reset backgrounds colors.
            txtId.BackColor = SystemColors.Window;
            txtFirstName.BackColor = SystemColors.Window;
            txtLastName.BackColor = SystemColors.Window;
            dtpDateOfBirth.BackColor = SystemColors.Window;
            mtxtPhone.BackColor = SystemColors.Window;
            txtSalary.BackColor = SystemColors.Window;
        }

        //Clear all the inputs.
        private void ClearInputs () {
            txtId.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            dtpDateOfBirth.Text = string.Empty;
            mtxtPhone.Text = string.Empty;
            txtSalary.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            //Read inputs.
            bool isEmployeeId = int.TryParse(txtId.Text, out int employeeId);
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            DateTime dateOfBirth = dtpDateOfBirth.Value.Date;
            string phone = mtxtPhone.Text;
            bool isSalary = double.TryParse(txtSalary.Text, out double salary);

            //Clean errors and status
            ChangeToInitialState();

            //Check if employee id already exists.
            foreach (Employee employee in employeesRecords.Employees) {
                if (employee.EmployeeId == employeeId) {
                    txtId.BackColor = Color.LightPink;
                    error += "Employee Id already exists.\n";

                    isEmployeeId = false;
                    break;
                }
            }

            //Validate and add employee or show errors.
            if (ValidateInputs(employeeId, isEmployeeId, firstName, lastName, dateOfBirth, phone, salary, isSalary) && employeesRecords.AddEmployee(employeeId, firstName, lastName, dateOfBirth, phone, salary)) {
                //Show list of employees
                rtxtOutput.Text = employeesRecords.ToString();

                ClearInputs();

                tsslStatus.Text = "Employee added.";
            } else {
                lblError.Text = error;
                lblError.Visible = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e) {
            //Read inputs.
            bool isEmployeeId = int.TryParse(txtId.Text, out int employeeId);
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            DateTime dateOfBirth = dtpDateOfBirth.Value.Date;
            string phone = mtxtPhone.Text;
            bool isSalary = double.TryParse(txtSalary.Text, out double salary);

            //Clean errors and status
            ChangeToInitialState();

            if (ValidateInputs(employeeId, isEmployeeId, firstName, lastName, dateOfBirth, phone, salary, isSalary)) {
                Employee employee = employeesRecords.SearchById(employeeId);

                if (employee.EmployeeId == employeeId) {
                    //Confirm delete.
                    if (MessageBox.Show("Are you sure you want to update this employee?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                        if (employeesRecords.EditEmployee(employeeId, firstName, lastName, dateOfBirth, phone, salary)) {
                            tsslStatus.Text = "Employee updated.";

                            ClearInputs();
                        }
                    } else {
                        tsslStatus.Text = "Edit canceled.";
                    }

                    rtxtOutput.Text = employeesRecords.ToString();
                } else {
                    tsslStatus.Text = "Employee not found.";
                }
            } else {
                lblError.Text = error;
                lblError.Visible = true;
            }
        }

        private void btnSearchById_Click(object sender, EventArgs e) {
            //Read id.
            bool isEmployeeId = int.TryParse(txtId.Text, out int employeeId);

            //Clean errors and status
            ChangeToInitialState();

            ValidateId(employeeId, ref isEmployeeId);

            if(isEmployeeId) {
                Employee employee = employeesRecords.SearchById(employeeId);

                if (employee.EmployeeId == employeeId) {
                    rtxtOutput.Text = "Search Results:\n";
                    rtxtOutput.Text += employee.ToString();

                    ClearInputs();

                    tsslStatus.Text = "Employee found.";
                } else {
                    rtxtOutput.Text = "Employee not found.";
                }
            } else {
                lblError.Text = error;
                lblError.Visible = true;
            }
        }

        private void btnSearchByName_Click(object sender, EventArgs e) {
            //Read fisrt name.
            string name = txtFirstName.Text;
            bool isName = false;

            List<Employee> employeesSearch = new List<Employee>();

            //Clean errors and status
            ChangeToInitialState();

            ValidateName(name, ref isName, txtFirstName, "first/last");

            if (isName) {
                employeesSearch = employeesRecords.SearchByName(name);

                if (employeesSearch.Count > 0) {

                    rtxtOutput.Text = "Search Results:\n";

                    //Show search result.
                    foreach (Employee employee in employeesSearch) {
                        rtxtOutput.Text += employee.ToString();
                    }

                    ClearInputs();

                    tsslStatus.Text = "Employee(s) founded.";
                } else {
                    rtxtOutput.Text = "Employee not found.";
                }
            } else {
                lblError.Text = error;
                lblError.Visible = true;
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e) {
            ChangeToInitialState();

            //Show list of employees
            rtxtOutput.Text = employeesRecords.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            //Read id.
            bool isEmployeeId = int.TryParse(txtId.Text, out int employeeId);

            //Clean errors and status
            ChangeToInitialState();

            ValidateId(employeeId, ref isEmployeeId);

            if(isEmployeeId) {
                Employee employee = employeesRecords.SearchById(employeeId);

                if (employee.EmployeeId == employeeId) {
                    rtxtOutput.Text = employee.ToString();

                    ClearInputs();

                    //Confirm delete.
                    if (MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                        if(employeesRecords.Delete(employeeId)) {
                            tsslStatus.Text = "Employee deleted.";
                        }
                    } else {
                        tsslStatus.Text = "Delete canceled.";
                    }

                    //Show list of employees
                    rtxtOutput.Text = employeesRecords.ToString();
                } else {
                    tsslStatus.Text = "Employee not found.";
                }
            } else {
                lblError.Text = error;
                lblError.Visible = true;
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e) {
            //Clean errors and status
            ChangeToInitialState();

            //Confirm delete all.
            if (MessageBox.Show("Are you sure you want to delete all records?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                if (employeesRecords.DeleteAll()) {
                    tsslStatus.Text = "All employees deleted.";
                }
            } else {
                tsslStatus.Text = "Delete all records canceled.";
            }

            //Show list of employees
            rtxtOutput.Text = employeesRecords.ToString();
        }
    }
}
