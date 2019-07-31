<Query Kind="Program">
  <Connection>
    <ID>eeb5e8df-091c-4835-9d09-0b24d40c73f3</ID>
    <Persist>true</Persist>
    <Server>192.168.7.16\RSMAUTO</Server>
    <SqlSecurity>true</SqlSecurity>
    <NoPluralization>true</NoPluralization>
    <NoCapitalization>true</NoCapitalization>
    <UserName>mhetadata</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAd7vEYFX2c0+7mvUO4qulmAAAAAACAAAAAAAQZgAAAAEAACAAAADZN5u6dsj0npZhTgRgRd/rMjhTXaa61r0HhSdst6lahQAAAAAOgAAAAAIAACAAAACplfeSc3YHEV1unL4VEypb+thW8pQ0OcANc4ec54KnDxAAAADHV2cldZxm9Ce6wJV7o8+LQAAAAOf2s8tTwr4AoPd+KpxyVVpDjfh7U8NPT0BPvVLJ3vdvV7sWlLTFZy6dWd1hPMIWTCUUL+psSPapwd8lt7eLBz4=</Password>
    <IsProduction>true</IsProduction>
    <Database>RSMint_1</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{

	RemovedEncodes.Take(100).Dump(); 
	
}

// Define other methods and classes here
public void oneBasic()
{
	123.Dump();

	Regex.Match("paper and envelope color is...", "colou?r").Dump();   // LINQPad is great for Regex testing!

	// Dump accepts an optional title for formatting:

	TimeZoneInfo.Local.Dump("Bet you never knew this type existed!");

	// Dump returns exactly what it was given, so you can sneakily inject
	// a Dump (or even many Dumps) *within* an expression. This is useful
	// for monitoring a query as it progresses:

	new[] { 11, 5, 17, 7, 13 }.Dump("Prime numbers")
	.Where(n => n > 10).Dump("Prime numbers > 10")
	.OrderBy(n => n).Dump("Prime numbers > 10 sorted")
	.Select(n => n * 10).Dump("Prime numbers > 10 sorted, times 10!");

	// Or you can do this:
	DateTime now = DateTime.Now.Dump();
}

