namespace Tetra_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GroupMessage
    {
        public int? MessageID { get; set; }

        public int? GroupID { get; set; }

        public int? SenderID { get; set; }

        public int id { get; set; }

        public virtual Group Group { get; set; }

        public virtual Message Message { get; set; }

        public virtual User User { get; set; }
    }
}
