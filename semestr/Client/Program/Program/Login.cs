using System;

namespace Program
{
    public static class Login
    {
        public static string Start(string login, string password)
        {
            var buf = AsyncClient.SendMessage("select role from users where login='" + login + "' and password='" + password + "'");
            Console.WriteLine(buf.TableName);
            if (buf.TableName == "<ERROR>")
                return null;
            if (buf.Rows.Count == 0)
                return "";
            return buf.Rows[0]["role"].ToString();
        }
    }
}
