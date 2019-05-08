<Query Kind="Program" />

public delegate void JBaseStrDel(int i, string s);



void Main()
{
	JClass_1 jc1 = new JClass_1(); 
	JClass_2 jc2 = new JClass_2(); 
	jc2.Method1(jc1); 
	jc1.Method1(4);
}


public class JClass_1
{
	JBaseStrDel JDerDel;
	public event JBaseStrDel someEvent; 
	
	public void Method1(int x)
	{
		JDerDel = new JBaseStrDel(SomeMethod); 
		JDerDel += SomeOtherMethod;
		JDerDel += YetAnother;
		
		switch (x)
		{
		case 2:
			JDerDel(x, ", Convert.ToString(x)= "+Convert.ToString(x));//del invokes all methods
			break;
		case 4:
			JDerDel(x, "OMG OMG OMG now I am in case 4 (((((((: geeeezzz! ^_^ "+Convert.ToString(x));
			if(someEvent != null) someEvent(22222, "Because we are in case 4: LET'S DO THIS!"); 
			break;
		case 8:
			JDerDel(x*8, ", Converted the int and multiplied by 8, which'll = "+Convert.ToString(x));
			break;
		default:
			Console.WriteLine ("sum went rong );");
			break;
		}
	}
	
	public void SomeMethod(int i, string s)
	{
		string a = "\n\nin some method\n"+(i/2)+s;
		Console.WriteLine (a);
	}
	
	public void  SomeOtherMethod(int i, string s)
	{
		string a = "\n\nNow I am in some Other method\n"+(i/2)+s;
		Console.WriteLine (a);
	}
	
	public void  YetAnother(int i, string s)
	{
		string a = "\n\nyet another method\n"+(i/2)+s;
		Console.WriteLine (a);
	}
}


public class JClass_2
{
	public void Method1(JClass_1 jc1)
	{
		Console.WriteLine ("\nin method1...");
		jc1.someEvent += Step1; 
	}
	
	public void Step1(int i, string s)
	{
		Console.WriteLine ("\n\nnow in Step 1");
		Step2(s);
		Step3(i); 
	}
	
	public void Step2(string s)
	{
		Console.WriteLine ("Step 2: s = "+s);
	}
	
	public void Step3(int i)
	{
		Console.WriteLine ("Step 3: i = " + i);
	}
}