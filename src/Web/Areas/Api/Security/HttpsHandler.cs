using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Web.Areas.Api.Security
{
    public class HttpsHandler : DelegatingHandler
    {
        protected override Task SendAsync(HttpRequestMessage request,
         CancellationToken cancellationToken)
        {
            #if (!DEBUG)
            if (!String.Equals(request.RequestUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
            {
                return Task.Factory.StartNew(() =>
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("HTTPS Required")
                    };
                });
            }
            #endif
            return base.SendAsync(request, cancellationToken);
        }
    }    
}