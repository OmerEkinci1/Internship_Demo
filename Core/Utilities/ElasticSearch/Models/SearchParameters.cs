﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.ElasticSearch.Models
{
    public class SearchParameters
    {
        public string IndexName { get; set; }
        public int From { get; set; }
        public int Size { get; set; }
    }
}
