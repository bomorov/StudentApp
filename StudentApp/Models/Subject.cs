using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudentApp.Models
{
    public class Subject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Предмет")]
        public string Name { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
  
        public Subject()
        {
            Groups = new List<Group>();
           
            
        }

    }
}