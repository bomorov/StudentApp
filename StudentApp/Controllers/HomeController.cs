using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Web.Security;

namespace StudentApp.Controllers
{
    public class HomeController : Controller
    {
        StudentContext db = new StudentContext();


        public ActionResult Index()
        {
            return View();
        }



        [Authorize]


        

        /// Список студентов и редактировние, удаление студентов
        #region 
        public ActionResult Student_List(int page = 1) ///Список студентов 
        {
            var students = db.Students.Include(p => p.Group).Include(p => p.Group.Facultat).Include(p => p.Gender);
            int pageSize = 4;
            IEnumerable<Student> studentsPerPages=students.OrderBy(x=>x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = students.Count() };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Students = studentsPerPages };
            return View(ivm);
        }

        public ActionResult Details()  ///Детальное описание студентов
        {
            var students = db.Students.Include(p => p.Group).Include(p => p.Group.Facultat).Include(p => p.Gender).FirstOrDefault();
            return View(students);
        }

        [HttpGet]
        public ActionResult Edit(int? id)  /// Изменение данных студента
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Student student=db.Students.Find(id);
            if (student != null)
            {
                SelectList groups = new SelectList(db.Groups, "Id", "Name");
                ViewBag.Groups = groups;
                SelectList gender = new SelectList(db.Genders, "Id", "Name");
                ViewBag.Genders = gender;
                return View(student);
            }
            return ViewBag("Student_List");
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Student_List");
        }

        public ActionResult Delete_Student(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Student student = db.Students.Find(id);
            if (student != null)
            {
                db.Students.Remove(student);
                db.SaveChanges();
            }
            return ViewBag("Student_List"); 
        }


        #endregion


        public ActionResult Student_Filter(int? group, int? gender)
        {
            

            IQueryable<Student> students = db.Students.Include(p => p.Group).Include(p => p.Group.Facultat).Include(p => p.Gender);
            if (group != null && group != 0)
            {
                students = students.Where(p => p.GroupId == group);
            }
            if (gender != null && gender!= 0)
            {
                students = students.Where(p => p.GenderId == gender);
            }

            List<Group> groups = db.Groups.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            groups.Insert(0, new Group { Name = "Все", Id = 0 });

            List<Gender> genders = db.Genders.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            genders.Insert(0, new Gender { Name = "Все", Id = 0 });

            StudentsListViewModel plvm = new StudentsListViewModel
            {
                Students = students.ToList(),
                Groups = new SelectList(groups, "Id", "Name"),
                Genders = new SelectList(genders, "Id", "Name"),

            };
            return View(plvm);

           
        }  /// Фильтрация студентов по группам и половому признаку
             
       
       
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
            SelectList genders = new SelectList(db.Genders, "Id", "Name");
            ViewBag.Genders = genders;
            return View();
        }
        [HttpPost]
        public ActionResult Add_Student(Student student)
        {
            if (student.Age < 15)
            {
                ModelState.AddModelError("Age", "Некорректный ввод");
            }
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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

        [HttpPost]
        public ActionResult StudentSearch(string name)
        {
           var student = db.Students.Where(a=>a.Name.Contains(name)).Include(p => p.Group).Include(p => p.Group.Facultat).Include(p => p.Gender).ToList();

            if (student.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(student);
        }

        public ActionResult StudentSearchTittle()
        {
            return View();
        }


       /* [HttpGet]
        public ActionResult Test(int id = 0)
        {
            SelectList groups = new SelectList(db.Groups, "Id", "Name");
            ViewBag.Groups = groups;
            SelectList gender = new SelectList(db.Genders, "Id", "Name");
            ViewBag.Genders = gender;
            return View();
        }

        [HttpPost]
        public ActionResult Test(Student student)
        {
            if (student.Age < 10)
            {
                ModelState.AddModelError("Age", "Некорректный ввод");
            }
            if (ModelState.IsValid)
            {
            
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        */



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