<Query Kind="SQL">
  <Connection>
    <ID>ca46296e-0f44-4357-b6ad-036bc050b4db</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAtdXET0A3JkGabfEdU099LwAAAAACAAAAAAAQZgAAAAEAACAAAAB0/Uv4blwNTH6CMGYmi2ItsoFDR3wZAVUHhZCkVvoZgQAAAAAOgAAAAAIAACAAAAD3ngYur/2QcH8Xbm47o3uOiGLg4fvRaPs+/33S7LsujGAAAACjUHaewg2DZd/lKni6m5+3ej6EqssBPpvDP2u7a352nAGgasmwHIumuEThPqjTFtPnMAWaOnsbN4nFJ6N01NlHlAL5owwXYMUC/Ssd8sWaEDrj/3n5TXQ06IXulOt3aLJAAAAA016mTycBpyvh+MYgMPEv02/cP4FPTNQCaW0QWcVoxUs4KipJbCKrEwpiP+hR/esuCvqBQ1xcSgo1j//bX3Lh9A==</CustomCxString>
    <NoCapitalization>true</NoCapitalization>
    <NoPluralization>true</NoPluralization>
    <Database>juliusphp112</Database>
    <UserName>be019174718b98</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAtdXET0A3JkGabfEdU099LwAAAAACAAAAAAAQZgAAAAEAACAAAABslIrz4rlnzL/sYhJtcbjl/BjVZZ0kfhcG59AEikQE/AAAAAAOgAAAAAIAACAAAAAEfBZcH3sRSjGtJI+aU0Atb5FHq7Drbn1Li0srsT1xvBAAAAAK7cU7SAEJuhJgBFx1FcY6QAAAAGEwNdypij2jd3ht7D2OcxNieUXDwVOtoIn3KjTpzopv27BmWjx0Ra6hbo5OfzL+TQb0cJSn2iAgs+dbV0t64kc=</Password>
    <DisplayName>Julius PHP 112</DisplayName>
    <Server>us-cdbr-azure-east-c.cloudapp.net</Server>
    <EncryptCustomCxString>true</EncryptCustomCxString>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
    </DriverData>
  </Connection>
</Query>



select * from movies where year between 1990 and 2009 and genre="comedy";
select * from movies where year between 1999 and 2009; 
select * from movies where name like '%G_%';
select * from movies where name like 'Se_en';
select * from movies where imdb_rating > 8.5;
select company, email from leads; 
select distinct genre from movies;
