<Query Kind="Program" />

List<JEmp> empList = JEmp.EmpList; 
List<EmpPay> payList = EmpPay.EmpPayList;

void Main()
{
	#region _extra data_
	empList.Add(new JEmp{Name = "JasonEmp", EmpID = 9955, JEmpGender = Gender.Male});
	empList.Add(new JEmp{Name = "NatalieEmp", EmpID = 4433}); 
	payList.Add(new EmpPay{Name = "JasonPay", WeeklyAmount = 900}); 
	payList.Add(new EmpPay{Name = "NataliePay", WeeklyAmount = 1900});
	#endregion 
	//NavagationAttempt();
	//JInto(); 
	//JoinIntoGroup();
	//CompleteCompleteBeginnerPrac();
	//LookUpPrac(); 
	//JReviewOfficial(); 
	//LookUpPrac2(); 
	April15prac(); 
}

void April15prac()
{
	var simpleJoin = empList.Join(payList, e => e.EmpID, p => p.EmpID, (e, p) => new {e.Name, p.WeeklyAmount}); 
	//simpleJoin.Dump("simpleJoin");
	
	var foo = empList.First(n => n.City == "San Jose");
	empList.Dump();
	Console.WriteLine( "foo = "+foo);
}

void JReviewOfficial()
{
	var justJoin = 
	empList.Join(payList, e => e.EmpID, p => p.EmpID, (e, p) => new {e, e.City, e.Ethnicity, p.WeeklyAmount, p.Date});
	
//	justJoin = justJoin.Where(n => n.e.Name == "Jessica").Select(n => n); //this will filter out emp obs who have a property == "Jessica".
	//justJoin.Dump("justJoin.Dump"); 
	
	var payAmount4 = 
	from e in empList.Where(n => n.JEmpGender == Gender.Male) 
	join p in payList.Where(n => n.WeeklyAmount < 1000) 
	on e.EmpID equals p.EmpID into empPayGroup
	where empPayGroup.Any()
	select empPayGroup; 
	
//	foreach(IGrouping<int, EmpPay> gr in payAmount4)
//	{
//		foreach(EmpPay emp in gr)
//		{
//			if(emp.Date == "Pay Week: 7") 
//				Console.WriteLine ("Name = {0} , Pay Date = {1}", emp.Name, emp.Date);
//		}
//	}
	
	payAmount4.Dump("payAmount4");
	
	var payAmount3 =
	empList.GroupJoin(payList, e => e.EmpID, p => p.EmpID, (e, p) =>//ternary op so that if p.Any() is false can be set to null, otherwise get ob 
				new {EmployeeRV = e, Payments = p, PaymentsRV = p.Any() ? p.FirstOrDefault(r => r.Date == "Pay Week: 12") : null })
	.Where(tp => tp.PaymentsRV != null)//if ob from element op is null take it out
	.Where(tp => tp.PaymentsRV.WeeklyAmount < 1000) //get obs who have weekly amount less than 1000
	.Select(tp => new {tp.EmployeeRV, tp.PaymentsRV, TheAmountOfTotalPayments = tp.Payments.Count() }) //this is equivlalent to 'let' keyword in q.syn
	.OrderByDescending(tp2 => tp2.PaymentsRV)//I manually implemented IComparable<T>, exactly same as from unity tut on lists pretty much
	.Select(tp2 => new {tp2.EmployeeRV.Name,  tp2.EmployeeRV.JEmpGender, tp2.PaymentsRV,
					WeeklyAmount = tp2.PaymentsRV == null ? 0 : tp2.PaymentsRV.WeeklyAmount, tp2.TheAmountOfTotalPayments}); 
	
	//payAmount3.Dump(" payAmount3 Total = " + payAmount3.Count()); 
	
	var payAmount =
	empList.GroupJoin(payList, e => e.EmpID, p => p.EmpID, (e, p) => new {EmployeeRV = e, 
						PaymentsRV = p.FirstOrDefault(r => r.Date == "Pay Week: 12")})
	.Select(tp => new {tp.EmployeeRV, tp.PaymentsRV/*, TheAmount = tp.PaymentsRV.Count*/ }) //this is equivlalent to 'let' keyword in q.syn
	.OrderByDescending(tp2 => tp2.PaymentsRV)
	.Select(tp2 => new {tp2.EmployeeRV.Name,  tp2.EmployeeRV.JEmpGender, tp2.PaymentsRV.WeeklyAmount/*, tp2.TheAmount*/}); 

	//payAmount.Dump("payAmount.Dump()"); 
	
	var payAmount2 = 
	from e in empList
	join p in payList on e.EmpID equals p.EmpID into GroupJoin
	let temp = GroupJoin.Count()
	select new {e.Name, e.JEmpGender, AmountOfPayments = temp}; 
	
	//payAmount2.Dump("payAmount2.Dump()"); 
	
	//practicing how to get the lowest paid emp use .Min() aggr. 
	int lowestPay = empList.Where(n => n.Sal > 0).Min(n => n.Sal); //this will get lowest paid that are above 0
	JEmp lowPaidEmp = empList.Where(n => n.Sal == lowestPay).Select(n => n).FirstOrDefault();
	List<JEmp> lowestPaidEmps = empList.Where(n => n.Sal == empList.Min(n2 => n.Sal)).Select(n => n).ToList();
	Console.WriteLine ("Lowest paid employee is " + lowestPaidEmps[1].Sal + ", " + lowestPaidEmps[1].Name);
	
}//END OF " void JReviewOfficial() "

public void LookUpPrac2()
{
	ILookup<string, JEmp> empLookup1 = empList.ToLookup(n => n.City, n => n); 
	//empLookup1.Dump(); 
	
	ILookup<int, JEmp> empLookupID = empList.ToLookup(n => n.EmpID, n => n); 
	
	ILookup<int, EmpPay> payLookup1 = payList.ToLookup(n => n.EmpID, n => n); 
	//payLookup1.Dump(); 
	
	//prac ILookup for a select many
	var query1 =
	from e in empList
	from p in payLookup1[e.EmpID]
	orderby p.WeeklyAmount descending 
	select new {e.Name, e.City, p.WeeklyAmount}; 
	//query1.Dump(); 
	
	//the Payments column will have an inner table w/this
	var query2 = 
	from  e in empList
	select new
	{
		e.Name,
		Payments = payLookup1[e.EmpID]
	};
	query2.Dump(); 
	
}

//from joining with lookups sub sec in book 
public void LookUpPrac()
{
	//created ILookup, now using simple iteration to retrieve data
	ILookup<int, EmpPay> PayLookUp = payList.ToLookup(p => p.EmpID, p => p); 
	foreach (EmpPay pay in PayLookUp[empList[12].EmpID])
		Console.WriteLine (pay.Name + ": " + pay.Date);
	var MirriamPayInfo = PayLookUp[empList[12].EmpID];
	//MirriamPayInfo.Dump("Count " + MirriamPayInfo.Count()); 
	
	Console.WriteLine ();
	
	string city = "San Jose"; 
	ILookup<int, JEmp> EmpLookUp = empList.ToLookup(e => e.EmpID, e => e);
	ILookup<string, JEmp> EmpCityLookUp = empList.ToLookup(e => e.City, e => e);
//	foreach (JEmp emp in EmpCityLookUp[city])
//		Console.WriteLine (city + ": " + emp.Name + ": " + emp.Ethnicity);
	
	
	//just having fun getting the count of this group. 
	IEnumerable<EmpPay> firstEmpInPayList = PayLookUp[empList[0].EmpID];
	int emp1count = firstEmpInPayList.Count(); //firstEmpInPayList.Dump("There are " + emp1count); 
	
	//
	#region a Select/SelectMany (which are equivilent to Join/GroupJoin) using ILookup, I'll never use... 
	var query = 
	from e in empList
	from p in PayLookUp[e.EmpID] // the index is what joins them 
	select new {e.Name, e.JEmpGender, p.Date}; 
	//query.Dump("Using ILookup"); 
	
	// using DefaultIfEmpty() operator makes this an outer join. 
	var queryOuterJoin =
	from e in empList
	from p in PayLookUp[e.EmpID].DefaultIfEmpty() // NOW its an outer join 
	select new 
	{
		e.JEmpGender, e.Name,
		PayName = (p == null) ? null : "Pay " + p.Name , 
		WeeklyPay = p == null ? (decimal?) null : p.WeeklyAmount//I'm not really sure of this
	};
	//queryOuterJoin.Dump("queryOuterJoin"); 
	#endregion
	
	// GroupJoin is equivilent to reading the Lookup inside a projection
	var queryGroupJoin = 
	from e in empList
	select new {e.Name, EmpPayments = PayLookUp[e.EmpID]/*.Count()*/};//the index is inside the projection.
											//could attach .Count() and any other valid op after index
	//queryGroupJoin.Dump("GroupJoin using Lookup"); 
	
	// practicing the Zip op
	int[] nums = {1,2,3};
	string[] numWords = {"one", "two", "three", "ehh"};
	IEnumerable<string> myZipPrac = nums.Zip(numWords, (n, s) => n + " = " + s);
	myZipPrac.Dump();
	
	//prac indexing an element
	IEnumerable<string> names = empList.Select(en => en.Name);
	var namesGrouped = 
	from n in names
	group n.ToUpper() by new {Letter = n[0], LengthOfName = n.Length}; //here we index a range variable 
	namesGrouped.Dump("Indexed range var"); 
	
	var namesGrouped2 = 
	from n in names 
	group n by n[0] into g //another prac w/ an Index 
	let sum = g.Count()
	orderby sum ascending 
	select new {Letter = g.Key, Count = sum}; 
	namesGrouped2.Dump("J and Count"); 
	//
	
	//
}// END OF "public void LookUpPrac()" 

// from j.kin's tuts, I will practice Join into group in this Method and some vid from part 30 onward(to maybe the end (: 
public void JoinIntoGroup()
{	//datasources (: 
	List<JEmp> empList = JEmp.EmpList; 
	List<EmpPay> payList = EmpPay.EmpPayList;
	
	#region some 'Join' prac
	var query = //query syntax
	from e in empList
	join p in payList
		on e.EmpID equals p.EmpID into empPayments //this is what makes joining into groups possible
	let sum = empPayments.Count()
	orderby sum descending 
	select new {JEmpName = "JEmp Name: " + e.Name, e.EmpID, TotalPayments = sum, e.City, e.Exp };
	//query.Dump(); 
	
	var query2 = //
	empList.GroupJoin(payList, e => e.EmpID, p => p.EmpID, (e, p) => new {e, EmpPayments = p})
	.Select(tp => new {tp, NumberOfPayments = tp.EmpPayments.Count()}) 
	.OrderByDescending(tp2 => tp2.NumberOfPayments)
	.Select(tp2 => new {Name = tp2.tp.e.Name,  tp2.NumberOfPayments});
	//query2.Dump("Query2 Fluent Syntax");
	#endregion

	var query3 = 
	from e in empList
	join p in payList
	on e.EmpID equals p.EmpID into EmpPayments
	let AmountOfPayments = EmpPayments.Count()
	orderby AmountOfPayments descending
	select new {e.Name, e.EmpID, AmountOfPayments}; 
	//query3.Dump("Query3 que.syn second prac.");
	
	
	var query4 = 
	empList.GroupJoin(payList, emp => emp.EmpID, pay => pay.EmpID, (emp, pay) => new {emp, EmpPayments = pay})
	.Select(tp => new {tp.emp, AmountOfEmpPayments = tp.EmpPayments.Count()})
	.OrderByDescending(tp2 => tp2.AmountOfEmpPayments)
	.Select(tp2 => new {tp2.emp.Name, tp2.emp.JEmpGender, tp2.AmountOfEmpPayments}); 
	//query4.Dump("Query4, 2nd flue.syn prac"); 
	
	
	//just some GroupJoin prac from sec.'s "GroupJoin" and "Flat Outer Join" from Book
	var mygroupjoin = 
	from e in empList
	join p in payList 
		on e.EmpID equals p.EmpID into EmpPayments
	select new {e.Name, EmpPayments}; 
	//mygroupjoin.Dump("mygroupjoin"); 
	//int[] sal = mygroupjoin.e
	foreach (var paymentSequence in mygroupjoin)
	{
		Console.WriteLine (paymentSequence.Name + ": " + 
							paymentSequence.EmpPayments.Count() + " payments" );
		
//		for(int i=0; i<paymentSequence.EmpPayments.Count(); i++)
//			Console.WriteLine (i+": Hello World");
	}

	var query5 =
	from e in empList
	join p in payList 
		on e.EmpID equals p.EmpID into EmpPayments
	select new {e.Name, e.Exp, AmountOfPayments = EmpPayments.Count()}; 
	//query5.Dump("query5 "); 

}// END OF "public void JoinIntoGroup()"

//here I will start practicing J.kin's vids 26 onward, navigation properties, join into groups, 
//join select and group, etc. 
public void NavagationAttempt()
{
	List<JEmp> empList = JEmp.EmpList; 
	List<EmpPay> payList = EmpPay.EmpPayList;
	
	//This is my Navigation properties attempt and it didn't work. 
//	var jessica = Customers.First(); 
//	Console.WriteLine (jessica.Name);
//	foreach (EmpPay p in jessica.EmpPay)
//	{
//		Console.WriteLine ("\t" + p.Date);		
//	}


	//practicing Join clauses translations with other clauses besides select
	var myJoinOrderby =
	from e in empList
	join p in payList
		on e.EmpID equals p.EmpID
	orderby e.Exp
	select new {Name = e.Name, Experience = e.Exp, PayDate = p.Date}; //here we could have just put e , c which would give entire table/class
	myJoinOrderby.Dump("aaaaaaaw, I do understand");
	/*//To use a forloop instead REM: 
	foreach(var pair in myJoinOrderby)//rem you could add a .Take(10) or other similar static methods in the foreach parameter collection
	{
		int i = 0;//just so I don't have to actually print to console
		Console.WriteLine (pair);
	}*/
	//to use fluent syntax we would have to use a 1 deep transparent identifier. 
	
	
	
	//now going to practice Join and Group
	var myJoinGroup = 
	from e in empList
	join p in payList
		on e.EmpID equals p.EmpID
	group e by p.Date into g//THIS IS BIG!!! Here what we do by saying 'p' alone we are using its RAM Address!!! ...
	//but I changed it to .Date because that is what will get grouped, all similar dates will combine yielding similar results
	//to Jamie. W/ 'p' alone each element in pay list was getting printed out and count was 1 for each. 
	let numOrder = g.Count()
	orderby numOrder descending 
	select new {JuliusKey = g.Key, JNumOrders = numOrder}; //rem Key is 'p' from our above group by 
	foreach(var pair in myJoinGroup)
	{
		int i = 0; i++; 
		//Console.WriteLine (pair.Key + "= " + pair.JNumOrders + " employee's got paid");
	}
	//myJoinGroup.Dump();
	
	//here is my 2nd attempt at this rather complex Join and Group Query 
	var que2 = 
	from e in empList
	join p in payList
		on e.EmpID equals p.EmpID
	group p by e into g
	let sum = g.Count()
	orderby sum descending 
	select new {JEmpName = g.Key, AmountOfPaymentsMade = sum };
	//que2.Dump(); 
	
	
}

// Here I will be coding the flue.syn of 'into' keyword from j.kin's 'C# LINQ - into translation' vid
public void JInto()
{
	List<JEmp> empList = JEmp.EmpList;
	List<EmpPay> payList = EmpPay.EmpPayList;
	
	//using the simpler empList
	#region 
	var que1 = 
	from n in empList
	group n by n.City into g
	let sum = g.Count()
	orderby sum descending
	select new {City = g.Key, EmpCountInCity = sum}; 
	//que1.Dump();
	#endregion
	
	//now I'll be using payList
	var payque1 = 
	from n in payList
	group n by n.EmpID into g
	let mysum = g.Count() 
	let some = payList.Intersect(g)
	orderby mysum descending 
	select new { g.Key, mysum, some};
	
	//payque1.Dump("\\^_^/");
	//payList.Dump(); 
	
	var query2 =
	from g in //g is our range variable , the bottom 2 lines is our data source. 
		from n in payList
		group new {n.Date, EmpId = n.EmpID + 20000} by n.Name
	let thetotal = g.Count()
	orderby thetotal descending
	select new {thetotal, g.Key}; 
	//query2.Dump("Oh yeeaaahh!");
	
	//Now I will be writing the equivalent to the above in flue.syn 
	var query3 =
	payList.GroupBy(n => n.Date)
	.Select(g => new{g, mysum = g.Count()})
	.OrderByDescending(tp => tp.mysum)
	.Select(tp => new {MyEmpDate = tp.g.Key, EmpInThatCity = tp.mysum, First = tp.g.Take(2)});  
	//query3.Dump("query3"); 
}


public void CompleteCompleteBeginnerPrac()
{
	IEnumerable<JGenderAmount> query2 =	
	from n in empList
	group n by n.JEmpGender into g
	let JSum = g.Count()
	select new JGenderAmount{Gender = g.Key.ToString(), GenderAmount = JSum};
	//query2.Dump("Query2");
	
	IEnumerable<EmpPay> query = 
	payList.Where<EmpPay>(n => n.WeeklyAmount < 500);
	//query.Dump("SUPER simple where just for fun"); 
	
	int genderf = 
	(from n in empList where n.JEmpGender == Gender.Female select n).Count(); 
	//Console.WriteLine ("Amount of females = " + genderf);
	int lessThan500 = payList.Where(n => n.WeeklyAmount < 500).Count(); 
	//Console.WriteLine ("<500 amount: " + lessThan500);
	
	string[] justNames = 
	empList.Select(n => n.Name).ToArray(); 
	
	//practicing wrapping here 
	 var noVowels = 
	 from n1 in 
	 (
	 	from n2 in justNames
		select n2.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
	 )
	 where n1.Length > 4 orderby n1 descending select new { JEmpNewName = n1, OldName = OldName(n1)};
	 //noVowels.Dump(); 
	
	var noVowels2 = justNames
	.Select(n2 => n2.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", ""))
	.Where(n => n.Length > 3)
	.OrderBy(n => n);
	//noVowels2.Dump("noVowels2"); 
	
	string OtherFemaleGenderQuery = query2.Select(n => n.Gender).First();
	
	var noVowels3 = 
	from n2 in justNames
	let vowels = n2.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
	where vowels.Length > 4 
	select new {OldName = n2, NewName = vowels, Other = 
	(OldName(vowels) == "Jennifer") ?  OtherFemaleGenderQuery : Gender.Unknown.ToString()};
	//noVowels3.Dump("noVowels3"); 
	
	int FemaleCount = empList.Where(n => n.JEmpGender == Gender.Female).Count();
	int MaleCount = empList.Where(n => n.JEmpGender == Gender.Male).Count(); 
	//Console.WriteLine ("Gender Counts: " + FemaleCount + " " + MaleCount);
	
	var aquery1 = 
	from e in empList
//	where e.JEmpGender == Gender.Female
//	where e.JEmpGender == Gender.Male
//	let aFemaleCount = empList.Where(n => n.JEmpGender == Gender.Female).Count()
//	let aMaleCount = empList.Where(n => n.JEmpGender == Gender.Male).Count() 
	//let someProjection = empList.Select(n => n.Exp)
	//orderby someProjection descending 
	group e by e.Ethnicity into g
	let count = g.Count()
	select new {g.Key, TheCount = count}; 
	//aquery1.Dump("'let' projection attempt"); 
	
	var pair = 
	empList.Select(n => n.Name).JPair();
	//pair.Dump(); 
	
	//just some GroupJoin prac from Book
	IEnumerable<IEnumerable<EmpPay>> mygroupjoin = 
	from e in empList
	join p in payList 
		on e.EmpID equals p.EmpID into EmpPayments
	select EmpPayments; 
	foreach (IEnumerable<EmpPay> paymentSequence  in mygroupjoin)
	{
		foreach (EmpPay payment in paymentSequence)
		{
			Console.WriteLine ("Payment goes to " + payment.Name);
		}
	}
	
}// END OF "public void CompleteCompleteBeginnerPrac()" 

//
#region  some stuff to help me prac w/CompleteCompleteBeginnerPrac() 
//little helper class
public class JGenderAmount
{
	public string Gender {get;set;}
	public int GenderAmount {get; set;}
}

//little helper class
public static class MyExtensions
{
	public static IEnumerable<string> JPair(this IEnumerable<string> source)
	{
		string firstHalf = null; 
		foreach (string element in source)
		{
			if(firstHalf == null)
				firstHalf = element;
			else
			{
				yield return firstHalf+ " & " +element;
				firstHalf = null; 
			}
		}
	}
}
//method I used in LINQ query for fun. 
 public string OldName(string oldname)
 {
 	//Console.WriteLine ("In OldName");
 	if(oldname.StartsWith("J")) return (oldname.IndexOf("n") == 1) ? "Jennifer" : "a name that starts w/'J'"; 
	else return (oldname.StartsWith("M")) ? "Michelle, I think" : "idk..."; 
	
 }
#endregion 


//classes that actually create and contain the databases
#region The "Database" 

//----------------------------------JEmp class-------------------------------------------
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
	public JEmp(){}
	//the below property is how we make navigation properties work w/the entity framework
	public List<EmpPay> Payments {get; set;}//this makes the one to many, many payments to one employee
	
	#region
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
			employeesList.Add(new JEmp(){Ethnicity = "Asian", City = "Santa Clara", Sal = 1400, Exp = 2090, Name = "Jenny", JEmpGender = Gender.Female});
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
				new JEmp{Ethnicity = "Asian", City = "Santa Clara",Exp = 600, Sal = 500, Name = "Lindburg", JEmpGender = Gender.Female},//7
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
				new JEmp{Ethnicity = "Indian", City = "Sunnyvale", Exp = 7200, Sal = 6200, Name = "Prita", JEmpGender = Gender.Female},
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
	#endregion
}//END OF "public class JEmp"


//--------------------EmpPay class-----------------------
public class EmpPay : IComparable<EmpPay>
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
	//the below property makes the navigation property possible, there should only
	public JEmp JEmp{get; set;}//be one JEmp to many EmpPay's 
	
	public int CompareTo(EmpPay TSelf)
	{
		if(TSelf== null) return 1; 
		
		return WeeklyAmount - TSelf.WeeklyAmount; 
	}
	
	#region 
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
		//not at all, so I will have to come back and fix, but for now I am going to get get back to my query studies as this loop gives me 
		//PLENTY of data to play with for now. its 6-18-@6:38pm , just getting to this small point took about 2 or 3 hours... UGH! 
		
		//Console.WriteLine ("listCount now = " + listCount + " and empPayList.Count = " + empPayList.Count);
	}
	
	//this is the property that must be accessed to get the EmpPayList
	public static List<EmpPay> EmpPayList{ get{ FillEmpPay(); return empPayList; } }
	#endregion
}// END OF "public class EmpPay"

#endregion