﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softitoflix.Models
{
    public class Plan
    {
        public short Id { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = "";

        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        [StringLength(20, MinimumLength = 2)]
        [Column(TypeName = "varchar(20)")]
        public string Resolution { get; set; } = "";


    }
}
