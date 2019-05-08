<Query Kind="SQL">
  <Connection>
    <ID>61e9f10d-bc20-4f15-8e6f-7f56fff94e67</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAn1kqCmv9akarwJUn1C3DfgAAAAACAAAAAAAQZgAAAAEAACAAAADKHBY+E6Pqi2pQEN6ABfOWiyDEJcMQiDBnNkxRcvmKCgAAAAAOgAAAAAIAACAAAADjWE1iiNLxTMgswthfyiuuIGC5VdrI7BgFb5lGX+EnFVAAAAAYCWE0M4LDn40bCbWkxFg+OXwNApolECNKTUWRICl9VF57t3703+nSq3zCtQSpebbZilizwkHcM017keD/rEt/TWqGQi1rQPtzcu7KXipXYkAAAABuRSQil/vqztxpZ8jU0nZ1x2WxdyAqwFq2uzTwUNXR24FKQi4m6MvlsbY2wRyC4ejadgrX4LOCgpzZoeEWXP1x</CustomCxString>
    <Server>127.0.0.1</Server>
    <Database>edhub_db1</Database>
    <UserName>edhub-admin</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAn1kqCmv9akarwJUn1C3DfgAAAAACAAAAAAAQZgAAAAEAACAAAAAOyvrr2vdehz+QdrEl99sUdEyPQB0kA12ius+MbYpQcgAAAAAOgAAAAAIAACAAAAB0WWGWfwDsd1uwXfjIvWne90koPru8lqAq8ep7y8DOLhAAAAD6rh5x3us6BVBy8sRHLqtkQAAAAKa4RU900qDNimifAzb+AGPu26exX3MTHLinGEbLNZW/shcTDl1WZk4She4/GUI16cgunNvdkv5Oe7ggF8cwSJE=</Password>
    <NoPluralization>true</NoPluralization>
    <NoCapitalization>true</NoCapitalization>
    <DisplayName>edhub_db1_v1</DisplayName>
    <EncryptCustomCxString>true</EncryptCustomCxString>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
    </DriverData>
  </Connection>
</Query>

UPDATE books 
SET author = 'Julius Alvarado'
WHERE author = 'julius';

UPDATE books 
SET created_by = 'ja'
WHERE created_by IS NULL;

SELECT * FROM books;