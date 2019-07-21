<Query Kind="Statements">
  <Connection>
    <ID>6cb2ec1c-65dc-4a98-bbbd-ea9e33009709</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>Server=localhost;Database=jtweet;Uid=root</CustomCxString>
    <Server>localhost</Server>
    <DisplayName>1and1_julius-wp</DisplayName>
    <Database>jtweet</Database>
    <UserName>root</UserName>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
    </DriverData>
  </Connection>
</Query>

// tweets table
var tweets = from t in Tweets select t;
// users table
var users = from u in Users select u; 

foreach (var t in tweets) {
	var userName = users.First (u => u.Id == t.Userid);
	var userNameSimple = Regex.Match(userName.Email, @".*@");
	var userNameSimpleStr = userNameSimple+"jtweets"; 
	userNameSimpleStr = userNameSimpleStr.ToString().Remove(userNameSimpleStr.ToString().IndexOf('@'), 8); 
	
	Console.WriteLine(); 
	Console.Write("<strong>"+userNameSimpleStr+" says "+t.Tweet+"</strong>"); 
}