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
        private readonly Regex _parameter = new Regex("\\d+");

        public void Define(Dictionary<string, string> routes)
        {
            _routes = routes;
        }

        public string Direct(string uri)
        {
            var originalUri = Regex.Replace(uri, "\\d+", "{id}");

            if (_routes.ContainsKey(originalUri) && uri != "/favicon.ico")
            {
                var myUri = _routes[originalUri].Split('@');
                var controller = myUri[0];
                var action = myUri[1];
                var parameter = uri.Split('/').Skip(1).LastOrDefault();
                var _namespace = nameof(MySimpleHttpServer) + "." + controller;
                var type = Type.GetType(_namespace);
                
                if (type != null)
                {
                    var method = type.GetMethod(action);
                    if (parameter != null)
                    {
                        var res = !_parameter.Match(parameter).Success ? 
                            method?.Invoke(null, null) : method?.Invoke(null, new object[] { int.Parse(parameter) });
                        return (string)res;
                    }
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
            return "Index";
        }
    }

    public class PostsController
    {
        public static string Index()
        {
            return "Index";
        }

        public static string Details(int id)
        {
            string page = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) +
                $"\\Views\\{nameof(PostsController).Replace("Controller", "")}\\{nameof(Details)}.html";
            TextReader tr = new StreamReader(page);
            string msg = tr.ReadToEnd();
            return msg;
        }
    }
}
