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
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController()
        {
            ServiceProxyFactory proxyFactory = new ServiceProxyFactory(
                context => new FabricTransportServiceRemotingClientFactory());

            _bookService = proxyFactory.CreateServiceProxy<IBookService>(
                new Uri("fabric:/LibraryManagement/LibraryManagement.BookService"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<ActionResult<BookDto[]>> Get()
        {
            return Ok(await _bookService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> Get(string id)
        {
            return Ok(await _bookService.GetAsync(new Guid(id)));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BookDto dto)
        {
            return Ok(await _bookService.AddAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] BookDto dto)
        {
            return Ok(await _bookService.UpdateAsync(new Guid(id), dto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _bookService.DeleteAsync(new Guid(id));
            return Ok(new
            {
                Success = true,
                Message = "Deleted successfully!"
            });
        }
    }
}
