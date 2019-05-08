<Query Kind="Program">
  <Namespace>System</Namespace>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Text</Namespace>
</Query>

void Main()
{
	LINQPrac1 pracObj = new LINQPrac1();
	
	pracObj.Start(); 

}

// Define other methods and classes here
public enum CarColor
	{
		White,
		Black,
		Red,
		Blue,
		Yellow
	}
	
	public class Car
	{
		public string Make { get; set; }
		public string Model { get; set; }
		public int Year { get; set; }
		public double StickerPrice { get; set; }
		public CarColor Color { get; set; }//we use an enum for this 
	}
	
	public class LINQPrac1 
	{	
		public void Start()
		{
			var myCars = new List<Car> 
			{
				new Car() { Make="BMW", Model="550i", Color=CarColor.Blue, StickerPrice=55000, Year=2009 },
				new Car() { Make="Toyota", Model="4Runner", Color=CarColor.White, StickerPrice=35000, Year=2010 },
				new Car() { Make="BMW", Model="745li", Color=CarColor.Black, StickerPrice=75000, Year=2008 },
				new Car() { Make="Ford", Model="Escape", Color=CarColor.White, StickerPrice=28000, Year=2008 },
				new Car() { Make="BMW", Model="550i", Color=CarColor.Black, StickerPrice=57000, Year=2010 },
				new Car() { Make="Honda", Model="Civic", Color=CarColor.Red, StickerPrice = 25000, Year=2009},
				new Car() { Make="Honda", Model="Accord", Color=CarColor.Black, StickerPrice=35000, Year=2011}
			};
			
			//  This is Query Syntax. 
			var bmw = from car in myCars 
					  where car.Make=="BMW" && car.Year==2010 
					  select car; 
					  
			var newCars = from car in myCars 
						  where car.Year>2009 
						  select new {car.Make, car.Model, car.Year};
			
			var orderedCars = from car in myCars 
							  orderby car.Year descending //descending is only used if I want it to go from newest to oldest. 'orderby' is a keyword. 
							  select car; 
			
			
			// this is fluent syntax
			var _bmw = myCars.Where(car => car.Year == 2010).Where(car => car.Model == "BMW");
			var _orderedCar = myCars.OrderByDescending(car => car.Year);//I think he said => means "return the value"
			
			foreach (var car in _orderedCar)
			{
				Console.WriteLine("Car Make:" + car.Make + " Model: " + car.Model + " Year: " + car.Year);
			}
			
			var sums = myCars.Sum (p=>p.StickerPrice);
			Console.WriteLine ("Total inventory value: " + sums);
			
			bmw.Dump(); newCars.Dump(); orderedCars.Dump(); 
			
		}// END OF "public void Start()" 
	}