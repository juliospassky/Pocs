using Application.Adapter.Amqp.Interfaces;
using Application.Adapter.Contract.Request;
using Application.Adapter.Contract.Response;
using AutoMapper;
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
        private readonly ITodoOutBound _outBound;
        private readonly IMapper _mapper;


        public TodoController(ITodoService todoService, ITodoOutBound outBound, IMapper mapper)
        {
            _service = todoService;
            _outBound = outBound;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TodoResponse), 202)]
        public async Task<ActionResult<Todo>> Post([FromBody] TodoRequest todoRequest)
        {
            var todo = _mapper.Map<Todo>(todoRequest);
            var response = _mapper.Map<TodoResponse>(todo);

            var todo = await _outBound.TodoSend(todoRequest);
            return Accepted(nameof(Post), todo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> Get(string id)
        {
            var todo = await _service.GetById(id);

            return Ok(todo);
        }


    }
}
