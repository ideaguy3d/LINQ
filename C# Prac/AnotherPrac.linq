<Query Kind="Program">
  <Connection>
    <ID>f2f6e2b4-0f40-48df-b2c7-82584a77c393</ID>
    <Server>.\SQLEXPRESS</Server>
    <AttachFile>true</AttachFile>
    <UserInstance>true</UserInstance>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\Nutshell.mdf</AttachFileName>
  </Connection>
</Query>

public delegate string JBaseGenericDelegate<T> (T par); 
List<JEmp> empList = JEmp.EmpList; 
List<EmpPay> payList = EmpPay.EmpPayList;


void Main()
{

	empList.Add(new JEmp{Name = "Jason", EmpID = 9955, JEmpGender = Gender.Male});
	empList.Add(new JEmp{Name = "Natalie", EmpID = 4433, Sal = 20500, City = "Sunnyvale"}); 
	payList.Add(new EmpPay{Name = "Jason P.", WeeklyAmount = 900}); 
	payList.Add(new EmpPay{Name = "Natalie P.", WeeklyAmount = 1900});

	//SomePrac(); 
	JFilteringPrac();
	JAttributeAndReflectionPrac(); 
}

public void JAttributeAndReflectionPrac()
{
	var myReflect =
	from t in Assembly.GetExecutingAssembly().GetTypes()
	where t.GetCustomAttributes(false).Any(a => a is TestAttribute)
	select t; 
	myReflect.Dump(); 
}

public void JFilteringPrac()
{
	//here I am prac where's 2nd optional range variable which is just an index 
	var names15 = 
	empList.Select(n => n.Name)
	.Where((n2, i) => i < 15);//.Select(n2 => n2); //the last .Select is redundent and
	//names15.Dump(); 
	
	// TakeWhile prac
	var takewhileprac = 
	empList.Select(e => new {JName = e.Name, JSalary = e.Sal, JCity = e.City})
	.OrderByDescending(tp => tp.JSalary)
	.TakeWhile(tp => tp.JSalary > 10000)
	.Select(tp => tp); 
	//takewhileprac.Dump("Take While Prac"); 
	
	
	//
}// END OF "public void JFilteringPrac()"  



#region Just some attribute prac stuff

public class TestAttribute : Attribute {}

[TestAttribute]
public class MyTestSuite 
{

}

[TestAttribute]
public class JAttPrac
{
	
}

#endregion 



#region practicing J.Kins ref vs val types and stuff vids & override .ToString()

public void SomePrac()
{
	// I am not sure if this way will
	// work since the delegate is pointing to a static method
	JBaseGenericDelegate<JEmp2> JDerGenDel = new JBaseGenericDelegate<JEmp2>(JBusinessClass.JStringMethod); 
	
	
	// here I am practicing with overriden methods of .ToString() in this files JEmp class 
	JEmp2 emp = new JEmp2(); 
	emp.FirstName = "Julius";
	emp.LastName = "Hernandez-Alvarado";
	//Console.WriteLine(emp.ToString()); //this is the first way to use my overriden method
	Console.WriteLine(Convert.ToString(emp)); //this is the second way to use it
	
	//practicing J.Kin's val vs ref types vid 
	JFraction frac = new JFraction { top = 5, bottom = 7 }; 
}

public class JEmp2 
{
	public int Sal{get; set;}
	public int Exp{get; set;}
	public string FirstName{get; set;}
	public string LastName{get; set;}
	
	public override string ToString()
	{
		return this.FirstName + " " + this.LastName + " is this object's name";  
	}
}

public class JBusinessClass 
{
	public static string JStringMethod(JEmp2 par)
	{
		if(par.Exp > 2000) return "True";
		else return "False"; 
	}
}

public struct JFraction
{
	public int top, bottom; 
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
	// the below property is how we make navigation properties work w/the entity framework
	public List<EmpPay> Payments {get; set;} // this makes the one to many, many payments to one employee
	
	#region
	
		public static void FillList()
		{	// Just adding a bunch of data to a list to have some data to play with ^_^ 
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
				
				// new list to insert using object initializer syntax
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
					new JEmp{Ethnicity = "Indian", City = "Sunnyvale", Exp = 7200, Sal = 6200, Name = "Prita", JEmpGender = Gender.Female},
				};
				employeesList.AddRange(employeesList2); 
			#endregion
		}
		
		// THIS VERSION OF DATA WILL HAVE CITY AND ETHNICITY AS WELL
		public static List<JEmp> EmpList
		{
			get
			{
				// Just adding a bunch of data to a list to have some data to play with ^_^ 
				FillList(); 
				int count = 0; 
				
				// a simple loop to give my elements a Primary key ID
				foreach (JEmp element in employeesList)
				{
					element.EmpID = count + 10000; // adding 10,000 just so it looks better, ID 1,2,3 doesn't look as good as 10001, 10002, etc.
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
	
}// END OF "public class JEmp"


//--------------------EmpPay class-----------------------
public class EmpPay
{
	//Foreign Key
	public int EmpID{get; set;}// this is what makes the 1toMany relationship to the JEmp class list,
	//in vid he was simply using a string type with .Join()
	public string Name{get; set;}
	public string Date{get; set;}//Date will be part of the composite key since there should only be one payment per date 
	//and will be combined with EmpID
	public int WeeklyAmount{get; set;}
	static List<JEmp> jEmpList; 
	static List<EmpPay> empPayList; 
	//the below property makes the navigation property possible, there should only
	public JEmp JEmp{get; set;}//be one JEmp to many EmpPay's 
	
	
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