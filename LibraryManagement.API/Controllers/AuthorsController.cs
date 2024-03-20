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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController()
        {
            ServiceProxyFactory proxyFactory = new ServiceProxyFactory(
                context => new FabricTransportServiceRemotingClientFactory());

            _authorService = proxyFactory.CreateServiceProxy<IAuthorService>(
                new Uri("fabric:/LibraryManagement/LibraryManagement.AuthorService"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<ActionResult<AuthorDto[]>> Get()
        {
            return Ok(await _authorService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> Get(string id)
        {
            return Ok(await _authorService.GetAsync(new Guid(id)));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AuthorDto dto)
        {
            return Ok(await _authorService.AddAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] AuthorDto dto)
        {
            return Ok(await _authorService.UpdateAsync(new Guid(id), dto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _authorService.DeleteAsync(new Guid(id));
            return Ok(new
            {
                Success = true,
                Message = "Deleted successfully!"
            });
        }
    }
}
