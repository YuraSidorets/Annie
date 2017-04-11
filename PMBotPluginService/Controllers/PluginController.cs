using PMBotPluginService.Models;
using System;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace PMBotPluginService.Controllers
{
    public class PluginController : ApiController
    {
        [HttpPost]
        public void Register()
        {
            //register plugin as {Call Name - Dll name}
        }

        [HttpPost]
        public string Run(Parameter parameter)
        {
            object result = null;

            try
            {
                var assembly = Assembly.LoadFile(HttpContext.Current.Server.MapPath("/Plugins/" + parameter.AssemblyName));

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
                                object[] parametersArray = new object[] { parameter.InvokeParameter };

                                result = methodInfo.Invoke(classInstance, parametersArray);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result.ToString();
        }
    }
}
