# Bug Tracker

A simple bug tracker application.

The app is split across three layers:

- CosmosDB - for the database and persistence
- ASP.NET Core Web API - Back end
- React App - Front end

# Usage

To run this app the Azure Cosmos DB emulator is required.

Once you have launched the db emulator, Launch the backend by starting up `src\api\PurpleRock.BugTracker.sln` and running the WebApi project. The database and containers are created automatically. Once running the Swagger UI for the API will be available at [localhost:7194](https://localhost:7194/index.html).

To start the frontend, navigate to the `src\frontend\bugtracker` directory and run `npm install` followed by `npm start`. This will startup the react app at [localhost:3000](http://localhost:3000/).

# Limitations
Currently, the UI only supports the following features:

- View list of all bugs
- View bug details
- Add a bug

