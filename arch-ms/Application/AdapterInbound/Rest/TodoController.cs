using Domain.Entities;
using Domain.Services;
using Domain.Common;
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
            //await _service.Get

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo([FromServices] TodoService service, [FromBody] Todo todo)
        {
            todo.Audit = new Audit("Julio Oliveira");
            await service.Create(todo);

            return CreatedAtAction(nameof(PostTodo), todo);
        }
    }
}
