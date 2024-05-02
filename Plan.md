# Meter Reading API

The instruction state a c# API and with a datastore.
Looking at the data it seems pretty normalized a relational.
As such I will use a c# .net core API with a sql backend. No-Sql does not seem like a great fit in this case.
Following this through I will use the most common sql ORM for c#, entity framework core.
It also mentions a UI being a good addition but as this is not a requirement I will do the requirements first.

# Assumptions

1. As the seed data is called Test_Accounts.csv I'm going to assume that the seeding is for test only and not include the seeding as part of the system but instead as part of the tests.
1. As this a technical test with just me I will be doing a few things quick, such as less unit testing, no branching policy ect. I would do these differently in a commercial environment.

# Solution

A .net core rest API with Entity framework, file posted up using multipart form data, validated using fluent validations and read using CsvHelper. Tested using Web Application Factory and Test Containers. There is a MeterReadingUploadsController  which receives the request and does some basic validation, the MeterReadingUploadService which calls first the MeterReadingCsvReader to read the CSV using CSV reader, then the MeterReadingCsvDataValidator which validates each row for basic structure (i.e NNNNNN), DatabaseDataValidator validates the data against the current data in the AC and finally the repository, which maps and uploads the data.


# Steps Taken

1. Created a default .ent core API
1. Added EF core code first with migrations.
1. Create blank unit test project, using NUnit.
1. Started with validation tests
1. in-built model validation won't easily allow validation of file content type so using fluent validation
1. Could look into steaming and batching the CSV for efficiency on very large file uploads, leaving that off for now as it seems like premature optimization.
1. Mostly Done with first pass at code but now need to test it with some rigor, adding unit tests. 
1. Have left the unit test a bit sparse in the interest of time, will add more if I get more time.
1. Have added in integration test and fixed some bugs picked up while integration testing, only created one integration test due to lack of time
1. Test manually and with the integration tests.
1. Move data validation on to it's own class out of the repo, with unit tests

# THoughts on solution

I would do a few things differently if I were to do this again:

1. Make it a bit simpler, I probably over engineered this.
1. Used the time from it being simpler to make a new UI.
1. I did like the CSV helper library, this was an effective library for reading in CSVs
1. It has been a while since I used web application factory along with test containers and I forgot how easy they can make it.
1. I would potentially would have changed the code first as there are disadvantages, but it does make starting out easier.
1. As usual I find fluent assertions to be good but I was not that enamoured by fluent validations (first time I have used it)
1. I did skimp a bit on the unit tests.