using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
			AsyncSocketListener server = new AsyncSocketListener();

			while (true)
			{
				string inMess = server.ShowMessage();
				if (inMess == null)
				{
					Thread.Sleep(300);
					continue;
				}
				Console.WriteLine("Сообщение клиента: {0}", inMess);
				
				//string dbLogin = "Database=autoshop;charset= utf8;Data Source=localhost;User Id=root;Password=";
				//string dbLogin = "Database=d81866z2_autosh;charset= utf8;Data Source=d81866z2.beget.tech;User Id=d81866z2_autosh;Password=gfhjkzytn7";
				string dbLogin = "Database=q91935b4_autosh;charset= utf8;Data Source=q91935b4.beget.tech;User Id=q91935b4_autosh;Password=gfhjkzytn7";
				string outMess = "";
				if (inMess.Contains("select"))
				{
					MySqlLib.MySqlExecuteData.MyResultData result = new MySqlLib.MySqlExecuteData.MyResultData();
					result = MySqlLib.MySqlExecuteData.SqlReturnDataset(inMess, dbLogin);
					if(result.HasError)
					{
						Console.WriteLine(result.ErrorText);
						outMess = "<ERROR>";
					}
					else
					{
						if (result.ResultData.Rows.Count == 0)
						{
							outMess = "<ERROR>";
						}
						for (int i = 0; i < result.ResultData.Rows.Count; i++)
						{
							if (i != 0)
								outMess += '\n';
							outMess += result.ResultData.Rows[i][result.ResultData.Columns[0]];
						}
					}
				}
				else
				{
					MySqlLib.MySqlExecute.MyResult result = new MySqlLib.MySqlExecute.MyResult();
					result = MySqlLib.MySqlExecute.SqlNoneQuery(inMess, dbLogin);
					if(result.HasError)
					{
						Console.WriteLine(result.ErrorText);
						outMess = "<ERROR>";
					}
					else
					{
						outMess = "<OK>";
					}
				}
				
				//Console.WriteLine("Ваш ответ: {0}", outMess);
				server.SendMessage(outMess);
			}

        }
    }
}
