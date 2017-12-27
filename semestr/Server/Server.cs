using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Server
{

	public class AsyncSocketListener 
	{
		private class StateObject
		{
			public Socket WorkSocket = null; // Клиентский сокет.
			public const int BufferSize = 1024; // Размер буфера приема.
			public byte[] Buffer = new byte[BufferSize]; // Получить буфер.
			public StringBuilder StringBuffer = new StringBuilder(); // Получена строка данных.
		}
		
		private string host;
		private int port;
		private int backlog;
		private ManualResetEvent allDone; // Сигнал потока.
		private Thread listeningThread;
		private List<KeyValuePair<Socket, string>> clientsMessages;

		public AsyncSocketListener(string host = "localhost", int port = 8080, int backlog = 100) 
		{
			this.host = host;
			this.port = port;
			this.backlog = backlog;
			allDone = new ManualResetEvent(false);
			listeningThread = new Thread(new ThreadStart(startListening));
			listeningThread.Start();
			clientsMessages = new List<KeyValuePair<Socket, string>>();
		}

		private void startListening() 
		{
			//IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
			IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
			// Создаем сокет TCP/IP.
			Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			// Связывание сокета с локальной конечной точкой и прослушивание входящих соединений.
			try 
			{
				listener.Bind(localEndPoint);
				listener.Listen(backlog);
				Console.WriteLine("Сервер запущен. Ожидание подключений...");
				while (true) 
				{
					// Устанавливаем событие в несогласованное состояние.
					allDone.Reset();
					// Запустите асинхронный сокет для прослушивания соединений.
					listener.BeginAccept(new AsyncCallback(acceptCallback), listener);
					// Подождите, пока соединение не будет выполнено до продолжения.
					allDone.WaitOne();
				}
			} 
			catch (Exception e) 
			{
				Console.WriteLine(e.ToString());
			}
		}

		private void acceptCallback(IAsyncResult ar) 
		{
			// Сигнал основного потока для продолжения.
			allDone.Set();
			// Получает сокет, обрабатывающий клиентский запрос.
			Socket listener = (Socket) ar.AsyncState;
			Socket handler = listener.EndAccept(ar);
			// Создаем объект состояния.
			StateObject state = new StateObject();
			state.WorkSocket = handler;
			handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(readCallback), state);
		}

		private void readCallback(IAsyncResult ar) 
		{
			String content = String.Empty;
			// Получить объект состояния и сокет обработчика из асинхронного объекта состояния.
			StateObject state = (StateObject) ar.AsyncState;
			Socket handler = state.WorkSocket;
			// Чтение данных из клиентского сокета. 
			int bytesRead = handler.EndReceive(ar);
			
			if (bytesRead > 0) 
			{
				// Может быть больше данных, поэтому храните данные, полученные до сих пор.
				state.StringBuffer.Append(Encoding.Unicode.GetString(state.Buffer, 0, bytesRead));
				// Проверяем тег конца файла. Если его нет, прочитайте больше данных.
				content = state.StringBuffer.ToString();
				if (content.IndexOf("<EOF>") > -1) 
				{
					content = content.Substring(0, content.Length - 5); // убираем "<EOF>" из строки
					clientsMessages.Add (new KeyValuePair<Socket, string>(handler, content));
					Console.WriteLine("[Входящих сообщений: {0}]", clientsMessages.Count);
				} 
				else
				{
					// Не все полученные данные. Получите больше.
					handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(readCallback), state);
				}
			}
		}

		public string ShowMessage()
		{
			if (clientsMessages.Count == 0)
				return null;
			return clientsMessages[0].Value;
		}

		public void SendMessage(string data) 
		{
			if (clientsMessages.Count == 0)
				return;
			// Преобразуем строковые данные в байтовые данные, используя Unicode-кодировку.
			byte[] byteData = Encoding.Unicode.GetBytes(data);
			// Начнем отправку данных на удаленное устройство.
			clientsMessages[0].Key.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(sendCallback), clientsMessages[0].Key);
			clientsMessages.RemoveAt(0);
		}

		private void sendCallback(IAsyncResult ar) 
		{
			try 
			{
				// Извлеките сокет из объекта состояния.
				Socket handler = (Socket) ar.AsyncState;
				// Завершение отправки данных на удаленное устройство.
				int bytesSent = handler.EndSend(ar);
				//Console.WriteLine("Отправлено {0} байт клиенту.", bytesSent);
				Console.WriteLine("Ответ отправлен");

				handler.Shutdown(SocketShutdown.Both);
				handler.Close();
			} 
			catch (Exception e) 
			{
				Console.WriteLine(e.ToString());
			}
		}
		
		public void Disconnect()
        {
			foreach (KeyValuePair<Socket, string> keyValue in clientsMessages)
			{
				keyValue.Key.Shutdown(SocketShutdown.Both);
				keyValue.Key.Close();
			}
			listeningThread.Abort();
        }
	}

}
