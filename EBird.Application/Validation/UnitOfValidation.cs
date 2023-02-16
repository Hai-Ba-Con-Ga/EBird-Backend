using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;

namespace EBird.Application.Validation
{
    public class UnitOfValidation : IUnitOfValidation
    {
        private IGroupValidation _group;
        // private IRequestValidation _request;
        private IBaseValidation _base;
        private IRoomValidation _room;
        private IBirdValidation _bird;
        private IBirdTypeValidation _birdType;
        private IWapperRepository _repository;

        public UnitOfValidation(IWapperRepository repository)
        {
            _repository = repository;
        }

        public IGroupValidation Group
        {
            get
            {
                if (_group == null)
                {
                    _group = new GroupValidation(_repository);
                }
                return _group;
            }

        }

        // public IRequestValidation Request
        // {
        //     get
        //     {
        //         if (_request == null)
        //         {
        //             _request = new RequestValidation(_repository);
        //         }
        //         return _request;
        //     }
        // }

        public IRoomValidation Room 
        {
            get
            {
                if (_room == null)
                {
                    _room = new RoomValidation(_repository);
                }
                return _room;
            }
        }

        public IBirdValidation Bird 
        {
            get
            {
                if (_bird == null)
                {
                    _bird = new BirdValidation(_repository);
                }
                return _bird;
            }
        }

        public IBirdTypeValidation BirdType 
        {
            get
            {
                if (_birdType == null)
                {
                    _birdType = new BirdTypeValidation(_repository);
                }
                return _birdType;
            }
        }

        public IBaseValidation Base
        {
            get
            {
                if (_base == null)
                {
                    _base = new BaseValidation(_repository);
                }
                return _base;
            }
        }
    }
}