using AutoMapper;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IMapper _mapper;
        private readonly IWapperRepository _repository;
        private readonly IUnitOfValidation _validation;

        public FriendshipService(IMapper mapper, IWapperRepository repository, IUnitOfValidation validation)
        {
            _mapper = mapper;
            _repository = repository;
            _validation = validation;
        }

        public async Task CreateInvitaion(Guid userId, Guid receiverId)
        {
            await _validation.Base.ValidateAccountId(receiverId);

            var fiendship = new FriendshipEntity()
            {
                InviterId = userId,
                ReceiverId = receiverId,
                CreateDatetime = DateTime.Now,

            };

            await _repository.Friendship.CreateAsync(fiendship);            

        }
    }
}
