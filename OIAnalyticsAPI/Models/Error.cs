﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace OIAnalyticsAPI.Models
{
    public class Error
    {
        public int StatusCode {get; set;}
        public string Message { get; set;}
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        


    }



}