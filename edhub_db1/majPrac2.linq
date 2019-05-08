<Query Kind="SQL">
  <Connection>
    <ID>61e9f10d-bc20-4f15-8e6f-7f56fff94e67</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAn1kqCmv9akarwJUn1C3DfgAAAAACAAAAAAAQZgAAAAEAACAAAADixj2pubBIru0Rl5IcFCgBA9Qcmz0ffG5b+JmTCjqmBgAAAAAOgAAAAAIAACAAAACiMfmkUCUFqG8XHW7ffQ4QW2LgrrLmj+vBOcTr7BKRBVAAAADwyFunPYhLbLlq/JwAs960DkVI53NvEyWN2zEUO+yVPH1koKkbE2ilBtS8drerPdqv71itbR7Zkoz7m/0GMqBkZw/quIKltZ/km28MLvo1ekAAAABZNcj+ww5SstdypRT10Um20556DBEowTjDKK73BTJNnKS4SGTKw2BcRAy7V0mBMT8AIzdmd3esJzSDgKaxS6D2</CustomCxString>
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

/* prac count vs group by */

select 
	date(timestamp) as 'the date',
	merchant_order_id as 'Merchant Order ID'
from majide_fba_sales_v1
limit 20;	

/* find the total sales by day */
select 
	date(timestamp) as 'the date',
	round(sum(item_price),2) as 'total sales'
from majide_fba_sales_v1
group by 1
order by 1 desc;