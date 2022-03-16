using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVReaderUpwork.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = from line in File.ReadAllLines("inputdata.csv").Skip(1)
                let columns = line.Split(';')
                select new InputModel
                {
                    OrderDate = DateTime.Parse(columns[0]),
                    Region = columns[1],
                    Rep = columns[2],
                    Item = columns[3],
                    Units = int.Parse(columns[4]),
                    UnitCost = decimal.Parse(columns[5]),
                };

            var summary = x.GroupBy(g => g.Region).Select(s => new OutputModel
            {
                Region = s.First().Region,
                LastOrderDate = s.OrderByDescending(d => d.OrderDate).First().OrderDate,
                TotalCost = s.Sum(d => d.TotalCost),
                TotalUnits = s.Sum(d => d.Units)
            });

            var dt = ToDataTable(summary.ToList());
            WriteToCsvFile(dt, "outoutdata.csv");
        }

        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static void WriteToCsvFile(DataTable dataTable, string filePath)
        {
            StringBuilder fileContent = new StringBuilder();

            foreach (var col in dataTable.Columns)
            {
                fileContent.Append(col.ToString() + ",");
            }

            fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);

            foreach (DataRow dr in dataTable.Rows)
            {
                foreach (var column in dr.ItemArray)
                {
                    fileContent.Append("\"" + column.ToString() + "\",");
                }

                fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
            }

            System.IO.File.WriteAllText(filePath, fileContent.ToString());
        }
    }
}
