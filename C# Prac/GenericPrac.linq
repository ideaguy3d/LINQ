<Query Kind="Program" />

public enum Gender{Female, Male, Unknown};  


//public delegate string Promote<T, string>(T num); 

void Main()
{
	KudvenFunc kfclass = new KudvenFunc();
	//kfclass.JFuncPrac(); 
	
	List<JEmp> someList = JEmp.employeesList; 
	//someList.Dump("count = " + someList.Count());
	
	//IEnumerable<int32> someOtherList = someList.Select(n => n.Exp*10); 
	Func<JEmp, int> someFunc = CCPrac2; 
	IEnumerable<int> someOtherList2 = someList.Select(rangeVar => someFunc(rangeVar)); 
	someOtherList2.Dump(); 
	Func<int, int, string> f1 = CCPrac1;
	Console.WriteLine ("="+f1(2,4)); 
}

public int CCPrac2(JEmp j)
{
	int[] iArray = {j.Exp,2,3,44,5};
	return iArray[3]; 
	//Console.WriteLine ("Hello world");
}
public string CCPrac1(int x, int y)
{
	return "ints added = "+(x+y).ToString(); 
}

public class JGenericMethod
{
	public static bool ObsEqual<T> (T x, T y) { return x.Equals(y); } 
}

public class JGenericClass<T> 
{
	public static bool ObsEqual (T x, T y){ return x.Equals(y); } 
}

//beginner generic practice from Kudvens generic video
public void JGenericBasicPractice()
{
	//Prac w/ generic method 
	string s = "Hello", s2 = "Hello";
	//Console.WriteLine("Hello World is " + JGenericMethod.ObsEqual<string>(s, s2)); 
	bool b = JGenericMethod.ObsEqual<string>(s, s2); //the <> are actually not needed beccause they can be inferred  

	// Prac w/ generic class 
	Console.WriteLine("s and s2 are " + JGenericClass<string>.ObsEqual(s, s2) ) ; //the <> are required when using generic classes 
}

// here I am going to be practicing 'Func<>' from kudvens Func delegate vid100
public class KudvenFunc
{
	List<JEmp> kudvenList = JEmp.empList; 
	
	public void JFuncPrac()
	{
		Func<JEmp, string> jselector = n => "Name is : " + n.Name;//this is completely Not needed. Just passing 
		//in the lambda expression into .Select() will get the job done 
		IEnumerable<string> names = kudvenList.Select(jselector);
		//here I am practicing w/the overloaded Func's		// Notice! how this is where we access the complex ob's properties
		Func<JEmp, JEmp, int, string> threeInFunc = (x, y, z) => x.Name + " " + y.Name + " => " + (x.Exp + y.Exp + z).ToString(); 
		IEnumerable<string> otherNamesWay = kudvenList.Select(n => "Name is : " + n.Name );
		
		Console.WriteLine( kudvenList[0].Exp + " + " + kudvenList[2].Exp + " + What? " + threeInFunc(kudvenList[0], kudvenList[2], 5) + "... Solve the equation! :) "); 
		otherNamesWay.Dump("Regular old lambda for .Select():");
		names.Dump("FINALLY I work through the first 6mins of Kudvans part 100 vid!!!!");
	}
}

///<summary>
///this entire class is just so that  I can have some data to play with, all this work
///just to have some LINQ to Object data to play with. DAMNED sql is just not easy enough 
///to learn quickly, I'd have to sit through a considerable amount of damned tutorials to learn 
///how to import and play with data from sql. Ugh! 
///</summary>
public class JEmp 
{
	public int Sal{get; set;}
	public int Exp{get; set;}
	public string Name{get; set;}
	public string LastName{get; set;}
	public static List<JEmp> employeesList, employeesList2;
	public Gender JEmpGender; 
	public JEmp()
	{
	}
	
	public static List<JEmp> empList
	{
		get
		{
		//Just adding a bunch of data to a list to have some data to play with ^_^ 
			#region
			employeesList =  new List<JEmp>();
			employeesList.Add(new JEmp {Exp = 1433, Sal = 4341, Name = "Jessica", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp {Exp = 5020, Sal = 709, Name = "Joe", JEmpGender = Gender.Male});
			employeesList.Add(new JEmp {Exp = 3902, Sal = 6510, Name = "Jane", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp {Exp = 5433, Sal = 9000, Name = "Jeanette", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp {Exp = 9000, Sal = 19000, Name = "Jeff", JEmpGender = Gender.Male}); 
			employeesList.Add(new JEmp {Exp = 5500, Sal = 5000, Name = "Katie", JEmpGender = Gender.Female}); 
			employeesList.Add(new JEmp(){Sal = 1400, Exp = 2090, Name = "Jenny", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp(){Sal = 2300, Exp = 3519, Name = "Michelle", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp(){Sal = 4000, Exp = 4007, Name = "Margret", JEmpGender = Gender.Unknown});
			employeesList2 = new List<JEmp>
			{
				new JEmp{Exp = 1520, Sal = 1500, Name = "Jake", JEmpGender = Gender.Male},//1
				new JEmp{Exp = 4533, Sal = 2700, Name = "Robert", JEmpGender = Gender.Male},//2
				new JEmp{Exp = 3560, Sal = 3600, Name = "Laura", JEmpGender = Gender.Female},//3
				new JEmp{Exp = 5675, Sal = 4000, Name = "Mirriam", JEmpGender = Gender.Female},//4
				new JEmp{Exp = 8900, Sal = 8500, Name = "Cynthia", JEmpGender = Gender.Female},//5
				new JEmp{Exp = 1090, Sal = 1000, Name = "Eddie", JEmpGender = Gender.Male},//6
				new JEmp{Exp = 600, Sal = 500, Name = "Stephanie", JEmpGender = Gender.Female},//7
				new JEmp{Exp = 7143, Sal = 12000, Name = "Micheal", JEmpGender = Gender.Male},//8
				new JEmp{Exp = 9143, Sal = 25000, Name = "Jennifer", JEmpGender = Gender.Female},//9
				new JEmp{Exp = 4563, Sal = 2500, Name = "Michelle", JEmpGender = Gender.Female},//10
				new JEmp{Exp = 1243, Sal = 900, Name = "Sarah", JEmpGender = Gender.Female},//11
				new JEmp{Exp = 7733, Sal = 8000, Name = "Susana", JEmpGender = Gender.Female},//12
				new JEmp{Exp = 5133, Sal = 7000, Name = "Jamie", JEmpGender = Gender.Unknown},//13
				new JEmp{Exp = 3300, Sal = 4900, Name = "Joe", JEmpGender = Gender.Male},//14
				new JEmp{Exp = 9500, Sal = 15000, Name = "Lorena", JEmpGender = Gender.Female},//15
			};
			employeesList.AddRange(employeesList2); 
			#endregion
			return employeesList;
		}
	}
	
	public override string ToString()
	{
		return this.Name + " " + this.LastName + " is this object's name";  
	}
}//END OF "public class JEmp"