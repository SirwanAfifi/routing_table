using System.Collections.Generic;
using MySimpleHttpServer.Models;

namespace MySimpleHttpServer.Core
{
    public class PostDataSource
    {
        public static IList<Post> GetPosts()
        {
            return new List<Post>
            {
                new Post { Id = 1, Title = "Learning C#", Author = "Sirwan Afifi", Body = "C# is a great programming language ever!"},
                new Post { Id = 2, Title = "Learning PHP", Author = "Hamed Qaderi", Body = "PHP is a great programming language ever!"},
                new Post { Id = 3, Title = "Learning Java", Author = "Sirwan Afifi", Body = "Java is a great programming language ever!"},
                new Post { Id = 4, Title = "Learning Android", Author = "Sirwan Afifi", Body = "Working with Android is great"},
                new Post { Id = 5, Title = "Learning Entity Framework", Author = "Sirwan Afifi", Body = "Entity Framework is way to ineract with database"},
                new Post { Id = 6, Title = "Learning ASP.NET Core", Author = "Sirwan Afifi", Body = "ASP.NET Core is a great web framework by Microsoft"},
                new Post { Id = 7, Title = "Learning ASP.NET Identity Core", Author = "Sirwan Afifi", Body = "ASP.NET Identity Core is new"},
                new Post { Id = 8, Title = "Learning SignalR", Author = "Sirwan Afifi", Body = "SignalR is a great way to communicate with server"},
                new Post { Id = 9, Title = "Learning CSS", Author = "Hamed Qaderi", Body = "CSS is great"},
                new Post { Id = 10, Title = "Learning SACC", Author = "Hamed Qaderi", Body = "SACC is cool"},
                new Post { Id = 11, Title = "Learning JavaScript", Author = "Sirwan Afifi", Body = "JavaScript is a great programming language ever!"},
                new Post { Id = 12, Title = "Learning Angular 2.0", Author = "Sirwan Afifi", Body = "Angular 2.0 is great"},
                new Post { Id = 13, Title = "Learning ReactJS", Author = "Sirwan Afifi", Body = "ReactJS is great"},
            };
        }
    }
}