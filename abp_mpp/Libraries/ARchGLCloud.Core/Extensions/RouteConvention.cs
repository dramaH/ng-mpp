using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ARchGLCloud.Core.Extensions
{
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;
        private readonly AttributeRouteModel _addRoute;

        public RouteConvention(IRouteTemplateProvider centralPrefix, IRouteTemplateProvider addRoute)
        {
            _centralPrefix = new AttributeRouteModel(centralPrefix);
            _addRoute = new AttributeRouteModel(addRoute);
        }
        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            selectorModel.AttributeRouteModel);
                    }
                }

                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();

                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = _centralPrefix;
                    }
                }
                if (_addRoute != null)
                {
                    foreach (var selectorModel in controller.Selectors.ToList())
                    {
                        var addRoute = AttributeRouteModel.CombineAttributeRouteModel(_addRoute, selectorModel.AttributeRouteModel);
                        var model = new SelectorModel(selectorModel)
                        {
                            AttributeRouteModel = addRoute
                        };
                        controller.Selectors.Add(model);
                    }
                }
            }
        }
    }

    public static class MvcOptionsExtensions
    {
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }
        public static void AddPrefixedRoute(this MvcOptions opts, IRouteTemplateProvider prefixAttribute, IRouteTemplateProvider addAttribute)
        {
            opts.Conventions.Insert(0, new RouteConvention(prefixAttribute, addAttribute));
        }
    }
}