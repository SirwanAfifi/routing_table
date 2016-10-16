using System.Linq;
using MySimpleHttpServer.Core;

namespace MySimpleHttpServer.Controllers
{
    public class PostsController : Controller
    {
        public static string Index()
        {
            return View(nameof(PostsController), nameof(Index), PostDataSource.GetPosts());
        }

        public static string Details(int id)
        {
            var post = PostDataSource.GetPosts().FirstOrDefault(p => p.Id == id);
            return View(nameof(PostsController), nameof(Details), post);
        }
    }
}