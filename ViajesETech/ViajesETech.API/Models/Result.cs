using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViajesETech.API.Models
{
    public class Result
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}