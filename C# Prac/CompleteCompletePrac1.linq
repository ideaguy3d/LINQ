<Query Kind="Program" />

public delegate int BaseIntDel< in T1in, in T2in, out TResult>(T2in x, T2in y);   

void Main()
{	
	//prac making my own delegate with in and out generic type pars, its right above ^^,  below was prac
	//using it. 
	//Func<int, int, string> AMethod = new Func<int, int, string>(SomeMethod); //old way
	Func<int, int, string> AMethod = SomeMethod; //new better way!
	Console.WriteLine ("The new way (:<  " + AMethod(5,6));
	
	 DelPracHolder();
}




//old methods from some other time I was using this file to prac
public static string SomeMethod(int x, int y)
{
	return "In SomeMethod()... Equal: " + x + " " + y; 
}

public static string PracFunc(string s, Func<int, int, string> fiis)
{
	return "In PracFunc(), s = " + s + " fiis(5,6) = " + fiis(5,6);  
}

public static string ReturnStringFunc(int x, int y)
{
	int tempInt1 = 90 + y; 
	x += tempInt1; 
	string s = "..... I am a newly created string ^_^ I now live !"; 
	return "After Calculations... x = " + x + s;
}

public void DelPracHolder()
{
	//here we are setting delegate Func<> to a lambda.
	Func<int, int, string> prac1 = (num0, num1) => num0.ToString() + " - " + num1.ToString() + " = " + (num0 - num1).ToString(); 
	string s = PracFunc("Hello World" , prac1); 
	Console.WriteLine ();
	Console.WriteLine (s);//this outputs all my fun prac
	
	Console.WriteLine ();
	
	//here we are setting delegate Func<> to a method. 
	Func<int, int, string> prac2 = ReturnStringFunc; 
	Console.WriteLine (prac2(5,6));
}