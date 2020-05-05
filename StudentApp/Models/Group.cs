using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? FacultatId { get; set; }
        public Facultat Facultat { get; set; }

        public ICollection<Student> Students { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public Group()
        {
            Students = new List<Student>();
            Subjects = new List<Subject>();
        }

    }
}