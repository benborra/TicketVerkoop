namespace Ticket.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tickets
    {
        public int id { get; set; }

        public int barcode { get; set; }

        public int Wedstrijdid { get; set; }

        [Required]
        [StringLength(100)]
        public string Persoonid { get; set; }

        public int plaatsId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual Plaats Plaats { get; set; }

        public virtual Wedstrijd Wedstrijd { get; set; }
    }
}
