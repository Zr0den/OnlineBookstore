﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Dto
{
    public class BookDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public string AuthorId { get; set; }
    }
}
