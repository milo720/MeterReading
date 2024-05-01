# Meter Reading API

The instruction state a c# API and with a datastore.
Looking at the data it seems pretty normalized a relational.
As such I will use a c# .net core API with a sql backend. No-Sql does not seem like a great fit in this case.
Following this through I will use the most common sql ORM for c#, entity framework core.
It also mentions a UI being a good addition but as this is not a requirement I will do the requirements first.

# Current Plan

1. Create basic API template, include docker fo easier testing.
1. Add in EF -> will use code first to start with.
1. Create API for uploading meter readings.
1. Add in test container integration test.
1. Seed data and test in integration test.

# Assumptions

1. As the seed data is called Test_Accounts.csv I'm going to assume that the seeding is for test only and not include the seeding as part of the system but instead as part of the tests.


# Current Steps

1. Created a default .ent core API
1. Added EF core code first with migrations.
1. Create blank unit test project, using NUnit.
