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

	//JPracOne();
	//SimpleSimple();
	//ZeroWidthPrac();
	//JSplit();
	MoreRegPrac();
	
}

public bool doRun, doRunOut, doMatchCollectionRun; 


public void SimpleAnchorPrac()
{
	StringBuilder matchesSb, amatchSb; 
	
	string start = "This is the start.", end = "and (now) if the (now) end."; 
	JOutputO(out amatchSb, Regex.Match(start, @"(?i)^.*?[iI]s"), "^ prac"); //
	string docs1 = "a.txt"+"\r\n"+"b.doc"+"\r\n"+"c.txt"+"\r\n"+"thetext.txt";
	string docs2 = "a.txt\r\nworld.txt, b.doc\r\nhello.doc, c.txt\r\nthetext.txt";
	string regDoc = @"(?m).+\.txt(?=\r?$)"; //pos lookahead to catch \r, notice $ is inside ()
	JOutputO(out matchesSb, Regex.Matches(docs2, regDoc), "Prac (?m) and $"); 
	string spaces = " \t\r\n\r\n  \nlast line";
	//JOutputO(out matchesSb, Regex.Matches(spaces, @"(?m)^[ \t]*(?=\r?$)"), "multi line and \\s prac"); 
	//Console.WriteLine ("Input:\n{0}\n" , docs2);
	//Console.WriteLine (matchesSb.ToString());
}

//some MSDN prac in here 
public void JPracOne()
{doRunOut = false; 
 	#region Data to practice on...  
	StringBuilder sb = new StringBuilder(); 
	StringBuilder sb2 = new StringBuilder(); 
	
	string[] a1 = {"gray", "grey", "gra"}; 
	a1.GetUpperBound(0); 
	
	string s1 = "What is your favorite colour Joe?"; 
	string s2 = "So, what is your fav color Jane?"; 
	
	Match m1 = Regex.Match(s1, @"colou?r"); 
	Match m2 = Regex.Match(s2, @"colou?r");
	
	string am = "I amwhat I am"; 
	string am2 = "I Am what I am";
	string am3 = "I am what I Am";
	string am4 = "I amAm what I Amam";
	string am5 = "I Am what I Am"; 
	
	string grey = "There is grey";
	string grey2 = "There is gray";
	string grey3 = "There is grey and gray";
	
	string amgrey = "I amgrey or greyam or am I Jen gray? ..."; 
	
	#endregion 
	
	Match mMatch1 = Regex.Match(am, @"am what");
	
	Match mMatch2 = Regex.Match(amgrey, @"(?i)(?x)gr[ea]y");
	//this is Alternator prac
	Match jen = Regex.Match("Jennifer", @"Jen(ny|nifer)?");JOutputO(out sb, jen, "\njen");
	Match jennygrey = Regex.Match(amgrey, @"Jen(ny|nifer)? (?i)(?x)gr[ea]y"); JOutputO(out sb, jennygrey,"\njennygrey"); 

	Match mMatch3 = Regex.Match(amgrey, @"(?x)am grey"); JOutputO(out sb, mMatch3, "\nmMatch3");  

	//Console.WriteLine (sb.ToString());
	
	
	//from MSDN balancing group definitions
	string input = "<abc><mno<xyz>>";
	string pattern = @"(?xi)^[^()]*(((?'open'\()[^()]*)(?#2nd)+((?'close-open'\))[^()]*)(?#3rd)+)(?#1st)*(?(open)(?!))$";
	string yetagain = "well, (abc)Hi(mno(xyz)dear)world"; 
	
	string condstr = "Wasn't word1 used awfully, couldn't a better use of Word1 have been explained...?";
	string regnest = @"(?xi)^[^<>]*(((?'Open'\'t)[^<>]*)+((?'Close-Open'1)[^<>]*)+)+$";//"(?(Open)(?!))$"; 
	
	Match m = Regex.Match(yetagain, pattern); 
	bool runit = false;
	if(m.Success && runit)
	{
		int grpCtr = 0; 
		Console.WriteLine (condstr+"\n"+m);
		foreach(Group grp in m.Groups)
		{
			Console.WriteLine("\nGroup["+grpCtr+"] = "+grp.Value+", Captures = "+grp.Captures.Count);
			grpCtr++;
			int cptrCtr = 0;
			foreach(Capture cptr in grp.Captures)
			{
				Console.WriteLine("\t\tCapture["+cptrCtr+"] = "+cptr.Value);
				//Console.WriteLine("FIRST || " + cptrCtr);
				cptrCtr++;
				//Console.WriteLine ("SECOND || " + cptrCtr);
			}
		}
	}
	//
	
	string simple = "This, /is... /a /short /sentence!! /YO. "; 
	
	// prac Noncapturing groups from MSDN sub.sec. & Captures and Group clarification
	string nonCapReg = @"(?:\b(?:\w+)\W*)+\.";
	string capReg = @"(\b(\w+)\W*)+\.";
	
	Match noncapMatch = Regex.Match(simple, capReg); 
	Console.WriteLine("Match: '' {0} '' = {1}\n", noncapMatch.Value, noncapMatch.Groups.Count);
	for(int ctr=1; ctr<noncapMatch.Groups.Count; ctr++)//notice we start from ctr = 1
	{
		Console.WriteLine("\t\tGroup[{0}] = {1}", ctr, noncapMatch.Groups[ctr].Value);
		for(int ctr2=0; ctr2<noncapMatch.Groups[ctr].Captures.Count; ctr2++)
		{
			Console.WriteLine("\t\t\tCapture[{0}] = {1}", ctr2, noncapMatch.Groups[ctr].Captures[ctr2].Value);
		}
	}
	
	if(doRunOut) Console.WriteLine(sb.ToString());
	
} // END OF " public void JPracOne() " method 

//just prac all the diff (?x) possibilities because it was Really confusing me. 
public void SimpleSimple()
{	doRun = false; doRunOut = false; doMatchCollectionRun = true;

	StringBuilder s1 = new  StringBuilder();
	StringBuilder outsb;
	StringBuilder out_sb_matches; 
	
	string q = "quiz qwerty qrm qhi"; 
	//with this ^ , we are looking for 2 letters, the first has to be q, the 2nd can be any except a vowel. 
	var mq1 = Regex.Match(q, @"q[^aeiou]"); JOutputO(out outsb, mq1, "mq1"); 
	var msq1 = Regex.Matches(q, @"q[^aeiou]");  
	JOutputO(out outsb, Regex.Match("b1-c4 4d", @"(?i)(?x)[a-h]\d-[a-h]\d(?-x)(?-i) [2461][a-d]"), "char set\n");
	//looking to see if there is an \n anywhere 
	JOutputO(out outsb, Regex.Match("Hello\nWorld\n", @"[\n]"), "looking for '\\n' in 'Hello\\nWorld\\n'"); 
	JOutputO(out outsb, Regex.Match("Yes, please", @"\p{P}"), "prac \\p{P}, which is category for punc in 'Yes, please'"); 
	
	//prac the * quantifer, it'll find zero or more matches
	JOutputO
	(
		out out_sb_matches, 
		Regex.Matches("cv15.doc, cv2.doc, cv7.doc, & cv.doc are all the docs", @"cv\d*\.doc"),
		"Prac the * quantifier"
	);
	
	//here we match anything except for \n from cv to .doc
//	Console.WriteLine(Regex.Match("cvjib\nberish.doc", @"cv.*\.doc").Success + "\n"); //false
//	Console.WriteLine(Regex.Match("cvjib\nberish.doc", @"cv.*\.doc").Success + "\n"); //true
	//
	
	// prac the {} quan
	Regex reg1 = new Regex(@"\d{2,3}/\d{2,3}"); Regex reg2 = new Regex(@"\d{4,5}/\d{4,5}");
//	Console.WriteLine("reg1 Count = " + reg1.Matches("old hr 175/110 now its 115/60").Count + "\n");
//	Console.WriteLine("reg2 Count = " + reg2.Matches("old hr 1754/11052 now its 11524/6045").Count + "\n");
	Regex reg3 = new Regex(@"a{2,3}/b{2,3}");
	//JOutputO(out out_sb_matches, reg3.Matches("aa/bbb but now aaa/bbb and then aa/bb"), "Still prac {} quan"); 
	//
	
	// now prac greedy vs. lazy excution 
	Regex htmlReg = new Regex(@"<i>.*</i>"); /*greedy, no ? to stop*/ Regex htmlReg2 = new Regex(@"<i>.*?</i>"); /* ? to stop*/
	string html = "<i> Some.definitions</i> and also <i> words ,</i> then <i>plus terms</i> ugh.";
	MatchCollection regMatches = htmlReg.Matches(html); //greedy
	MatchCollection regMatches2 = htmlReg2.Matches(html); //lazy
	JOutputO(out out_sb_matches, regMatches, "Prac greedy");
	JOutputO(out out_sb_matches, regMatches2, "Prac lazy");
	
	//
	
	var m1 = Regex.Match("hello world", @"(?x)helloworld"); JOutputR(ref s1, m1, "m1"); //false
	var m2 = Regex.Match("hello world", @"(?x)hello world"); JOutputR(ref s1, m2, "m2"); //false, I think regex become 'helloworld' when (?x) 
	var m5 = Regex.Match("helloworld", @"(?x)hello world"); JOutputR(ref s1, m5, "m5"); //true
	var m6 = Regex.Match("helloworld", @"(?x)helloworld"); JOutputR(ref s1, m6, "m6"); //true 
	
	var m7 = Regex.Match("hello world", @"hello world"); JOutputR(ref s1, m7, "m7"); //true
	var m3 = Regex.Match("helloworld", @"helloworld"); JOutputR(ref s1, m3, "m3"); //true
	var m8 = Regex.Match("hello world", @"helloworld"); JOutputR(ref s1, m8, "m8"); //false  
	var m4 = Regex.Match("helloworld", @"hello world"); JOutputR(ref s1, m4, "m4"); //false
	
	//prac \s real quick. match whitespace 
	string whitespace = "This is\na bunch\tof    whitespace"; 
	Console.WriteLine (whitespace);
	JOutputO(out out_sb_matches, Regex.Matches(whitespace, @".+"), "Just prac \\s.");  
	//
	
	if(doRun) Console.WriteLine (s1.ToString());
	if(doRunOut) Console.WriteLine (outsb.ToString());
	Console.WriteLine(doMatchCollectionRun ? out_sb_matches.ToString() : " ");
	
}//end of " SimpleSimple() " method 

//a TON of organized well commented prac in this method
public void ZeroWidthPrac()
{   doMatchCollectionRun = true; doRunOut = false;
	
	StringBuilder matchesSb, amatchSb; 
	
	//prac ?=expr , rem expr is not included, but we can continue to express including expr if we want
	string s1 = "i'm Jay 25 age, but will be 22age when I wake up from this nightmare. Hello."; 
	MatchCollection mc1 = Regex.Matches(s1, @"\d+\s?(?=age)"); 
	JOutputO(out matchesSb, mc1, "Prac Positive lookahead, ?=expr"); 
	mc1 = Regex.Matches(s1, @"\d+(?=age).*?\."); 
	JOutputO(out matchesSb, mc1, "Prac Positive lookahead, ?=expr");
	//
	
	// Now prac {6,} 6chars or more with a pos lookahead
	string pw = "ji92ha"; 
	Match pwmatch = Regex.Match(pw, @"(?=.*\d){6,}"); //
	JOutputO(out amatchSb, pwmatch, "pass word ex.");
	//
	
	// now prac an inner neg lookahead with an alternator in there. 
	string neg = "You did Great work J, you did blahblah blah , but there was";
	string pos = "how You did great work J, you did blahblah blah , How awesome there was";
	string reg = @"(?i)Great(?!.*(but|however))";//cool how we used a string for the regex in this ex
	var mMatch = Regex.Match(neg, reg); 
	//JOutputO(out amatchSb, mMatch, "neg version"); 
	string regPre = @"(?i)(?<!(however|but).*)great";
	mMatch = Regex.Match(pos, regPre); 
	//JOutputO(out amatchSb, mMatch, "pos version"); 
	//JOutputO(out amatchSb, mMatch, "pre neg version");
	//
	
	// ^ startline and $ endline prac
	string start = "This is the start.", end = "and (now) if the (now) end."; 
	//JOutputO(out amatchSb, Regex.Match(start, @"(?i)^.*?[iI]s"), "^ prac"); //
	string docs1 = "a.txt"+"\r\n"+"b.doc"+"\r\n"+"c.txt"+"\r\n"+"thetext.txt";
	string docs2 = "a.txt\r\nb.doc\r\nc.txt\r\nthetext.txt";
	string regDoc = @"(?m).+\.txt(?=\r?$)"; //pos lookahead to catch \r, notice $ is inside ()
	//JOutputO(out matchesSb, Regex.Matches(docs2, regDoc), "Prac (?m) and $"); 
	string spaces = " \t\r\n\r\n  \nlast line";
	//JOutputO(out matchesSb, Regex.Matches(spaces, @"(?m)^[ \t]*(?=\r?$)"), "multi line and \\s prac"); 
	//
	
	// prac \b ... \b
	JOutputO(out matchesSb, Regex.Matches(start, @"\b\w+\b"), "\\b prac");
	//if we used look ahead then () should be placed after, notice lookbehind () before word we're matching 
	JOutputO(out matchesSb, Regex.Matches(end, @"(?<=\(now\))\s\b\w+\b"), "\\b prac with look behind");
	string think = "Thinking too much in thinking"; 
	Regex.Matches(think, @"in"); // 3 count
	Regex.Matches(think, @"\bin\b"); // 1 count 
	//
	
	// now prac grouping 
	string tel = "408-551-8032"; 
	var regGroup = Regex.Match(tel, @"(\d{3})-(\d{3}-\d{4})");
//	Console.WriteLine(regGroup.Groups[1]); //408
//	Console.WriteLine (regGroup.Groups[2]); //551-8032
	string innergroup = "poap, down, downed, Pope, peep, Juliuj, roar. scipoapa ."; 
	var mInnerGroup = Regex.Matches(innergroup, @"(?i)\b(\w)\w+\1\b");
	JOutputO(out matchesSb, mInnerGroup, "inner group prac, 1st letter of word has to match last"); 
	var mcInner = Regex.Match(innergroup, @"\b([pP]\w+)\b.*(\w{1,10}\1)");//I was trying to think of another
	//way to use an inner group and this was the only thing I could think of. 
//	Console.WriteLine(mcInner.Groups[1]); 
//	Console.WriteLine (mcInner.Groups[2]);
	//
	
	// prac named groups 
	string xml = "<sometag> Hello World (: </sometag>";
	string xmlregex = @"<(?'tag'\w+?).*>(?'text'.*?)</\k'tag'>";
	Match mxmlMatch = Regex.Match(xml, xmlregex);
//	Console.WriteLine (mxmlMatch.Groups["tag"]);
//	Console.WriteLine (mxmlMatch.Groups["text"]);
	//this next part is con. from pg.1002 that uses 2 named groups for output. 
	string xmlregReplace = @"<${tag} value = ""${text}"" />";//when @ in front "" "" = " " in output
	Console.WriteLine(Regex.Replace(xml, xmlregex, xmlregReplace));
    //Console.WriteLine ("\n" + @" ""Hello"" ... ""World"" :)" + "\n"); //just prac "" "" w/@ in front
	//
	
	
	//if(doMatchCollectionRun) Console.WriteLine (matchesSb.ToString());
	//if(doRunOut) Console.WriteLine (amatchSb.ToString());

}// END OF " public void ZeroWidthPrac() " METHOD 

void JSplit()
{ doMatchCollectionRun = false; doRunOut = true; 

	StringBuilder matchesSb = new StringBuilder(), matchSb = new StringBuilder();

	//practicing .Replace()
	string s2replace = "I am using someword with some meaning."; 
	string regReplace = @"\bsome\b", replaceWith = "impressive";
	//s2replace = Regex.Replace(s2replace, regReplace, replaceWith); 
	string hasReplaced = Regex.Replace(s2replace, regReplace, replaceWith); 
	//Console.WriteLine ("*Regex.Replace()*\t=\t" + hasReplaced);
	
	string replacegroup = "age 25 minus 3 becomes 22"; 
	var hasReplaced1 = Regex.Replace(replacegroup, @"(\d+)", "~$0>"); //I guess this is just a way to insert stuff
	var hasReplaced2 = Regex.Replace(replacegroup, @"(?'number'\d+)\s\b(?'word'\w+)\b", "<${number}><${word}>"); 
	//here I am trying to continue to isolate the input string so that I could, just for prac, enclose everything w/ <>
	hasReplaced2 = Regex.Replace(hasReplaced2, @"\s?(?<!<)(?'number'\d+)(?!>)\s?", "<${number}>");  //these can easily
	//next line does require '^' so that it only matches word in beginning of string
	hasReplaced2 = Regex.Replace(hasReplaced2, @"^(?<!<)(?'word'\w+)(?!<)\b", "<${word}>"); //be put in an if else tree
	//
	string namedReplace = Regex.Replace(replacegroup, @"(?'age'\d+)", "\"${age}years old\"");
	namedReplace = Regex.Replace(namedReplace, @"(?<=3)years old", "years"); //a lil prac2get just right, we don't want it to say "minus 3 years old"
	//
//	Console.WriteLine ();
//	Console.WriteLine(hasReplaced1);
//	Console.WriteLine(hasReplaced2);
//	Console.WriteLine(namedReplace);
	//
	
	// prac MatchEvaluator delegate 
	string myparse = "10 times 50 is 500.";
	string mMatchEval = Regex.Replace(myparse, @"\d+", (m => (int.Parse(m.Value) * 1000000).ToString() )  );
	//Console.WriteLine("MatchEval del prac: "+mMatchEval);
	//
	
	// .Split() prac
	StringBuilder sbSplit = new StringBuilder(); 
	string sint = "this1is/was4seperated9by2numbers", camel = "camelCaseString"; 
	string[] regexsplit = Regex.Split(sint, @"(?=\d)");//this will keep numbers in front
	string[] regexsplit2 = Regex.Split(sint, @"\d");//this will replace is digit with a space
	foreach(string s in regexsplit) sbSplit.Append(s + " ");
	sbSplit.Append("\n");//Just so that the next .Append is on a diff line
	foreach (string s in regexsplit2) sbSplit.Append(s + " ");
	//Console.WriteLine (sbSplit.ToString());
	//
	
	// 'Extracting "name=value" one per line' prac from Cookbook sec.
	string multi = @"Disciplined = Focus
	  Hardwork = Effort
	 Determined = Motivated";
	string multi2 = @"Just writing
			on multiple lines
	 for fun to see
		if I can, maybe I'll regex it. Lol";
	string regMulti = @"(?m)^\s*(?'name'\w+)\s*=\s*(?'value'\w+)\s*(?=\r?$)";
	MatchCollection strArr = Regex.Matches(multi, regMulti);
	JOutputO(out matchesSb, Regex.Matches(multi, regMulti), "prac multi lines and multiple named groups");
//	foreach(Match m in strArr)
//		Console.WriteLine (m.Groups["name"] + " is equal to " + m.Groups["value"]);
	//
	
	//strong pw prac from cookbook
	string pw = "ji92ha"; string myname = "My njulame is Julius";
	//JOutputO(out matchSb, Regex.Match(myname, @"(?i)\bJu(l|li|lius|lz)\b"), "Another Alternator prac"); 
	string pwRegex = @"(?x)^(?=.*(\d|\p{P}|\p{S})).{6,}";
	//Console.WriteLine("PW good? " + Regex.IsMatch(pw, pwRegex));
	//
	
	//prac 'Removing Repeated words' from cookbook
	string removeRepeatReg = @"(?ix)(?'repeat'\w+)\W\k'repeat'";
	string begin = "This this is the the the beginning..."; 
	MatchCollection mc = Regex.Matches(begin, removeRepeatReg); 
	begin = Regex.Replace(begin, removeRepeatReg, "${repeat}"); 
	//Console.WriteLine ("begin:\n"+begin);
	//
	
	//prac "Word Count" recipe 
	string regWordCount = @"\b(\w|[-'])+\b";
	MatchCollection wordCount = Regex.Matches("It's all Mumbo-Jumbo to me.", regWordCount);
	foreach(Match m in wordCount) Console.WriteLine (m.Value);
	//Console.WriteLine ("\nReg word count = " + wordCount.Count);
	//
	
	//"Unescaping word chars in a HTTP query" Prac from cookbook
	string http = "C%23 rocks";
	string httpReg = @"%[0-9a-f][0-9a-f]";
	http = Regex.Replace(http, httpReg,
	m => ((char)Convert.ToByte(m.Value.Substring(1), 16)).ToString() + " Dude!", RegexOptions.IgnoreCase); 
	Console.WriteLine (http);
	//
	
	
	Console.WriteLine ();
	if(doMatchCollectionRun) Console.WriteLine (matchesSb.ToString());
	else if(doRunOut) Console.WriteLine (matchSb.ToString());
	Console.WriteLine ();
	Console.WriteLine ();
//	Console.WriteLine (multi2);

} // END OF "void JSplit()"


//a lot of MSDN prac in this method 
void MoreRegPrac()
{	doMatchCollectionRun = true;

	StringBuilder matchesSb = new StringBuilder(); 
	
	//prac conditional named group match from MSDN "Alternator Constructs in Regular Expressions"
	string condstr = "Wasn't word1 such a nice explanation?";
	string social = "77-1112223 followed 12-1234567 and then 1234-1234 by 123-12-3456";
	string regcond = @"\b(?<n2>\d{2}-)*(?(n2)\d{7}|\d{3}-\d{2}-\d{4})\b";
	string regcond2 = @"\b(?(\d{2}-)\d{2}-\d{7}|\d{3}-\d{2}-\d{4})\b"; 
	string regword = @"(?i)\b(?(\sword\d\s)\sword\d\ssuch\s|Wasn't\sword\d)\b";
	var reg1 = Regex.Match(condstr, @"\sword\d\s");
//	Console.WriteLine (reg1.Success + " " + reg1.Index);
	Console.WriteLine ();
	//JOutputO(out matchesSb, Regex.Matches(condstr, regword), "Named conditional group prac");
	//
	
	//prac from MSDN sub.sec."Balancing Group Definitions"
	string input = "<abc><mno<xyz>>"; 
	string pattern = @"^[^<>]*(((?'Open'<)[^<>]*)+ ((?'Close-Open'>)[^<>]* )+ )*(?(Open)(?!))$";
	//                        123        3      2  ab              b       a  1 x y    yz  zx
	
	string pattern2 = @"^[^<>]*(((?'open'<)[^<>]*)+((?'close-open'>)[^<>]*)+)*(?(open)(?!))$";
	Match groupBal = Regex.Match(input, pattern2, RegexOptions.IgnorePatternWhitespace);
	if(groupBal.Success)
	{
		Console.WriteLine ("Input: "+input+" \nMatch: "+groupBal);
		Console.WriteLine ();
		int grpCtr = 0;
		foreach(Group group in groupBal.Groups)
		{ 
			Console.WriteLine("Group index: " + grpCtr + ", " + group.Value);
			grpCtr++; 
			int capCtr = 0;
			foreach(Capture capture in group.Captures)
			{
				Console.WriteLine("\tCapture index: " + capCtr + ", " + capture.Value);
				capCtr++; 
			}
		}
	}
	// END OF "Balancing Group Definitions" PRAC
	
	if(doMatchCollectionRun) Console.WriteLine (matchesSb.ToString());
}


//--------------------------------------------------------------------
#region  out and ref pars methods here 
public void JOutputR(ref StringBuilder sb, Match mMatch2, string rv)
{	
	sb.Append("\n"+rv+"\n"); 
	sb.AppendLine(mMatch2.Success +  " ");
	sb.AppendLine(mMatch2.Length + " = .Length"); 
	sb.AppendLine(mMatch2.Index + " = .Index");
	sb.AppendLine(mMatch2.Value + " = .Value"); 	
}

public void JOutputO(out StringBuilder sb2, Match mMatch2, string rv)
{
	sb2 = new StringBuilder(); 
	
	sb2.Append(rv+"\n"); 
	sb2.AppendLine(mMatch2.Success + " = .Success");
	sb2.AppendLine(mMatch2.Length + " = .Length"); 
	sb2.AppendLine(mMatch2.Index + " = .Index");
	sb2.AppendLine(mMatch2.Value + " = .Value"); 
}

public void JOutputO(out StringBuilder sb2, MatchCollection ms, string rv)
{
	sb2 = new StringBuilder(); 
	
	sb2.AppendLine(ms.Count+" = this.MatchCollection.Count");
	sb2.Append(rv+"\n\n"); 
	foreach(Match match in ms)
	{
		sb2.AppendLine(match.Success + " = .Success");
		sb2.AppendLine(match.Length + " = .Length"); 
		sb2.AppendLine(match.Index + " = .Index");
		sb2.AppendLine(match.Value + " = .Value"); 
		sb2.AppendLine(); 
	}
}
#endregion