namespace Tetra_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            GroupMessages = new HashSet<GroupMessage>();
            Participants = new HashSet<Participant>();
        }

        public int GroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupName { get; set; }

        public string Description { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? GroupPicture { get; set; }

        public int? Owner { get; set; }

        [StringLength(50)]
        public string GroupType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }

        public virtual Medium Medium { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
