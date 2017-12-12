import java.security.*;

public class Edge {
	
	private Point pointFirst, pointSecond;
	
	public Edge(Point pointFirst, Point pointSecond) {
		if(pointFirst.equals(pointSecond))
			throw new InvalidParameterException("error: Длина ребра должна быть больше 0");
		this.pointFirst = pointFirst;
		this.pointSecond = pointSecond;
	}
	
	public Point GetPointFirst() {
		return pointFirst;
	}
	
	public Point GetPointSecond() {
		return pointSecond;
	}

	@Override
    public boolean equals(Object obj) {
		if (obj == this)
			return true;
		if (obj == null || obj.getClass() != this.getClass())
			return false;
		Edge buf = (Edge)obj;
		return pointFirst.equals(buf.GetPointFirst()) && pointSecond.equals(buf.GetPointSecond());
    }
    
    public double Length() {
		return Math.sqrt (Math.pow (pointSecond.GetX() - pointFirst.GetX(), 2) + 
						  Math.pow (pointSecond.GetY() - pointFirst.GetY(), 2));
    }

}
