namespace Tetra_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Contact
    {
        public int ContactID { get; set; }

        public int? UserID { get; set; }

        [Key]
        public int FriendID { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
