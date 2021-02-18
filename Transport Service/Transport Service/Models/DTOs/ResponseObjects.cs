using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.DTOs
{
    /// <summary>
    /// Model used for testing purposes to validate response body
    /// </summary>
    public class ResponseObjects
    {
        /// <summary>
        /// Status text that describes reponse more detailed
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Body content
        /// </summary>
        public String Content { get; set; }
    }
}
