//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobCompany
{
    using System;
    using System.Collections.Generic;
    
    public partial class Picket
    {
        public int Id { get; set; }
        public int NumberAreaId { get; set; }
        public int NumberPicket { get; set; }
        public int Weight { get; set; }
        public System.DateTime DateAdd { get; set; }
    
        public virtual Area Area { get; set; }
    }
}
