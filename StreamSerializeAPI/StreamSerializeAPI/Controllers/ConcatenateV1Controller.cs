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
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/Concatenate")]
    public class ConcatenateV1Controller : ConcatenateBaseController
    {
        public ConcatenateV1Controller(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        [HttpGet]
        public async Task<ConcatenateModel<JRaw>> Get()
        {
            return await FetchData(this.GetAsRaw);
        }

    }
}
