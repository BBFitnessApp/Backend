﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CalorieData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
       
        [ForeignKey(nameof(UserId))]
        public User ? User { get; set; } 
        public DateTime Datum { get; set; }
        public int Kalorienaufnahme { get; set; }
        public int Fette { get; set; }
        public int Proteine { get; set; }
        public int Kohlenhydrate { get; set; }
    }
}
