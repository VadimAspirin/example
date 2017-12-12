import java.util.Scanner;

public class Main {
	
	public static Logger Log = Logger.getLogger(Client.class.getName());
	
	private static void LoadLogger(){
		try {
		    LogManager.getLogManager().readConfiguration(
			    ChatServer.class.getResourceAsStream("/logging.properties"));
		} catch (IOException e) {
		    Log.log(Level.SEVERE, "Не удалось создать лог. ", e);
		}
	    }
	
	public static void main(String[] args) {
		LoadLogger();
		new StartClientWindow();
	}
}
