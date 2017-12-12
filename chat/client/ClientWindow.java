import java.awt.*;
import java.awt.event.*;
import javax.swing.*;

public class ClientWindow extends JFrame {

	private Client client;
	
	private JTextArea incomingMessages;
	private JTextField inputMess;
  
	public ClientWindow(Client client) {
		
		setTitle("Чат");
		setBounds(400, 150, 600, 500);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	   
		incomingMessages = new JTextArea();
		incomingMessages.setEditable(false);
		incomingMessages.setLineWrap(true);
		JScrollPane jsp = new JScrollPane(incomingMessages);
		add(jsp);
		
		JButton jbSendMessage = new JButton("Отправить");
		JPanel bottomPanel = new JPanel(new BorderLayout());
		bottomPanel.add(jbSendMessage, BorderLayout.EAST);
		inputMess = new JTextField("Введите ваше сообщение... ");
		bottomPanel.add(inputMess);
		add(bottomPanel, BorderLayout.SOUTH);

		inputMess.addFocusListener(new FocusAdapter() {
			@Override
			public void focusGained(FocusEvent e) {
				inputMess.setText("");
			}
		});
		
		jbSendMessage.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				if (!inputMess.getText().trim().isEmpty()) {
					client.SendMessages(inputMess.getText());
					inputMess.grabFocus();
				}
			}
		});
		
		inputMess.addKeyListener(new KeyAdapter() {
			@Override
			public void keyPressed(KeyEvent e) {		
				int key = e.getKeyCode();
				if (key == KeyEvent.VK_ENTER) {
					if (!inputMess.getText().trim().isEmpty()) {
						client.SendMessages(inputMess.getText());
						inputMess.setText("");
					}
				}
			}
		});

		Thread thread = new Thread(new Runnable() {
			@Override
			public void run() {
				try {
					while (true) {
						String mess = client.GetMessages();
						if(mess == null)
							continue;
						incomingMessages.append(mess);
						incomingMessages.append("\n");
					}
				} catch (Exception e) {}
			}
		});
		thread.start();

		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent e) {
				super.windowClosing(e);
				try {
					thread.stop();
					client.Close();
				} catch (Exception exc) {}
			}
		});

		setVisible(true);
		
	}
}
