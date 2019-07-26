<Query Kind="Program" />

void Main()
{	
	Preval2 mstruct = new Preval2(1, "Joe"); 
	Preval2.Nature(9); 
	Console.WriteLine (mstruct.EmpInfo().empStringID);
}


void SimpleIndexerPrac()
{
	Preval name = new Preval(123, "Jefferson");//this is a struct
	Preval2 name2 = new Preval2(123, "Jefferson");//this is a class
	AClass ac = new AClass(); 
	Console.WriteLine(ac[name2]);
	Console.WriteLine(ac["Jeff"]);
	Console.WriteLine (name.str2);
}

public struct Preval
{
	public int num1;
	public string str2;
	
	public Preval(int n1, string str2)
	{
		num1 = n1; this.str2 = str2; 
	}
}

public class Preval2
{
	static string[] terms = {"a Forest", "the Nature", "the Earth", "the World", "a Mountain"};
	Preval mstruct = new Preval(222, "Jess"); 
	Dictionary<string, AClass> AClassDicitonary = new Dictionary<string, AClass>(); 
	AClass mclass = new AClass(); 
	
	public AClass EmpInfo()
	{
		AClassDicitonary.Add(mstruct.str2, mclass); 
		Console.WriteLine ("zzz nig as EmpID = " + AClassDicitonary["Jess"].empStringID);
		return AClassDicitonary[mstruct.str2]; 
	}
	
	public static void Nature(int i)
	{
		ILookup<char, string> NatureLookup = terms.ToLookup(s => s[0], s => s);
		if(i==1){foreach(string str in NatureLookup['a']) Console.WriteLine (str);}
		else if(i==2){foreach(string str in NatureLookup['t']) Console.WriteLine (str);}
		else Console.WriteLine ("Enter '1' or '2' only.");
	}
	public int num1;
	public int num2 = 456;
	//public string str1;
	public string str2;
	
	public Preval2(int n1, string str2)
	{
		num1 = n1; this.str2 = str2; 
	}
}

public class AClass
{
	 
	int empID = 123, empID2; 
	public string empStringID = "Jefferson"; 
	public string this[Preval empID]
	{
		get
		{
			if(empID.num1 == this.empID) return "true";
			else return "false"; 
		}
	}
	
	public string this[string empID]
	{
		get
		{
			if(empID == empStringID) return "true";
			else return "false"; 
		}
	}

	public string this[Preval2 empID]
	{
		get{
			if(empID.num2 == this.empID2) return "true";
			else if(empID.num1 == this.empID) return "other 1 true";
			else return "false"; 
		}
	}	
}