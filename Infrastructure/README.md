# Variables
You need to add a .env files with the CONNECTION_STRING, MSSQL_PASSWORD and COMPOSE_PROJECT_NAME variables

The CONNECTION_STRING variable has to be in the following format:
Server=mssql-db,1433;Database=_Chosen Database_;User Id=sa;Password=_Chosen Password_;TrustServerCertificate=True;
The password has to be the same as the MSSQL_PASSWORD variable.

The MSSQL_PASSWORD can be anything really.

COMPOSE_PROJECT_NAME should be named note-webapp. This variable is just so we dont need to specify the name of the compose wrapper container each time we write docker compose up

Also need to set up a .env file in the frontend folder with the variable VITE_API_URL and the value http://localhost:5000/api



# User-secret
Add the user-secret for local development

# Migration
Apply migrations in EF Core under /Backend_API so the database gets crated with the necessary relationships

# Frontend setup
Remember to write npn install under /frontend