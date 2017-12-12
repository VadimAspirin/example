public class Point {
	
	private double x, y;
	
	public Point(double x, double y) {
		this.x = x;
		this.y = y;
	}
	
	public double GetX() {
		return x;
	}
	
	public double GetY() {
		return y;
	}

	@Override
    public boolean equals(Object obj) {
		if (obj == this)
			return true;
		if (obj == null || obj.getClass() != this.getClass())
			return false;
		Point buf = (Point)obj;
		return x == buf.GetX() && y == buf.GetY();
    }

}
