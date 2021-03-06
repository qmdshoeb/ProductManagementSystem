//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS_DAL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Payments = new HashSet<Payment>();
        }
    
        public int OrderId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ProductQuantity { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> BookingOn { get; set; }
        public Nullable<System.DateTime> DeliveredOn { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual UserMaster UserMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
