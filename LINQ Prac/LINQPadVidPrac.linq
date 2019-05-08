<Query Kind="Program">
  <Connection>
    <ID>f2f6e2b4-0f40-48df-b2c7-82584a77c393</ID>
    <Server>.\SQLEXPRESS</Server>
    <AttachFile>true</AttachFile>
    <UserInstance>true</UserInstance>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\Nutshell.mdf</AttachFileName>
  </Connection>
</Query>

void Main()
{
	//ProjectionPrac(); 
	//OrderByPrac(); 
	//JLinqWords();

	Customers.Dump();
	Purchases.Dump();
}

void JLinqWords()
{
	List<Words> words = new List<Words>
	{
		new Colloquial(),
		new Loquacious(),
		new Verisimilitude(),
		new Verity(),
		new Nominal(),
		new Ignominious()
	};
	
	string linqOps = "Take, TakeWhile, Where, Skip, SkipWhile, Distinct, Select, SelectMany, " +
	"Join, GroupJoin, Zip, OrderBy, ThenBy, Reverse, GroupBy, Concat, Union, Intercept, Except, " +
	"OfType, Cast, ToList, ToArray, ToDictionary, ToLookup, AsQueryable, AsEnumerable, " +
	"First, FirstOrDefault, Last, LastOrDefault, Single, SingleOrDefault, ElementAt, ElementAtOrDefault, " +
	"DefaultIfEmpty, Aggregate, Average, Count, LongCount, Sum, Max, Min, All, Any, Contains, " +
	"SequenceEqual, Empty, Range, Repeat"; 
	
	string linqPattern = @"\b\w+\b,?\s*";
	
	MatchCollection matches = Regex.Matches(linqOps, linqPattern); 
	
	//excellent Excellent prac of finding ',' then removing it then outputting result. 
	foreach(Match m in matches)
	{
		string temp = Regex.IsMatch(m.Value, ",") ? m.Value.Remove(m.Value.IndexOf(',')) : m.Value; 
		//Console.WriteLine (temp);
	}Console.WriteLine();
		
	Console.WriteLine ("Regex matches = {0}", matches.Count);
}

void SuperSimple()
{
	Purchases.Take (100).Dump();
	
	var query1 =
	from n in Purchases
	where n.Price >= 500 && n.Description.StartsWith("B")
	select n; 
	query1.Dump("My First query of a kind of real Database :)"); 
}

public void ProjectionPrac()
{
	var query2 = 
	from n in Purchases
	select new {n.Description, n.Price} ; //this is the actual projection
	query2.Dump("Just 'projecting'"); 
	
	//using fluent syntax
	var query = 
	Purchases.Select (n => new {n.Description, n.Customer});//Rem we put the 'new' on left side before 
	//the {} which contain the elements we are projecting
	
	//here we are just putting "Enumerable.Select()" rather than "datasource.Select()"
	var query3 = Enumerable.Select(Purchases, n => new {n.Date, n.Customer}); 
	Debug.Print("Hello World");
	//query3.Dump("Here is Query3");
	
	//in this next part we are taking out the lambda syntaxtic sugar. 
	var noLambdaQ = Purchases.Select(NoLambdaMethod);
	noLambdaQ.Dump("This is without a lambda");
	
}

//this is step2 to not having to do a lambda, we will pass this method into .Select() rather than write a lambda
//expresion, we will ONLY pass in the name of this method without any pars. 
public static ForNoLambda NoLambdaMethod(Purchase p)
{
	return new ForNoLambda(p.Description, p.Date);
}

public class ForNoLambda
{
	public string JDescription{get; set;}
	
	public DateTime JDate{get; set;}
	
	public ForNoLambda(string des, DateTime date)
	{
		JDescription = des; 
		JDate = date; 
	}
}

public void OrderByPrac()
{
	var query = 
	from n in Purchases 
	orderby n.Price descending, n.Date descending//rem we have a comma seperating the two, no && or || ops
	select n; 
	
	var fluent = 
	Purchases
	.OrderByDescending(n => n.Price)
	.ThenByDescending(n => n.Date);
	
	fluent.Dump("In OrderBy using fluent"); 
}