namespace Ticket.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetUserClaims
    {
        [StringLength(100)]
        public string Id { get; set; }

        [StringLength(100)]
        public string UserId { get; set; }

        [StringLength(255)]
        public string ClaimType { get; set; }

        [StringLength(255)]
        public string ClaimValue { get; set; }
    }
}
