import java.util.*;  

public class BinaryTree<T extends Comparable<T>> {

	private T value = null;
	private BinaryTree<T> right = null;
	private BinaryTree<T> left = null;
	
	public void Add(T data) {
		if(value == null) {
			value = data;
		} else if(value.compareTo(data) < 0) {
			if(right == null)
				right = new BinaryTree<T>();
			right.Add(data);
		} else {
			if(left == null)
				left = new BinaryTree<T>();
			left.Add(data);
		}
	}

	public List ToList() {
		List<T> result = new ArrayList<T>();
		if (value != null) {
			if (left != null)
				result.addAll(left.ToList());
			result.add(value);
            if (right != null)
				result.addAll(right.ToList());
        }
        return result;
	}
	
	public String ToString() {
		String result = "";
		if (value != null) {
			if (left != null) 
				result += left.ToString();
            result += value + " ";
            if (right != null)
				result += right.ToString();
        }
        return result;
	}

}
