<Query Kind="Program" />

public delegate string JBaseGenericDelegate<T> (T par); 

void Main()
{
	Chp8SubQueryPrac(); 
	
	int x = 23413249; 
	string s = x.ToString(); 
	string s2 = s.Insert(1, ","); //I was going to try to do comma formatting but it is just taking wayyy to long
	//maybe I'll finish someother time 
	
	Console.WriteLine("After insertion: " + s2); 
}

// Define other methods and classes here


public void Chp8SubQueryPrac() //sub query practice, also using .Min() 
{
	string[] names = {"Harry", "Marry", "Jay", "Heather", "May", "Hey", "Lay", "Katie", "Mai"};
	
	//Query practice fun 1:) that I minimized to save some space 
	#region
	
	IEnumerable<string> query = from n in names
	where n.Length == (from n2 in names orderby n2.Length select n2.Length).First()
	select n; 
	
	var query2 = names.Where (n => n.Length == names.OrderBy (n2 => n2.Length).Select (n2 => n2.Length).First ()); 
	
	IEnumerable<string> query3 = 
	from n in names
	where n.Length == names.OrderBy((string n2) => n2.Length).First().Length //using fluent syntax combined w/query syntax
	select n; 
	
	
	IEnumerable<string> query4 = 
	from n in names 
	where n.Length == names.Min(n2 => n2.Length)
	select n; 
	
	//Console.WriteLine("this is query4 from console = " + query4); 
	
	//using 2 queries rather than a subquery
	int min = names.Min (n => n.Length);//this query is so that we don't have to subquery 
	IEnumerable<string> query5 = 
	from x in names
	where x.Length == min
	select x;  
	
	#endregion 
	
	//Some more fun w/diff stuff like string insertion and delegates pointing to anonymous methods using lambda expressions
	#region
	
	//here I am practicing w/LINQ's .Count() 
	var mycountQuery = names.Count(n => n.StartsWith("M")); 
	
	//here I will practice having => goto a method body. that a del is pointing to
	JBaseGenericDelegate<int> derDelGen = (int x) => {return "Hello World " + x; }; 
	
	#endregion
	
	//query.Dump("Query Results"); 
	//query.Dump("Query2 Results"); 
	//query3.Dump("Query3 Results");
	//query4.Dump(" using Min aggregation"); 
	//query5.Dump("took out the subquery"); 
	mycountQuery.Dump("Using .Count() " + derDelGen(102684654) ); 

}// END OF "public void Chp8SubQueryPrac()" method 

public string JCommaFormat(int x)
{
	string s = x.ToString();
	int x2 = s.Length; 
	
	if(x2%3 == 0)
	{
		
	}
	else if(x2%3 == 1)
	{
	
	}
	else if(x2%3 == 2)
	{
	
	}
	
	return " "; 
}