using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using Oak.Controllers;
using DNUG.Controllers;
using System.Web.Routing;
using Moq;
using System.Web;
using System.Web.Mvc;

namespace DNUG.Tests
{
    class describe_BlogsController : nspec
    {
        SeedController seedController;
        BlogsController blogsController;

        void before_each()
        {
            blogsController = new BlogsController();
            MockRouting(blogsController);
            seedController = new SeedController();

            seedController.PurgeDb();
            seedController.All();
        }
        
        void it_shows_new_blog_creations_on_main_page()
        {
            blogsController.Create(new { Title = "Hello" }.ToPrototype());

            (Blogs().First().Title as string).should_be("Hello");
        }

        void it_ignores_duplicate_blog_creations()
        {
            blogsController.Create(new { Title = "Hello" }.ToPrototype());

            blogsController.Create(new { Title = "Hello" }.ToPrototype());

            Blogs().Count().should_be(1);
        }

        IEnumerable<dynamic> Blogs()
        {
            return blogsController.List().Data.Blogs;
        }

        public void MockRouting(BaseController controller)
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("", UriKind.Relative));
            request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

            var response = new Mock<HttpResponseBase>();
            response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>((s) => s);

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);

            controller.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);
        }
    }
}
