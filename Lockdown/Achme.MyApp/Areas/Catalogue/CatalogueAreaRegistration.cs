using System.Web.Mvc;

namespace Achme.MyApp.Areas.Catalogue
{
    public class CatalogueAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Catalogue";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Catalogue_default",
                "Catalogue/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
