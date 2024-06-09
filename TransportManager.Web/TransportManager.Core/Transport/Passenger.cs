using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TransportManager.Core.Transport
{
    public class Passenger
    {
        public int Id { get; set; }

        [StringLength(45)]

        [Required(ErrorMessage = "Enter your name, try again")]
        public string Name { get; set; }

        [StringLength(45)]

        [Required(ErrorMessage = "Enter your Lastname, try again")]
        public string LastName { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Age { get; set; }

        // Constructor initializing the non-nullable properties
        public Passenger()
        {
            Name = string.Empty;  // Default value
            LastName = string.Empty;  // Default value
        }
    }
}
