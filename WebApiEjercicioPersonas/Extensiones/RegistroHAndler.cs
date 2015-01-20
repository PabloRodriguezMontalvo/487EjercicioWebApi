using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApiEjercicioPersonas.Models;

namespace WebApiEjercicioPersonas.Extensiones
{
   public class RegistroHAndler:DelegatingHandler
    {
       protected override Task<HttpResponseMessage>
        SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
       {
           using (var db = new PersonasEntities())
           {
               var id = 0;


               try
               {
                   id= db.Registroes.Max(o=>o.id);
               }
               catch (Exception e)
               {
                   Console.WriteLine(e);
               }

               id++;

               var re = new Registro()
               {
                   fecha = DateTime.Now,
                   origen = request.Headers.From??"",
                   destino = request.Method + ":" + request.RequestUri,
                   id=id


               };
               db.Registroes.Add(re);
               db.SaveChanges();

           }

           return base.SendAsync(request, cancellationToken);
       }

    }
}
