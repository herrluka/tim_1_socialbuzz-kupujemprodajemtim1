using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    /// <summary>
    /// Predstavlja model proizvoda
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Id proizvoda
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Naziv proizvoda
        /// </summary>
        public String ProductName { get; set; }

        /// <summary>
        /// Id korisnika koji je prodavac datog proizvoda
        /// </summary>
        public int SellerID { get; set; }

        /// <summary>
        /// Opis datog proizvoda
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        ///Tezina proizvoda 
        /// </summary>
        public String Weight { get; set; }

        /// <summary>
        /// Id cene proizvoda
        /// </summary>
        public int PriceID { get; set; }

        /// <summary>
        /// Id valute u kojoj je cena izrazena
        /// </summary>
        public int CurrencyID { get; set; }

        /// <summary>
        /// Id stanja proizovda
        /// </summary>
        public int ProductConditionID { get; set; }

        /// <summary>
        /// Id kategorije kojoj proizvod pripada
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Kolicina u kojoj se proizvod prodaje
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Datum objavljivanja proizvoda
        /// </summary>
        public DateTime PublicationDate { get; set; }
    }
}
