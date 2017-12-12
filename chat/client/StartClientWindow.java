import java.awt.*;
import java.awt.event.*;
import javax.swing.*;

public class StartClientWindow extends JFrame {

	private Client client;
	
	public StartClientWindow() {
		setTitle("Чат");
		setBounds(400, 150, 325, 88);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		JTextField fieldIp = new JTextField(20);
		JLabel labelIp = new JLabel("IP сервера");
		
		JTextField fieldName = new JTextField(20);
		JLabel labelName = new JLabel("Ваше имя");
		
		JButton jbEntry = new JButton("Вход");
		
		JPanel panelNorth = new JPanel(new BorderLayout());
		panelNorth.add(labelIp, BorderLayout.WEST);
		panelNorth.add(fieldIp, BorderLayout.EAST);
		
		JPanel panelCenter = new JPanel(new BorderLayout());
		panelCenter.add(labelName, BorderLayout.WEST);
		panelCenter.add(fieldName, BorderLayout.EAST);
		
		JPanel bottomPanel = new JPanel(new BorderLayout());
		bottomPanel.add(jbEntry, BorderLayout.CENTER);
		
		
		add(panelNorth, BorderLayout.NORTH);
		add(panelCenter, BorderLayout.CENTER);
		add(bottomPanel, BorderLayout.SOUTH);
		
		jbEntry.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				if (!fieldIp.getText().trim().isEmpty() && !fieldName.getText().trim().isEmpty()) {
					client = new Client(fieldIp.getText(), fieldName.getText());
					setVisible(false);
					new ClientWindow(client);
				}
			}
		});
		
		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent e) {
				System.exit(1);
			}
		});
		
		setVisible(true);
	}
	
	public Client GetClient() {
		return client;
	}
}
