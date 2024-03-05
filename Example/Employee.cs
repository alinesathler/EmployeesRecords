using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Example {
    //Class of object Employee with employee id, first name, last name, date of birth, phone and salary.
    internal class Employee {
        private int _employeeId;

        public int EmployeeId {
            get { return _employeeId; }
            set {
                if (value > 0 && value <= 99999) {
                    _employeeId = value;
                    return;
                } else {
                    throw new ArgumentOutOfRangeException("Please enter a valid id.");
                }
            }
        }

        private string _firstName;

        public string FirstName {
            get { return _firstName; }
            set {
                if (!String.IsNullOrWhiteSpace(value)) {
                    _firstName = value;
                    return;
                } else {
                    throw new ArgumentNullException("Please enter a valid first name.");
                }
            }
        }

        private string _lastName;
        public string LastName {
            get { return _lastName; }
            set {
                if (!String.IsNullOrWhiteSpace(value)) {
                    _lastName = value;
                    return;
                } else {
                    throw new ArgumentNullException("Please enter a valid last name.");
                }
            }
        }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth {
            get { return _dateOfBirth; }
            set {
                if (_dateOfBirth < DateTime.Now) {
                    _dateOfBirth = value;
                    return;
                } else {
                    throw new ArgumentOutOfRangeException("Please enter a valid date.");
                }
            }
        }

        private string _phone;
        public string Phone {
            get { return _phone; }
            set {
                string pattern = @"^\(\d{3}\)\s\d{3}\-\d{4}$";
                Regex regexPhone = new Regex(pattern);
                if (regexPhone.IsMatch(value)) {
                    _phone = value;
                    return;
                } else {
                    throw new FormatException("Please enter a valid phone number.");
                }
            }
        }

        private double _salary;

        public double Salary {
            get { return _salary; }
            set {
                if (value >= 0) {
                    _salary = value;
                    return;
                } else {
                    throw new ArgumentOutOfRangeException("Please enter a valid salary.");
                }
            }
        }

        //Default constructor.
        public Employee() {

        }

        //Override constructor.
        public Employee(int employeeId, string firstName, string lastName, DateTime dateOfBirth, string phone, double salary) {
            this.EmployeeId = employeeId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Phone = phone;
            this.Salary = salary;
        }
    }
}
