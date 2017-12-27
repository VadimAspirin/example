using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Program
{
    class CompletedWorkMan : User
    {
        private string dateCompletedWork;
        private string idWorkman;

        private string dateSolvedProblems;

        public CompletedWorkMan(string name = "CompletedWorkMan")
        {
            tables = new DataSet(name);
            dateCompletedWork = null;
            idWorkman = null;
            dateSolvedProblems = null;
            UpdateTables();
        }

        public override void UpdateTables()
        {
            flagConnection = true;
            var bufDataSet1 = updateTablesForCompletedWork();
            var bufDataSet2 = updateTablesForSolvedProblems();
            if (!flagConnection)
                return;
            tables = bufDataSet1;
            tables.Merge(bufDataSet2);
        }

        //**************************************************************

        private DataSet updateTablesForCompletedWork()
        {
            var bufDataSet = new DataSet();
            // "s_completed_work": "Дата", "Работник", "Тип работы", "Товар", "Кол-во товара"
            var completedWork = AsyncClient.SendMessage("select * from s_completed_work");
            var workman = AsyncClient.SendMessage("select * from s_workman");
            var typeCompletedWork = AsyncClient.SendMessage("select * from s_type_completed_work");
            var product = AsyncClient.SendMessage("select * from s_product");
            if (completedWork.TableName == "<ERROR>" || workman.TableName == "<ERROR>" || typeCompletedWork.TableName == "<ERROR>" || product.TableName == "<ERROR>")
            {
                flagConnection = false;
                return new DataSet();
            }
            completedWork = newColumn(completedWork, workman, "n_id_workman", "Работник", "c_second_name_workman", "c_first_name_workman", "c_last_name_workman");
            completedWork = newColumn(completedWork, typeCompletedWork, "n_id_type_completed_work", "Тип работы", "c_name_completed_work");
            completedWork = newColumn(completedWork, product, "n_id_product", "Товар", "c_name_product");
            completedWork.Columns["d_date_completion"].ColumnName = "Дата";
            completedWork.Columns["n_product_count"].ColumnName = "Кол-во товара";
            completedWork.Columns.Remove("n_id_completed_work");
            completedWork.Columns.Remove("n_id_workman");
            completedWork.Columns.Remove("n_id_type_completed_work");
            completedWork.Columns.Remove("n_id_product");
            completedWork.TableName = "s_completed_work";
            bufDataSet.Tables.Add(completedWork);
            // "s_product": "n_id_product", "c_name_product"
            product.Columns.Remove("n_product_count");
            product.Columns.Remove("n_product_price");
            product.Columns.Remove("n_id_type_product");
            product.TableName = "s_product";
            bufDataSet.Tables.Add(product);
            // "Работник": "n_id_workman", "Работник"
            if (dateCompletedWork == null)
                return bufDataSet;
            var journalVisit = AsyncClient.SendMessage("select * from s_journal_visit where d_journal_visit='" + dateCompletedWork + "' and n_id_workman not in (select n_id_workman from s_workman where n_id_post_workman=(select n_id_post_workman from s_post_workman where c_name_post_workman='Менеджер'))");
            if (journalVisit.TableName == "<ERROR>")
            {
                flagConnection = false;
                return new DataSet();
            }
            journalVisit = newColumn(journalVisit, workman, "n_id_workman", "Работник", "c_second_name_workman", "c_first_name_workman", "c_last_name_workman");
            journalVisit.Columns.Remove("d_journal_visit");
            journalVisit.TableName = "Работник";
            bufDataSet.Tables.Add(journalVisit);
            // "Тип работы": "n_id_type_completed_work", "c_name_completed_work"
            if (idWorkman == null)
                return bufDataSet;
            var postWorkman = AsyncClient.SendMessage("select n_id_post_workman from s_workman where n_id_workman='" + idWorkman + "'");
            var typeCompletedWorkForWorkman = AsyncClient.SendMessage("select * from s_type_completed_work where n_id_post_workman='" + postWorkman.Rows[0]["n_id_post_workman"].ToString() + "'");
            if (postWorkman.TableName == "<ERROR>" || typeCompletedWorkForWorkman.TableName == "<ERROR>")
            {
                flagConnection = false;
                return new DataSet();
            }
            typeCompletedWorkForWorkman.Columns.Remove("n_id_post_workman");
            typeCompletedWorkForWorkman.TableName = "Тип работы";
            bufDataSet.Tables.Add(typeCompletedWorkForWorkman);

            return bufDataSet;
        }

        public bool NewDateCompletedWork(string date)
        {
            string newDate = changeDate(date);
            if (newDate == null)
                return false;
            dateCompletedWork = newDate;
            return true;
        }

        public bool NewIdWorkman(string id)
        {
            if (id == null)
                return false;
            idWorkman = id;
            return true;
        }

        public bool AddCompletedWork(string date, string idWorkman, string idTypeCompletedWork, string idProduct, string countProduct)
        {
            string newDate = changeDate(date);
            if (newDate == null)
                return false;
            var product = AsyncClient.SendMessage("select n_product_count from s_product where n_id_product='" + idProduct + "'");
            try
            {
                int count = Convert.ToInt32(product.Rows[0]["n_product_count"]);
                int countWork = Convert.ToInt32(countProduct);
                if (countWork <= 0)
                    return false;
                if ((idTypeCompletedWork == "1" || idTypeCompletedWork == "2") && (count - countWork) < 0)
                    return false;
                if (idTypeCompletedWork == "1" || idTypeCompletedWork == "2")
                    count = count - countWork;
                if (idTypeCompletedWork == "3")
                    count = count + countWork;
                var buf1 = AsyncClient.SendMessage("update s_product set n_product_count='" + count + "' where n_id_product='" + idProduct + "'");
                if (buf1.TableName == "<ERROR>")
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            var buf = AsyncClient.SendMessage("insert into s_completed_work (d_date_completion, n_id_product, n_id_type_completed_work, n_id_workman, n_product_count) values ('" + newDate + "', '" + idProduct + "', '" + idTypeCompletedWork + "', '" + idWorkman + "', '" + countProduct + "')");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        //**************************************************************

        private DataSet updateTablesForSolvedProblems()
        {
            var bufDataSet = new DataSet();
            // "s_solved_problems": "Решённая проблема", "Менеджер", "Дата"
            var solvedProblems = AsyncClient.SendMessage("select * from s_solved_problems");
            var workman = AsyncClient.SendMessage("select * from s_workman");
            if (solvedProblems.TableName == "<ERROR>" || workman.TableName == "<ERROR>")
            {
                flagConnection = false;
                return new DataSet();
            }
            solvedProblems = newColumn(solvedProblems, workman, "n_id_workman", "Менеджер", "c_second_name_workman", "c_first_name_workman", "c_last_name_workman");
            solvedProblems.Columns["c_description_solved_problems"].ColumnName = "Решённая проблема";
            solvedProblems.Columns["d_date_completion"].ColumnName = "Дата";
            solvedProblems.Columns.Remove("n_id_workman");
            solvedProblems.Columns.Remove("n_id_solved_problems");
            solvedProblems.TableName = "s_solved_problems";
            bufDataSet.Tables.Add(solvedProblems);
            // "Менеджер": "n_id_workman", "Менеджер"
            if (dateSolvedProblems == null)
                return bufDataSet;
            var journalVisit = AsyncClient.SendMessage("select * from s_journal_visit where d_journal_visit='" + dateSolvedProblems + "' and n_id_workman in (select n_id_workman from s_workman where n_id_post_workman=(select n_id_post_workman from s_post_workman where c_name_post_workman='Менеджер'))");
            if (journalVisit.TableName == "<ERROR>")
            {
                flagConnection = false;
                return new DataSet();
            }
            journalVisit = newColumn(journalVisit, workman, "n_id_workman", "Менеджер", "c_second_name_workman", "c_first_name_workman", "c_last_name_workman");
            journalVisit.Columns.Remove("d_journal_visit");
            journalVisit.TableName = "Менеджер";
            bufDataSet.Tables.Add(journalVisit);

            return bufDataSet;
        }

        public bool NewDateSolvedProblems(string date)
        {
            string newDate = changeDate(date);
            if (newDate == null)
                return false;
            dateSolvedProblems = newDate;
            return true;
        }

        public bool AddSolvedProblems(string date, string idWorkman, string dataProblems)
        {
            string newDate = changeDate(date);
            if (newDate == null)
                return false;
            var buf = AsyncClient.SendMessage("insert into s_solved_problems (c_description_solved_problems, d_date_completion, n_id_workman) values ('" + dataProblems + "', '" + newDate + "', '" + idWorkman + "')");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }
    }
}
