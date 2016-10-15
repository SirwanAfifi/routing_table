using System.Collections.Generic;
using MySimpleHttpServer.Models;

namespace MySimpleHttpServer.Controllers
{
    public class HomeController : Controller
    {
        public static string Index()
        {
            var model = new List<Person>
            {
                new Person { Name = "Sirwan", LastName = "Afifi"},
                new Person { Name = "Hamed", LastName = "Qaderi"}
            };
            return View(nameof(HomeController), nameof(Index), model);
        }
    }
}