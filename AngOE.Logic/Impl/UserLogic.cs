using System;
using System.Collections.Generic;
using System.Linq;
using AngOE.Common;
using AngOE.Entities;
using AngOE.Logic.Converter;
using AngOE.Logic.ILogic;
using AngOE.Logic.UICommands.User;
using AngOE.Logic.ViewModels;
using AngOE.Repository.IRepository;
using AngOE.Repository.UnitOfWork;

namespace AngOE.Logic.Impl
{
    public class UserLogic : IUserLogic
    {
        private const int AuthTokenValidDays = 1;
        //private const int ResetPasswordTokenValidDays = 1;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentTimeProvider _currentTimeProvider;

        public UserLogic(IUserRepository userRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            ICurrentTimeProvider currentTimeProvider)
        {
            _userRepository = userRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentTimeProvider = currentTimeProvider;
        }

        public void Add(CreateUserUiCommand command)
        {
            if (string.IsNullOrEmpty(command.Name))
            {
                throw new AngOeException(ErrorMessage.UserLoginFault);
            }

            if (string.IsNullOrEmpty(command.Email))
            {
                throw new AngOeException(ErrorMessage.UserEmailIsEmpty);
            }

            if (_userRepository.GetAll(x => x.Email == command.Email).Any())
            {
                throw new AngOeException(ErrorMessage.UserEmailIsExist);
            }

            if (string.IsNullOrEmpty(command.Password))
            {
                throw new AngOeException(ErrorMessage.UserPasswordIsEmpty);
            }

            if (command.Password != command.ConfirmPassword)
            {
                throw new AngOeException(ErrorMessage.UserPasswordIsNotEqualConfirmPassword);
            }
            var user = new User
            {
                Name = command.Name,
                Email = command.Email,
                PasswordHash = PasswordHasher.CreateHash(command.Password)
            };
            using (var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork())
            {
                _userRepository.Add(user);
                unitOfWork.Commit();
            }
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public void Update(UpdateUserUiCommand command)
        {
            if (string.IsNullOrEmpty(command.Name))
            {
                throw new AngOeException(ErrorMessage.UserLoginFault);
            }

            if (string.IsNullOrEmpty(command.Email))
            {
                throw new AngOeException(ErrorMessage.UserEmailIsEmpty);
            }

            if (_userRepository.GetAll(x => x.Id != command.Id && x.Email == command.Email).Any())
            {
                throw new AngOeException(ErrorMessage.UserEmailIsExist);
            }
            var user = _userRepository.Get(command.Id);

            user.Name = command.Name;
            user.Email = command.Email;

            using (var unitOfWork = _unitOfWorkFactory.GetCurrentUnitOfWork())
            {
                _userRepository.Update(user);
                unitOfWork.Commit();
            }
        }

        public UserViewModel Get(int userId)
        {
            return _userRepository.Get(userId).ToViewModel();
        }

        public IEnumerable<UserViewModel> GetAll()
        {
            return _userRepository.GetAll().Select(x => x.ToViewModel());
        }

        public IEnumerable<UserViewModel> GetAll(FilterUserUiCommand command)
        {
            return _userRepository.GetAll(x => string.IsNullOrEmpty(command.Name)
            || x.Name.Contains(command.Name)).Select(x => x.ToViewModel());
        }

        public PagedCollection<UserViewModel> GetAllByPageAndSorting(UserPageAndSortingUiCommand pageAndSorting)
        {
            int totalRecordes;
            var uses = _userRepository.GetPagingData(
                x => pageAndSorting.Filter.Name == null || x.Name.Contains(pageAndSorting.Filter.Name),
                pageAndSorting.PageNumber, PagedCollection<UserViewModel>.PerPageResults,
                pageAndSorting.OrderProperty, pageAndSorting.Ascending, out totalRecordes).Select(x => x.ToViewModel());

            return new PagedCollection<UserViewModel>(pageAndSorting.PageNumber, totalRecordes, uses);
        }

        public AuthenticatedViewModel Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new AngOeException(ErrorMessage.UserLoginFault);
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new AngOeException(ErrorMessage.UserLoginFault);
            }
            var currentUser = _userRepository.GetAll(x => x.Email == email).SingleOrDefault();
            if (currentUser == null)
            {
                throw new AngOeException(ErrorMessage.UserLoginFault);
            }
            if (!PasswordHasher.ValidateHash(password, currentUser.PasswordHash))
            {
                throw new AngOeException(ErrorMessage.UserLoginFault);
            }
            currentUser.AuthenticationToken = CreateToken();
            currentUser.LastLoginDate = _currentTimeProvider.CurrentTime();
            currentUser.AuthenticationTokenValidTo = _currentTimeProvider.CurrentTime()
                .AddDays(AuthTokenValidDays);
            _userRepository.Update(currentUser);
            return new AuthenticatedViewModel
            {
                AuthenticationToken = currentUser.AuthenticationToken,
                Email = currentUser.Email,
                Name = currentUser.Name
            };
        }

        private static string CreateToken()
        {
            return SHA256Hash.CreateHash(Guid.NewGuid().ToString());
        }
    }
}