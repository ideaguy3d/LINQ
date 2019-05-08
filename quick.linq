<Query Kind="Program" />

void Main()
{
	int x = 0; 
	StringBuilder sb = new StringBuilder("hi");
	
	while(true)
	{
		x++;
		if(x == 12) break;
		sb.Append(" "+x.ToString());
	}
	Console.Write(sb.ToString());
}

// Define other methods and classes here
