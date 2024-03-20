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
    public class BookTransactionsController : ControllerBase
    {
        private readonly IBookTransactionService _bookTransactionService;

        public BookTransactionsController()
        {
            ServiceProxyFactory proxyFactory = new ServiceProxyFactory(
                context => new FabricTransportServiceRemotingClientFactory());

            _bookTransactionService = proxyFactory.CreateServiceProxy<IBookTransactionService>(
                new Uri("fabric:/LibraryManagement/LibraryManagement.BookTransactionService"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<ActionResult<BookTransactionDto[]>> Get()
        {
            return Ok(await _bookTransactionService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookTransactionDto>> Get(int id)
        {
            return Ok(await _bookTransactionService.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BookTransactionDto dto)
        {
            return Ok(await _bookTransactionService.AddAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] BookTransactionDto dto)
        {
            return Ok(await _bookTransactionService.UpdateAsync(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _bookTransactionService.DeleteAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Deleted successfully!"
            });
        }
    }
}
