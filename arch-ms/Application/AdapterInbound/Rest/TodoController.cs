using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.AdapterInbound.Rest
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Todo>> Get()
        {
            var todo = new Todo
            {
                Name = "Julio Oliveira",
                Email = "juliocto2011@gmail.com"                
            };

            return Ok(todo);
        }
    }
}
