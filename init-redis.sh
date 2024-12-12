#!/bin/bash
redis-cli <<EOF
SET BookInventory:9780743273565 50
SET BookInventory:9780451524935 100
EOF