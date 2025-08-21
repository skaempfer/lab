# Aspire MinimalAPI SQL

This project demonstrates a basic [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/) application consisting of

1. a MinimalAPI web application and
2. an Microsoft SQL server that serves as the data store for the application

There are several points of interest here:

1. The orchestrator code resides inside the `AspireApp.AppHost/AppHost.cs` file
2. We are creating a container for an Microsoft SQL server with admin password `Foobar12345` which is reachable on port `51350`. If we would not pin these values then Aspire would possibly recreate the database container with new values for these parameters. By pinning those values we can also set up a persistent connection in a database tool to access the database directly.
3. The database container is configured with `ContainerLifetime.Persistent` in order to not destroy and recreate the whole container during restarts of the Aspire application
4. Before we start the web app we are waiting for the SQL container to be up and running.

