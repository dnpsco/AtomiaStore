﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core;
using System.Threading;

namespace Atomia.Store.AspNetMvc.Filters
{
    public class LanguageFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            
            if (filterContext.Controller.ControllerContext.IsChildAction || request.IsAjaxRequest())
            {
                return;
            }

            var languagePreferenceProvider = DependencyResolver.Current.GetService<ILanguagePreferenceProvider>();

            if (request.QueryString["lang"] != null)
            {
                var resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();

                var languageTag = request.QueryString["lang"];
                var language = Language.CreateLanguage(resourceProvider, languageTag);

                languagePreferenceProvider.SetPreferredLanguage(language);
            }

            var currentLanguage = languagePreferenceProvider.GetCurrentLanguage();
            var currentCulture = currentLanguage.AsCultureInfo();

            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }
    }
}
