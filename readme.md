# Setup database
    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=W@ffles!123" --name sqlserver -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest 


The username will be sa and the password W@ffles!123



## Database connection string
    Server=localhost;Database=OE_Tenant_11;User Id=sa;Password=W@ffles!123;Trusted_Connection=False;