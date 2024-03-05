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
        private void UpdateRecords() {
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

        //Create object employee and add it to the list of employees.
        public bool AddEmployee(int employeeId, string firstName, string lastName, DateTime dateOfBirth, string phone, double salary) {
            Employee newEmployee = new Employee(employeeId, firstName, lastName, dateOfBirth, phone, salary);

            this.Employees.Add(newEmployee);

            UpdateRecords();

            return true;
        }

        //Create object employee in the list of employees.
        public bool EditEmployee(int employeeId, string firstName, string lastName, DateTime dateOfBirth, string phone, double salary) {
            Employee replaceEmployee = new Employee(employeeId, firstName, lastName, dateOfBirth, phone, salary);

            this.Employees.Remove(SearchById(employeeId));

            this.Employees.Add(replaceEmployee);

            UpdateRecords();

            return true;
        }

        //Override method ToString.
        public override string ToString() {
            string output = "List of Employees:\n";

            foreach (Employee employee in Employees) {
                output += employee.ToString();
            }

            return output;
        }

        //Search employee in the Employees list by id.
        public Employee SearchById(int employeeId) {
            Employee employeeOutput = new Employee();

            foreach (Employee employee in Employees) {
                if (employee.EmployeeId == employeeId) {
                    employeeOutput = employee;
                    break;
                }
            }

            return employeeOutput;
        }

        //Search employee in the Employees list by name.
        public List<Employee> SearchByName(string name) {
            List<Employee> employeesOutput = new List<Employee>();

            foreach (Employee employee in Employees) {
                if (employee.FirstName == name || employee.LastName == name) {
                    employeesOutput.Add(employee);
                }
            }

            return employeesOutput;
        }

        //Delete employee in the Employees list by id.
        public bool Delete(int employeeId) {
            Employees.Remove(SearchById(employeeId));

            UpdateRecords();

            return true;
        }

        //Delete all employess in the Employees list
        public bool DeleteAll() {
            Employees.Clear();

            UpdateRecords();

            return true;
        }
    }
}
