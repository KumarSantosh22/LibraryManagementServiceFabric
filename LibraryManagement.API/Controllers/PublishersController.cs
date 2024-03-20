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
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublishersController()
        {
            ServiceProxyFactory proxyFactory = new ServiceProxyFactory(
                context => new FabricTransportServiceRemotingClientFactory());

            _publisherService = proxyFactory.CreateServiceProxy<IPublisherService>(
                new Uri("fabric:/LibraryManagement/LibraryManagement.PublisherService"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<ActionResult<PublisherDto[]>> Get()
        {
            return Ok(await _publisherService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> Get(string id)
        {
            return Ok(await _publisherService.GetAsync(new Guid(id)));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PublisherDto dto)
        {
            return Ok(await _publisherService.AddAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] PublisherDto dto)
        {
            return Ok(await _publisherService.UpdateAsync(new Guid(id), dto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _publisherService.DeleteAsync(new Guid(id));
            return Ok(new
            {
                Success = true,
                Message = "Deleted successfully!"
            });
        }
    }
}
