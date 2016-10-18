### The goal of this simple console application is to look at a incoming HTTP request and try to map that request to a method of a C# class:  
You can define your routes this way:
```
var router = new Router();
router.Define(new Dictionary<string, string>
{
    { "/", "HomeController@Index" },
    { "/about", "AboutController@Index" },
    { "/posts", "PostsController@Index" },
    { "/posts/{id}", "PostsController@Details" },
    { "/favicon.ico", "Fav icon" }
});
```