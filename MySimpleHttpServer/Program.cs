using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

namespace MySimpleHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {

            SwitchToNewAppDomain();


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

            StartingServer(router);
        }

        private static void StartingServer(Router router)
        {
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

        private static bool SwitchToNewAppDomain()
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // RazorEngine cannot clean up from the default appdomain...
                Console.WriteLine("Switching to secound AppDomain, for RazorEngine...");
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var current = AppDomain.CurrentDomain;
                // You only need to add strongnames when your appdomain is not a full trust environment.
                var strongNames = new StrongName[0];

                var domain = AppDomain.CreateDomain(
                    "MyMainDomain", null,
                    current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                    strongNames);
                return true;
            }
            return false;
        }
    }
}


