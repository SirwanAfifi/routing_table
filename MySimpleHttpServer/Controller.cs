using System.IO;
using RazorEngine;

namespace MySimpleHttpServer
{
    public class Controller
    {
        public static string View(string controller, string action, object data)
        {
            controller = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) +
                         $"\\Views\\{controller.Replace("Controller", "")}\\{action}.html";
            var tr = new StreamReader(controller);
            var msg = tr.ReadToEnd();

            string result = Razor.Parse(msg, data);

            return result;
        }
    }
}