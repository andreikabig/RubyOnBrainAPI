﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubyOnBrain.API.Models;
using RubyOnBrain.Data;
using RubyOnBrain.Domain;
using System.Security.Claims;

namespace RubyOnBrain.API.Services
{
    public class CourseService
    {
        // Лол
        private DataContext db;
        private List<Course>? coursesList;

        public CourseService(DataContext db)
        { 
            this.db = db;
            GetCourses();
        }

        private void GetCourses()
        {
            coursesList = db?.Courses
                .Include(p => p.ProgLang)
                .ToList();
        }

        public CourseDTO? GetCourse(int id)
        {
            var course = db?.Courses.FirstOrDefault(c => c.Id == id);

            if (course != null)
                return ConvertData(course);
            
            return null;
        }

        public List<CourseDTO>? GetAll() => ConvertData(coursesList);

        private List<CourseDTO>? ConvertData(List<Course>? dataCourses)
        { 
            List<CourseDTO> courses = new List<CourseDTO>();

            if (dataCourses != null)
            {
                foreach (var course in dataCourses)
                {
                    courses.Add(new CourseDTO()
                    {
                        Id = course.Id,
                        Description = course.Description,
                        Name = course.Name,
                        ProgLang = course.ProgLang.Name,
                        Rating = course.Rating
                    });
                }

                return courses;
            }
            else
                return null;
        }

        private CourseDTO ConvertData(Course dataCourse)
        {
            return new CourseDTO()
            {
                Id = dataCourse.Id,
                Description = dataCourse.Description,
                Name = dataCourse.Name,
                ProgLang = dataCourse.ProgLang.Name,
                Rating = dataCourse.Rating
            };
        }

        public bool AddCourse(CourseDTO course)
        {
            var findCourse = db?.Courses.FirstOrDefault(c => c.Name == course.Name);
            var progLang = db?.ProgLangs.FirstOrDefault(p => p.Name == course.ProgLang);

            if (findCourse == null && progLang != null)
            {
                db.Courses.Add(new Course { Name = course.Name, ProgLang = progLang, Description = course.Description, Rating = 0 });
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeCourse(CourseDTO course)
        { 
            var _course = db?.Courses.Include(p => p.ProgLang).FirstOrDefault(c => c.Id == course.Id);

            if (_course != null)
            { 
                
                var ProgLang = db?.ProgLangs.FirstOrDefault(p => p.Name == course.ProgLang);
                if (ProgLang != null)
                {
                    _course.Description = course.Description;
                    _course.Name = course.Name;
                    _course.ProgLang = ProgLang;
                    db.Courses.Update(_course);
                    db.SaveChanges();
                    return true;
                }
            } 
            return false;
        }

        public bool AddStudent(int id, int studentId)
        {
            var findedUser = db?.Users.FirstOrDefault(u => u.Id == studentId);
            var findedCourse = db?.Courses.Include(c => c.Users).FirstOrDefault(c => c.Id == id);

            if (findedUser != null && findedCourse != null)
            {
                findedCourse.Users.Add(findedUser);

                db?.Courses.Update(findedCourse);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public List<TopicDTO>? GetTopics(int id, ClaimsPrincipal? user)
        {
            List<TopicDTO>? _topics = null;
            if (user.IsInRole("admin"))
            {
                var findedTopics = db?.Topics.Where(t => t.CourseId == id).ToList();

                foreach (var topic in findedTopics)
                {
                    _topics = new List<TopicDTO>();
                    _topics.Add(new TopicDTO() { Id = topic.Id, Name = topic.Name, Rating = topic.Rating, CourseId = topic.CourseId });
                }
            }
            else
            { 
                int userId = db.Users.FirstOrDefault(u => u.Email == user.Identity.Name).Id;

                var course = db.UserCourses.FirstOrDefault(uc => uc.UserId == userId && uc.CourseId == id);

                if (course != null)
                {
                    _topics = new List<TopicDTO>();
                    var findedTopics = db?.Topics.Where(t => t.CourseId == id).ToList();

                    foreach (var topic in findedTopics)
                    {
                        _topics.Add(new TopicDTO() { Id = topic.Id, Name = topic.Name, Rating = topic.Rating, CourseId = topic.CourseId });
                    }
                }

            }
            
            return _topics;
        }

        public bool DeleteCourse(int id)
        {
            var courseDelete = db?.Courses.FirstOrDefault(c => c.Id == id);
            if (courseDelete != null)
            {
                db?.Courses.Remove(courseDelete);
                db?.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
