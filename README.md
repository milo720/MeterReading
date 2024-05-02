# MeterReading

This is my implementation of the coding test. I ended up doing most of the spec but not getting a UI in place.
In the end I have created a .net api, a sql backend using EF core, unit test and test container integration tests.
You will find plan.md which has my rough notes.

## Things I would do next

1. Add a UI
1. Increase the number of integration and unit tests
1. General cleanup
1. Add tool to setup DB locally outside of automated tests.
1. Adding in logging to the app
1. Adding in docker compose with data setup, UI and api for local development.
1. Adding in a deployment pipeline using azure DO and terraform or bicep.

# Things I would do differently

1. I believe I over complicated this so doing a very rough iteration and using that to make UI and integration tests.
1. After that fixing structure and errors.
1. Then adding nice to haves like logging.
1. A more iterative approach with a rough alpha to start, then move from there.

# Running the App

You can run the App up in using the dotnet CLI or through an IDE such as visual studio. By default it looks at a sql express instance as the DB. For testing purposes I have created an integration test using the WebApplicationFactory and test containers, this will require and installation of docker to run. To test with the seeded data I was using the automated tests, you can also pause the automated test if needed and point the app at that connection string. My initial plan was to put a tool in place to initializes the DB but as the automated test fill that role for the moment I have left it.

