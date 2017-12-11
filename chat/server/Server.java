import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Iterator;
import java.util.List;
import java.util.logging.*;

public class Server {

	public static final int Port = 8283;
	private static Logger logger = Logger.getLogger(Server.class.getName());
	
	private List<Connection> connections = Collections.synchronizedList(new ArrayList<Connection>());
	private ServerSocket server;

	public Server() {
		setupLogger();
		try {
			server = new ServerSocket(Port);

			System.out.println("Сервер ожидает подключений...");

			while (true) {
				Socket socket = server.accept();

				Connection con = new Connection(socket);
				connections.add(con);

				con.start();

			}
		} catch (IOException e) {
			logger.log(Level.SEVERE, "Exception: ", e);
			e.printStackTrace();
		} finally {
			closeAll();
		}
	}
	
	private static void setupLogger() {
		try {
			FileHandler fh = new FileHandler("LogServer");
			logger.addHandler(fh);
			
		} catch (SecurityException e) {
			logger.log(Level.SEVERE,
					"Не удалось создать файл лога из-за политики безопасности.", 
					e);
		} catch (IOException e) {
			logger.log(Level.SEVERE,
					"Не удалось создать файл лога из-за ошибки ввода-вывода.",
					e);
		}
	}

	private void closeAll() {
		try {
			server.close();
			
			synchronized(connections) {
				Iterator<Connection> iter = connections.iterator();
				while(iter.hasNext()) {
					((Connection) iter.next()).close();
				}
			}
		} catch (Exception e) {
			logger.log(Level.SEVERE, "Exception: ", e);
			System.err.println("Потоки не были закрыты!");
		}
	}

	private class Connection extends Thread {
		private BufferedReader in;
		private PrintWriter out;
		private Socket socket;
	
		private String name = "";
	
		public Connection(Socket socket) {
			this.socket = socket;
	
			try {
				in = new BufferedReader(new InputStreamReader(
						socket.getInputStream()));
				out = new PrintWriter(socket.getOutputStream(), true);
	
			} catch (IOException e) {
				e.printStackTrace();
				logger.log(Level.SEVERE, "Exception: ", e);
				close();
			}
		}
	
		@Override
		public void run() {
			try {
				name = in.readLine();
				
				System.out.println(name + " подключился к серверу");
				
				synchronized(connections) {
					Iterator<Connection> iter = connections.iterator();
					while(iter.hasNext()) {
						((Connection) iter.next()).out.println(name + " зашёл в Чат");
					}
				}
				
				String str = "";
				while (true) {
					str = in.readLine();
					if(str.equals("exit")) break;
					
					synchronized(connections) {
						Iterator<Connection> iter = connections.iterator();
						while(iter.hasNext()) {
							((Connection) iter.next()).out.println(name + ": " + str);
						}
					}
				}
				
				System.out.println(name + " покинул сервер");
				
				synchronized(connections) {
					Iterator<Connection> iter = connections.iterator();
					while(iter.hasNext()) {
						((Connection) iter.next()).out.println(name + " покинул Чат");
					}
				}
			} catch (IOException e) {
				logger.log(Level.SEVERE, "Exception: ", e);
				e.printStackTrace();
			} finally {
				close();
			}
		}
	
		public void close() {
			try {
				in.close();
				out.close();
				socket.close();
	
				connections.remove(this);
				if (connections.size() == 0) {
					Server.this.closeAll();
					System.exit(0);
				}
			} catch (Exception e) {
				logger.log(Level.SEVERE, "Exception: ", e);
				System.err.println("Потоки не были закрыты!");
			}
		}
	}
}
