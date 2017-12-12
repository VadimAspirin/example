import java.util.Scanner;

public class Main {
	
	private static Logger log = Logger.getLogger(Client.class.getName());
	
	private static void InitLoader(){
		try {
		    LogManager.getLogManager().readConfiguration(
			    ChatServer.class.getResourceAsStream("/logging.properties"));
		} catch (IOException e) {
		    log.log(Level.SEVERE, "Не удалось создать лог. ", e);
		}
	    }
	
	public static void main(String[] args) {
		InitLoader();
		new StartClientWindow();
	}
}
