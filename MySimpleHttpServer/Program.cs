using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace MySimpleHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {

            // Route table
            var router = new Router();
            router.Define(new Dictionary<string, string>
            {
                { "/", "HomeController@Index" },
                { "/about", "AboutController@Index" },
                { "/posts/index", "PostController@Index" },
                { "/favicon.ico", "Fav icon" }
            });

            // First all of we should grant permissions to the particular URL. e.g.
            // netsh http add urlacl url=http://+:80/MyUri user=DOMAIN\user
            // source: http://stackoverflow.com/a/4115328/1646540

            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:80/");

            server.Start();

            while (true)
            {
                HttpListenerContext context = server.GetContext();
                HttpListenerResponse response = context.Response;

                HttpListenerRequest request = context.Request;

                
                

                var res = router.Direct(request.Url.AbsolutePath);
                

                byte[] buffer = Encoding.UTF8.GetBytes(res);
                response.ContentLength64 = buffer.Length;
                Stream st = response.OutputStream;
                st.Write(buffer, 0, buffer.Length);

                context.Response.Close();
            }
        }
    }

    public class Router
    {
        private Dictionary<string, string> _routes;

        public void Define(Dictionary<string, string> routes)
        {
            _routes = routes;
        }

        public string Direct(string uri)
        {
            if (_routes.ContainsKey(uri) && uri != "/favicon.ico")
            {
                var myUri = uri.Split('/').Skip(1).ToArray();
                var controller = uri == "/" ? "HomeController" : myUri[0].FirstCharToUpper() + "Controller";
                var action = uri == "/" ? "Index" : myUri[1].FirstCharToUpper();

                var type = Type.GetType(nameof(MySimpleHttpServer) + "." + controller);
                var method = type?.GetMethod(action);
                var res = method?.Invoke(null, null);
                return (string)res;
            }

            return "No route defined for this URI.";
        }
    }

    public class HomeController
    {
        public static string Index()
        {
            return "<h1>Home Page</h1>";
        }
    }

    public class PostsController
    {
        public static string Index()
        {
            return "Hello I am an action method";
        }
    }

    public static class StringHelper
    {
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
    }
}
