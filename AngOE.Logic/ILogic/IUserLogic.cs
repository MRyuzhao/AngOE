using System.Collections.Generic;
using AngOE.Logic.UICommands.User;
using AngOE.Logic.ViewModels;

namespace AngOE.Logic.ILogic
{
    public interface IUserLogic
    {
        void Add(CreateUserUiCommand command);

        void Delete(int id);

        void Update(UpdateUserUiCommand command);

        UserViewModel Get(int userId);

        IEnumerable<UserViewModel> GetAll();

        IEnumerable<UserViewModel> GetAll(FilterUserUiCommand command);

        PagedCollection<UserViewModel> GetAllByPageAndSorting(UserPageAndSortingUiCommand pageAndSorting);

        AuthenticatedViewModel Login(string reqEmail, string reqPassword);
    }
}