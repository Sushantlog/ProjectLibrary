//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRUDAjaxDemo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_SynopsisDetails
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_SynopsisDetails()
        {
            this.tbl_FilesDetails = new HashSet<tbl_FilesDetails>();
        }
    
        public int SynopsisID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> CollegeID { get; set; }
        public string SynopsisHeader { get; set; }
        public string SynopsisDescription { get; set; }
    
        public virtual tbl_CategoryMaster tbl_CategoryMaster { get; set; }
        public virtual tbl_CollegeMaster tbl_CollegeMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_FilesDetails> tbl_FilesDetails { get; set; }
        public virtual tbl_Registration tbl_Registration { get; set; }
    }
}
