using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oak;
using Massive;

namespace DNUG.Controllers
{
    public class Blogs : DynamicRepository { }

    public class Blog : Gemini
    {
        static Blog()
        {
            Gemini.Extend<Blog, Validations>();
        }

        Blogs blogs = new Blogs();

        public Blog(object dto) : base(dto) { }

        IEnumerable<dynamic> Validates()
        {
            yield return new Uniqueness("Title", blogs);
        }
    }

    public class BlogsController : BaseController
    {
        Blogs blogs = new Blogs();

        public ActionResult List()
        {
            return Json(new
            {
                Blogs = blogs.All(),
                CreateUrl = Url.RouteUrl(new { action = "Create" })
            });
        }

        [HttpPost]
        public void Create(dynamic @params)
        {
            dynamic blog = new Blog(@params);

            if (blog.IsValid())
            {
                blogs.Insert(blog);    
            }
            else
            {
                Console.Out.WriteLine("it wasn't valid: " + blog.FirstError());
                (blog.Errors() as IEnumerable<dynamic>).ForEach(s => Console.Out.WriteLine(s));
                Console.Out.WriteLine(blog);
            }
        }
    }
}
