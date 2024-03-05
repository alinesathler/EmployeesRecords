using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Example {
    //Class with a list of objects Employee with employee id, first name, last name, date of birth, phone and salary.
    internal class EmployeeRecords {
        public List<Employee> Employees { get; private set; }

        private const string Path = @"C:\Demo\EmployeesRecords.txt";

        public EmployeeRecords() {
            this.Employees = new List<Employee>();
        }

        //Create object employee and add it to the list of employees.
        public bool AddEmployee(int employeeId, string firstName, string lastName, DateTime dateOfBirth, string phone, double salary) {
            Employee newEmployee = new Employee(employeeId, firstName, lastName, dateOfBirth, phone, salary);

            this.Employees.Add(newEmployee);

            UpdateRecords();

            return true;
        }

        public override string ToString() {
            string output = "List of Employees:\n";

            foreach (Employee employee in Employees){
                output += $"Id: {employee.EmployeeId}, Name: {employee.FirstName} {employee.LastName}, Date of Birth: {employee.DateOfBirth.ToString("MM/dd/yyyy")}, Phone: {employee.Phone}, Salary: {employee.Salary.ToString("C2")}\n";
            }

            return output;
        }

        //Get records from the textfile to the list.
        public void GetEmployees() {
            using (StreamReader textIn = new StreamReader(new FileStream(Path, FileMode.OpenOrCreate))) {
                while (textIn.Peek() != -1) {
                    string row = textIn.ReadLine();
                    string[] columns = row.Split('|');
                    Employee employee = new Employee();
                    employee.EmployeeId = Convert.ToInt16(columns[0]);
                    employee.FirstName = columns[1];
                    employee.LastName = columns[2];
                    employee.DateOfBirth = Convert.ToDateTime(columns[3]).Date;
                    employee.Phone = columns[4];
                    employee.Salary = Convert.ToDouble(columns[5]);
                    Employees.Add(employee);
                }
            }
        }

        //Update records from the list to the text file.
        private void UpdateRecords () {
            using (StreamWriter textOut = new StreamWriter(new FileStream(Path, FileMode.Create))) {
                foreach (Employee employee in Employees) {
                    textOut.Write(employee.EmployeeId + "|");
                    textOut.Write(employee.FirstName + "|");
                    textOut.Write(employee.LastName + "|");
                    textOut.Write(employee.DateOfBirth + "|");
                    textOut.Write(employee.Phone + "|");
                    textOut.WriteLine(employee.Salary + "|");
                }
            }
        }

        public string SearchById(int employeeId) {
            string output = string.Empty;

            foreach (Employee employee in Employees) {
                if (employee.EmployeeId == employeeId) {
                    output = $"Id: {employee.EmployeeId}, Name: {employee.FirstName} {employee.LastName}, Date of Birth: {employee.DateOfBirth.ToString("MM/dd/yyyy")}, Phone: {employee.Phone}, Salary: {employee.Salary.ToString("C2")}";
                    break;
                }
            }

            if (string.IsNullOrEmpty(output)) {
                output = $"EmployeeId {employeeId} not found.";
            }

            return output ;
        }
    }
}
