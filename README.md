-- Design Choices --

We are using: 
- MongoDB for Books & Authors 
- MySQL for Customers & Orders
- Redis for Inventory Management

Our reasoning boils down to this: 
- books and authors are the kind of data that rarely needs updating, but often needs to be read, and therefore a NoSQL database type with fast look-up like MongoDB seems to make the most sense for performance.
- for customers & orders we chose MySql partially just because needed and wanted to use a relational database for something, but we also think the strong ACID compliance makes a good case for using MySQL regardless. Consistency especially is important for our orders.
- lastly we use Redis for inventory management because the primary concern here is performance for updates, since we expect inventory data to change a lot quite regularly.

For caching, we use Redis even for things like books and authors despite MongoDB being our database of choice to store said objects. 
This is because Redis is essentially an in-memory database, making it a lot faster than MongoDB at reading and writing cached data (which is only ever meant to exist in memory). 