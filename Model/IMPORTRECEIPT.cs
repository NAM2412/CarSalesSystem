//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarSalesSystem.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class IMPORTRECEIPT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IMPORTRECEIPT()
        {
            this.PRODUCTs = new HashSet<PRODUCT>();
        }
    
        public string IRECEIPT_ID { get; set; }
        public string EMPLOYEE_ID { get; set; }
        public Nullable<System.DateTime> DATETIME_IMPORT { get; set; }
        public Nullable<decimal> TOTAL_PRICE { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCT> PRODUCTs { get; set; }
        public virtual IMPORTRECEIPTINFO IMPORTRECEIPTINFO { get; set; }
    }
}
