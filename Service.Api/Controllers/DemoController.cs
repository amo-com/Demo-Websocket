using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Api.Common;
using Service.Api.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Api.Controllers
{
    [Route("demo")]
    [EnableCors("AllowCors")]
    [ApiController]
    public class DemoController : Controller
    {
        protected readonly HttpContext _httpContext;
        protected readonly WebServiceHelper _webService;
        protected readonly string _wsUrl;

        public DemoController(IHttpContextAccessor accessor, IConfiguration configuration)
        {
            this._httpContext = accessor.HttpContext;
            this._wsUrl = configuration.GetValue<string>("Setting:WSUrl");
            this._webService = new WebServiceHelper(_wsUrl);
        }

        [HttpGet]
        [Route("ws")]
        public async Task<MachineLogVo> GetWSData()
        {
            return await _webService.Post<MachineLogVo>();
        }
    }
}
