namespace Ticket.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clubs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clubs()
        {
            Abonnement = new HashSet<Abonnement>();
            Wedstrijdid = new HashSet<Wedstrijd>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string naam { get; set; }

        public int Stadionid { get; set; }

        [StringLength(255)]
        public string logo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Abonnement> Abonnement { get; set; }

        public virtual Stadion Stadion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wedstrijd> Wedstrijdid { get; set; }
    }
}
