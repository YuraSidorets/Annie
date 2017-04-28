using PMBotPluginService.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Elmah;

namespace PMBotPluginService.Controllers
{
    public class PluginController : ApiController
    {
        [HttpPost]
        [EnableCors("*", "*", "*")]
        public void Register()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpClient client = new HttpClient();
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var path = HttpContext.Current.Server.MapPath("~/bin/" + postedFile?.FileName);
                    postedFile?.SaveAs(path);

                    docfiles.Add(postedFile?.FileName);
                    client.PostAsJsonAsync(Request.Headers.Referrer.AbsoluteUri + "/Home/RegisterAssembly", docfiles);
                }
            }
            else
            {
                client.PostAsJsonAsync(Request.Headers.Referrer.AbsolutePath + "/Home/RegisterAssembly", new List<string>());
            }
        }

        [HttpPost]
        [EnableCors("*", "*", "*")]
        public void RegisterDependency()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var path = HttpContext.Current.Server.MapPath("~/bin/" + postedFile?.FileName);
                    postedFile?.SaveAs(path);
                }
            }
        }

        [HttpPost]
        public HttpResponseMessage Run(Parameter parameter)
        {
            object result = null;

            try
            {
                var assembly = Assembly.LoadFile(HttpContext.Current.Server.MapPath("~/bin/" + parameter.AssemblyName));

                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.Name.Contains("Bot"))
                    {
                        var methodName = "Run";
                        MethodInfo methodInfo = type.GetMethod(methodName);
                        if (methodInfo != null)
                        {
                            ParameterInfo[] parameters = methodInfo.GetParameters();
                            object classInstance = Activator.CreateInstance(type, null);

                            if (parameters.Length == 0)
                            {
                                result = methodInfo.Invoke(classInstance, null);
                            }
                            else
                            {
                                object[] parametersArray = { parameter.InvokeParameter };

                                result = methodInfo.Invoke(classInstance, parametersArray);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                return null;
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(result?.ToString(), System.Text.Encoding.UTF8, "text/html")
            }; 
        }
    }
}
