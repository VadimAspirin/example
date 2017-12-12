import java.util.Scanner;

public class Main {
	
	public static Logger Log = Logger.getLogger(Client.class.getName());
	
	private static void InitLoader(){
		try {
		    LogManager.getLogManager().readConfiguration(
			    ChatServer.class.getResourceAsStream("/logging.properties"));
		} catch (IOException e) {
		    Log.log(Level.SEVERE, "Не удалось создать лог. ", e);
		}
	    }
	
	public static void main(String[] args) {
		InitLoader();
		new StartClientWindow();
	}
}
