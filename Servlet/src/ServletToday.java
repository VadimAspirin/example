import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import java.util.Date;
import java.text.DateFormat;
import java.text.SimpleDateFormat;

public class ServletToday extends HttpServlet {
	@Override
	public void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		response.setContentType("text/html");
		PrintWriter out = response.getWriter();
		
		DateFormat df = new SimpleDateFormat("dd.MM.yyyy hh:mm:ss.SSS");
		String today = df.format(new Date());
		
		out.write("<!DOCTYPE html>\n" + 
				  "<html>\n" +
				  "<head><title>Today</title></head>\n" +
					  "<body>\n" +
				          "<h1>" + today + "</h1>\n" +
					  "</body>\n" +
				  "</html>"); 
	}
}
