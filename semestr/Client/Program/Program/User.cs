using System;
using System.Data;

namespace Program
{
    abstract class User
    {
        protected DataSet tables;
        protected bool flagConnection;

        public abstract void UpdateTables();

        public DataSet Tables
        {
            get
            {
                return tables;
            }
        }

        public bool Connection
        {
            get
            {
                return flagConnection;
            }
        }

        // #1 - firstTable, #2 - secondTable, #3 - namePrimaryKey, #4 - nameNewColumn, #5... - addInNewColumn
        protected DataTable newColumn(DataTable firstTable, DataTable secondTable, string namePrimaryKey,
                                                                string nameNewColumn, params string[] addInNewColumn)
        {
            int countDataForNewColumn = addInNewColumn.Length;
            if (countDataForNewColumn == 0)
                return new DataTable("<ERROR>");
            DataColumn newColumn = new DataColumn(nameNewColumn);
            newColumn.DataType = secondTable.Columns[addInNewColumn[0]].DataType;
            firstTable.Columns.Add(newColumn);
            for (int i = 0; i < firstTable.Rows.Count; i++)
                for (int j = 0; j < secondTable.Rows.Count; j++)
                    if (firstTable.Rows[i][namePrimaryKey].ToString() == secondTable.Rows[j][namePrimaryKey].ToString())
                    {
                        string buf = "";
                        for (int x = 0; x < countDataForNewColumn; x++)
                        {
                            if (x != 0)
                                buf += " ";
                            buf += secondTable.Rows[j][addInNewColumn[x]];
                        }
                        firstTable.Rows[i][nameNewColumn] = buf;
                        continue;
                    }
            return firstTable;
        }

        // "dd.mm.yyyy" --> "yyyy.mm.dd"
        protected string changeDate(string date)
        {
            string[] bufString = date.Split(new Char[] { '.' });
            if (bufString.Length != 3)
                return null;
            bufString[2] = bufString[2].Substring(0, 4);
            string newDate = bufString[2] + "." + bufString[1] + "." + bufString[0];
            return newDate;
        }

    }
}
