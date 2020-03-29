using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Z_Market.Models;

namespace Z_Market
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Models.Z_MarketContext, Migrations.Configuration>()
                );
            ApplicationDbContext db = new ApplicationDbContext();

            CreateRoles(db);
            db.Dispose();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CreateRoles(ApplicationDbContext db)
        {
            var rolManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if(!rolManager.RoleExists("View"))
            {
                rolManager.Create(new IdentityRole("View"));
            }
            if (!rolManager.RoleExists("Edit"))
            {
                rolManager.Create(new IdentityRole("Edit"));
            }
            if (!rolManager.RoleExists("Create"))
            {
                rolManager.Create(new IdentityRole("Create"));
            }
            if (!rolManager.RoleExists("Delete"))
            {
                rolManager.Create(new IdentityRole("Delete"));
            }
        }
    }
}
