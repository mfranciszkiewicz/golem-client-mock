﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class Offer
    {
        public string NodeId { get; set; }
        //public string Id { get; set; }
        public IDictionary<string, JToken> Properties { get; set; }
        public string Constraints { get; set; }
    }
}
