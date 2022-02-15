using Domain.Entities;
using Domain.Services;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Common;

namespace Application.AdapterInbound.Rest
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Todo>> Post([FromServices] TodoService service, [FromBody] Todo todo)
        {
            todo.Audit = new Audit("Julio Oliveira");
            await service.Create(todo);

            return CreatedAtAction(nameof(Post), todo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> Get([FromServices] TodoService service, string id)
        {
            var todo = await service.GetById(id);

            return Ok(todo);
        }

        [HttpGet("query")]
        public async Task<ActionResult<Todo>> GetPages([FromServices] TodoService service, [FromQuery] FilterBase _filters)
        {
            var todos = await service.GetPages(_filters.SkipSize, _filters.LimitSize);

            return Ok(todos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> Put([FromServices] TodoService service, [FromBody] Todo todo, string id)
        {
            await service.Update(id, todo);

            return Ok(todo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromServices] TodoService service, string id)
        {
            await service.Delete(id);

            return Ok();
        }

    }
}
