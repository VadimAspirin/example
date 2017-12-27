using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;

namespace Server
{
    // Набор компонент для простой работы с MySQL базой данных.
    public class MySqlLib
    {

        // Методы реализующие выполнение запросов с возвращением одного параметра либо без параметров вовсе.
        public class MySqlExecute
        {

            // Возвращаемый набор данных.
            public class MyResult
            {
                // Возвращает результат запроса.
                public string ResultText;
                // Возвращает True - если произошла ошибка.
                public string ErrorText;
                // Возвращает текст ошибки.
                public bool HasError;
            }

            // Для выполнения запросов к MySQL с возвращением 1 параметра.
            // "sql" - Текст запроса к базе данных
            // "connection" - Строка подключения к базе данных
            // Возвращает значение при успешном выполнении запроса, текст ошибки - при ошибке.
            public static MyResult SqlScalar(string sql, string connection)
            {
                MyResult result = new MyResult();
                try
                {
                    MySql.Data.MySqlClient.MySqlConnection connRC = new MySql.Data.MySqlClient.MySqlConnection(connection);
                    MySql.Data.MySqlClient.MySqlCommand commRC = new MySql.Data.MySqlClient.MySqlCommand(sql, connRC);
                    connRC.Open();
                    try
                    {
                        result.ResultText = commRC.ExecuteScalar().ToString();
                        result.HasError = false;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorText = ex.Message;
                        result.HasError = true;
                    }
                    connRC.Close();
                }
                catch (Exception ex)//Этот эксепшн на случай отсутствия соединения с сервером.
                {
                    result.ErrorText = ex.Message;
                    result.HasError = true;
                }
                return result;
            }


            // Для выполнения запросов к MySQL без возвращения параметров.
            // "sql" - Текст запроса к базе данных
            // "connection" - Строка подключения к базе данных
            // Возвращает True - ошибка или False - выполнено успешно.
            public static MyResult SqlNoneQuery(string sql, string connection)
            {
                MyResult result = new MyResult();
                try
                {
                    MySql.Data.MySqlClient.MySqlConnection connRC = new MySql.Data.MySqlClient.MySqlConnection(connection);
                    MySql.Data.MySqlClient.MySqlCommand commRC = new MySql.Data.MySqlClient.MySqlCommand(sql, connRC);
                    connRC.Open();
                    try
                    {
                        commRC.ExecuteNonQuery();
                        result.HasError = false;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorText = ex.Message;
                        result.HasError = true; 
                    }
                    connRC.Close();
                }
                catch (Exception ex)//Этот эксепшн на случай отсутствия соединения с сервером.
                {
                    result.ErrorText = ex.Message;
                    result.HasError = true;
                }
                return result;
            }
             
        }

        // Методы реализующие выполнение запросов с возвращением набора данных.
        public class MySqlExecuteData
        {
            // Возвращаемый набор данных.
            public class MyResultData
            {
                // Возвращает результат запроса.
                public DataTable ResultData;
                // Возвращает True - если произошла ошибка.
                public string ErrorText;
                // Возвращает текст ошибки.
                public bool HasError;
            }


            // Выполняет запрос выборки набора строк.
            // "sql" - Текст запроса к базе данных
            // "connection" - Строка подключения к базе данных
            // Возвращает набор строк в DataSet.
            public static MyResultData SqlReturnDataset(string sql, string connection)
            {
                MyResultData result = new MyResultData();
                try
                {
                    MySql.Data.MySqlClient.MySqlConnection connRC = new MySql.Data.MySqlClient.MySqlConnection(connection);
                    MySql.Data.MySqlClient.MySqlCommand commRC = new MySql.Data.MySqlClient.MySqlCommand(sql, connRC);
                    connRC.Open();

                    try
                    {
                        MySql.Data.MySqlClient.MySqlDataAdapter AdapterP = new MySql.Data.MySqlClient.MySqlDataAdapter();
                        AdapterP.SelectCommand = commRC;
                        DataSet ds1 = new DataSet();
                        AdapterP.Fill(ds1);
                        result.ResultData = ds1.Tables[0];
                    }
                    catch (Exception ex)
                    {
                        result.HasError = true;
                        result.ErrorText = ex.Message;
                    }
                    connRC.Close();
                }
                catch (Exception ex)//Этот эксепшн на случай отсутствия соединения с сервером.
                {
                    result.ErrorText = ex.Message;
                    result.HasError = true;
                }
            
                return result;

            }

        }
    }
}
