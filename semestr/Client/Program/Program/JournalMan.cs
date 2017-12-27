using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Program
{
    class JournalMan : User
    {
        public JournalMan(string name = "JournalMan")
        {
            tables = new DataSet(name);
            UpdateTables();
        }

        public override void UpdateTables()
        {
            flagConnection = true;

            var journalVisit = AsyncClient.SendMessage("select * from s_journal_visit");
            var workman = AsyncClient.SendMessage("select * from s_workman");
            if (journalVisit.TableName == "<ERROR>" || workman.TableName == "<ERROR>")
            {
                flagConnection = false;
                return;
            }
            journalVisit.TableName = "s_journal_visit";
            journalVisit = newColumn(journalVisit, workman, "n_id_workman", "Работник", "c_second_name_workman", "c_first_name_workman", "c_last_name_workman");
            journalVisit.Columns["d_journal_visit"].ColumnName = "Дата";
            journalVisit.Columns.Remove("n_id_workman");

            workman.TableName = "s_workman";
            workman = newColumn(workman, workman, "n_id_workman", "ФИО", "c_second_name_workman", "c_first_name_workman", "c_last_name_workman");
            workman.Columns["n_id_workman"].ColumnName = "Идентификатор";
            workman.Columns.Remove("c_first_name_workman");
            workman.Columns.Remove("c_last_name_workman");
            workman.Columns.Remove("c_second_name_workman");
            workman.Columns.Remove("n_id_chief");
            workman.Columns.Remove("n_id_office");
            workman.Columns.Remove("n_id_post_workman");

            var bufDataSet = new DataSet();
            bufDataSet.Tables.Add(journalVisit);
            bufDataSet.Tables.Add(workman);
            tables = bufDataSet;
        }

        //**************************************************************

        public bool AddJournalEntry(string idWorkman, string date) // date: "dd.mm.yyyy"
        {
            string newDate = changeDate(date);
            if (newDate == null)
                return false;
            var buf = AsyncClient.SendMessage("insert into s_journal_visit (n_id_workman, d_journal_visit) values ('" + idWorkman + "', '" + newDate + "')");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }
    }
}
