using Application.Adapter.Contract.Request;
using Application.Adapter.Contract.Response;
using AutoMapper;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace Application.AdapterInbound.Contract.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoRequest, Todo>();

            CreateMap<Todo, TodoResponse>();
        }
    }
}
