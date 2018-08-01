namespace WebScrapperCSCI586
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GameSpotGame")]
    public partial class GameSpotGame
    {

        [Key]
        [Column(Order = 1)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 2)]
        public double Score { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Recommendation { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(500)]
        public string Comments { get; set; }

        [Key]
        [Column(Order = 5)]
        public double UserScore { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(500)]
        public string Wiki { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string ReleaseDate { get; set; }

        [Key]
        [Column(Order = 8)]
        public string Platforms { get; set; }
    }
}
