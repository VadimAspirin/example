using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Program
{
    class HumanResources : User
    {
        public HumanResources(string name = "HumanResources")
        {
            tables = new DataSet(name);
            UpdateTables();
        }

        public override void UpdateTables()
        {
            flagConnection = true;

            var workman = AsyncClient.SendMessage("select n_id_workman, c_second_name_workman, c_first_name_workman, c_last_name_workman, n_id_post_workman, n_id_chief, n_id_office from s_workman");
            var office = AsyncClient.SendMessage("select * from s_office");
            var postWorkman = AsyncClient.SendMessage("select * from s_post_workman");
            var manager = AsyncClient.SendMessage("select n_id_workman as 'n_id_chief', concat_ws('',c_second_name_workman,' ',c_first_name_workman,' ',c_last_name_workman) as name from s_workman where n_id_post_workman=(select n_id_post_workman from s_post_workman where c_name_post_workman='Менеджер')");
            if (workman.TableName == "<ERROR>" || office.TableName == "<ERROR>" || postWorkman.TableName == "<ERROR>" || manager.TableName == "<ERROR>")
            {
                flagConnection = false;
                return;
            }
            workman.TableName = "s_workman";
            workman = newColumn(workman, office, "n_id_office", "Офис", "c_address_office");
            workman.Columns.Remove("n_id_office");
            workman = newColumn(workman, postWorkman, "n_id_post_workman", "Должность", "c_name_post_workman");
            workman.Columns.Remove("n_id_post_workman");
            workman = newColumn(workman, manager, "n_id_chief", "Начальник", "name");
            workman.Columns.Remove("n_id_chief");
            workman.Columns["c_second_name_workman"].ColumnName = "Фамилия";
            workman.Columns["c_first_name_workman"].ColumnName = "Имя";
            workman.Columns["c_last_name_workman"].ColumnName = "Отчество";
            workman.Columns["n_id_workman"].ColumnName = "Идентификатор";

            office.TableName = "s_office";
            postWorkman.TableName = "s_post_workman";
            manager.TableName = "manager";

            var bufDataSet = new DataSet();
            bufDataSet.Tables.Add(workman);
            bufDataSet.Tables.Add(office);
            bufDataSet.Tables.Add(postWorkman);
            bufDataSet.Tables.Add(manager);
            tables = bufDataSet;
        }

        //**************************************************************

        public bool AddWorkman(string secondName, string firstName, string lastName, string idPostWorkman, string idChief, string idOffice)
        {
            DataTable buf;
            if ((lastName == null || lastName == "") && ((idChief == null || idChief == "")))
                buf = AsyncClient.SendMessage("insert into s_workman (c_second_name_workman, c_first_name_workman, n_id_post_workman, n_id_office) values ('" + secondName + "', '" + firstName + "', '" + idPostWorkman + "', '" + idOffice + "')");
            else if (lastName == null || lastName == "")
                buf = AsyncClient.SendMessage("insert into s_workman (c_second_name_workman, c_first_name_workman, n_id_post_workman, n_id_chief, n_id_office) values ('" + secondName + "', '" + firstName + "', '" + idPostWorkman + "', '" + idChief + "', '" + idOffice + "')");
            else if (idChief == null || idChief == "")
                buf = AsyncClient.SendMessage("insert into s_workman (c_second_name_workman, c_first_name_workman, c_last_name_workman, n_id_post_workman, n_id_office) values ('" + secondName + "', '" + firstName + "', '" + lastName + "', '" + idPostWorkman + "', '" + idOffice + "')");
            else
                buf = AsyncClient.SendMessage("insert into s_workman (c_second_name_workman, c_first_name_workman, c_last_name_workman, n_id_post_workman, n_id_chief, n_id_office) values ('" + secondName + "', '" + firstName + "', '" + lastName + "', '" + idPostWorkman + "', '" + idChief + "', '" + idOffice + "')");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        public bool ChangeWorkman(string idWorkman, string secondName, string firstName, string lastName, string idPostWorkman, string idChief, string idOffice)
        {
            var buf = AsyncClient.SendMessage("update s_workman set c_second_name_workman='" + secondName + "', c_first_name_workman='" + firstName + "', c_last_name_workman='" + lastName + "', n_id_post_workman='" + idPostWorkman + "', n_id_chief='" + idChief + "', n_id_office='" + idOffice + "' where n_id_workman='" + idWorkman + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        public bool DelWorkman(string idWorkman)
        {
            var buf = AsyncClient.SendMessage("delete from s_workman where n_id_workman='" + idWorkman + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }
    }
}
