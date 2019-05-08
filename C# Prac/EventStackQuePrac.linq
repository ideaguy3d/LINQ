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
	SomeClass sc = new SomeClass();
	sc.SomeEventPrac(); 
}

// Define other methods and classes here
public partial class SomeClass : IComparable<SomeClass>
{

	public int tracker{get; set;}
	PartialOne po = new PartialOne(); 
	
	void Main()
	{
		po.cashEvent += HandleCashEvent2;
		SomeEventPrac(); 
		Console.WriteLine ("In start");
	}

	public void SomeEventPrac()//this is like the Main function for this event practice. 
	{//once this method is called we will subscribe to the event in PartialOne
		po.cashEvent += HandleCashEvent;
		po.AddCash(45);
		po.AddCash(65);
	}

	//this is the method we have PartialOne's event point to. 
	public int HandleCashEvent (int xMax)
	{
		Console.WriteLine ("We are in The EVENT YO!!");
		return xMax * 5;
	}
	
	public int HandleCashEvent2 (int xMax)
	{
		return xMax * 25;
	}
	
	public int CompareTo(SomeClass t)
	{
		if(tracker > 0) return 1;
		else if(tracker < 0) return -1;
		else return 0; 
	}
}//END OF "public partial class SomeClass : MonoBehaviour, IComparable<SomeClass>" 

//---------------------------------------------NEW CLASS-------------------------------------------------------

public partial class PartialOne
{
	public delegate int BaseDelPrac(int xMax);
	public event BaseDelPrac cashEvent; 
	  
	private int cash; 
	public int Cash//I am practicing events w/this simple property
	{
		get
		{
			return cash;
		}
		set
		{
			cash = value;
			if(cash > 100)
			{
				cashEvent += SameClassEvent;//make a method that the event points to  
				if(cashEvent != null)
				{//there is more than $100 signal our event. 
					int cashInt = cashEvent(5);
					
					OtherLambdaPrac();
					Console.WriteLine (SameClassEvent(8)+" | Whoa! You're rich! ( = " + cashInt);
				}
			}
		}
	}

	public int SameClassEvent(int x)
	{
		Console.WriteLine ("In the same class !!! (: x * 8 = " + (x*8));
		return x*8; 
	}
	
	//this function will be used to increase cash field. In the property if cash gets to > 100
	//then this will signel our event. 
	public void AddCash(int amount)
	{
		Cash += amount; 
	}

	public void OtherLambdaPrac()
	{
		BaseDelPrac expressionLambdaDel = temp => temp + 15;
		BaseDelPrac statementLambdaDel = (int temp) =>//doing it with a body lets me do anything I would  
		{//regularly do in a method body. 
			for(int i=0; i<5; i++)
			{
				temp += i;
				Console.WriteLine(temp);
			}
			return temp;
		};
		BaseDelPrac statementLambdaDel2 = (int temp) =>
		{
			foreach(int i in Enumerable.Range(1, 5))//same as previous except uses .Range()
			{
				temp += i;
				Console.WriteLine(temp+" "+i);
			}
			return temp; 
		};
		
		//Console.WriteLine ("FROM IN OtherLambdaPrac() METHOD.");
		statementLambdaDel2(5); // using del lambda statement
		Console.WriteLine("expressionLambdaDel(5) = " + expressionLambdaDel(5));//using del lambda expression
	}

	//this is the method I will be practicing Kudvens part 82,83,84,85 Queues & Stacks 
	public void JQueuePrac(List<JEmp> somelist)
	{
		Queue<JEmp> jqueue = new Queue<JEmp>(); 
		jqueue.Enqueue(somelist[0]); // <- this will be first item that can get removed. The first item added. 
		jqueue.Enqueue(somelist[1]);
		jqueue.Enqueue(somelist[2]);
		jqueue.Enqueue(somelist[3]);

		Stack<JEmp> jstack = new Stack<JEmp>(); 
		jstack.Push(somelist[0]);
		jstack.Push(somelist[1]);
		jstack.Push(somelist[2]);
		jstack.Push(somelist[3]); // <- this will be first item that can get removed. The last item added. 
	}

}//END OF "public partial class PartialOne"