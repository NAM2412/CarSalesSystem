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
    
    public partial class CUSTOMER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CUSTOMER()
        {
            this.MAINTENANCEBILLs = new HashSet<MAINTENANCEBILL>();
            this.SELLBILLs = new HashSet<SELLBILL>();
            this.ORDERBILLs = new HashSet<ORDERBILL>();
        }
    
        public string CUS_ID { get; set; }
        public string CUS_NAME { get; set; }
        public string CUS_ACCOUNT { get; set; }
        public Nullable<System.DateTime> CUS_DATE_OF_BIRTH { get; set; }
        public string GENDER { get; set; }
        public string PHONE { get; set; }
        public string CUS_ADDRESS { get; set; }
        public Nullable<System.DateTime> REGIST_DATE { get; set; }
        public Nullable<decimal> REVENUE { get; set; }
        public Nullable<int> PRODUCT_NUMBER { get; set; }
        public byte[] IMG { get; set; }
        public string RANK_ID { get; set; }
        public string CUS_EMAIL { get; set; }
    
        public virtual ACCOUNT ACCOUNT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAINTENANCEBILL> MAINTENANCEBILLs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SELLBILL> SELLBILLs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDERBILL> ORDERBILLs { get; set; }
        public virtual RANK_MONEY RANK_MONEY { get; set; }
    }
}
