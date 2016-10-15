using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MySimpleHttpServer
{
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
                var _namespace = nameof(MySimpleHttpServer) + ".Controllers." + controller;
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
}