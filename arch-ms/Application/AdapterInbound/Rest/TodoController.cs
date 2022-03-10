using Domain.Common;
using Domain.Entities;
using Domain.Entities.Common;
using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.AdapterInbound.Rest
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService todoService)
        {
            _service = todoService;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Post([FromBody] Todo todo)
        {
            todo.Audit = new Audit("Julio Oliveira");
            await _service.Create(todo);

            return CreatedAtAction(nameof(Post), todo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> Get(string id)
        {
            var todo = await _service.GetById(id);

            return Ok(todo);
        }

        [HttpGet("query")]
        public async Task<ActionResult<Todo>> GetPages([FromQuery] FilterBase _filters)
        {
            var todos = await _service.GetPages(_filters.SkipSize, _filters.LimitSize);

            return Ok(todos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> Put([FromBody] Todo todo, string id)
        {
            await _service.Update(id, todo);

            return Ok(todo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);

            return Ok();
        }

    }
}
