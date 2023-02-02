using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class BirdService : IBirdService
    {
        private IWapperRepository _repository;
        private IMapper _mapper;

        public BirdService(IWapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BirdDTO> AddBird(BirdDTO birdDTO)
        {
            await BirdValidation.ValidateBird(birdDTO, _repository);

            BirdEntity birdEntity = _mapper.Map<BirdEntity>(birdDTO);

            birdEntity = await _repository.Bird.AddBirdAsync(birdEntity);

            if(birdEntity == null)
            {
                throw new BadRequestException("Bird is not added");
            }
            
            return birdDTO;
        }

        public async Task<BirdDTO> DeleteBird(Guid birdID)
        {
            var birdEntity = await _repository.Bird.SoftDeleteBirdAsync(birdID);
            
            if(birdEntity == null)
            {
                throw new NotFoundException("Not found Bird for delete");
            }
            
            return _mapper.Map<BirdDTO>(birdEntity);
        }

        public async Task<BirdDTO> GetBird(Guid birdID)
        {
            var birdEntity = await _repository.Bird.GetBirdActiveAsync(birdID);
            
            if(birdEntity == null)
            {
                throw new NotFoundException("Can not found bird");
            }
            
            var birdDTO = _mapper.Map<BirdDTO>(birdEntity);
            
            return birdDTO;
        }

        public async Task<List<BirdDTO>> GetBirds()
        {
            var listBirdEntity = await _repository.Bird.GetBirdsActiveAsync();
            
            if(listBirdEntity.Count == 0)
            {
                throw new NotFoundException("Can not found bird");
            }
            
            var listBirdDTO = _mapper.Map<List<BirdDTO>>(listBirdEntity);
            
            return listBirdDTO;
        }

        public async Task<PagedList<BirdDTO>> GetBirdsByPagingParameters(BirdParameters parameters)
        {
            BirdValidation.ValidateBirdParameter(parameters);
            
            var birdList = await _repository.Bird.GetBirdsActiveAsync(parameters);

            var birdListDTO = _mapper.Map<PagedList<BirdDTO>>(birdList);
            birdListDTO.MapMetaData(birdList);


            return birdListDTO;
        }
        
        public async Task<BirdDTO> UpdateBird(Guid birdID, BirdDTO birdDTO)
        {
            await BirdValidation.ValidateBird(birdDTO, _repository);

            var birdEntity = await _repository.Bird.GetBirdActiveAsync(birdID);

            if(birdEntity == null)
            {
                throw new NotFoundException("Can not found bird");
            }

            _mapper.Map(birdDTO, birdEntity);

            var result = await _repository.Bird.UpdateBirdAsync(birdEntity);

            if(result == null)
            {
                throw new BadRequestException("Update is fail");
            }

            return birdDTO;
        }

        public async Task<List<BirdDTO>> GetAllBirdByAccount(Guid accountId)
        {
            var listBirdEntity = await _repository.Bird.GetAllBirdActiveByAccountId(accountId);
            if(listBirdEntity.Count == 0)
            {
                throw new NotFoundException("Can not found bird");
            }
            return _mapper.Map<List<BirdDTO>>(listBirdEntity);
        }

    }
}
