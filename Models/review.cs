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
    
    public partial class review
    {
        public int id { get; set; }
        public string img { get; set; }
        public string rating { get; set; }
        public Nullable<System.DateTime> review_date { get; set; }
        public Nullable<int> star { get; set; }
        public Nullable<int> book_id { get; set; }
        public Nullable<int> detail_order_id { get; set; }
        public Nullable<long> user_id { get; set; }
    
        public virtual book book { get; set; }
        public virtual order_detail order_detail { get; set; }
        public virtual users users { get; set; }
    }
}
