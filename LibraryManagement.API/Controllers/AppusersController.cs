using LibraryManagement.Interfaces.Contracts.Services;
using LibraryManagement.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppusersController : ControllerBase
    {
        private readonly IAppuserService _appuserService;

        public AppusersController()
        {
            ServiceProxyFactory proxyFactory = new ServiceProxyFactory(
                context => new FabricTransportServiceRemotingClientFactory());

            _appuserService = proxyFactory.CreateServiceProxy<IAppuserService>(
                new Uri("fabric:/LibraryManagement/LibraryManagement.AppuserService"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<ActionResult<AppuserDto[]>> Get()
        {
            return Ok(await _appuserService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppuserDto>> Get(string id)
        {
            return Ok(await _appuserService.GetAsync(new Guid(id)));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AppuserDto appuser)
        {
            return Ok(await _appuserService.AddAsync(appuser));
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] AppuserDto appuser)
        {
            return Ok(await _appuserService.UpdateAsync(new Guid(id), appuser));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _appuserService.DeleteAsync(new Guid(id));
            return Ok(new
            {
                Success = true,
                Message = "Deleted successfully!"
            });
        }
    }
}
