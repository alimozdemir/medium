using System.Collections.Generic;
using ChatSample.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace ChatSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientList _ClientLists;

        public ClientController(IClientList ClientLists)
        {
            _ClientLists = ClientLists;
        }

        [HttpGet]
        public IEnumerable<Client> GetClients()
        {
            return _ClientLists.GetClients();
        }
    }
}