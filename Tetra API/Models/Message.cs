namespace Tetra_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Message()
        {
            GroupMessages = new HashSet<GroupMessage>();
            Messages1 = new HashSet<Message>();
        }

        public int MessageID { get; set; }

        public DateTime SentTime { get; set; }

        public string Content { get; set; }

        public int? Attachment { get; set; }

        public int? ReplyMessageID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }

        public virtual Medium Medium { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages1 { get; set; }

        public virtual Message Message1 { get; set; }
    }
}
