-- Setup --
The solution uses docker, so use the command docker-compose up -d to get it running
After that, the only entry point into the system is through the API on port localhost:8080.


-- Design Choices --

We are using: 
- MongoDB for Books & Authors 
- MySQL for Customers & Orders
- Redis for Inventory Management

Our reasoning boils down to this: 
- books and authors are the kind of data that rarely needs updating, but often needs to be read, and therefore a NoSQL database type with fast look-up like MongoDB seems to make the most sense for performance.
- for customers & orders we chose MySql partially just because needed and wanted to use a relational database for something, but we also think the strong ACID compliance makes a good case for using MySQL regardless. Consistency especially is important for our orders.
- lastly we use Redis for inventory management because the primary concern here is performance for updates, since we expect inventory data to change a lot quite regularly.
To expand on Redis: Maybe it would make more sense to have the inventory data persisted in our MySql database, as Redis by default does not persist data.
However, we have tried 'AOF' (Append-Only File) Logging through our etc/redis/redis.conf file, where we append to our file once every second. We are however aware that this does not guarantee consistency.
A better solution might have been to persist the data in MySql, load it into Redis on startup and read it from there, and then overwrite our MySql data with our Redis data whenever required (restarts), or something.

For caching, we use Redis even for things like books and authors despite MongoDB being our database of choice to store said objects. 
This is because Redis is essentially an in-memory database, making it a lot faster than MongoDB at reading and writing cached data (which is only ever meant to exist in memory).

We cache Books & Authors for 24 hours (arbitrary-ish number) whenever they are read, and update them in the cache if they are updated or deleted. This is because we expect neither to be updated or deleted very often, so caching is just going to save us a lot of calls to our database.
We also cache Orders for 10 minutes when they are first created, both because that is a typical timeframe for a customer to want to check their order, but also because it means we can get a list of the most recent Orders without needing access to the database.
