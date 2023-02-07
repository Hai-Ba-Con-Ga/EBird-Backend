using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IMapper
{
    public interface IMapTo<T>
    {
        void MappingTo(Profile profile) => profile.CreateMap(GetType(), typeof(T));

    }
}
