﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.ElasticSearch.Models
{
    public class ElasticSearchInsertManyModel : ElasticSearchModel
    {
        public object[] Items { get; set; }
    }
}
