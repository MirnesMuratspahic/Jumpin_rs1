﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jumpin.Models
{
    public class UserCar
    {
        [JsonIgnore][Key] public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Car Car { get; set; }

    }
}
