using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models
{
    /// <summary>
    /// Model used as data transfer object when internal server
    /// error comes up
    /// </summary>
    public class ErrorDetailsDto
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
