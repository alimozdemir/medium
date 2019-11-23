using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;

namespace hfdynamic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IBackgroundJobClient _client;

        public ValuesController(IBackgroundJobClient client)
        {
            _client = client;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var resolver = new DynamicTypeResolver();
            var asm = resolver.Load("plugin1/hfdynamic_job1.dll");
            
            var jobType = asm.GetType("hfdynamic_job1.JohnDoeJob");

            var method = jobType.GetMethod("ExecuteJob");

            var job = new Job(method, 100);
            var c = _client.Create(job, new Hangfire.States.EnqueuedState());
            
            // var result = _client.Enqueue(c);

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            _client.Enqueue(() => Console.WriteLine("Hello world."));
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
