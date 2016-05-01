namespace Ticket.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetUsers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetUsers()
        {
            Abonnement = new HashSet<Abonnement>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            Tickets = new HashSet<Tickets>();
        }

        [StringLength(100)]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(255)]
        public string SecurityStamp { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        public int? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string AdressLine { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Abonnement> Abonnement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
