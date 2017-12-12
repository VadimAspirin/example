public class Main {
	public static void main(String[] args) {
		Triangle triangle = new Triangle(new Point(0, 0), new Point(5.1, 0), new Point(0, 5.4));
		
		System.out.println("Периметр треугольника: " + triangle.Perimeter());
		System.out.println("Площадь треугольника: " + triangle.Area());
		System.out.println("Треугольник правильный?: " + triangle.Right());
		System.out.println("Треугольник равнобедренный?: " + triangle.Isosceles());	
	}
}
