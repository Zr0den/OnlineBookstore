db = db.getSiblingDB('BookstoreDb'); 

db.Books.insertMany([
    { title: "Axel Lugter", author: "Kristian Bech Lund", isbn: "9780743273565" },
    { title: "How not to write a C# Program", author: "Axel Bech Lund", isbn: "9780451524935" }
]);

db.Authors.insertMany([
    { name: "Kristian Bech Lund", birthDate: "1998-10-14" },
    { name: "Axel Bech Lund", birthDate: "2000-04-12" }
]);