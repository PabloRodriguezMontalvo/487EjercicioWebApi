using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiEjercicioPersonas.Extensiones
{
   public class FiltroLog:Attribute,IActionFilter
    {
       public bool AllowMultiple {
           get { return true; }
       }
       public async Task<HttpResponseMessage> ExecuteActionFilterAsync
           (HttpActionContext actionContext, 
           CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
       {
           using (var f=File.AppendText(@"c:\log\LogGet.txt"))
           {
               f.WriteLine("Recibida peticion de get");
               foreach (var actionArgument in actionContext.ActionArguments)
               {
                  f.WriteLine(String.Format("{0}:{1}", actionArgument.Key,
                       actionArgument.Value));
               }


           }
           
           var resp = await continuation();

           return resp;
       }
    }
}
