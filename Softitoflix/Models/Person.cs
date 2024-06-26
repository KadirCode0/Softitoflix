﻿using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Softitoflix.Models
{
	public class Person
	{
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 1)]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; } = "";
    }
}

