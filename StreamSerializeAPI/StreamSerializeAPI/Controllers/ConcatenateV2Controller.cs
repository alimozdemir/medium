using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StreamSerializeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StreamSerializeAPI.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/Concatenate")]
    public class ConcatenateV2Controller : ConcatenateBaseController
    {
        public ConcatenateV2Controller(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        [HttpGet]
        public async Task<ConcatenateModel<JRaw>> Get()
        {
            return await FetchData(this.GetAsStream);
        }
    }
}
