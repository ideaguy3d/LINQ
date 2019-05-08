<Query Kind="Expression">
  <Namespace>System</Namespace>
  <Namespace>UnityEngine</Namespace>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>


public enum Gender {Male, Female, Unknown};

namespace Kudven
{
	//    <<< THE MAIN CLASS >>>
	public class JPracOne : MonoBehaviour 
	{
		public Sprite sr1;
		public Sprite sr2;
		public Transform t;
		string s1 = "Hello", s2 = "Hello";
		int i1 = 2;
		StringBuilder sb1 = new StringBuilder("hello");
		JPrivateStruct<Transform> jStruct = new JPrivateStruct<Transform>(45);
		SomeClass<Sprite> sc1 = new SomeClass<Sprite>(); 
		JSprites jSpriteIndex; // this is a struct!!
		JCompany jcom; 

		//this is equivelent to void Main() 
		void Start()
		{
			jcom = new JCompany();
			sc1[0] = sr1; 
			jStruct[0] = t;
			string s = SomeClass<Sprite>.AreEqualsClassType(sr1, sr2) ? "class: TRUE \n" : "class: FALSE \n";
			string s3 = SomeClass<int>.AreEqualsMethodType<string>(s1, s2) ? "method: True " : "method: False ";
			s3 += " I did += )";//rem this will create a new object being pointed to 
//			jSpriteIndex[0] = sr1;
//			jSpriteIndex[1] = sr2;
			sb1.Append(" world, ");
			sb1.Append( " I love ");
			sb1.Append( " life therefore ");
			sb1.Append(" I love God ^_^");

//			print("jcom[1433] = " + jcom[1433]);
//			print ("jcom[5020] = " + jcom[5020]);
//			print ("Now we will change 1433 and 5020");
			jcom[1433] = "1433 changed";
			jcom[5020] = "5020 changed";
//			print("Now jcom[1433] = " + jcom[1433]);
//			print ("Now jcom[5020] = " + jcom[5020]);
			//print ("----------------------------------------------------------------------------------------------------------");
//			print ("jcom[Gender.Male], Total Male employee's = " + jcom[Gender.Male]);
//			print ( "jcom[\"Jane\"] = " + jcom["Jane"]);
			jcom["Jane"] = "Jessy";
			//print("Jane now " + jcom["Jessy"]);
			//jcom[Gender.Male] = Gender.Female;
			//print ("NOW jcom[Gender.Male] UPDATED, Total Male employee's = " + jcom[Gender.Male]);
			int count = 0;
		
//
//			print (jIndex["Interesting"].ToString + " " + jIndex["Umm"].ToString());

//			print ("JPrivateClass jStruct[0] = " + jStruct[0] + ", the end.");
//			print(s + s3);
//			print (sb1 + ", sc1[0].name = "  + sc1[0].name + ", sc1[0].ToString() = " + sc1[0].ToString() + ", the end.");
		

			// ----------Interface Prac------------ from PartialOne.cs file
			PartialOne parOne1 = new PartialOne(); //here we are practicing Interfaces that I defined in PartialOne.cs
			parOne1.IJPrint2(); // I commented out the Print() so this won't do anything now. 

			IJCustomer inter2 = new PartialOne(); //an interface ref var pointing to an instance of a class that implements it
			inter2.IJPrint(); // I commented out the Print() so this won't do anything now. 

		}//END OF " void Start() " 

	}//	END OF "  class JPracOne : MonoBehaviour  "
	

	public class JCompany
	{
		private List<JEmp> employeesList;
		string s;
		public JCompany()
		{
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
		}

		//nothing but a property
		public List<JEmp> JEmpList{ get {return employeesList;} }

		//here is my practice for indexers
		public string this[int empExp]
		{
			get 
			{
				if(empExp < 4000)
				{
					var empname = from temp in employeesList where temp.Exp == empExp select temp;
					foreach(var temp in empname){s = temp.Name; break;}
					return s + " \nusing the 'from, in, where, select' LINQ keywords. ";
				}
				else
				{
					return employeesList.FirstOrDefault(temp => temp.Exp == empExp).Name
						+ "\nusing LINQ method notation"; 
				}
			}
			set
			{
				employeesList.FirstOrDefault(temp => temp.Exp == empExp).Name = value;
			}
		}

		public string this[Gender gender]
		{
			get
			{
				if(gender != Gender.Unknown)
				{
					return employeesList.Count(temp => temp.JEmpGender == gender).ToString() 
						+ " Is the total of this Gender :)";
				}
				else
				{
					return "The gender is Unknown.";
				}
			}
			set
			{
				foreach(JEmp temp in employeesList)
				{
					//if(temp.JEmpGender == gender) temp.JEmpGender = value;
				}
			}
		}

		//try to use some string manipulation and regular expressions to put in get and set scopes for practice
		public string this[string name]
		{
			get
			{
				return "The experiance of " + name + " is " + employeesList.FirstOrDefault(temp => temp.Name == name).Exp;
			}
			set
			{
				foreach(JEmp temp in employeesList)
				{
					if(temp.Name == name) temp.Name = value; 
				}
			}
		}

	}// END OF public class JCompany

	public struct JSprites
	{
		Sprite[] asprite; 

		public Sprite this[int s]
		{
			get
			{
				return asprite[s];
			}
			set
			{
				asprite[s] = value;
			}
		}
	}// END OF " public struct JSprites "

	//Two prac Generic classes
	#region
	public class SomeClass <T>
	{
		public static bool AreEqualsClassType(T x, T y)
		{
			return x.Equals(y);
		}

		public static bool AreEqualsMethodType<A>(A x, A y)
		{
			return x.Equals(y);
		}

		private T[] ind = new T[100];

		public T this[int i]
		{
			get { return ind[i]; }
			set { ind[i] = value;}
		}
	}//end of public class SomeClass <T>

	class JPrivateStruct<T>
	{
		int memSize; 
		T[] jps;

		public JPrivateStruct()
		{
			jps = new T[100];
		}

		public JPrivateStruct(int x)
		{
			memSize = x;
			jps = new T[memSize];
		}

		public T this[int i] //rem this indexer can be of pretty much any type, not just int
		{
			get
			{ 
				return jps[i];
			}
			set{ jps[i] = value; }
		}
	}//end of class JPrivateStruct<T>
	#endregion
}
