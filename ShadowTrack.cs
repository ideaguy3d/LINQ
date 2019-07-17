void Main()
{

	// put in all the mins from the inbetween quickly noted lost time from the ShadowTrack
	// into this array ......Right Below Here.......
	int[] minArray = { 0 };
	//if there happens to be spans of an hour or more plug into this array
	int[] hourArray = { 0 };

	//this will be used like "hourArray2 minArray2 - hourArray minArray", typical use case would be when I have an 
	//amount of time being subtracted by another amount of time e.g. 4h28m - 2h37m...
	int[] hourArray2 = { 0 }, minArray2 = { 0 };

	float[] hourFloatArray = { };

	string timeDifference = ShadowTrack.TimeAdd(9, 50, "am", 3, 58, "pm"); // Enter data here <<<<<<<<<<<<<<<<<<<
	string studyTime = ShadowTrack.StudyAdd(hourArray, minArray, "+");  // and here <<<<<<<<<<<<<<<<<<
	float studyTimeFloat = ShadowTrack.StudyAdd(hourFloatArray);

	#region ... display the data ...

	float totalCalc = (ShadowTrack.StudyAdd(hourArray2, minArray2) - ShadowTrack.StudyAdd(hourArray, minArray));

	Console.WriteLine("\n h2m2 - h1m1... \n" + ShadowTrack.StudyAdd(hourArray2, minArray2) + "mins - " +
		ShadowTrack.StudyAdd(hourArray, minArray) + "mins = " +
		totalCalc + "mins or " + Math.Round((totalCalc / 60.0f), 2) + "hours\n\n"
	);

	Console.WriteLine("Added float +time = " + studyTimeFloat.ToString() + "hours\n\n");

	Console.WriteLine(timeDifference + "\n");//the par contains info and display message about times  

	#endregion
	//_____________________________________END____________________________________________
}

//---------------------------Define other methods and classes here-----------------------------
#region

public float QuickStudyProjection(int x)
{   // allowedLostTime includes household chores, prep&eat, Bible.s, ehh&in-between time, etc.
	// jobTime includes estimated drive time, set up time & subtle in-between time(e.g. walking from my car to class, etc.)
	float allowedLostTime = 2.5f, gymTime = 2.5f, jobTime = 4.5f, girlTime = 1.5f, hoursAwake = 16.0f;

	// I will ALWAYS include girlTime AND allowedLostTime, if I happen to not waste any time on girls then that will 
	// automatically
	// increase my score and help reach Goal time, IF I happen to be above Goal time then obviously 
	// that would be GREAT!!! ^_^                             |||
	float GOALTime_1 = hoursAwake - (allowedLostTime + girlTime + gymTime + jobTime);// includes job&gym time 
	float GOALTime_2 = hoursAwake - (allowedLostTime + girlTime); // <NO> job&gym time
	float GOALTime_3 = hoursAwake - (allowedLostTime + girlTime + gymTime); // JUST gym time
	float GOALTime_4 = hoursAwake - (allowedLostTime + girlTime + jobTime); // JUST job time

	if (x == 1) return GOALTime_1;
	else if (x == 2) return GOALTime_2;
	else if (x == 3) return GOALTime_3;
	else if (x == 4) return GOALTime_4;
	else return 0.0f;
}

public class ShadowTrack
{
	// 6-1-15@9:27am, I am tracking my HORRIFIC t3w/green/+ time these past 3+months since living in this shithole w/my dad
	static float[] greenResults = { 26.0f, 25.3f, 30.6f, 23.3f, 42.1f, 14.1f, 18.9f, 18.5f, 9.1f, 15.7f, 22.4f, 19.2f };
	public static List<float> greenTime = new List<float>(greenResults);
	public static List<float> GreenTimeWeeklyResults()
	{
		return greenTime;
	}

	static int totalTimeLost;
	static int timeDifferenceTotal;

	public static int TotalTimeLost // REM: this property is in total mins, if I want hours
	{                               // and mins I would have to do the math. 
		get
		{
			return ShadowTrack.totalTimeLost;
		}
		set
		{
			if (value != 0)
			{
				//Console.WriteLine("Hurray value doesn't equal 0"); 
				ShadowTrack.totalTimeLost = value;
			}
		}
	}

	public static int TimeDifferenceTotal //diff in from time A to time B (eg.5:22pm to 9:17pm)
	{
		get
		{
			return timeDifferenceTotal;
		}
		set
		{
			if (value != 0) timeDifferenceTotal = value; // this would be a good spot to throw an exception 
		}
	}

	public static string HourMinsDifferenceString
	{
		get
		{
			Console.Write(ShadowTrack.TimeDifferenceTotal + " - " + ShadowTrack.TotalTimeLost + "... ");
			int totalMins = ShadowTrack.TimeDifferenceTotal - ShadowTrack.TotalTimeLost;//this is calculating
			int hourA1 = totalMins / 60; //the last TimeAdd() minus the last StudyAdd()
			int minsA1 = totalMins % 60; //so it is pretty hard coded. 
			return hourA1 + "hour(s) " + minsA1 + "mins";
		}
	}

	// the difference from 5:47pm to 9:04pm for example 
	//... also 11:19am to 1:02pm or 11:07pm to 2:33am 
	public static string TimeAdd(int hour1, int min1, string am, int hour2, int min2, string pm)
	{

		//hour1 and hour2 for the 12hour format stuff so that I can display orig w/o changes
		int hour1copy = hour1, hour2copy = hour2; //since I make changes to original


		int hourDiff = 0;
		int minDiff = 0;

		if (hour1 == 12) hour1 = 0;

		if ((am == "am" && pm == "am") || (am == "pm" && pm == "pm"))//no conversions needed
		{
			if (am == "pm" && pm == "pm")
			{
				if ((hour1 > hour2) || (hour2 == 12))
				{
					if (hour2 != 12) hour2 += 12;
					hourDiff = 12 + hour2 - hour1;
				}
				else if (hour1 < hour2) hourDiff = hour2 - hour1;
			}
			else if ((am == "am" && pm == "am"))
			{
				if ((hour1 > hour2) || (hour2 == 12))
				{
					if (hour2 != 12) hour2 += 12;
					hourDiff = 12 + hour2 - hour1;
				}
				else if (hour1 < hour2) hourDiff = hour2 - hour1;
			}
		}
		else if (am == "am" && pm == "pm")//if we're going from am to pm
		{
			if (hour2 != 12) hour2 += 12; //11am to 2pm = 3hours... just as 14-11=3hours as well
			hourDiff = hour2 - hour1;
		}
		else if (am == "pm" && pm == "am")//if we're going from pm to am 
		{
			if (hour2 != 12) hour2 += 12; //11pm to 2am = 3hours... just as 11-14= ?hours as well
			hourDiff = hour2 - hour1;
		}

		if (hour1 == 12) hour1 = 12;
		hour2 -= 12;
		minDiff = min2 - min1; //4-47 = -43
		int hourDiffInMins = hourDiff * 60;

		if (minDiff < 0)
		{
			hourDiffInMins += minDiff;
			//I should probably just make this next calc its own method since I 
			//seem to use it so much to get correct hours and mins 
			hourDiff = hourDiffInMins / 60;
			minDiff = hourDiffInMins % 60;
		}

		//here I am setting the TimeDifferenceTotal property so that I 
		//can 6hours 11mins - 2hour 42mins for example 
		ShadowTrack.TimeDifferenceTotal = (hourDiff * 60) + minDiff;

		//this is for formatting for the console window
		string min1_s = (min1 < 10) ? "0" + min1 : min1.ToString();
		string min2_s = (min2 < 10) ? "0" + min2 : min2.ToString();
		//

		return "Time Diff from " + hour1copy + ":" + min1_s + am + " to "
		+ hour2copy + ":" + min2_s + pm + " = " + hourDiff + "hour(s) " + minDiff + "mins";

	}// END OF "public static string TimeAdd(int hour1, int min1, string am, int hour2, int min2, string pm)" method

	//this method is where I would add my lost time and figure out how much time was 
	//spent positively as well. 
	public static string StudyAdd(int[] hour, int[] min, string plusOrMinus)
	{
		int totalMin = 0;
		foreach (int temp in min)
		{
			totalMin += temp;
		}

		int totalHour = 0;
		foreach (int temp in hour)
		{
			totalHour += temp;
		}
		totalHour = totalHour * 60;
		int totalTimeInMins = totalHour + totalMin; //should have combined total lost time in 
													//minutes
		totalHour = totalTimeInMins / 60;
		int remainderInMins = totalTimeInMins % 60;
		ShadowTrack.TotalTimeLost = totalTimeInMins; //setting this class's property

		//right here I am just testing to make sure that this.Class's property has been set
		//Console.WriteLine("test1... TotalTimeLost for this.Class = " + ShadowTrack.TotalTimeLost); 

		return plusOrMinus + totalHour + "hour(s) " + remainderInMins + "mins";
	}// END OF "public static string StudyAdd(int[] hour, int[] min, string plusOrMinus)" method

	//this is an overloaded method 
	public static int StudyAdd(int[] hour, int[] min)
	{
		int totalMin = 0;
		foreach (int temp in min)
		{
			totalMin += temp;
		}

		int totalHour = 0;
		foreach (int temp in hour)
		{
			totalHour += temp;
		}
		totalHour = totalHour * 60;
		int totalTimeInMins = totalHour + totalMin; //should have combined total lost time in 
													//minutes
		totalHour = totalTimeInMins / 60;
		int remainderInMins = totalTimeInMins % 60;
		ShadowTrack.TotalTimeLost = totalTimeInMins; //setting this class's property

		//right here I am just testing to make sure that this.Class's property has been set
		//		Console.WriteLine("test1... TotalTimeLost for this.Class = " + ShadowTrack.TotalTimeLost); 

		return totalTimeInMins;
	}// END OF "public static string StudyAdd(int[] hour, int[] min, string plusOrMinus)" method

	public static float StudyAdd(float[] hourFloats)
	{
		float totalFloat = 0.0f;
		foreach (float element in hourFloats)
		{
			totalFloat += element;
		}
		return totalFloat;
	}
	
}//	 END OF "public protected class ShadowTrack{}" class

#endregion