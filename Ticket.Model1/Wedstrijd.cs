namespace Ticket.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wedstrijd")]
    public partial class Wedstrijd
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wedstrijd()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int id { get; set; }

        public int thuisPloeg { get; set; }

        public int bezoekersPloeg { get; set; }

        public int stadionId { get; set; }

        public virtual Clubs Clubs { get; set; }

        public virtual Stadion Stadion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
