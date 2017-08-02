using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudyDoIT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             "SearchProduct",
             "tim-kiem-san-pham/{p}",
             new { controller = "Search", action = "ResulftSearch"},
             new { p = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "Product",
             "sp/{seoName}-{id}-{p}",
             new { controller = "Category", action = "Index", seoName = "" },
             new { id = @"^\d+$", p = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "News",
             "tin/{seoName}-{id}-{p}",
             new { controller = "Category", action = "News", seoName = "" },
             new { id = @"^\d+$", p = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "Page",
             "p/{seoName}-{id}",
             new { controller = "Category", action = "Page", seoName = "" },
             new { id = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "Tag",
             "tag-tin/{seoName}-{p}",
             new { controller = "Category", action = "Tag", seoName = "" },
             new { p = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "TagProduct",
             "tag-sp/{seoName}-{p}",
             new { controller = "Category", action = "TagTour", seoName = "" },
             new { p = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "CustomerComment",
             "y-kien-khach-hang",
             new { controller = "Category", action = "CustomerComment"},
             new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "Comment",
             "chi-tiet-y-kien-khach-hang/{id}",
             new { controller = "Single", action = "Comment" },
             new { id = @"^\d+$"}, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "DetailProduct",
             "chi-tiet-sp/{seoName}-{id}",
             new { controller = "Single", action = "Index", seoName = "" },
             new { id = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "DetailNews",
             "chi-tiet-tin/{seoName}-{id}",
             new { controller = "Single", action = "News", seoName = "" },
             new { id = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "Contact",
             "lien-he",
             new { controller = "Home", action = "Contact" },
             new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "RecoverPassword",
             "lay-lai-mat-khau",
             new { controller = "Customer", action = "RecoverPassword"},
             new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "UserAuthentication",
             "xac-thuc-tai-khoan/{username}",
             new { controller = "Customer", action = "UserAuthentication", username = "" },
             new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                "Login",
                "dang-nhap",
                new { controller = "Customer", action = "Login"},
                new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                "Register",
                "dang-ky",
                new { controller = "Customer", action = "Register" },
                new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                "Logout",
                "dang-xuat",
                new { controller = "Customer", action = "Logout" },
                new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                "InfoUser",
                "trang-ca-nhan/{user}",
                new { controller = "Customer", action = "InfoUser", user = "" },
                new string[] { "StudyDoIT.Controllers" }
            );
           
            routes.MapRoute(
                "Compare",
                "danh-sach-san-pham-so-sanh",
                new { controller = "Customer", action = "Compare" },
                new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                "Wishlist",
                "danh-sach-yeu-thich-{p}",
                new { controller = "Customer", action = "Wishlists", p = 1 },
                new { p = @"^\d+$" }, new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                "ShoppingCart",
                "gio-hang",
                new { controller = "ShoppingCart", action = "ShoppingCart" },
                new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                "Checkout",
                "thuc-hien-mua-hang",
                new { controller = "Checkout", action = "Index" },
                new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                "ComplateOrder",
                "hoan-tat-dat-hang",
                new { controller = "Checkout", action = "ComplateOrder" },
                new string[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "StudyDoIT.Controllers" }
            );

            routes.MapRoute(
             "AdminIT",
             "AdminIT/{seoName}",
             new { controller = "Home", action = "Menu", seoName = "" },
             new string[] { "StudyDoIT.Areas.Admin.Controllers" }
            );

            //routes.MapRoute(
            //  name: "Admin",
            //  url: "AdminIT/{controller}/{action}/{id}",
            //  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //  constraints: new string[] { "StudyDoIT.Areas.Admin.Controllers" }
            //);
        }
    }
}
