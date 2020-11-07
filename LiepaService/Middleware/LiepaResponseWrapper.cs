using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LiepaService.Models.Views;
using LiepaService.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace LiepaService.Middleware {
    public class LiepaResponseWrapper : ObjectResultExecutor
    {
        public LiepaResponseWrapper(OutputFormatterSelector formatterSelector, 
                                                IHttpResponseStreamWriterFactory writerFactory, 
                                                ILoggerFactory loggerFactory, 
                                                IOptions<MvcOptions> mvcOptions) : base(formatterSelector, writerFactory, loggerFactory, mvcOptions)
        {
        }

        public override Task ExecuteAsync(ActionContext context, ObjectResult result)
        {
            ResponseView response = result.Value is ResponseView ? 
                                    result.Value as ResponseView : 
                                    CreateSuccessResponse(result.Value as UserView);
            
            result.Value = response;

            return base.ExecuteAsync(context, result);
        }

        private ResponseView CreateSuccessResponse(UserView user) {
            return new ResponseView() {
                Success = true,
                User = user
            };
        }
    }
}