using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace EBird.Application.Interfaces.IMapper
{
    public interface IMapFrom<T>
    {
        void MappingFrom(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}