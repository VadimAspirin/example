import java.util.*;  

public class Main {

    public static void main(String[] args) {
        BinaryTree<Double> test = new BinaryTree<>();
        test.Add(29.12);
        test.Add(29.9);
        test.Add(123.2321);
        test.Add(13.43);
        test.Add(1.43);
        test.Add(0.00001);
        test.Add(0.043);
        
        System.out.println(test.ToString());
        System.out.println();
        
        List buf = test.ToList();
        for(int i = 0; i < buf.size(); i++)
			System.out.println(buf.get(i));
    }

}
