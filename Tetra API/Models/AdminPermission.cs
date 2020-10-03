namespace Tetra_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdminPermission
    {
        public int? AdminID { get; set; }

        public int? PermissionID { get; set; }

        public int id { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
