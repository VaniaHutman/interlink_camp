using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();
            List<DateTime> dates = new List<DateTime>();
            reading(employees, dates);
            sorting(employees, dates);
            writing(employees, dates);
        }

        public static void reading(List<Employee> employees, List<DateTime> dates)
        {
            int counter = 0;
            var line = "";
            using (var reader = new StreamReader(@"acme_worksheet.csv"))
            {
                while (!reader.EndOfStream)
                {
                    if (counter == 0)
                    {
                        line = reader.ReadLine();
                        counter++;
                    }
                    else
                    {
                        line = reader.ReadLine();
                        var values = line.Split(',');
                        string n = values[0];
                        DateTime d = Convert.ToDateTime(values[1]);
                        string h = values[2];
                        if (employees.Exists(x => x.name == n))
                        {
                            foreach (Employee aEmp in employees)
                            {
                                if (aEmp.name == n)
                                {
                                    aEmp.date.Add(d);
                                    aEmp.hours.Add(h);
                                }
                            }
                        }
                        else
                        {
                            employees.Add(new Employee(n));
                            foreach (Employee aEmp in employees)
                            {
                                if (aEmp.name == n)
                                {
                                    aEmp.date.Add(d);
                                    aEmp.hours.Add(h);
                                }
                            }
                        }
                        if (dates.Exists(x => x == d))
                        { }
                        else
                        {
                            dates.Add(d);
                        }
                    }
                }
            }
        }

        public static void sorting(List<Employee> employees, List<DateTime> dates)
        {
            employees.Sort(delegate (Employee x, Employee y)
            {
                if (x.name == null && y.name == null) return 0;
                else if (x.name == null) return -1;
                else if (y.name == null) return 1;
                else return x.name.CompareTo(y.name);
            });

            dates.Sort();
        }

        public static void writing(List<Employee> employees, List<DateTime> dates)
        {
            string[,] write_string = new string[employees.Count + 1, dates.Count + 1];
            write_string[0, 0] = "Name / Date";
            int count = 1;
            foreach (Employee employee in employees)
            {
                write_string[count, 0] = employee.name;
                count++;
            }
            for (int i = 0; i < dates.Count; i++)
            {
                write_string[0, i + 1] = dates[i].ToString("yyyy-MM-dd");
            }

            bool exist;

            for (int j = 0; j < employees.Count; j++)
            {
                for (int i = 0; i < dates.Count; i++)
                {
                    exist = false;
                    for (int k = 0; k < employees[j].date.Count; k++)
                    {
                        if (dates[i] == employees[j].date[k])
                        {
                            write_string[j + 1, i + 1] = employees[j].hours[k];
                            exist = true;
                        }
                    }
                    if (exist == false)
                    {
                        write_string[j + 1, i + 1] = "0";
                    }
                }
            }

            var file = @"myOutput.csv";
            using (var stream = File.CreateText(file))
            {
                string csvRow = null;
                for (int i = 0; i < write_string.GetLength(0); i++)
                {
                    for (int j = 0; j < write_string.GetLength(1); j++)
                    {
                        if (j < write_string.GetLength(1) - 1)
                            csvRow += write_string[i, j] + ",";
                        else
                            csvRow += write_string[i, j];
                    }
                    stream.WriteLine(csvRow);
                    csvRow = null;
                }
            }
        }
    }

    class Employee
    {
        public string name;
        public List<DateTime> date = new List<DateTime>();
        public List<string> hours = new List<string>();

        public Employee(string n)
        {
            name = n;
        }

        public void show_info()
        {
            for (int i = 0; i < date.Count; i++)
                Console.WriteLine("{0,10}   {1,10}   {2,5}", name, date[i], hours[i]);
        }
    }
}
