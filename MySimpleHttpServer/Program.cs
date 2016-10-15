using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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
                { "/posts", "PostsController@Index" },
                { "/posts/{id}", "PostsController@Details" },
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
                var context = server.GetContext();
                var response = context.Response;
                var request = context.Request;

                
                

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
        private readonly Regex _matchIdPattern = new Regex("{\\d+}");

        public void Define(Dictionary<string, string> routes)
        {
            _routes = routes;
        }

        public string Direct(string uri)
        {
            if (_routes.ContainsKey(uri) && uri != "/favicon.ico")
            {
                var myUri = _routes[uri].Split('@');
                var controller = myUri[0];
                var action = myUri[1];
                var _namespace = nameof(MySimpleHttpServer) + "." + controller;
                var type = Type.GetType(_namespace);
                if (type != null)
                {
                    var method = type.GetMethod(action);
                    var res = method?.Invoke(null, null);
                    return (string)res;
                }
                return "The action method or controller not found.";
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
}
