namespace WebScrapperCSCI586
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MetaCriticsGame")]
    public partial class MetaCriticsGame
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
        public string GenreKeyWords { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string Rating { get; set; }

        [Key]
        [Column(Order = 6)]
        public string Publisher { get; set; }
    }
}
