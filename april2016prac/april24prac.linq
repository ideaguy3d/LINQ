<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

var mylist = new List<string>() {"Apples", "Pears", "Mangos"};

var customers = new [] {
	new {name="Julius", email="julius@t3direct.com"},
	new {name="Villi", email=""},
	new {name="Nick", email="nick@t3direct.com"},
	new {name="Al", email="ceo@t3Direct.com"},
	new {name="Anthony", email=""}
};

foreach (var customer in customers.Where (c => !String.IsNullOrEmpty(c.email)))
{
	Console.WriteLine("Sending email to: {0}", customer.email);
}

foreach (var customer in 
	from c in customers 
	where !String.IsNullOrEmpty(c.email) 
	select c)
{
		
}