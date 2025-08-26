# Variables
You need to add a .env files with the CONNECTION_STRING and MSSQL_PASSWORD variables

The CONNECTION_STRING variable has to be in the following format:
Server=mssql-db,1433;Database=_Chosen Database_;User Id=sa;Password=_Chosen Password_;TrustServerCertificate=True;

The MSSQL_PASSWORD can be anything really.

Also need to set up a .env file in the frontend folder with the variable VITE_API_URL and the value http://localhost:5000/api
