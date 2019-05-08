<Query Kind="Program" />

void Main()
{
//	JGroupCapturePrac();
//	JAnchorPracONE(); 
	JContiguous();
//	JSub(); 
//	JSimpleReview();
}


//prac from sub.sec. "Start of string or line: ^  " 
public void JAnchorPracONE()
{
	int startPos = 0, endPos = 70; 
	string cr = Environment.NewLine;
	
	string input = "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957\n" +
                     "Chicago Cubs, National League, 1903-present\n" + 
                     "Detroit Tigers, American League, 1901-present\n" + 
                     "New York Giants, National League, 1885-1957\n" +  
                     "Washington Senators, American League, 1901-1960\n"; 
					 
	string input2 = "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957" + cr +
                     "Chicago Cubs, National League, 1903-present" + cr +
                     "Detroit Tigers, American League, 1901-present" + cr +
                     "New York Giants, National League, 1885-1957" +  cr +
                     "Washington Senators, American League, 1901-1960"; 
	string[] teams = input2.Split(new string[] {cr}, StringSplitOptions.RemoveEmptyEntries);
	string[] teams2 = Regex.Split(input2, cr); 
	//just wanted to see what the array looked like
	foreach(var i in teams) Console.WriteLine (i + " string " + i.Length + "\n.");
//	Console.WriteLine ("-------------------------");
//	foreach(var i in teams2) Console.WriteLine (i + " Regex " +teams.Length);//just wanted to see what the array looked like
	Console.WriteLine ("-------------------------");
	#region The Regex Pattern...				 
	string pattern =
	@"^(?'team'(\w+(\s?)){2,})"+ //1st, 2nd, and 3rd capturing group
	@",\s"+
	@"(?'league'\w+\s\w+)"+ //4th cap grp 
	@","+
	@"(?'years'\s\d{4}"+ //start of 5th cap grp, note it caps \s
	@"(-"+ //start of 6th cap grp
	@"(?'myAlternator'\d{4}|present)"+ //alternator group is 7th cap grp
	@")?,?)+";//end of 5th & 6th cap grp's. Each w/quan ... ADDED \r?\Z so won't work as expected.
	#endregion
	
	//match and bools to know which branch to do 
	Match match; bool doFirst = false, do2 = true, do3 = true;
	
	if(doFirst && input.Substring(startPos, endPos).Contains(","))//I will rewrite this using Regex.Match
	#region this will only print out 1 line. 
	{
		match = Regex.Match(input, pattern);
		while(match.Success)// <- INTERESTING (: 
		{
			Console.WriteLine ("The <{0}> played in the <{1}> in", 
			match.Groups[1].Value, match.Groups["league"].Value/*, match.Groups[2].Value, match.Groups[3].Value*/);
			
			foreach(Capture cap in match.Groups["years"].Captures)
				Console.WriteLine("Capture: " + cap.Value);
				
			//Console.WriteLine (".");
			startPos = match.Index + match.Length; 
			endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos; 
			Console.WriteLine ("\n\n___{0}, {1}___", startPos, endPos);
			
			if(!input.Substring(startPos, endPos).Contains(",")) break;
			match = match.NextMatch(); 
		}
		Console.WriteLine();
	}
	#endregion 
	else if(do2 && input.Substring(startPos, endPos).Contains(","))
	#region this will print multiple lines
	{
		Console.WriteLine ("In 'do2' ... \n\n\n");
		match = Regex.Match(input, pattern, RegexOptions.Multiline);
		while(match.Success)
		{
			Console.WriteLine("The <{0}> were in the <{1}> in", 
				match.Groups["team"].Value, match.Groups["league"].Value);
				
			foreach (Capture capture in match.Groups["years"].Captures)
				Console.WriteLine(capture.Value);
			
			Console.WriteLine("match.Index = {0}\nmatch.Length = {1}\nmatch.Value = {2}",
				match.Index, match.Length, match.Value);
			startPos = match.Index + match.Length;// I AM NOT GETTING WHAT WE ARE DOING WITH startPos AND endPos
			endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos; 
			Console.WriteLine ("___{0}, {1}___\n", startPos, endPos);
			
			if(!input.Substring(startPos, endPos).Contains(",")) break; 
			match = match.NextMatch(); 
		}
		Console.WriteLine();
	}
	#endregion
	else if(do3)
	#region this is using an Array to do same as previous... 
	{
		Console.WriteLine ("Using an array :)\n");
		foreach(string team in teams)
		{
			if(team.Length > 70) continue; 
			
			match = Regex.Match(team, pattern);//this is Super Uber cool, we are doing an individual match for each element 
			if(match.Success)
			{
				Console.WriteLine("The {0} played in the {1} from", match.Groups["team"], match.Groups["league"]);
				
				foreach(Capture capture in match.Groups["years"].Captures)
					Console.WriteLine(capture.Value);
				Console.WriteLine("");
			}
		}
	}
	#endregion
	
	
	//Console.WriteLine("\ninput[] = " + input[9] + ",  " );
	//Console.WriteLine(input);
}

// from MSDN, prac. "Contiguous Match" ex. 
void JContiguous()
{
	string input = "capybara, squirrel ,chipmunk,porcupine,gopher," + 
                   "beaver,groundhog,hamster,guinea pig,gerbil," + 
                   "chinchilla,prairie dog,mouse,rat.";
    string pattern = @"\G(?'rodent'\w+(\s?)(\w*))(,|.)";
	string pattern2 = @"(?'rodent'\w+\s?\w*),?";//doesn't seem to make a diff if \G is in pat or not
	Match match = Regex.Match(input, pattern2);
	while (match.Success)
	{
		Console.WriteLine(match.Groups["rodent"].Value);
		match = match.NextMatch(); 
	}
}

//from substitution construct '?index' sub.sec.
void JSub()
{
	string input = "$16.32 12.19 £16.29 €18.29  €18,29";
	string pattern1 = @"\p{Sc}*(\s?\d+[.,]?\d*)\p{Sc}*";
	string pattern = @"\p{Sc}*(\s?\d+[.,]?\d*)\p{Sc}*";
	Match match = Regex.Match(input, pattern); 
	var matches = Regex.Matches(input, pattern); 
	Console.WriteLine (match.Value);
//	foreach(Group grp in match.Groups)
//	{
//		Console.WriteLine ("\tGroup: " + grp.Value);
//		foreach(Capture capture in grp.Captures) Console.WriteLine ("\t\tCapture: "+capture.Value);
//	}
	Console.WriteLine ();
	Console.WriteLine ("MatchCollection now");
	foreach(Match m in matches) 
	{
		Console.WriteLine ("\tMatch: " + m);
		foreach(Group grp in m.Groups)
		{
			Console.WriteLine ("\t\tGroup: " + grp.Value);
			foreach(Capture capture in grp.Captures) Console.WriteLine ("\t\t\tCapture: "+capture.Value);
		}
	}
	Console.WriteLine ();
	Console.WriteLine ();
	var replace = Regex.Replace(input, pattern1, @"<$1>");
	Console.WriteLine ("Input         = {1}\nReplacement = {0}", replace, input);
}

//re-reviewed this 11-3-14 @7:15pm
void JSimpleReview()
{
	string s = "Hello there World", dis = "Discipline Over Regret!!!",
	sdis = s+", I believe in "+dis; 
	string pattern1 = @"\b(?'hunter'\w+)\b\s\b(?'theta'\w+)\b\s\b(?'wasp'\w+)\b";
	string pattern2 = @"(?xi) \b(?'hunter'\w+ \s (?'theta'\w+))\b \s \b(?'wasp'\w+)\b";
	
	Match m = Regex.Match(s, pattern1);
	Match m2 = Regex.Match(sdis, pattern2);
	
	while(m2.Success)
	{
		int ctr = 1; 
		foreach(Capture c in m2.Groups[ctr].Captures)
		{
			Console.WriteLine(" m2.Groups["+ctr+"] ="+c.Value);
			ctr++; Console.WriteLine (ctr);
		}
		m2 = m2.NextMatch(); ++ctr; if(ctr > 1000) break; 
	}
	
	m2 = Regex.Match(sdis, pattern1);
	Console.WriteLine ();Console.WriteLine (sdis);Console.WriteLine ();
	Console.WriteLine ("Group 'hunter': "+m2.Groups["hunter"].Value);
	Console.WriteLine ("Group[1]: "+m2.Groups[1].Value);

	Console.WriteLine ("Group 'theta': "+m2.Groups["theta"].Value);
	Console.WriteLine ("Group 'wasp': "+m2.Groups["wasp"].Value);
}

//11-4-14 @2:14pm (: 
void JGroupCapturePrac()
{
	string simple = "This, /is... /a /short /sentence!! /YO. "; 
	
	// prac Noncapturing groups from MSDN sub.sec. & Captures and Group clarification
	string pattern1 = @"(\b(\w+)\W*)+\.";
	Match match = Regex.Match(simple, pattern1);
	
	for(int ctr=0; ctr < match.Groups.Count; ctr++)
	{
		Console.WriteLine ();
		Console.WriteLine("match.Groups["+ctr+"] = "+match.Groups[ctr].Value);
		Console.WriteLine ();
		for(int ctr2=0; ctr2 < match.Groups[ctr].Captures.Count; ctr2++)
		{
			Console.WriteLine ("\t\t\t1st) match.Groups["+ctr+"].Captures["+ctr2+"].Value = "+match.Groups[ctr].Captures[ctr2].Value);
		}
		Console.WriteLine ("-----------------------------------------------");
		foreach(Capture capture in match.Groups[ctr].Captures)
		{
			Console.WriteLine ("\t\t\t2nd) capture = "+capture.Value);
		}
	}
	//
}
//