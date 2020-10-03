namespace Tetra_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            BlockedAccounts = new HashSet<BlockedAccount>();
            BlockedAccounts1 = new HashSet<BlockedAccount>();
            Contacts = new HashSet<Contact>();
            Contacts1 = new HashSet<Contact>();
            GroupMessages = new HashSet<GroupMessage>();
            Groups = new HashSet<Group>();
            NotActivatedAccounts = new HashSet<NotActivatedAccount>();
            Participants = new HashSet<Participant>();
        }

        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string NickName { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(50)]
        public string Bio { get; set; }

        [Required]
        public string Email { get; set; }

        public int? ProfilePicture { get; set; }

        [Required]
        [StringLength(3)]
        public string Activated { get; set; }

        public DateTime? LastSeen { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlockedAccount> BlockedAccounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlockedAccount> BlockedAccounts1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group> Groups { get; set; }

        public virtual Medium Medium { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotActivatedAccount> NotActivatedAccounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
