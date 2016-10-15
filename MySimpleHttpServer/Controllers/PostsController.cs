namespace MySimpleHttpServer.Controllers
{
    public class PostsController : Controller
    {
        public static string Index()
        {
            return View(nameof(PostsController), nameof(Index), new {  });
        }

        public static string Details(int id)
        {
            return View(nameof(PostsController), nameof(Details), new { });
        }
    }
}