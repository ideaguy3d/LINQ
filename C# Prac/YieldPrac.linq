<Query Kind="Program">
  <Connection>
    <ID>f2f6e2b4-0f40-48df-b2c7-82584a77c393</ID>
    <Server>.\SQLEXPRESS</Server>
    <AttachFile>true</AttachFile>
    <UserInstance>true</UserInstance>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\Nutshell.mdf</AttachFileName>
  </Connection>
</Query>

public static Random irand = new Random(); 

void Main()
{
//	JOrderByPrac();
//	MultipleFields(); 
//	JLetClauses(); 
	MyYieldPrac(); 
}


#region ___ALL OF the attempted Yield prac___
public void MyYieldPrac()
{
	foreach (int element in GetRandomNumbers(5))
	{//VERY interesting how we are passing in a method as the 2nd par
		Console.WriteLine ("\t\tabove");
		Console.Write("yield till limit: "+element);
		Console.WriteLine ("\n\t\tbeneath");
	}
	
	Console.WriteLine("\n---------------------------------\n"); 
	
	foreach (int element in RandomCondition())
	{
		Console.WriteLine("yield break: "+element); 
	}
}

//here I am practicing using the 'yield' keyword. 
public IEnumerable<int> GetRandomNumbers(int x)
{
	Console.WriteLine ("<");
	int temp = 0;
	if(temp == 0) temp = x; 
	Console.WriteLine (">");
	for(int i=0; i<x; i++)
	{     
		Console.WriteLine ("i="+i);
		yield return irand.Next();
		Console.WriteLine("\t\t\t\t/"+ temp-- +" temp="+temp+"\n");
	}
}

public IEnumerable<int> RandomCondition()//this is the code from JamieKings vid
{
	while(true)
	{
		int n = irand.Next(); 
		Console.WriteLine ("n="+n);
		if(n % 10 == 0) yield break; 
		yield return n; 
	}
}
#endregion

//in this method I am practicing .Join() , and 'let' keyword, for the quadratic formula, using que.syn(I didn't translate it to flue.syn
//because it is just wayyy too tedious with all the transparent identifiers. 
public void JLetClauses()
{
	//Very Interesting, we are making an anon.array of anon.types. REM to put [] to 
	//indicate that we are making this an anon array
	var inputs = new []
	{
		new { a = 2, b = -7, c = 3},
		new { a = -4, b = -16, c = 8},
		new { a = 7, b = 2, c = 10},
	};
	
	var result = 
	from coef in inputs
	let negB = -coef.b //Let the 'let's begin ^_^ 
	let determinant = coef.b * coef.b - 4 * coef.a * coef.c
	let twoA = 2 * coef.a
	select new 
	{
		Pos = (negB + determinant) / twoA,
		Neg = (negB - determinant) / twoA
	};	
	//the flue.syn of these let clauses would at the end require tp3.tp2.tp1.coef.b , tp=transparent identifier
	//so I'll stick with que.syn. 
	result.Dump("The Quadratic Formula"); 
	
	//HERE I will start practicing the .Join clauses for flue.syn and que.syn
	//...data sources below 
	List<JEmp> empList = JEmp.EmpList; 
	List<EmpPay> payList = EmpPay.EmpPayList; 
	
	var myjoin = //que.syn
	from e in empList
	join p in payList
		on e.EmpID equals p.EmpID
	select new {Name = e.Name, PayWeek = p.Date}; //this is an IEnumerable collection type that has two properties
	
	List<string> payNamePrac = new List<string>(); 
	foreach (var pair in myjoin.Take(100))//We are able to use a foreach, notice we can use .Take() 
	{
		payNamePrac.Add(pair.Name);  //for fun and prac I made a list that stores the first 100 names 
		//Console.WriteLine (pair);
	}
	//payNamePrac.Dump(); //will print first 100 names of anonymous type 'myjoin'
	//myjoin.Dump(); 
	
	//now I will practice .Join() using flue.syn
	var fluentjoin = 
	empList.Join(payList, e => e.EmpID, p => p.EmpID, (e, p) => new {NameFluent = e.Name, Experience = e.Exp, PayDateFluent = p.Date}); 
	fluentjoin.Dump(); 
}// END OF METHOD 

//here I will be practicing J.Kin's Multiple fields vid 
public void MultipleFields()
{
	List<JEmp> jList = JEmp.EmpList; 
	
	var multFields1 =
	from n in jList //if we wanted we can do another 'from in' instead of 'jList'
	group n by new {n.City, n.Ethnicity} into g
	let JSum = g.Count() 
	orderby JSum descending 
	select new { g.Key, Amount = JSum, Employees =g}; 
//	multFields1.Dump("City and Ethinicity"); 
	
	var novPrac =
	from n in jList //if we wanted we can do another 'from in' instead of 'jList'
	group n by new {n.City,n.Ethnicity} into g
	//let JSum = g.Count() 
	//orderby JSum descending
	select g; 
	
//	foreach(var k in novPrac)
//	{
//		int c = k.Count(n=>n.JEmpGender== Gender.Female);
//		string s = c > 1 ? " girls:" : " girl:"; 
//		Console.WriteLine ("In "+k.Key.City+" I have "+c+
//		" " +k.Key.Ethnicity+s);
//		foreach(var v in k)
//		{
//			if(v.JEmpGender== Gender.Female) Console.WriteLine (v.Name);
//		}
//		Console.WriteLine ();
//	}

	var multFields2 =
	from n in jList //if we wanted we can do another 'from in' instead of 'jList'
	orderby n.City, n.Ethnicity//this will now be order first by city, then by ethn
	group n by new {City = n.City, Ethnicity = n.Ethnicity}; 
	//multFields2.Dump();
	
	//here I will write the fluent syntax version of "selecting while grouping" vid
	var myquery = 
	from n in jList
	group new {n.Name, n.Ethnicity} by new { n.City } ; //this is projecting while grouping , n.City will be the key
//	myquery.Dump("group new ... by new ...");
	
	var myquery2 = //with fluent syntax the key goes first, then the projection. 
	jList.GroupBy (n => new {n.City, n.Ethnicity}, n => new {zzzName = "YO YO YO ! "+n.Name, n.JEmpGender}); 
	//myquery2.Dump("myquery2"); 
	foreach (var grouping in myquery2)
	{
		Console.WriteLine (grouping.Key.City + ": ");
		foreach (var element2 in grouping)//Rem! When we do the inner foreach to iterate the element from the outer foreach
		{
			Console.WriteLine ("       Inner Grouping Property: " + element2.zzzName+", "+element2.JEmpGender);
		}
		Console.WriteLine (" ---------------------------------------------- ");
	}
	
}// END OF "public void MultipleFields()"

//LOTS of 'group by' prac in this method
string sp1, sp2, sp3;
StringBuilder sb = new StringBuilder(); 
public void JOrderByPrac()
{
	//Jamie Kings vid 'C# LINQ - group by'
	var mygroup =
	from n in Purchases
	group n by n.Price >= 2000; 
	//mygroup.Dump("Purchase table grouped by n.price"); 
	
	List<JEmp> jempList = JEmp.EmpList; 
	
	//using group by, which will return a type of IGroupable 
	var myGroupbyQuery = 
	from n in jempList
	group new {JName = n.Name, JExperience = n.Exp} by n.JEmpGender; 
	//myGroupbyQuery.Dump("myGroupQuery"); 
	
	var results = 
	jempList.GroupBy (n => n.JEmpGender);//here we are using .GroupBy() in fluent syntax
	
	//myGroupByQuery.Dump("Experience > 5000: ");//LINQPad will show two tables, one for if 
	//false and one for Key = true
	
	//this is how we access our group by,a foreach loop and an inner foreach loop 
	bool printGroupBy = false;  
	if(printGroupBy)
	{
		foreach(var igrouping in myGroupbyQuery)
		{
			Console.WriteLine ("Count of "+igrouping.Count()+" in "+igrouping.Key + " key." );
			var cTemp = igrouping.FirstOrDefault(n => n.JName == "Jane"); //Console.WriteLine (cTemp.JName+" is the name");
			foreach (var c in igrouping)
			{	 
				//cTemp = c.JName == igrouping.FirstOrDefault(n => n.JName == "Jane") ? "Jane was found ! (((: ," : "null"; 
				sp1 = c.JName; 
				
				sp3 = c.JName == "aofj;adf" ? "Umm? What?!?" : "null!!!!!!"; 
				Console.WriteLine ("= " + c.JName + "  " + c.JExperience);
				
				if(cTemp!=null){if(c.JName == cTemp.JName) sb.Append("John was found ! whoo who! Lol");}
			}
			Console.WriteLine ("\n.....sp's = "+sp1+", "+sb.ToString()+", "+sp3);
		}
	}
	
	// Here I am going to be practicing the vid "Counting Elements in groups" 
	var queryOfGroupByQuery =
	from n in myGroupbyQuery//remember n is a group of JEmpGender.Female or JEmpGender.Male or JEmpGender.Unknown
	orderby n.Count() descending 
	from m in n
	where m.JExperience > 5000
	select new {QueryId = n.Key, NumEmps = n.Count(), Name=m.JName, Exp="Greater than 5000"};//by assigning we give new column names 

	//queryOfGroupByQuery.Dump("Using .Dump()"); //much cleaner results than using the foreach loop on previous line of code
	
	// Now I'll be practicing Let clause from J.kin vid
	var countPrac = 
	from g in myGroupbyQuery
	let NumEmps = g.Count()
	orderby NumEmps descending 
	select new {JEmpGender = g.Key , NumEmps}; 
	countPrac.Dump("This is it! ");
	//
	
	// Now practicing querying a group by query all in one query. Beginning 
	//of J.Kin's vid 'Intro to into'
	var theresult = 
	from n in jempList.GroupBy(n => n.JEmpGender)//this is the query in a query, but it is not 
	let tempHolder = n.Count()					//recomended to do this. 
	orderby tempHolder descending 
	select new {JEmpGender = n.Key, tempHolder};  
	
}// END OF "public void JOrderByPrac()" 


//----------------------------------JEmp class-------------------------------------------
#region
public enum Gender{Female, Male, Unknown}; 
public class JEmp 
{
	public int EmpID{get; set;}
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
	
	public static void FillList()
	{	//Just adding a bunch of data to a list to have some data to play with ^_^ 
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
	}
	
	//THIS VERSION OF DATA WILL HAVE CITY AND ETHNICITY AS WELL
	public static List<JEmp> EmpList
	{
		get
		{
				//Just adding a bunch of data to a list to have some data to play with ^_^ 
				FillList(); 
				int count = 0; 
				
				//a simple loop to give my elements a Primary key ID
				foreach (JEmp element in employeesList)
				{
					element.EmpID = count + 10000;//adding 10,000 just so it looks better, ID 1,2,3 doesn't look as good as 10001, 10002, etc.
					count++; 
				}
				return employeesList;
			}
		}
	
	public override string ToString()
	{
		return this.Name + " " + this.LastName + " is this object's name";  
	}
	
}//END OF "public class JEmp"


//--------------------EmpPay class-----------------------
public class EmpPay
{
	//Foreign Key
	public int EmpID{get; set;}//this is what makes the 1toMany relationship to the JEmp class list,
	//in vid he was simply using a string type with .Join()
	
	public string Name{get; set;}
	public string Date{get; set;}//Date will be part of the composite key since there should only be one payment per date 
	//and will be combined with EmpID
	
	public int WeeklyAmount{get; set;}
	static List<JEmp> jEmpList; 
	static List<EmpPay> empPayList; 
	
	
	// so with this method I am filling up my EmpPayList using JEmpList by setting
	//indexes to elements inside the JEmpList
	public static void FillEmpPay()
	{
		empPayList = new List<EmpPay>(); 
		jEmpList = JEmp.EmpList;//this gives us a list of another object class type 
		
		for(int i = 0; i<JEmp.EmpList.Count; i++)//just giving dummy vals here 
			empPayList.Add(new EmpPay{EmpID = 0, Date = "0", WeeklyAmount = 0});
		//Console.WriteLine ("empPayList length = " + empPayList.Count);//was just making sure it filled, it did.
		
		int count = 0; 
		
		//a simple way to fill up an EmpPay list just to have a new data source by setting some of its 
		//properties to trasnformed properties of JEmpList
		foreach (var element in jEmpList)//
		{//THIS IS A VERY IMPORTANT LOOP BECAUSE THIS IS WHERE I FILL THE LIST WITH initial DATA
			empPayList[count].EmpID = element.EmpID;
			empPayList[count].Date = "Pay Week: " + count; 
			empPayList[count].WeeklyAmount = element.Sal / 4;
			empPayList[count].Name = element.Name; 
			count++;
		}
		
		//here  I am trying to increase the size of the data to play with by adding more 
		//EmpID's to empPayList but at the same time making sure they don't have the same Date prop. 
		count = 0; int listCount = empPayList.Count; 
		for(int i=0; i<listCount; i++)
		{
			//this if statement should ensure that Date doesn't get added twice since it is supposed to be 
			//EmpPay's part of the composite key.
			if(empPayList[count].Date == empPayList[i].Date) continue;
			
			empPayList.Add(new EmpPay(){EmpID = jEmpList[count].EmpID, WeeklyAmount = jEmpList[count].Sal / 4, 
			Name = jEmpList[count].Name, Date = empPayList[i].Date} );
			
			if(i == (listCount-1)){//this resets the loop so I can start over. 
				count++;
				i = 0; 
			}
		}//This loop is not full proof, each emp should only get paid once per pay Date but it pays some emps twice on the same date and some 
		//not at all, so I will have to come back and fix, but for now I am going to get get back to my query studies as this loop give me 
		//PLENTY of data to play with for now. its 6-18-@6:38pm , just getting to this small point took about 2 or 3 hours... UGH! 
		
		//Console.WriteLine ("listCount now = " + listCount + " and empPayList.Count = " + empPayList.Count);
	}
	
	//this is the property that must be accessed to get the EmpPayList
	public static List<EmpPay> EmpPayList{ get{ FillEmpPay(); return empPayList; } }
	
}

#endregion 
//--------------------------------