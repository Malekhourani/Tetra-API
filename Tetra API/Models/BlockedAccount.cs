namespace Tetra_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BlockedAccount
    {
        public int? BlockerID { get; set; }

        public int? BlockedID { get; set; }

        [Key]
        public int BlockID { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
