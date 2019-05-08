<Query Kind="Program" />

void Main()
{
	string cr = Environment.NewLine;
	
	string input = 
	"Brooklyn Dodgers, National League, 1911, 1912, 1932-1957" + cr +
	"Detroit Tigers, American League, 1960-present" + cr +
	"New York Giants, American League, 1895-1950" + cr +
	"Hello 4, there  dddee, World" + cr +
	"Washing Senators, National League, 1886, 1901-1960";
	
	string pattern =
	@"^(?'team'(\w+(\s?)){2,})" +
	@",\s" +
	@"(?'league'\w+\s\w+)," +
	@"(?'years'\s\d{4}" + //5th open
	@"(-(\d{4}|present)" + //6th open nd alternator group
	@")?,?)+" ;//5th nd 6th closed 
	
	
	Match match; 
	
	string[] teams = input.Split(new String[] {cr}, StringSplitOptions.RemoveEmptyEntries); 
	
	foreach(string team in teams) 
	{	
		if(team.Length > 70) continue; // WHY DO WE DO THIS?!?
		Console.WriteLine ("\n:) ------------------------------- (: \n");
		
		match = Regex.Match(team, pattern); //rem each iteration match will change 
		
		while (match.Success)
		{
			Console.WriteLine("The {0} were in the {1} from", 
				match.Groups["team"].Value, match.Groups["league"].Value);
		
			foreach(Capture capture in match.Groups["years"].Captures)
				Console.WriteLine(capture.Value);
				
			match = match.NextMatch(); 
		} 
	}
}

// Define other methods and classes here
