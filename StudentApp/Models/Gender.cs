using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models
{
    public class Gender
    {
        [HiddenInput(DisplayValue = false)]

        public int Id { get; set; }
        [Display(Name = "Пол")]
        public string Name { get; set; }


        public ICollection<Student> Students { get; set; }
        public Gender()
        {
            Students = new List<Student>();
        }
    }
}