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
            blogs.Insert(@params);
        }
    }
}
