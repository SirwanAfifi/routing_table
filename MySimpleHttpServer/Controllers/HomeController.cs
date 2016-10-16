using System.Linq;
using MySimpleHttpServer.Core;

namespace MySimpleHttpServer.Controllers
{
    public class HomeController : Controller
    {
        public static string Index()
        {
            return View(nameof(HomeController), nameof(Index), PostDataSource.GetPosts().Take(5));
        }
    }
}