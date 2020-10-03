namespace Tetra_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Participant
    {
        public int? UserID { get; set; }

        public int? GroupID { get; set; }

        public int? AdminID { get; set; }

        public int id { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual Group Group { get; set; }

        public virtual User User { get; set; }
    }
}
