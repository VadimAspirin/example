import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.Scanner;
import java.util.logging.*;

public class Client {

	private static final int port = 8283;
	
	private static Logger log = Logger.getLogger(Client.class.getName());

	private BufferedReader in;
	private PrintWriter out;
	private Socket socket;

	public Client(String ip, String name) {
		try {
			FileHandler fileLog = new FileHandler("ChatLog.txt", true);
			ChatLogFormatter formatter = new ChatLogFormatter();
			fileLog.setFormatter(formatter);
			log.addHandler(fileLog);
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		try {
			socket = new Socket(ip, port);
			in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
			out = new PrintWriter(socket.getOutputStream(), true);
			out.println(name);
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	public void Close() {
		try {
			out.println("exit");
			in.close();
			out.close();
			socket.close();
		} catch (Exception e) {
			System.err.println("Потоки не были закрыты!");
		}
	}
	
	public void SendMessages(String mess) {
		out.println(mess);
	}

	public String GetMessages() {
		try {
			String str = in.readLine();
			log.info(str);
			return str;
		} catch (IOException e) {
			System.err.println("Ошибка при получении сообщения.");
			e.printStackTrace();
			return null;
		}
	}

}
