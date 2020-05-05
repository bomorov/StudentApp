using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace StudentApp.Controllers
{
    public class HomeController : Controller
    {
        StudentContext db = new StudentContext();


        public ActionResult Index()
        {
            return View();
        }


       
        public ActionResult Student_List()
        {
            var student = db.Students.Include(p => p.Group).Include(p=>p.Group.Facultat).Include(p=>p.Gender);
            return View(student.ToList());
        }  ///Список студентов
        public ActionResult Group_List()
        {
            var group = db.Groups.Include(p => p.Facultat);
            return View(group.ToList());
        }  ///Список Групп




        /// Добавление студента   
        #region
        [HttpGet]
        public ActionResult Add_Student(int id = 0)
        {
            SelectList groups = new SelectList(db.Groups, "Id", "Name");
            ViewBag.Groups = groups;
            SelectList gender = new SelectList(db.Genders, "Id", "Name");
            ViewBag.Genders = gender;
            return View();
        }
        [HttpPost]
        public ActionResult Add_Student(Student student)
        {
            db.Students.Add(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        /// Добавление групп
        #region
        
        [HttpGet]
        public ActionResult Add_Group(int id=0)
        {
            SelectList facultats = new SelectList(db.Facultats, "Id", "Name");
            ViewBag.Facultats = facultats;
            return View();
            
        }
        [HttpPost]
        public ActionResult Add_Group(Group group)
        {
            db.Groups.Add(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        /// Добавление Предметов
        #region
        public ActionResult Add_Subject()
        {
          
            return View();

        }
        [HttpPost]
        public ActionResult Add_Subject(Subject subject)
        {
            db.Subjects.Add(subject);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        //Просмотр и добавление предметов для Групп
        #region
        public ActionResult Group_Subjects(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }
        public ActionResult Edit_Group_Subjects(int id=0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewBag.Subjects = db.Subjects.ToList();
            return View(group);
        }

        [HttpPost]
        public ActionResult Edit_Group_Subjects(Group group, int[] selectedSubjects)
        {
            Group newGroup = db.Groups.Find(group.Id);
            newGroup.Name = group.Name;


            newGroup.Subjects.Clear();
            if (selectedSubjects != null)
            {
                //получаем выбранные курсы
                foreach (var c in db.Subjects.Where(co => selectedSubjects.Contains(co.Id)))
                {
                    newGroup.Subjects.Add(c);
                }
            }

            db.Entry(newGroup).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion



        public 




        public ActionResult About()
        {
            ViewBag.Message = "StudentAPP";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "bomorov1998@gmail.com";

            return View();
        }
    }
}