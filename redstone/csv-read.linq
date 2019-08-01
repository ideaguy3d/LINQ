<Query Kind="Program" />

void Main()
{
	string csv = @"C:\Users\julius\Desktop\alloc-item.csv";
	
	var csvData = from row in MyExtensions.ReadFrom(csv).Skip(1)
	let columns = row.Split(',')
	select new
	{
		ID = int.Parse(columns[0]),
		Name = columns[1],
		Email = columns[2]
	};

	csvData.Dump();
}

// Define other methods and classes here
