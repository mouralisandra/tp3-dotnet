using Microsoft.AspNetCore.Mvc;
using tp3sandramourali.Models;

namespace tp3sandramourali.Controllers
{
    [Route("Person")]
    public class PersonController : Controller
    {
        private readonly IConfiguration _configuration;
        public PersonController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Index()
        {
            var allPersons = new PersonalData(_configuration).GetAllPerson();
            return View(allPersons);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult SearchID(int id)
        {
            var person = new PersonalData(_configuration).GetPerson(id);
            if (person == null)
                return NotFound(new NotFoundObjectResult("Cette personne n'existe pas!"));
            return View(person);
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [Route("search")]
        public IActionResult Search(string firstName, string country)
        {
            if (ModelState.IsValid)
            {
                var result = new PersonalData(_configuration).GetPersons(firstName, null, null, country);
                if (result.Count == 0) return View(new Tuple<Person, bool, bool>(null, true, false));
                var id = result[0].Id;
                return RedirectToAction("SearchID", new { id = id });
            }

            return View(new Tuple<Person, bool, bool>(null, false, true));
        }
    }
```
}
