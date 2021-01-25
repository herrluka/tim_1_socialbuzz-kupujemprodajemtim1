using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Models
{
    /// <summary>
    /// Model used as data transfer object when internal server
    /// error comes up
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Http status code
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
