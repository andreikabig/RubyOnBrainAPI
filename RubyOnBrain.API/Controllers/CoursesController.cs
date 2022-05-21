using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RubyOnBrain.API.Models;
using RubyOnBrain.API.Services;
using RubyOnBrain.Data;
using RubyOnBrain.Domain;

namespace RubyOnBrain.API.Controllers
{
    [ApiController] // Атрибут ApiController
    [Route("api/[controller]")] // Маршрут контроллера
    public class CoursesController : ControllerBase
    {
        // Сервис для работы с курсами
        private CourseService courseService;

        public CoursesController(CourseService courseService)
        { 
            this.courseService = courseService;
        }

        // GET: /api/courses
        [HttpGet(Name = "get")]
        public List<CourseDTO>? GetCourses()
        {
            return courseService.GetAll();
        }

        // GET: /api/courses/{id}
        [HttpGet("{id}")]
        public ActionResult<CourseDTO> GetCourse(int id)
        { 
            var course = courseService.GetCourse(id);

            if (course != null)
                return course;
            else
                return NotFound();
        }

        // POST: /api/courses
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Course> AddCourse(CourseDTO course)
        {
            if (course == null)
            { 
                return BadRequest();
            }


            var result = courseService.AddCourse(course);

            if (result)
                return Ok(result);
            return Problem("Something went wrong. Maby this course is already exists.");
        }

        // PUT: /api/courses/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult UpdateCourse(int id, CourseDTO course)
        {
            if (course == null || course.Id != id)
            {
                return BadRequest();
            }

            bool result = courseService.ChangeCourse(course);

            if (result)
                return Ok(result);

            return Problem("Something went wrong. You'r course is invalid. We can't update info.");

        }

        // DELETE: /api/courses/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteCourse(int id)
        { 
            bool result = courseService.DeleteCourse(id);

            if (result)
            {
                return Ok(result);
            }
            return Problem($"Something went wrong. We can't find course with id {id}.");
        }

    }
}
