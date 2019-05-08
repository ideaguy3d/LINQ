<Query Kind="Program" />

void Main()
{
//	JExtentions.JTesting();
//	JExtentions.JTesting2();
//	JAssemblyLine(); 
	CompileAsQueryPrac(); 
	
	#region none of this works, rewatch vid
	//using the ext.method .Pair() I defined following along j.kins tut 
	List<JEmp> jList = JEmp.EmpList; 
	IEnumerable<string> strIEnum = jList.Select(n => n.Name); 
	IEnumerable<string> myPairs = strIEnum.Pair(); 
	myPairs.Dump("my .Pair() ext.method");
	foreach(var e in strIEnum.Pair())
	{
		Console.WriteLine (e);
		Console.WriteLine (e.ToString());
		Console.WriteLine ("type = "+e.GetType());
		Console.WriteLine ("length = "+e.Length);
	}
	#endregion 
}

List<JEmp> empList = JEmp.EmpList;
//THIS IS an expression. IT WILL WORK. IT IS A PUBLIC FIELD ((((((= SO CAN BE USED IN ANY FUNC
public Expression<Func<JEmp, bool>> IsPaidAlot = 
		emp => (emp.JEmpGender != Gender.Female) && (emp.Sal > 7000);

//this function will work if I use form 'ob.IsPaidAlotFunc().Compile()  c:, so using method 
//I have to use () as a opposed to the field version which does not need () 
public static Expression<Func<JEmp, bool>> IsPaidAlotFunc()
{
	return emp => (emp.JEmpGender != Gender.Female) && (emp.Sal > 7000); 
}
public static Expression<Func<JEmp, bool>> IsPaidAlotFunc(int x)
{
	x+=450;
	return emp => (emp.JEmpGender == Gender.Female) && (emp.Sal > x+7000); 
}

public static Expression<Func<JEmp, JEmp>> IsPaidAlotStrName()
{
	return emp => JTry(emp); //to modify a property w/out having to use an anoynmous type. It takes a few steps
}

public static JEmp JTry(JEmp itry){
	itry.Name = "TOP EMP:"+itry.Name;
	return itry;
}
//

public IEnumerable<JEmp> ReturnQueSyn(IEnumerable<JEmp> source)
{
	return from emp in source
			where emp.JEmpGender == Gender.Female && emp.Sal > 7000
			orderby emp.Sal descending 
			select emp; 
}

public void CompileAsQueryPrac()
{
	IEnumerable<JEmp> query = 
	empList.Where(IsPaidAlot.Compile()); 
//	query.Dump("expression from public field"); 
	
	IEnumerable<JEmp> query2 = 
	empList.Where(IsPaidAlotFunc().Compile()); 
//	query2.Dump("expression From a function");
	
	// using an expression type
	IEnumerable<JEmp> alphaQuery = 
	empList.Where(IsPaidAlotFunc(45).Compile()).Select(IsPaidAlotStrName().Compile());//The query in 1 go.
	//thanks to "JTry()" I am able to transform each range and keep the original type and its' properties. 
//	alphaQuery = alphaQuery.Select(IsPaidAlotStrName().Compile());//if I wanted to split up I could do this
	alphaQuery.Dump("Nov 22nd prac");
	IEnumerable<JEmp> betaQuery = 
	empList.Where(n => (n.JEmpGender == Gender.Male) && (n.Sal > 7000))
	.Select(n => new JEmp(){EmpID = n.EmpID, JEmpGender = n.JEmpGender, Name = "LAMBDA+"+n.Name});
	betaQuery.Dump("JEmp() type"); 
	
	IEnumerable<JEmp> query3 = 
	empList.Where(emp => (emp.JEmpGender!=Gender.Female) && (emp.Sal>7000) ); 
//	query3.Dump(".Where() with && and NO expression"); 
	
	// returns a query in que.syn
	IEnumerable<JEmp> query4 = ReturnQueSyn(empList);
//	query4.Dump("Method where it returns a que.syn"); 
}

public void JAssemblyLine()
{
	List<JEmp> jList = JEmp.EmpList;
	 
	var query = 
	jList
	.Where(n => n.Exp < 5500)
	.Where(n => 3000 - SimpleAlgorithm() < n.Exp)
	.Select(n => n.Exp * 3).Where(n => n % 2 == 0)
	.Select(n => new {Exp = n, ExpToString = n + " Julius"});
	query.Dump(); 
	
	var query2 = 
	empList
	.Where(n => n.Exp < 5500)
	.Where(n => 3000 - SimpleAlgorithm() < n.Exp)
	.Select(n => new {Exp = n.Exp * 3}).Where(q => q.Exp % 2 == 0).Select(q => "Julius " + q.Exp);
	//query.Dump();
}

public static int SimpleAlgorithm()
{
	int x = 500, y = x/2;
	return (x/y) + 248; 
}

// This is the class that I am going to be putting in heavy, Heavy compiler translation C# LINQ extention code from 
// the deferred execution code vid by J.Kin
public static class JExtentions 
{
	// my version of '.Where()'
	static IEnumerable<T> JWhere<T>(this IEnumerable<T> items, Func<T, bool> gauntlet)
	{
		Console.WriteLine ("JWhere");
		foreach (T item in items)
		{
			if(gauntlet(item))
				yield return item;
		}
	}
	
	// my version of '.Select()'
	static System.Collections.Generic.IEnumerable<R> JSelect<T, R>(this System.Collections.Generic.IEnumerable<T> items, Func<T, R> transform)
	{
		Console.WriteLine ("JSelect");
		foreach (T item in items)
		{
			yield return transform(item);
		}
	}
	
	//Using my defined extention methods 
	public static void JTesting()
	{
		List<JEmp> jList = JEmp.EmpList; 
		
		//jList.Dump("Items in database are " + jList.Count());
		
		var query1 = 
		jList.JWhere(n => n.Exp < 5500).JSelect(n => new {EmpName = "Engineer " + n.Name, NewExperience = n.Exp + 17000000});
		//query1.Dump();
		
		IEnumerator numerator = query1.GetEnumerator();
		while (numerator.MoveNext())
		{
			Console.WriteLine ("Numerator result: " + numerator.Current);
		}
		IEnumerable<int> query2 = jList.JWhere(n => n.Exp>0).JSelect(n => n.Exp + 1700000);//this would be just 1 column with the amount added 
		//query2.Dump(); 
		
		IEnumerator<int> rator = query2.GetEnumerator();
//		while(rator.MoveNext())//this will work, 
//			Console.WriteLine ("New Exp = " + rator.Current);
	}
	
	public static void JTesting2()
	{
		List<JEmp> jList = JEmp.EmpList; 
		
		var query1 = 
		jList.JWhere(n => n.Exp < 5500).JSelect(n => n);//changed JSelect to just 'n'
		//query1.Dump();
		
		IEnumerator numerator = query1.GetEnumerator();
		while (numerator.MoveNext())
		{
			Console.WriteLine ("Numerator result: " + numerator.Current);
		}
		IEnumerable<int> query2 = jList.JWhere(n => n.Exp>0).JSelect(n => n.Exp + 1700000);//this would be just 1 column with the amount added 
		//query2.Dump(); 
		
		IEnumerator<int> rator = query2.GetEnumerator();
//		while(rator.MoveNext())//this will work, 
//			Console.WriteLine ("New Exp = " + rator.Current);
	}
	
	public static IEnumerable<string> Pair(this IEnumerable<string> source)
	{
		string firsthalf = null;
		foreach (var element in source)
		{
			if(firsthalf == null)
				firsthalf = element;
			else 
				yield return firsthalf + element;
				firsthalf = null; 
		}
	}
	
}// END OF "public static class JExtentions" CLASS ENDS 



/*
#region ---------------------------------This Class is just so I can have some data to practice with--------------------------------------------
public enum Gender{Female, Male, Unknown}; 
public class JEmp 
{
	public int Sal{get; set;}
	public int Exp{get; set;}
	public string Name{get; set;}
	public string LastName{get; set;}
	public string Ethnicity{get; set;}
	public string City{get; set;}
	public static List<JEmp> employeesList, employeesList2;
	public Gender JEmpGender; 
	public JEmp()
	{
	}
	
	//this static method will not work :( 
	public static Expression< Func<JEmp, bool> > IsPaidAlot()
	{
		return emp => (emp.JEmpGender != Gender.Female) && (emp.Sal > 7000); 
	}
	
	//THIS VERSION OF DATA TO PLAY WITH WILL HAVE CITY AND ETHNICITY AS WELL
	public static List<JEmp> EmpList
	{
		get
		{
		//Just adding a bunch of data to a list to have some data to play with ^_^ 
			#region
			employeesList =  new List<JEmp>();
			employeesList.Add(new JEmp {Ethnicity = "Asian", City = "Santa Clara", Exp = 1433, Sal = 4341, Name = "Jessica", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp {Ethnicity = "White", City = "San Jose", Exp = 5020, Sal = 709, Name = "Joe", JEmpGender = Gender.Male});
			employeesList.Add(new JEmp {Ethnicity = "White", City = "San Jose", Exp = 3902, Sal = 6510, Name = "Jane", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp {Ethnicity = "Asian", City = "Sunnyvale", Exp = 5433, Sal = 9000, Name = "Jeanette", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp {Ethnicity = "White", City = "Santa Clara",Exp = 9000, Sal = 19000, Name = "Jeff", JEmpGender = Gender.Male}); 
			employeesList.Add(new JEmp {Ethnicity = "Asian", City = "Sunnyvale", Exp = 5500, Sal = 5000, Name = "Katie", JEmpGender = Gender.Female}); 
			employeesList.Add(new JEmp(){Ethnicity = "Hispanic", City = "Santa Clara", Sal = 1400, Exp = 2090, Name = "Jenny", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp(){Ethnicity = "White", City = "Sunnyvale", Sal = 2300, Exp = 3519, Name = "Michelle", JEmpGender = Gender.Female});
			employeesList.Add(new JEmp(){Ethnicity = "Indian", City = "San Jose", Sal = 4000, Exp = 4007, Name = "Margret", JEmpGender = Gender.Unknown});
			
			//new list to insert using object initializer syntax
			employeesList2 = new List<JEmp>
			{
				new JEmp{Ethnicity = "White", City = "San Jose", Exp = 1520, Sal = 1500, Name = "Jake", JEmpGender = Gender.Male},//1
				new JEmp{Ethnicity = "White", City = "Santa Clara",Exp = 4533, Sal = 2700, Name = "Robert", JEmpGender = Gender.Male},//2
				new JEmp{Ethnicity = "Asian", City = "Santa Clara",Exp = 3560, Sal = 3600, Name = "Laura", JEmpGender = Gender.Female},//3
				new JEmp{Ethnicity = "White", City = "Sunnyvale", Exp = 5675, Sal = 4000, Name = "Mirriam", JEmpGender = Gender.Female},//4
				new JEmp{Ethnicity = "Indian", City = "Santa Clara",Exp = 8900, Sal = 8500, Name = "Cynthia", JEmpGender = Gender.Female},//5
				new JEmp{Ethnicity = "White", City = "San Jose", Exp = 1090, Sal = 1000, Name = "Eddie", JEmpGender = Gender.Male},//6
				new JEmp{Ethnicity = "Asian", City = "Santa Clara",Exp = 600, Sal = 500, Name = "Stephanie", JEmpGender = Gender.Female},//7
				new JEmp{Ethnicity = "White", City = "San Jose", Exp = 7143, Sal = 12000, Name = "Micheal", JEmpGender = Gender.Male},//8
				new JEmp{Ethnicity = "White", City = "San Jose", Exp = 9143, Sal = 25000, Name = "Jennifer", JEmpGender = Gender.Female},//9
				new JEmp{Ethnicity = "Indian", City = "Sunnyvale", Exp = 4563, Sal = 2500, Name = "Michelle", JEmpGender = Gender.Female},//10
				new JEmp{Ethnicity = "Indian", City = "San Jose", Exp = 1243, Sal = 900, Name = "Sarah", JEmpGender = Gender.Female},//11
				new JEmp{Ethnicity = "White", City = "San Jose", Exp = 7733, Sal = 8000, Name = "Susana", JEmpGender = Gender.Female},//12
				new JEmp{Ethnicity = "Indian", City = "Santa Clara",Exp = 5133, Sal = 7000, Name = "Jamie", JEmpGender = Gender.Unknown},//13
				new JEmp{Ethnicity = "White", City = "Santa Clara",Exp = 3300, Sal = 4900, Name = "Joe", JEmpGender = Gender.Male},//14
				new JEmp{Ethnicity = "Hispanic", City = "San Jose", Exp = 9500, Sal = 15000, Name = "Lorena", JEmpGender = Gender.Female},//15
				new JEmp{Ethnicity = "Asian", City = "San Jose", Exp = 6500, Sal = 9500, Name = "Lynn", JEmpGender = Gender.Female},
				new JEmp{Ethnicity = "Hispanic", City = "San Jose", Exp = 20000, Sal = 20500, Name = "Julius", JEmpGender = Gender.Male},
				new JEmp{Ethnicity = "Black", City = "Sunnyvale", Exp = 4500, Sal = 3200, Name = "Crystal", JEmpGender = Gender.Female},
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
#endregion
*/