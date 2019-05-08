<Query Kind="Program" />

void Main()
{
	//I think what happens is:
	// zval = "8*","7*","6*","
	int zval = Fibonacci(5);
	int zRecursiveVal = FibonacciRecursion(5); 
	
	Console.Write("\n\nIterative Result = "+zval); 
	Console.Write("\n\nRecusive Result = "+zRecursiveVal); 
}

// Define other methods and classes here
 

//basic recursion
int Faculty(int n) {	 
	if(n == 1) {
		return 1;
	} 
	return n * Faculty(n-1); 
}

//fibonacci recursion
int FibonacciRecursion(int v){
	if(v <= 1){
		return 1;
	}
	//this will work if v < 40, but will take an infinite time to compute if v > 40
	return Fibonacci(v-1) + Fibonacci(v-2);
}

internal int Fibonacci(int n) {	
	int low=1,high=1;
	for (int i = 0; i < n; i++)
	{
		var oldHigh = high;
		high = low+high;
		low = oldHigh;
	}
	return low; 
}






//\\