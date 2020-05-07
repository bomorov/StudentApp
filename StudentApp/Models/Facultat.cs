using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudentApp.Models
{
    public class Facultat
    {

        [HiddenInput(DisplayValue = false)] 
        public int Id { get; set; }

        [Required]
        [Display(Name = "Факультет")]
        public string Name { get; set; }

        public ICollection<Group> Groups { get; set; }
        public Facultat()
        {
            Groups = new List<Group>();
        }
    }
}