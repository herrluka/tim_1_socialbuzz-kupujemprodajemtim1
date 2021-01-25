using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loggerMicroservice.Models
{
    /// <summary>
    /// Dto za logove
    /// </summary>
    public record LogDto(
        /// <summary>
        /// Jedinstveni identifikator loga
        /// </summary>
        string ID,
        /// <summary>
        /// Nivo logovanja
        /// </summary>
        string LogLevel,
        /// <summary>
        /// Poruka za logovanje
        /// </summary>
        string Message,
        /// <summary>
        /// Naziv mikroservisa
        /// </summary>
        string Microservice)
    {
       
    }
}
