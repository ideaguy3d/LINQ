<Query Kind="SQL">
  <Connection>
    <ID>002fff27-4e94-4568-b6aa-35f569d4bb5d</ID>
    <Persist>true</Persist>
    <Server>localhost</Server>
    <NoPluralization>true</NoPluralization>
    <NoCapitalization>true</NoCapitalization>
    <Database>TutorialDB</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

INSERT INTO Customers (CustomerId, Name, Location, Email)
VALUES
	(8, 'Julius', 'California', 'julius@rsmail.com'),
	(9, 'Micheal', 'California', 'mhemphill@rsmail.com'),
	(10, 'Ray', 'California', 'ray@rsmail.com'); 
	
SELECT * FROM Customers;