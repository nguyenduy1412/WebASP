//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLHS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public book()
        {
            this.order_detail = new HashSet<order_detail>();
            this.warehouse = new HashSet<warehouse>();
            this.review = new HashSet<review>();
            this.detail_receipt = new HashSet<detail_receipt>();
            this.image_products = new HashSet<image_products>();
            this.cart_item = new HashSet<cart_item>();
            this.favourite_item = new HashSet<favourite_item>();
        }
    
        public int id { get; set; }
        public string book_name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public long price { get; set; }
        public long price_enter { get; set; }
        public Nullable<int> profit { get; set; }
        public long price_sale { get; set; }
        public Nullable<int> publication_year { get; set; }
        public Nullable<int> sale { get; set; }
        public Nullable<double> star { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<int> author_id { get; set; }
        public Nullable<int> category_id { get; set; }
        public Nullable<int> publicsher_id { get; set; }
    
        public virtual author author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_detail> order_detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<warehouse> warehouse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<review> review { get; set; }
        public virtual categories categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detail_receipt> detail_receipt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<image_products> image_products { get; set; }
        public virtual publicsher publicsher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart_item> cart_item { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<favourite_item> favourite_item { get; set; }
    }
}
