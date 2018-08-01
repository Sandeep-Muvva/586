namespace WebScrapperCSCI586
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IGNGame")]
    public partial class IGNGame
    {
        [Key]
        [Column(Order = 1)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 2)]
        public double Score { get; set; }

        [Key]
        [Column(Order = 3)]
        public double UserScore { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(200)]
        public string Wiki { get; set; }
    }
}
