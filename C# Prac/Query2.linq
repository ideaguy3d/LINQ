<Query Kind="Program" />

static string cr = Environment.NewLine; 
string inputStr = "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957" + cr +
                   "Chicago Cubs, National League, 1903-present" + cr +
                   "Detroit Tigers, American League, 1901-present" + cr +
                   "New York Giants, National League, 1885-1957" +  cr +
                   "Washington Senators, American League, 1901-1960"; 
	
void Main()
{
	//Prac1();
	Prac2(); 
}

void Prac3()
{
	string veris = "VERISIMILITUDE";
	string veris2 = veris.ToLower();
	string veris3 = veris[0]+veris2.Remove(veris2.IndexOf('v'), 1); 
	
	string repeats = "Hello hello world world I am repeating Words Words on purpose PURPOSE Lol lol LOL."; 
	
	MatchCollection mc = Regex.Matches(repeats, @"[a-zA-Z]*(?=(\s|\.))"); 
	string[] repeatsArray = new string[mc.Count];
	int i = -1; 
	foreach (Match element in mc) repeatsArray[++i] = element.Value; 
	foreach(string s in repeatsArray) Console.Write (s);
	IEnumerable<string> repeatsArray2 = repeatsArray.Distinct(n => n[0] != 'w');
	repeatsArray2.Dump("r2"); 
}

void Prac1()
{
	string[] inputArray = inputStr.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries); 	
	
	string pattern = @"(?ix)^(?'team'(?'word1'\w+)(?'space_word'\s(\w+)))*,";
	
//	foreach(string temp in inputArray)
//	{
//		//Console.WriteLine (temp);
//		Match m = Regex.Match(temp, pattern);
//		foreach(Capture c in m.Captures) Console.WriteLine ("\t\t" + c);
//		Console.WriteLine (m.Value);
//		Console.WriteLine();
//	}
	
	
	
	string s2 = "This is a sentence. And this is another sentence. Yet another lol.";
	string pattern2 = @"\b(?'wordAndSpace'(?'word'\w+)(?'space'\s*))+(\.)";
	string pattern3 = @"\b(?'wordAndSpace'\w+\s*)+\.";
	Match mCapture = Regex.Match(s2, pattern3); 
	//Console.WriteLine ("1="+mCapture.Groups["wordAndSpace"].Captures[0].Value);
	//Console.WriteLine ("2="+mCapture.Groups["wordAndSpace"].Value);
	
	
	Console.WriteLine ("\n");
	if(mCapture.Success)
	{
		for(int ctr=1; ctr<mCapture.Groups.Count; ctr++)
		{
			Console.WriteLine ("Group "+ctr+": "+mCapture.Groups["wordAndSpace"].Value);
			int capCtr = 0;
			foreach(Capture c in mCapture.Groups["wordAndSpace"].Captures)
			{
				Console.WriteLine ("\t\tCapture "+capCtr+": "+c.Value);
				capCtr++; 
			}
		}
	}
}

//The <team> played in the <league> in <years w/formatting>. 
void Prac2()
{
	int startPos = 0, endPos = 70; 
	string pattern = @"^(?'team'\w+\s\w+)+,";
	//rem to see what happens if we put ? outside of () in .Group["team"], 
	string patternMsdn = 
	@"(?m)^(?'team'(\w+(\s)?){2,}),\s"+
	@"(?'league'\w+\s\w+),"+
	@"(?'years'\s\d{4}(-(\d{4}|present))?,?)+";
	if(inputStr.Substring(startPos, endPos).Contains(","))
	{
		Match m = Regex.Match(inputStr, patternMsdn); 
		while(m.Success)
		{
			Console.Write ("The "+m.Groups["team"].Value+" played in the "+m.Groups["league"].Value+" from:");
			foreach(Capture cap in m.Groups["years"].Captures) Console.Write (cap.Value);
			Console.WriteLine (".");
			
			startPos = m.Index + m.Length; 
			endPos = startPos+70 <= inputStr.Length ? 70 : inputStr.Length - startPos; 
			if( !inputStr.Substring(startPos, endPos).Contains(",")) break; 
			m = m.NextMatch(); 
			Console.WriteLine ("\t\t"+m.Value+"..."+m.Groups[1].Captures.Count);
		}
	}
}