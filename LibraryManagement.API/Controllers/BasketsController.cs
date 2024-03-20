using LibraryManagement.AppuserActor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<ActionResult> Get(string userId)
        {
            IAppuserActor actor = GetActor(userId);
            return Ok(await actor.GetBasket());
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> Post(string userId, [FromBody] BasketItem basketItem)
        {
            IAppuserActor actor = GetActor(userId);
            await actor.AddToBasket(basketItem);
            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> Delete(string userId)
        {
            IAppuserActor actor = GetActor(userId);
            await actor.ClearBasket();
            return Ok();
        }


        private IAppuserActor GetActor(string userId)
        {
            return ActorProxy.Create<IAppuserActor>(
                new ActorId(userId),
                new Uri("fabric:/LibraryManagement/AppuserActorService"));
        }
    }
}
