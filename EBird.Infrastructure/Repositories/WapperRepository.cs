﻿using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    public class WapperRepository : IWapperRepository
    {
        private readonly ApplicationDbContext _context;
        
        private IBirdTypeRepository _birdTypeRepository;

        private IBirdRepository _birdRepository;

        private IGroupRepository _groupRepository;

        private IRoomRepository _roomRepository;

        private IResourceRepository _resourceRepository;
        
        private IPlaceRepository _placeRepository;
        
        private IRequestRepository _requestRepository;
        private IMatchDetailRepository _matchDetailRepository;
        private IMatchRepository _matchRepository;
        private INotificationRepository _NotificationRepository;
        private INotificationTypeRepository _NotificationTypeRepository;
        private IPostRepository _postRepository;
        private IGroupMemberRepository _groupMemberRepository;
        private IAccountRepository _accountRepository;

        public WapperRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBirdTypeRepository BirdType
        {
            get
            {
                if(_birdTypeRepository == null)
                {
                    _birdTypeRepository = new BirdTypeRepository(_context);
                }
                return _birdTypeRepository;
            }
        }

        public IBirdRepository Bird
        {
            get
            {
                if(_birdRepository == null)
                {
                    _birdRepository = new BirdRepository(_context);
                }
                return _birdRepository;
            }
        }
        
        public IAccountRepository Account
        {
            get
            {
                if(_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_context);
                }
                return _accountRepository;
            }
        }

        public IGroupRepository Group
        {
            get
            {
                if(_groupRepository == null)
                {
                    _groupRepository = new GroupRepository(_context);
                }
                return _groupRepository;
            }
        }


        public IRoomRepository Room
        {
            get
            {
                if (_roomRepository == null)
                {
                    _roomRepository = new RoomRepository(_context);
                }
                return _roomRepository;
            }
        }

        public IResourceRepository Resource 
        {
            get 
            {
                if(_resourceRepository == null)
                {
                    _resourceRepository = new  ResourceRepository(_context);
                }
                return _resourceRepository;
            }
        }
        public IPlaceRepository Place 
        {
            get 
            {
                if(_placeRepository == null)
                {
                    _placeRepository = new  PlaceRepository(_context);
                }
                return _placeRepository;
            }
        }

        public IRequestRepository Request
        {
            get
            {
                if(_requestRepository == null)
                {
                    _requestRepository = new RequestRepository(_context);
                }
                return _requestRepository;
            }
        }

        public IMatchDetailRepository MatchDetail
        {
            get
            {
                if(_matchDetailRepository == null)
                {
                    _matchDetailRepository = new MatchDetailRepository(_context);
                }
                return _matchDetailRepository;
            }
        }

        public IMatchRepository Match
        {
            get
            {
                if(_matchRepository == null)
                {
                    _matchRepository = new MatchRepository(_context);
                }
                return _matchRepository;
            }
        }
        public INotificationRepository Notification
        {
            get
            {
                if (_NotificationRepository == null)
                {
                    _NotificationRepository = new NotificationRepository(_context);
                }
                return _NotificationRepository;
            }
        }
        public INotificationTypeRepository NotificationType
        {
            get
            {
                if (_NotificationTypeRepository == null)
                {
                    _NotificationTypeRepository = new NotificationTypeRepository(_context);
                }
                return _NotificationTypeRepository;
            }
        }

        public IPostRepository Post
        {
            get
            {
                if (_postRepository == null)
                {
                    _postRepository = new PostRepository(_context);
                }
                return _postRepository;
            }
        }

        public IGroupMemberRepository GroupMember
        {
            get
            {
                if (_groupMemberRepository == null)
                {
                    _groupMemberRepository = new GroupMemberRepository(_context);
                }
                return _groupMemberRepository;
            }
        }
    }
}
