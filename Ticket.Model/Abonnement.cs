//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ticket.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Abonnement
    {
        public int id { get; set; }
        public int Seizoenid { get; set; }
        public int Clubsid { get; set; }
        public string Persoonid { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Clubs Clubs { get; set; }
        public virtual Seizoen Seizoen { get; set; }
    }
}