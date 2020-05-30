using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace attrService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ITransientService _transientService;
        private readonly IScopedService _scopedService;
        private readonly ISingletonService _singletonService;

        public ServiceController(ITransientService transientService, IScopedService scopedService, ISingletonService singletonService)
        {
            _transientService = transientService;
            _scopedService = scopedService;
            _singletonService = singletonService;
        }

        [HttpGet("Transient")]
        public string Transient()
        {
            return _transientService.GetValue();
        }

        [HttpGet("Scoped")]
        public string Scoped()
        {
            return _scopedService.GetValue();
        }

        [HttpGet("Singleton")]
        public string Singleton()
        {
            return _singletonService.GetValue();
        }

    }
}
