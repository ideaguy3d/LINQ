<Query Kind="Program" />

List<JEmp> empList = JEmp.EmpList;
static string CityOfEmp2;

void Main()
{
	CityOfEmp2 = empList[3].City;
//	JKeyValue(); 
//	FromPracOne(); 
//	CookBookPrac(); 
//	JAnchorMSDN(); 
//	JStatementLambdaPrac(); 
	JAlternationPrac();
}
		//statement lambda prac ((((((((:
//Func<T, bool> sLambda_1 = (T t) =>	//I'll have to fig out how to use generics later
//{
//	if(t is JEmp) return true; 
//};

Func<JEmp, bool> sLambda_2 = (JEmp emp) =>
{	
	if(emp.JEmpGender == Gender.Female) emp.City = CityOfEmp2;
	if(emp.City == CityOfEmp2 && emp.Ethnicity == "White") return true; 
	else return false;
};

void JKeyValue()
{
	string prac1 = 
	@"alpha = 1
	 God = Yahweh
	  Faith = Succuess
	 No Sacrifice = No Victory"; 

	string regPattern1 = @"(?m)^\s*(?'key'\w*)\s*=\s*(?'value'\w*)\s*(?=\r?$)";
	
	foreach(Match m in Regex.Matches(prac1, regPattern1))
	{
		string temp = "<"+m.Value+"\\r>";
		temp = temp.Remove(temp.IndexOf("\r"), 1); 
		//Console.WriteLine (temp+"\n"+m.Groups["key"]+" EQUALs "+m.Groups["value"]);
	}
	
	
	string[] myNames = 
	{
		"My name is Julz and I am 25 years old",
		"My name is Julius and I am 25 years old",
		"My name is Juwels and I am 25 years old",
		"My name is Julious and I am 25 years old"
	}; 
	
	string[] myNames2 = 
	{
		"My name is Julz and I am 25 years old",
		"My name is Juoius and I am 25 years old",
		"My name is Juwels and I am 25 years old",
		"My name is Juzious and I am 25 years old"
	}; 
	
	//just prac some alternators with the above string array
	string myPattern = @"(i?)Ju[lz|lius|lious|wels]"; //this is the wrong way
	// to do an alternator
	string myPattern2 = @"(i?)\bJu(lz|lius|lious|wels)\b";//simple alternator works well
	//the following does not work... AFTER REVIEWING FOR HOURS COME BACK AND FIX!!!!!!! <<<<<<
	string myPattern3 = @"(i?)\bJu(lz|lius|lious|wels)\b.*(?'age'\d*)(?=\s*years\s*old)";
	
	foreach (string element in myNames2)
	{
//		Console.WriteLine ("-----------------------");
		Match m = Regex.Match(element, myPattern); // 
//		Console.WriteLine (m.Value);
	}
	
	//now prac parsing Dates/Times from cookbook 
	string datesTime = "11/1/14 8:30:15 PM";
	string dateTimePattern = @"(?i)(?x)" +
	@"(\d{1,2}) [./-]" +
	@"(\d{1,2}) [./-]" +
	@"(\d{2,4}) [\sT]" +
	@"(\d*):(\d*):(\d*)\s?(a\.?m\.?|p\.?m\.?)?";
	
	string dateTimePattern2 = @"(?x)(?i)
	(\d{1,2}) [./-] 
	(\d{1,2}) [./-] 
	(\d{2,4}) [\sT] 
	(\d*:\d*:\d*\s[ap]m)";
	
	Match dateTimeMatch = Regex.Match(datesTime, dateTimePattern);
	foreach(Group g in dateTimeMatch.Groups)
		Console.WriteLine (g.Value + " ");
	//
}

//let's enhance and add to them ! ((:, actually I did that in CookBookPrac() 
//pretty much just copied and pasted these from other file w/no change 
void FromPracOne()
{
	//prac 'Removing Repeated words' from cookbook, ALL figure out how to use .Distinct() LINQ op
	string removeRepeatReg = @"(?ix)(?'repeat'\w+)\W\k'repeat'";
	string begin = "This this is the the the beginning..."; 
	MatchCollection mc = Regex.Matches(begin, removeRepeatReg); 
	begin = Regex.Replace(begin, removeRepeatReg, "${repeat}"); 
//	Console.WriteLine ("begin:\n"+begin);
	//
	
	Console.WriteLine ("-----------------------------------------------");
	//prac "Word Count" recipe 
	string wordCountPattern = @"\b(\w|[-'])+\b";
	MatchCollection wordCountMC = Regex.Matches("It's all Mumbo-Jumbo to me.", wordCountPattern);
	foreach(Match m in wordCountMC) Console.Write (m.Value+" ");
	Console.WriteLine ("\nReg word count = " + wordCountMC.Count);
	//
	
	Console.WriteLine ("-----------------------------------------------");
	//"Unescaping chars in a HTTP query" Prac from cookbook
	string http = "C%24 rocks";
//	Console.WriteLine (http);
	string httpPattern = @"%[0-9a-f][0-9a-f]";
	http = Regex.Replace(http, httpPattern,
		m => ((char)Convert.ToByte(m.Value.Substring(1), 16)).ToString() + " Dude!",
		RegexOptions.IgnoreCase); 
//	Console.WriteLine ("\n"+http);
	//
}

//reworked through many of the cookbook examples from C# in a Nutshell 5.0
void CookBookPrac()
{
	Console.WriteLine ("-----------------------------------------------");
	//modifying  "Unescaping chars in a HTTP query" Prac from cookbook
	string http = "C%25 rocks";
	string http2 = "C%25 rocks, C%24 rocks, C%22 rocks";//prac .Matches w/this str
	string httpPattern = @"%[0-9][0-9]";
	
	string httpOut = Regex.Replace(http, httpPattern,
		m => ((char)Convert.ToByte(m.Value.Substring(1), 16)).ToString() + " YHWH",
		RegexOptions.IgnoreCase);
	string holder1; 
	string httpOut2 = Regex.Replace(http, httpPattern,
	m => (m.Value.Replace(m.Value[1].ToString(), "two")+
	m.Value.Replace(m.Value[2].ToString(), holder1 = m.Value[2] == 5 ? "five":"umm?")+" Oh Yeeeaah") ); 
	
	string httpOut3 = Regex.Replace(http, httpPattern,
	m => (m.Value.Replace(m.Value[1].ToString(), "two")+" Oh Yeeeaah") ); 
//	Console.WriteLine (httpOut3);
//	Console.WriteLine (httpOut2);
	
	Console.WriteLine ("-----------------------------------------------");
	
	//mod "Word Count" recipe
	string pracStrInput = "Well hello there World, It's about time that I've re-leaned all this stuff. Lol."; 
	string wordCountPattern = @"\b(\w|[-'])+\b";
	MatchCollection wordCountMC = Regex.Matches(pracStrInput, wordCountPattern);
	//now converting this MC into a list (: 
	List<Match> listMC = new List<Match>();
	foreach(Match m in wordCountMC) listMC.Add(m); 
//	listMC.Dump("listMC"); //seeing all the Match data is so cool! ^_^
	
//	Console.WriteLine (wordCountMC.Count);
//	foreach(Match e in wordCountMC) Console.Write(e.Value+" "); 
	//
	
	Console.WriteLine ("-----------------------------------------------");
	
	//modifying 'Removing Repeated words' from cookbook, ALL figure out how to use .Distinct() LINQ op
	string patternRemoveRepeats1 = @"(?xi)(?'dupe'\w+)\W\k'dupe'"; 
	string pracStrRepeats1 = "In the The the beginning The God said let let there be be be light light";
	string pracStrRepeats2 = "In the beginning beginning";
	string repeatsRemoved1 = Regex.Replace(pracStrRepeats2, patternRemoveRepeats1, "${dupe}");
//	Console.WriteLine ("string before: "+pracStrRepeats2+"\nstring after: "+repeatsRemoved1);
	Console.WriteLine ();
	//this is where I start prac Regex.Split() and .Distinct()
	string pracStrRepeats1b = pracStrRepeats1.ToLower(); 
	string[] repeatStrArr1 = Regex.Split(pracStrRepeats1b, @",?\s+");//making an array to use .Distinct() 
	
	//this will actually take out 'The' before 'God', which I don't want but I'll have to figure out 
	//how to fix some other time.
	repeatStrArr1 = repeatStrArr1.Distinct().ToArray(); //removed repeats
	repeatStrArr1[3] = "God";//I had to dump it then count manually to know the index. Lists would be beter
	//since it has the .IndexOf()
	//all of this meticulous bit of coding just to change the first letter in a string in a string[]
	string temp = repeatStrArr1[0]; 
	char c = char.ToUpper(temp[0]);
	temp = c + temp.Substring(1);//glad I studied this earlier othewise wouldn't have known I could do this:)
	repeatStrArr1[0] = temp; 
//	repeatStrArr1.Dump(); 

	#region _did not work the way I wanted it to :\ _ 
//	char[] repeatsRemoved2 = new char[pracStrRepeats1.Length]; 
	//this does not do what I thought it would do. I was just trying to get each char in each index of 
	//a str array to fill an array to a char and try to fill so I can try to output as a string
//	for(int i=0; i<repeatStrArr1.Count(); i++) //use a string builder after this, it'll probably be easier
//	{
//		int counter=0;
//		foreach(char c in repeatStrArr1[i]) 
//		{
//			repeatsRemoved2[counter]= c;
//			counter++; 
//		}
//	}
//	string repeatsRemoved3 = new string(repeatsRemoved2); 
	#endregion
	
	StringBuilder repeatsRemoved4 = new StringBuilder(); 
	foreach(string s in repeatStrArr1) repeatsRemoved4.Append(s+" ");
//	Console.WriteLine ("string before: "+pracStrRepeats1+"\nstring after: "+repeatsRemoved4.ToString());
	
	Console.WriteLine ("-----------------------------------------------");
	
	//this will be the first time I actually work through "Escaping Unicode chars for HTML"
	//won't be exactly the same but it'll be similar
	string strInput1 = "@ 2014"; //he actually uses the copyright char 
	string patternCopyright = @"\p{P}"; 
	string copyrightReplace = Regex.Replace(strInput1, patternCopyright,
	m => "#?... " + ((int)m.Value[0]).ToString());//this will just convert @ to its' int equivalent
	Console.WriteLine (copyrightReplace);
//	Console.WriteLine (Regex.IsMatch(strInput1, patternCopyright));
	//
}


#region ENDED UP WORKING IN JAnchorMSDN2
//Here I will again work through many of the MSDN examples 
//this will be from the "Anchors In Regex" main sec. 11-3-14 @7pm
void JAnchorMSDN()
{
	int startPos = 0, endPos = 70; 
	string input = "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957\n" +
                     "Chicago Cubs, National League, 1903-present\n" + 
                     "Detroit Tigers, American League, 1901-present\n" + 
                     "New York Giants, National League, 1885-1957\n" +  
                     "Washington Senators, American League, 1901-1960\n"; 
					 
	string pattern = @"^(?x)((?#1st)  (\w+(\s?)){2,}),\s((?#4th)  \w+\s\w+),((?#5th)  \s\d{4}(-(\d{4}|present))?,?)+";
	Match m;
	if(input.Substring(startPos, endPos).Contains(","))
	{
		int ctr = 0; 
		m = Regex.Match(input, pattern, RegexOptions.Multiline); 
		while(m.Success)
		{
			Console.WriteLine ("------------------------------------------------------------------------------");
			Console.WriteLine("The "+m.Groups[1].Value+" played in the "+m.Groups[4].Value+" from:");
			foreach(Capture c in m.Groups[5].Captures) Console.Write("Capture: "+c.Value+" ");
			
			startPos = m.Index+m.Length;
			endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos;
			
			Console.WriteLine ("\n\n   \t\tstartPos= "+startPos+", endPos="+endPos
				+", m.Index= "+m.Index+", m.Length= "+m.Length+", input.Length="+input.Length);
			
			//just so I can see the substr w/out actually have \n take effect
			string inputSub = Regex.Replace(input.Substring(startPos, endPos), @"\n", "<\\n>");
			if(!input.Substring(startPos, endPos).Contains(","))
			{
				Console.WriteLine ("\n\t\t\t\t>>>>>>>>>>>>there was Not a ','");
				break;
			}else Console.Write ("\n\t\t\t\t>>>>>>>>>>>>>>>>>>>>>>>>input.Substring("+startPos+","+endPos+")="+inputSub);
				
			m = m.NextMatch();
			Console.WriteLine ("\n\n\t\t\t\t>>>>>>>>>>>> m.NextMatch()= "+m.Value);
			
		}//++ctr; if(ctr > 500){Console.WriteLine ("\n\nctr="+ctr); break;}//safe gaurd so it does not infinitely loop.
	}
}
#endregion 

//con. from previous
void JAnchorMSDN2()
{
	string input = "Brooklyn elle Dodgers, National League, 1911, 1912, 1932-1957, 1957-present\n" +
                     "Chicago Cubs, National League, 1899, 1901-1910, 1933-present\n" + 
                     "Detroit Tigers, American League,1912, 1932-1957, 1901-present\n" + 
                     "New York Giants, National League, 1999, 1885-1957\n" +  
                     "Washington Senators, American League, 1912, 1932-1957,1901-1960\n"; 
	
	string pattern1 = @"(?mxi)^ ( (\w+(\s?)){2,} ),\s (\w+\s\w+),(\s\d{4}(-(\d{4}|present))?,? )+";
	Match match; 
	int startPos = 0, length = 70; 
	
	if(input.Substring(startPos, length).Contains(","))
	{
		match = Regex.Match(input, pattern1);
		Console.WriteLine (match.ToString().Length+" / "+match.NextMatch()+"\n"+match.NextMatch().Value+"\n");
		
		while(match.Success)
		{
			Console.WriteLine("Team "+match.Groups[1].Value+" played in the "+match.Groups[4].Value+" from:");
			foreach(Capture capture in match.Groups[5].Captures)
			{
				Console.Write(capture.Value+ " ");
			}
			Console.WriteLine ("\t\tTotal Captures in '"+match.Groups[5].Captures[1].Value
				+"': "+match.Groups[5].Captures.Count);
				
			// re-calc the input length and pos
			startPos = match.Index + match.Length;
			length = startPos + 70 <= input.Length ? 70 : input.Length - startPos;
			if(!input.Substring(startPos, length).Contains(",")) break;
			match = match.NextMatch(); 
		}
	}
}

//prac LINQ statement lambda's here 
void JStatementLambdaPrac()
{
	Console.WriteLine ("Hello World\n\n");
	IEnumerable<JEmp> emps1 = 
	empList.Where(sLambda_2);
	emps1.Dump("Females: "+
	emps1.Where(emp => emp.JEmpGender==Gender.Female).Count()+"\nMales: "+
	emps1.Where(emp => emp.JEmpGender==Gender.Male).Count());
}

//prac alternation from "Alternation Contructs" MSDN sec.
void JAlternationPrac()
{
	string sample = "12-2227777 225-25-4659 77-8577777 456-147852 645-52-7894";
	string pattern = @"\b(?(\d{2}-)(?'EIN'\d{2}-\d{7})|(?'SSN'\d{3}-\d{2}-\d{4}))";//expression match
	string pattern2 = @"\b(?'num'\d{2}-)*(?(num)\d{7}|\d{3}-\d{2}-\d{4})";//it's like we're combining group 'num' w/positive side |
	

	#region prac testing for an expression alternation construct
	//for the next to portions of code I just wanted to use .Match and .Matches for prac to compare...
	// turns out there is hardly no difference. So far I still think .Matches is easier to understatnd
	MatchCollection mc = Regex.Matches(sample, pattern);
	foreach(Match m2 in mc)
	{
		Console.WriteLine ("Value: "+m2.Value+" at index: "+m2.Index);
		
		if(m2.Groups["EIN"].Success)
			Console.WriteLine ("EIN = "+m2.Groups["EIN"].Value+" count = "+m2.Groups["EIN"].Captures.Count);
		else Console.WriteLine ("SSN = "+m2.Groups["SSN"].Value+" count = "+m2.Groups["SSN"].Captures.Count);
		Console.WriteLine ();
	}
	Console.WriteLine ("\n...Now using .Match...\n");
	
	Match m = Regex.Match(sample, pattern);
	int ctr = 0; 
	while(m.Success)
	{
		Console.WriteLine("Value: "+m.Value+" at index: "+m.Index);
		//okay so here I am storing hashcodes so I can know which group is which and output a str accordingly,
		//I do this because outputting a var name of the LHS of = can't be done. So I use the hash to know which var
		int hashCode_EIN = m.Groups["EIN"].GetHashCode(); 
		int hashCode_SSN = m.Groups["SSN"].GetHashCode();
		string grpStr; 
		foreach(int grp in Enumerable.Range(1, m.Groups.Count))//just wanted to use .Range() for prac, took a half hour to figure out, but hey. 
		{
			grpStr = m.Groups[grp].GetHashCode() == hashCode_EIN ? "EIN" : "SSN"; 
			foreach(Capture c in m.Groups[grp].Captures) Console.WriteLine ("Group "+grpStr+
				", Capture c = "+c.Value);
		}
		Console.WriteLine ();
		m = m.NextMatch(); ++ctr; if(ctr>150) break; 
	}
	#endregion 
	//
	
	#region prac testing for a matched group alternation construct
	//well hello world
	MatchCollection groupMC = Regex.Matches(sample, pattern); 
	foreach(Match temp in groupMC)
	{
		Console.WriteLine ("Value "+temp.Value+" at index "+temp.Index);
	}
	#endregion
	//
}




//just for extra space lol (: