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
//	JLoopingQuantifiersPrac2(); 
//	JSubstitutionPrac(); 
	
	JBalGrpDef(); 
}

//11-8-14 @6:35pm... This will be the file I use to continue prac the pages I've had loaded 
//since going to De Anza. Oh yeah! ^_^
public void JLoopingQuantifiersPrac2()
{
	//this is from the sub.sec. what back references match
	string[] inputs = {"AA22ZZ","aaBBCCnN", "AABB"};
	string pattern = @"(\p{Lu}{2})(\d{2})?(\p{Lu}{2})";
	string patternInner = @"^(?'first'\w{1})(?=\k'first')";
	
	foreach(int i in Enumerable.Range(0, inputs.Length))
	{
		Console.WriteLine ("\n ~input: "+inputs[i]); Console.WriteLine();
		Match m = Regex.Match(inputs[i], pattern);
		Console.WriteLine ("Match = "+m.Value);
		foreach(Group g in m.Groups)
		{
			Match mInner =  Regex.Match(g.Value, patternInner); 
			Console.WriteLine("    g.Value = "+g.Value);
			Console.WriteLine("        the starting letter is: "+mInner.Value);
		}
	}
}

//Now I am going to start prac substitutions in regular expressions 
//Prac substituting a numbered group
void JSubstitutionPrac()
{
	string money = "$12.19 44.91 $24.09 $999.10";
	string pattern = @"\p{Sc}\s?(?'first'\d+[.,]?\d*)";
	string pattern2 = @"\p{Sc}\s?(?'first'\d+)[.,]?(?'second'\d*)";
	string replace = ">$0<";
	string replace2=  "++${first}++(${second})";
	
	string result = Regex.Replace(money, pattern2, replace);
//	Console.WriteLine (money+"\n"+result);

	string[] books = {"The Hello World",
		"A Tale of Hello world",
		"the story of Hello world"};
		
	foreach (string element in books)
		Console.WriteLine (Regex.Replace(element, @"^(\w+\s*)+$", "\"$&\""));
	
}

void JBalGrpDef()
{
	string input = "<abc><mno<xyz>>";
	string input2 = "<abc so <hello there> i am a submitter><mno<xyz>123> <love>";
	string input3 = "<Oh my<oh my oh my>><<abc><mno<xyz>>>";
	
	string pattern = "^[^<>]*" +
					 "(" + //1st 
					 "((?'open'<)[^<>]*)+" + //2nd , 'open' is an inner grp
					 "((?'close-open'>)[^<>]*)+" +
					 ")*" +
					 "(?(open)(?!))$";
	
	Match m = Regex.Match(input2, pattern, RegexOptions.IgnorePatternWhitespace); 
	
	int openGrpHash = m.Groups["open"].GetHashCode(); 
	int closeGrpHash = m.Groups["close"].GetHashCode(); 
	
	if(m.Success)
	{
		int grpCtr = 0; 
		foreach(Group grp in m.Groups)
		{
			string s;
			if(grp.GetHashCode() == openGrpHash) s = "'open' Group "+grpCtr+" = "+grp.Value;
			else if(grp.GetHashCode() == closeGrpHash) s = "'close' Group "+grpCtr+" = "+grp.Value;
			else s = "Group "+grpCtr+" = "+grp.Value;
			Console.WriteLine("---------------------------------------------------------------------------\n"+s);
			grpCtr++; 
			int capCtr = 0; 
			foreach(Capture cap in grp.Captures)
			{
				Console.WriteLine("\n    Capture "+capCtr+" = "+cap.Value);
				capCtr++; 
			}
		}
	}else Console.WriteLine ("zzzMatch failed.");
}





//
//
//...