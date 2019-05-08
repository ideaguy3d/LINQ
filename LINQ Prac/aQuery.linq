<Query Kind="Program">
  <Connection>
    <ID>f2f6e2b4-0f40-48df-b2c7-82584a77c393</ID>
    <Server>.\SQLEXPRESS</Server>
    <AttachFile>true</AttachFile>
    <UserInstance>true</UserInstance>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\Nutshell.mdf</AttachFileName>
  </Connection>
</Query>

List<JEmp> jempList = JEmp.EmpList; 
static string whichGender; 
static Gender genderEnum; 

void Main()
{
	string hello = "Hellow world, this is my sentence."; 
	
	string[] strArrayHello = Regex.Split(hello, @",?\s+"); 
//	strArrayHello.Dump("-"+strArrayHello[2]+"-"); 
	
	JQueryOutput();
	JExtentions.MyFirstPrac(); 
}

//alrighty then , this will be I rewrite all the expression tree stuff... as of now there are 9lines 
//of code... lol that'll sure change aha 11-2-14 @6:03pm

//this will check based on string val
public static Expression<Func<JEmp, bool>> PaidAlotString()
{
//could write more complext algorigthms for whatever reason here (:
	int x = 4, y = 19; 
	//with these I'd have to spell correctly so I should probably use an enum next
	if(whichGender == "female") 
	{
		MyMethod(x);
		return emp => (emp.JEmpGender == Gender.Female) && (emp.Sal > 7000);
	}
	else if(whichGender == "male")
	{
		YourMethod(y);
		return emp => (emp.JEmpGender == Gender.Male) && (emp.Sal > 7000); 
	}
	else return emp => emp.Sal > 7000; 
	
}

//this will check depending on enum val
public static Expression<Func<JEmp, bool>> PaidAlotEnum()
{
//could write more complext algorigthms for whatever reason here (:
	int x = 4, y = 19; 
	//with these I'd have to spell correctly so I should probably use an enum next
	if(genderEnum == Gender.Female) 
	{
		MyMethod(x);
		return emp => (emp.JEmpGender == Gender.Female) && (emp.Sal > 7000);
	}
	else if(genderEnum == Gender.Male)
	{
		YourMethod(y);
		return emp => (emp.JEmpGender == Gender.Male) && (emp.Sal > 7000); 
	}
	else return emp => emp.Sal > 7000; 
	
}

static public void MyMethod(int x)//can always make static so I can access from other files
{
	Console.WriteLine ("hardy har har"+(x+4)+"Im in My method");
}

static public void YourMethod(int y)//can always make static so I can access from other files
{
	Console.WriteLine ("Im in Your method.. hehe "+(y*2));
}

//this will check by way of enum
public static IEnumerable<JEmp> JEmpIEnum(IEnumerable<JEmp> sequence)
{
	int x = 2, y = 19; 
	//with these I'd have to spell correctly so I should probably use an enum next
	if(genderEnum == Gender.Female) 
	{
		MyMethod(x);
		return from e in sequence 
		where e.JEmpGender==Gender.Female && e.Sal>7000
		orderby e.Sal descending select e;
	}
	else if(genderEnum == Gender.Male)
	{
		YourMethod(y);
		return from e in sequence 
		where e.JEmpGender==Gender.Male && e.Sal>7000
		orderby e.Sal descending select e; 
	}
	else return from e in sequence where e.Sal>7000 orderby e.Sal descending select e; 
}

void JQueryOutput()
{
	genderEnum = Gender.Unknown; //whichGender = "female"; 
	//uses an Expression<> method (:
	var query1 =  
	jempList.Where(PaidAlotString().Compile()); 
//	query1.Dump(); 
	var query2 = jempList.Where(PaidAlotEnum().Compile()); 
//	query2.Dump(); 
	
	//here I am using a method that takes an IEnum and returns an IEnum
	IEnumerable<JEmp> query3 = JEmpIEnum(jempList);
//	query3.Dump(); 
}

public static class JExtentions
{
	public static IEnumerable<T> JWhere<T>(this IEnumerable<T> sequence, Func<T, bool> filter)
	{
		foreach(T element in sequence)
		{
			if(filter(element))
				yield return element; 
		}
	}
	
	public static IEnumerable<R> JSelect<T, R>(this IEnumerable<T> sequence, Func<T, R> transform)
	{
		foreach (T element in sequence)
		{
			yield return transform(element); 
		}
	}
	
	public static void MyFirstPrac()
	{
		List<JEmp> emplist2 = JEmp.EmpList; 
		
		var query1 = emplist2.JWhere(ob => ob.Sal > 7000);
//		query1.Dump(); 
		
		//now trying to plug in a method into .JWhere() 
		var query2 = emplist2.JWhere(rangeVar => JFuncWhere(rangeVar));
//		query2.Dump(); 
		
		var query3 = emplist2
		.JWhere(rangeVar => JFuncWhere(rangeVar))
		.JSelect(rangeVar => new {rangeVar.Name, rangeVar.Ethnicity}); 
		query3.Dump(); 
	}
	
	public static bool JFuncWhere(JEmp emp)
	{
		if(emp.Sal > 7000) return true;
		else return false; 
	}
}



//...