using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Program
{
    class Admin : User
    {
        public Admin(string name = "Admin")
        {
            tables = new DataSet(name);
            UpdateTables();
        }

        public override void UpdateTables()
        {
            flagConnection = true;

            var bufDataSet1 = updateTablesForCompletedWork();
            var bufDataSet2 = updateTablesForSolvedProblems();
            var bufDataSet3 = updateTablesForJournalVisit();
            var bufDataSet4 = updateTablesForProduct();

            if (!flagConnection)
                return;

            var office = AsyncClient.SendMessage("select * from s_office");
            var users = AsyncClient.SendMessage("select * from users");
            if (office.TableName == "<ERROR>" || users.TableName == "<ERROR>")
            {
                flagConnection = false;
                return;
            }
            // "s_office": "Идентификатор", "Адрес"
            office.TableName = "s_office";
            office.Columns["n_id_office"].ColumnName = "Идентификатор";
            office.Columns["c_address_office"].ColumnName = "Адрес";
            // "users": "Логин", "Пароль", "Тип пользователя"
            users.TableName = "users";
            users.Columns["login"].ColumnName = "Логин";
            users.Columns["password"].ColumnName = "Пароль";
            users.Columns["role"].ColumnName = "Тип пользователя";
            // "roles": "role"
            DataTable roles = new DataTable("roles");
            roles.Columns.Add("role", typeof(String));
            roles.Rows.Add("admin");
            roles.Rows.Add("hr");
            roles.Rows.Add("journalman");
            roles.Rows.Add("storekeeper");
            roles.Rows.Add("workman");

            tables = bufDataSet1;
            tables.Merge(bufDataSet2);
            tables.Merge(bufDataSet3);
            tables.Merge(bufDataSet4);
            tables.Tables.Add(office);
            tables.Tables.Add(users);
            tables.Tables.Add(roles);
        }

        //**************************************************************

        public bool AddOffice(string address)
        {
            var buf = AsyncClient.SendMessage("insert into s_office (c_address_office) values ('" + address + "')");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        public bool ChangeOffice(string idOffice, string address)
        {
            var buf = AsyncClient.SendMessage("update s_office set c_address_office='" + address + "' where n_id_office='" + idOffice + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        public bool DelOffice(string idOffice)
        {
            var buf = AsyncClient.SendMessage("delete from s_office where n_id_office='" + idOffice + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        //**************************************************************

        public bool AddUser(string login, string password, string role)
        {
            var buf = AsyncClient.SendMessage("insert into users (login, password, role) values ('" + login + "', '" + password + "', '" + role + "')");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        public bool DelUser(string login)
        {
            var user = AsyncClient.SendMessage("select role from users where login='" + login + "'");
            if (user.TableName == "<ERROR>" || user.Rows.Count == 0)
                return false;
            if (user.Rows[0]["role"].ToString() == "admin")
            {
                var countAdmin = AsyncClient.SendMessage("select count(*) as cnt from users where role='admin'");
                if (countAdmin.TableName == "<ERROR>" || countAdmin.Rows.Count == 0)
                    return false;
                if (countAdmin.Rows[0]["cnt"].ToString() == "1")
                    return false;
            }
            var buf = AsyncClient.SendMessage("delete from users where login='" + login + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        //**************************************************************
        // DelSolvedProblems

        private DataSet updateTablesForSolvedProblems()
        {
            var bufDataSet = new DataSet();
            // "s_solved_problems": "Ид-ор решённой проблемы", "Решённая проблема", "Менеджер", "Дата"
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
            solvedProblems.Columns["n_id_solved_problems"].ColumnName = "Ид-ор решённой проблемы";
            solvedProblems.Columns.Remove("n_id_workman");
            solvedProblems.TableName = "s_solved_problems";
            bufDataSet.Tables.Add(solvedProblems);

            return bufDataSet;
        }

        public bool DelSolvedProblems(string idSolvedProblems)
        {
            var buf = AsyncClient.SendMessage("delete from s_solved_problems where n_id_solved_problems='" + idSolvedProblems + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        //**************************************************************
        // DelCompletedWork

        private DataSet updateTablesForCompletedWork()
        {
            var bufDataSet = new DataSet();
            // "s_completed_work": "Ид-ор выполненной работы", "Дата", "Работник", "Тип работы", "Товар", "Кол-во товара"
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
            completedWork.Columns["n_id_completed_work"].ColumnName = "Ид-ор выполненной работы";
            completedWork.Columns.Remove("n_id_workman");
            completedWork.Columns.Remove("n_id_type_completed_work");
            completedWork.Columns.Remove("n_id_product");
            completedWork.TableName = "s_completed_work";
            bufDataSet.Tables.Add(completedWork);

            return bufDataSet;
        }

        public bool DelCompletedWork(string idCompletedWork)
        {
            var buf = AsyncClient.SendMessage("delete from s_completed_work where n_id_completed_work='" + idCompletedWork + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        //**************************************************************
        // DelJournalEntry

        private DataSet updateTablesForJournalVisit()
        {
            var bufDataSet = new DataSet();
            // "s_journal_visit": "Дата", "Работник", "Ид-ор работника"
            var journalVisit = AsyncClient.SendMessage("select * from s_journal_visit");
            var workman = AsyncClient.SendMessage("select * from s_workman");
            if (journalVisit.TableName == "<ERROR>" || workman.TableName == "<ERROR>")
            {
                flagConnection = false;
                return new DataSet();
            }
            journalVisit.TableName = "s_journal_visit";
            journalVisit = newColumn(journalVisit, workman, "n_id_workman", "Работник", "c_second_name_workman", "c_first_name_workman", "c_last_name_workman");
            journalVisit.Columns["d_journal_visit"].ColumnName = "Дата";
            journalVisit.Columns["n_id_workman"].ColumnName = "Ид-ор работника";
            bufDataSet.Tables.Add(journalVisit);

            return bufDataSet;
        }

        public bool DelJournalEntry(string date, string idWorkman)
        {
            string newDate = changeDate(date);
            if (newDate == null)
                return false;
            var buf = AsyncClient.SendMessage("delete from s_journal_visit where n_id_workman='" + idWorkman + "' and d_journal_visit='" + newDate + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        //**************************************************************
        // ChangeProductCount

        private DataSet updateTablesForProduct()
        {
            var bufDataSet = new DataSet();
            // "s_product": "Идентификатор", "Название", "Количество", "Цена", "Тип"
            var product = AsyncClient.SendMessage("select * from s_product");
            var typeProduct = AsyncClient.SendMessage("select * from s_type_product");
            if (product.TableName == "<ERROR>" || typeProduct.TableName == "<ERROR>")
            {
                flagConnection = false;
                return new DataSet();
            }
            product.TableName = "s_product";
            product = newColumn(product, typeProduct, "n_id_type_product", "Тип", "c_name_type_product");
            product.Columns["n_id_product"].ColumnName = "Идентификатор";
            product.Columns["c_name_product"].ColumnName = "Название";
            product.Columns["n_product_count"].ColumnName = "Количество";
            product.Columns["n_product_price"].ColumnName = "Цена";
            product.Columns.Remove("n_id_type_product");
            bufDataSet.Tables.Add(product);

            return bufDataSet;
        }

        public bool ChangeProductCount(string idProduct, string count)
        {
            var buf = AsyncClient.SendMessage("update s_product set n_product_count='" + count + "' where n_id_product='" + idProduct + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }
    }
}

