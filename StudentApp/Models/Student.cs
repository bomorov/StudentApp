using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace StudentApp.Models
{
    public class Student
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }


        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Заплните поле")]
        public string Name { get; set; }

      
        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Заплните поле")]
        public int Age { get; set; }

    
        public int GenderId { get; set; }
     
        public Gender Gender { get; set; }
    
        public int? GroupId { get; set; }
    
        public Group Group { get; set; }
      
     
    }
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class IndexViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public PageInfo PageInfo { get; set; }
    }


    public class StudentsListViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public SelectList Groups { get; set; }
        public SelectList Genders { get; set; }
    }

}