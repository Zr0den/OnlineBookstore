#!/bin/bash
redis-cli <<EOF
SET book:1 "Axel Lugter"
SET book:2 "How not to write a C# Program"
HSET inventory "9780743273565" 50
HSET inventory "9780451524935" 100
EOF