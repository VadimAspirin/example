import java.security.*;

public class Triangle {

	private Point pointFirst, pointSecond, pointThird;
	private Edge edgeFirst, edgeSecond, edgeThird;
	
	public Triangle(Point pointFirst, Point pointSecond, Point pointThird) {
		if(pointFirst.equals(pointSecond) || pointSecond.equals(pointThird) || pointFirst.equals(pointThird))
			throw new InvalidParameterException("error: Задан не треугольник! Две или более точек равны");
		this.pointFirst = pointFirst;
		this.pointSecond = pointSecond;
		this.pointThird = pointThird;
		edgeFirst = new Edge(pointFirst, pointSecond);
		edgeSecond = new Edge(pointSecond, pointThird);
		edgeThird = new Edge(pointFirst, pointThird);
	}
	
	public Point GetPointFirst() {
		return pointFirst;
	}
	
	public Point GetPointSecond() {
		return pointSecond;
	}
	
	public Point GetPointThird() {
		return pointThird;
	}
	
	@Override
    public boolean equals(Object obj) {
		if (obj == this)
			return true;
		if (obj == null || obj.getClass() != this.getClass())
			return false;
		Triangle buf = (Triangle)obj;
		return pointFirst.equals(buf.GetPointFirst()) && 
			   pointSecond.equals(buf.GetPointSecond()) && 
			   pointThird.equals(buf.GetPointThird());
    }
	
	public double Length(int i) {
		if(i == 0)
			return edgeFirst.Length();
		else if(i == 1)
			return edgeSecond.Length();
		else if(i == 2)
			return edgeThird.Length();
		else
			throw new InvalidParameterException("error: У треугольника 3 стороны");
	}
	
	public double Perimeter() {
		return edgeFirst.Length() + edgeSecond.Length() + edgeThird.Length();
	}
	
	public double Area() {
		double p = Perimeter() / 2;
		return Math.sqrt(p * (p - edgeFirst.Length()) *
					         (p - edgeSecond.Length()) * 
					         (p - edgeThird.Length()));
	
	}
	
	public boolean Right() {
		return ((Math.pow(edgeFirst.Length(), 2) + Math.pow(edgeSecond.Length(), 2)) == Math.pow(edgeThird.Length(), 2) ||
				(Math.pow(edgeFirst.Length(), 2) + Math.pow(edgeThird.Length(), 2)) == Math.pow(edgeSecond.Length(), 2) ||
				(Math.pow(edgeThird.Length(), 2) + Math.pow(edgeSecond.Length(), 2)) == Math.pow(edgeFirst.Length(), 2));
	}
	
	public boolean Isosceles() {
		return (edgeFirst.equals(edgeSecond) || 
				edgeFirst.equals(edgeThird) ||
				edgeSecond.equals(edgeThird));
	}

}
