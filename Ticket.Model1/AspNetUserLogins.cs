namespace Ticket.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetUserLogins
    {
        [StringLength(255)]
        public string LoginProvider { get; set; }

        [Key]
        [StringLength(255)]
        public string ProviderKey { get; set; }

        [StringLength(100)]
        public string UserId { get; set; }
    }
}
