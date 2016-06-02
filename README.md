# Paybook Lite.NET - C# - WEB API

**Overview**
----
  This is an example that shows how can we consume Paybook SDK
  
**Configuration**
----
  Set your api_key in appSettings section in Web.Config file
  
      <appSettings >
        <add key="API_KEY" value="YOUR_API_KEY_GOES_HERE" />
	  </appSettings >
	  
  Set up your SQLite database into AppData folder
      paybook.db
  
**Prerequisites**
----
    -- Visual Studio 2015
    -- Microsoft .NET Framework 4.6
    -- Your server must have support for WEB API
	-- Entity Framework 6 
	-- All data tables must have at least one primary key in order to use Entity Framework 6

**Execution from Source code**
----
    Open in Visual Studio pb_net_lite/pb_net_api/pb_net_api.sln
    Press F5 to run the pb_net_api project
	Under API View you will see all Web Api help page with all documented webApi methods
	
**Signup**
----
  Register a new user to Paybook

* **URL**

  http://<_your_ip_>:5000/api/User/signup

* **Method:**
  
  <_Content-type:application/json_>

  `POST` 

   **Required:**
 
   `username=[string]`
   `password=[string]`

* **Success Response:**
  
   `{'signed_up':true}`
 
* **Error Response:**
  
  `{'signed_up':false}`


* **Sample Call:**

 ```curl
  http://<_your_ip_>:5000/api/user/signup?username=[someusername]&password=[somepassword]
 ```

**Login**
----
  Login an exisiting user

* **URL**

  http://<_your_ip_>:5000/api/User/login

* **Method:**
  
  <_Content-type:application/json_>

  `POST` 

   **Required:**
 
   `username=[string]`
   `password=[string]`

* **Success Response:**
  
   `{'token':'your_session_token'}`
 
* **Error Response:**
  
  `{'token':false}`


* **Sample Call:**

 ```curl
  http://<_your_ip_>:5000/api/User/login?username=[someusername]&password=[somepassword]
 ```

**Catalogs**
----
  Retrieve the set of institutions available

* **URL**

  http://<_your_ip_>:5000/api/Catalogs

* **Method:**
  
  <_Content-type:application/json_>

  `GET`

   **Required:**
 
   `token=[string]`

* **Success Response:**
  
   `{'catalogs':[Catalogs]}`
 
* **Error Response:**
  
  `{'catalogs':[]}`


* **Sample Call:**

 ```
  http://<_your_ip_>:5000/api/catalogs/get?token=[yourtoken]
  ```

**Credentials**
----
  Register credentials for a specific institution

* **URL**

  http://<_your_ip_>:5000/api/credentials/credentials

* **Method:**
  
  <_Content-type:application/json_>

  `POST`

   **Required:**
 
   `token=[string]`
   `id_site=[string]`
   `credentials_user=[string]`
   `credentials_password=[string]`

* **Success Response:**
  
   `{'credentials':NEW_CREDENTIALS}`
 
* **Error Response:**
  
  {'catalogs':false}`


* **Sample Call:**

 ```
    Request JSON object parameter
 
	{
	  "token":"yourtoken",
	  "name":"somename",
	  "id_site":"someidsite",
	  "credentials":{
	     "contract":"somecontract",
	     "password":"somepassword"
	  }
	}
	
	credentials parameter is an example, but it is dinamyc according to institution
	
	 ```
     http://<_your_ip_>:5000/api/credentials/credentials
     ```
  ```

**Status**
----
  Get the sync status of a specific institution

* **URL**

  http://<_your_ip_>:5000/api/credentials/status

* **Method:**
  
  <_Content-type:application/json_>

  `GET`

   **Required:**
 
   `token=[string]`
   `id_site=[string]`

* **Success Response:**
  
   `{'status':STATUS}`
 
* **Error Response:**
  
  {'status':false}`


* **Sample Call:**

 ```
   http://<_your_ip_>:5000/api/credentials/status?token=sometoken
  ```
  
**Accounts**
----
  Get the accounts registered in a specific institution

* **URL**

  http://<_your_ip_>:5000/api/accounts/accounts

* **Method:**
  
  <_Content-type:application/json_>

  `GET`

   **Required:**
 
   `token=[string]`
   `id_site=[string]`

* **Success Response:**
  
   `{'accounts':[Accounts]}`
 
* **Error Response:**
  
  {'accounts':[]}`


* **Sample Call:**

 ```
  http://<_your_ip_>:5000/api/accounts/accounts?token=sometoken&id_site=someidsite
  ```
  
**Transactions**
----
  Get the accounts registered in a specific account

* **URL**

  http://<_your_ip_>:5000/api/transactions/transactions

* **Method:**
  
  <_Content-type:application/json_>

  `GET`

   **Required:**
 
   `token=[string]`
   `id_account=[string]`

* **Success Response:**
  
   `{'transactions':[Transactions]}`
 
* **Error Response:**
  
  {'transactions':[]}`


* **Sample Call:**

 ```
 http://<_your_ip_>:5000/api/transactions/transactions?token=sometoken&id_account=someidaccount

  ```
