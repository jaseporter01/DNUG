using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oak;
using Massive;

namespace DNUG.Controllers
{
    public class Blogs : DynamicRepository
    {
        public Blogs()
        {
            Projection = d => new Blog(d);
        } 
    }

    public class Comments : DynamicRepository { }

    public class Blog : DynamicModel
    {
        Blogs blogs = new Blogs();

        Comments comments = new Comments();

        public Blog(object dto) : base(dto) { }

        IEnumerable<dynamic> Validates()
        {
            yield return new Uniqueness("Title", blogs);
        }

        IEnumerable<dynamic> Associates()
        {
            yield return new HasMany(comments);
        }
    }

    public class BlogsController : BaseController
    {
        dynamic blogs = new Blogs();

        public dynamic List()
        {
            var all = blogs.All().Include("Comments");

            (all as IEnumerable<dynamic>).ForEach(s => 
            {
                s.Comments = s.Comments();
                s.PostCommentUrl = Url.RouteUrl(new { controller = "Comments", action = "Create" });
            });

            return Json(new
            {
                Blogs = all,
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
        }
    }
}
