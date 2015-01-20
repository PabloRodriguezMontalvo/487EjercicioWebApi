using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEjercicioPersonas.Extensiones
{
   public class AuthHandler:DelegatingHandler
    {

        private GenericPrincipal Autenticar(String us, String pwd)
        {
            if (us == pwd)
            {
                var id = new GenericIdentity(us);
                var pr = new GenericPrincipal(id, new string[] { "Pringao" });
                return pr;
            }
            return null;

        }
        protected override async Task<HttpResponseMessage>
            SendAsync(HttpRequestMessage request,
            System.Threading.CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null &&
                request.Headers.Authorization.Scheme == "Basic")
            {
                var datos = request.Headers.Authorization.Parameter.Trim();
                var us =
                    Encoding.Default.GetString(Convert.FromBase64String(datos));
                var logpas = us.Split(':');
                var principal = Autenticar(logpas[0], logpas[1]);
                if (principal == null)
                {
                    var resp = request.CreateResponse(HttpStatusCode.
                        Unauthorized);
                    resp.Headers.Add("WWW-Authenticate", "Basic");
                    return resp;
                }
                else
                {
                    request.GetRequestContext().Principal = principal;
                }
            }
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                response.Headers.Add("WWW-Authenticate", "Basic");

            return response;
        } 
    }
}
