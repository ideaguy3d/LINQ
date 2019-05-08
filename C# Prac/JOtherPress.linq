<Query Kind="Program">
  <Namespace>UnityEngine</Namespace>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
</Query>

void Main()
{
	
	
}

// Define other methods and classes here

	/// <summary>
	/// J other press is was used in another class to print out the results to the console. I simply made an instance of 
	/// it and was able to use all the other classes with in this script easily making instances of them to. 
	/// 
	/// This script is also being used to practice Delegates that pass in an entire method that it is pointing to as a method 
	/// parameter. 
	/// 
	/// </summary>
	
	public delegate bool Promote(JAppClass exp);//this is the actual Base Delegate of type 'Promote'
	
	public class JOtherPress : MonoBehaviour
	{
		List<JAppClass> jAppList; 
		Promote isPromote;//this is a delegate pointer of type 'Promote', see line 14
	
		void Awake()
		{
			jAppList = new List<JAppClass>(); 
			jAppList.Add(new JAppClass(){Name = "todd", Salary = 24431, Exp = 5});
			jAppList.Add(new JAppClass(){Name = "mike", Exp = 3});
			jAppList.Add(new JAppClass(){Exp = 2, Salary = 2342, Name = "tom"});
	
			isPromote = new Promote(SomeButtonPressed);//we are using a delegate in these 
			JAppClass.ShouldPromote(jAppList, isPromote);//two lines of code to figure out the logic
		}
	
		float health = 100.0f; 
		InvGameItem[] jItems; 
		public int polySides; 
	
		public enum pracEnum {first, second, third, fourth, fifth, _last}; 
	
		public float Health
		{
			get{return health;}
			set{health = value;}
		}
	
		public bool Sides{get; set;}
	
		//this is the method that our delegate will be pointing to. Remember to make it static so that an instance 
		//of it is not needed. 
		public static bool SomeButtonPressed(JAppClass exp)
		{
			if(exp.Exp >= 3) return true;
			else return false; 
		}
	
	}// END OF " public class JOtherPress : MonoBehaviour " ~_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_~
	
	
	/// <summary>
	/// J square. I was practicing with extention methods with this script, also I started practicing with classes and particulary 
	/// the 'this' keyword. I was working through (and on my own a bit) through the "30 days to learn C#" tut youtube vids serties. 
	/// 
	/// </summary>
	
	class JSquare : JOtherPress
	{
		public JSquare(){base.Sides = true; base.polySides = 4;}
	}
	
	class JCircle : JOtherPress 
	{
		public JCircle(){base.Sides = false; base.polySides = 0;}
	}
	
	//this is the class that will extend the methods of class JOtherPress which 'JSqaure and JCircle' inherit from. 
	public static class AClassB
	{
		public static bool IsPolygon (this JOtherPress other)
		{
			return other.Sides; 
		}
	
		public static int PolySides(this JOtherPress other) {return other.polySides;}
	}//END OF "public static class AClassB" ~_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_~
	
	
	public class JAppClass 
	{
		JCircle cir = new JCircle(); //practicing extention methods 
		JSquare sq = new JSquare(); //practicing extention methods 
		string name = "Julius";  
		public string Name{get; set;}
		public int Salary{get; set;} 
		public int Exp{get; set;} 
	
		//this is a method that is taking a delegate as a parameter. 
		public static void ShouldPromote(List<JAppClass> jList, Promote promote)
		{
			foreach(JAppClass temp in jList)
			{
				if(promote(temp)) Debug.Log(temp.Name + " Got Promoted.!(:"); 
			}
		}
	
	
		//this is where the extention methods are actually used. 
		public string IsPoly()
		{
			string s = "cir.IsPolygon() = " + cir.PolySides() + "\nsq.IsPolygon() = " + sq.PolySides(); 
			return s;
		}
	}//END OF "public class JAppClass" ~_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_^_~
	
	//--------------------------------