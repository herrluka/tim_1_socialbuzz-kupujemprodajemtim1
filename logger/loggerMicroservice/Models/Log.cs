using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace loggerMicroservice.Models
{
    /// <summary>
    /// Predstavlja model za kreiranje logova
    /// </summary>
    public record Log(
        /// <summary>
        /// Nivo logovanja (Information,Warning,Erro)
        /// </summary>
        [RegularExpression(@"^(Information|Error|Warning)(, (?!\1)(Information|Error|Warning))*$",ErrorMessage ="Vrednosti koje mogu biti unete kao nivo logovanja su : Information,Error,Warning")]
        [Required(ErrorMessage ="Nivo logovanja je obavezan")] string LogLevel,

        /// <summary>
        /// ID event-a
        /// </summary>
        string EventID,

        /// <summary>
        /// Jedinstvena oznaga http zahteva
        /// </summary>
        string RequestID,

        /// <summary>
        /// Predhodni identifikator zahteva
        /// </summary>
        string PreviousRequestID,

        /// <summary>
        /// Poruka našeg novog loga
        /// </summary>
        [Required] string Message,

        /// <summary>
        /// Tip greške koju baca ako je greška u pitanju
        /// </summary>
        string ExceptionType,

        /// <summary>
        /// Poruka greške ako je greška u pitanju
        /// </summary>
        string ExceptionMessage,

        /// <summary>
        /// Datum i vreme logovanja
        /// </summary>
        [Required(ErrorMessage = "Datum i vreme logovanja je obavezno")] string TimeOfAction,

        /// <summary>
        /// Naziv mikroservisa koji je izvršio logovanje
        /// </summary>
        [Required(ErrorMessage = "Naziv mikroservisa koji loguje je obavezan")] string Microservice)
    {
        /// <summary>
        /// Interni identifikator loga
        /// </summary>
        [BsonId]
        public Guid ID { get; set; }
    }
}
