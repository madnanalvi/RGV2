using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileIOMonitor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Column
    {
        public string Name { get; }
        public Type DataType { get; }

        public Column(string name, Type dataType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DataType = dataType ?? throw new ArgumentNullException(nameof(dataType));
        }
    }

    public class MyDataTable
    {
        private string tableName;
        private List<Column> columns;
        private List<List<object>> rows;

        public MyDataTable(string tableName)
        {
            this.tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            columns = new List<Column>();
            rows = new List<List<object>>();
        }

        public void AddColumn(string columnName, Type dataType)
        {
            columns.Add(new Column(columnName, dataType));
        }

        public int GetRowCount()
        {
            return rows.Count;
        }

        public void DeleteRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= rows.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid row index.");
            }

            rows.RemoveAt(rowIndex);
        }
        public object GetValueInRow(List<object> row, string columnName)
        {
            int columnIndex = columns.FindIndex(c => c.Name == columnName);

            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column with name '{columnName}' not found.");
            }

            return row[columnIndex];
        }
        public List<List<object>> GetAllRows()
        {
            return rows.ToList(); // Return a copy of the list to prevent external modifications
        }

        public List<int> GetAllRowIndices()
        {
            return Enumerable.Range(0, rows.Count).ToList();
        }

        public void AddRow(params object[] values)
        {
            if (values.Length != columns.Count)
            {
                throw new ArgumentException("Number of values must match the number of columns.");
            }

            var row = new List<object>(values);
            rows.Add(row);
        }

        public List<object> GetRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= rows.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid row index.");
            }

            return rows[rowIndex];
        }

        public void UpdateRow(int rowIndex, params object[] newValues)
        {
            if (rowIndex < 0 || rowIndex >= rows.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid row index.");
            }

            if (newValues.Length != columns.Count)
            {
                throw new ArgumentException("Number of values must match the number of columns.");
            }

            for (int i = 0; i < columns.Count; i++)
            {
                rows[rowIndex][i] = newValues[i];
            }
        }

        public void UpdateColumn(int columnIndex, string newColumnName, Type newDataType, IEnumerable<object> newValues)
        {
            if (columnIndex < 0 || columnIndex >= columns.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid column index.");
            }

            if (newValues.Count() != rows.Count)
            {
                throw new ArgumentException("Number of values must match the number of rows.");
            }

            columns[columnIndex] = new Column(newColumnName, newDataType);

            for (int i = 0; i < rows.Count; i++)
            {
                rows[i][columnIndex] = newValues.ElementAt(i);
            }
        }

        public void UpdateValueByColumnName(string columnName, object newValue)
        {
            int columnIndex = columns.FindIndex(c => c.Name == columnName);

            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column with name '{columnName}' not found.");
            }

            for (int i = 0; i < rows.Count; i++)
            {
                rows[i][columnIndex] = newValue;
            }
        }

        public MyDataTable Clone()
        {
            var clonedTable = new MyDataTable(tableName);

            foreach (var column in columns)
            {
                clonedTable.AddColumn(column.Name, column.DataType);
            }

            return clonedTable;
        }

        public void ExportToCsv(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write table name as a comment
                writer.WriteLine($"# Table: {tableName}");

                // Write column headers with data types
                writer.WriteLine(string.Join(",", columns.Select(c => $"{c.Name} ({c.DataType.Name})")));

                // Write each row
                foreach (var row in rows)
                {
                    writer.WriteLine(string.Join(",", row));
                }
            }

            Console.WriteLine($"Table '{tableName}' exported to CSV file: {filePath}");
        }

        public void Print()
        {
            Console.WriteLine($"Table: {tableName}");
            Console.WriteLine(string.Join("\t", columns.Select(c => $"{c.Name} ({c.DataType.Name})")));

            foreach (var row in rows)
            {
                Console.WriteLine(string.Join("\t", row));
            }
        }

        public List<List<object>> Search(string expression)
        {
            // Case-insensitive search in all columns and rows
            return rows.Where(row => row.Any(value => value.ToString().IndexOf(expression, StringComparison.OrdinalIgnoreCase) >= 0))
                       .ToList();
        }

        public void setValue(int rowIndex, string columnName, object newValue)
        {
            int columnIndex = columns.FindIndex(c => c.Name == columnName);

            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column with name '{columnName}' not found.");
            }

            if (rowIndex < 0 || rowIndex >= rows.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid row index.");
            }

            rows[rowIndex][columnIndex] = newValue;
        }

        public int GetColumnIndex(string columnName)
        {
            return columns.FindIndex(c => c.Name == columnName);
        }

        public object getValue(int rowIndex, string columnName)
        {
            int columnIndex = columns.FindIndex(c => c.Name == columnName);

            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column with name '{columnName}' not found.");
            }

            if (rowIndex < 0 || rowIndex >= rows.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid row index.");
            }

            return rows[rowIndex][columnIndex];
        }

        public List<int> SearchExpressionIndices(string columnName, string value)
        {            
            string operatorSymbol = "=";
            int columnIndex = columns.FindIndex(c => c.Name == columnName);
            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column with name '{columnName}' not found.");
            }

            switch (operatorSymbol)
            {
                case "=":
                    return rows
                        .Select((row, index) => new { Row = row, Index = index })
                        .Where(item => item.Row[columnIndex].Equals(Convert.ChangeType(value, columns[columnIndex].DataType)))
                        .Select(item => item.Index)
                        .ToList();

                // Add more cases for other comparison operators if needed
                default:
                    throw new ArgumentException($"Unsupported operator: {operatorSymbol}");
            }
        }



        public List<List<object>> SearchExpression(string columnName, string value)
        {

            string operatorSymbol = "=";

            int columnIndex = columns.FindIndex(c => c.Name == columnName);

            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column with name '{columnName}' not found.");
            }

            switch (operatorSymbol)
            {
                case "=":
                    return rows.Where(row => row[columnIndex].Equals(Convert.ChangeType(value, columns[columnIndex].DataType)))
                               .ToList();
                // Add more cases for other comparison operators if needed
                default:
                    throw new ArgumentException($"Unsupported operator: {operatorSymbol}");
            }
        }
    }
}
