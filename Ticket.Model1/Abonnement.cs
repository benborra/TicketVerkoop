namespace Ticket.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Abonnement")]
    public partial class Abonnement
    {
        public int id { get; set; }

        public int Seizoenid { get; set; }

        public int Clubsid { get; set; }

        [Required]
        [StringLength(100)]
        public string Persoonid { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual Clubs Clubs { get; set; }

        public virtual Seizoen Seizoen { get; set; }
    }
}
