import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@WebServlet("/MyServlet")
public class MyServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		
		double x, n;
		Double result;
		String out = "";
		try {
        	x = Double.parseDouble(request.getParameter("x"));
        	n = Double.parseDouble(request.getParameter("n"));
        	result = Math.pow(x, n);
        	out = result.toString();
        } catch (NumberFormatException e) {
            out = ("Error format!");
        } finally {
        	PrintWriter printWriter = response.getWriter();
        	printWriter.println(out);
        	printWriter.close();
        }
	}

}
