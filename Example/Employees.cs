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
//REV01 - 2024/03/05 - Adding text file and functions search by id and view all records  

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
            bool isFirstName = true;

            bool isLastName = true;

            bool isDate = true;

            string pattern = @"^\(\d{3}\)\s\d{3}\-\d{4}$";
            Regex regexPhone = new Regex(pattern);
            bool isPhone = true;

            //Validate id.
            ValidateId(employeeId, ref isEmployeeId);

            //Change backgroud color and display error message for employee first name input.
            if (string.IsNullOrEmpty(firstName)) {
                txtFirstName.BackColor = Color.LightPink;
                error += "Please enter a valid first name.\n";

                isFirstName = false;
            }

            //Change backgroud color and display error message for employee last name input.
            if (string.IsNullOrEmpty(lastName)) {
                txtLastName.BackColor = Color.LightPink;
                error += "Please enter a valid last name.\n";

                isLastName = false;
            }

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
            ChangeToInitialState();

            //Read inputs.
            bool isEmployeeId = int.TryParse(txtId.Text, out int employeeId);
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            DateTime dateOfBirth = dtpDateOfBirth.Value.Date;
            string phone = mtxtPhone.Text;
            bool isSalary = double.TryParse(txtSalary.Text, out double salary);

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

        private void btnSearchById_Click(object sender, EventArgs e) {
            ChangeToInitialState();

            //Read id.
            bool isEmployeeId = int.TryParse(txtId.Text, out int employeeId);

            ValidateId(employeeId, ref isEmployeeId);

            if(isEmployeeId) {
                rtxtOutput.Text = employeesRecords.SearchById(employeeId);

                ClearInputs();

                tsslStatus.Text = "Employee founded.";
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


    }
}
