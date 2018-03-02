using System.Collections.Generic;
using System.Web.Http;
using AngOE.Logic.ILogic;
using AngOE.Logic.UICommands.User;
using AngOE.Logic.ViewModels;

namespace AngOE.API.Controllers.API
{
    public class UserController : BaseApiController
    {
        private readonly IUserLogic _userLogic;
        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [Route("api/users")]
        [HttpPost]
        public void Post([FromBody] CreateUserUiCommand command)
        {
            Execute(() =>
            {
                _userLogic.Add(command);
            });
        }

        [Route("api/users/{id}")]
        [HttpDelete]
        public void Delete(int id)
        {
            Execute(() =>
            {
                _userLogic.Delete(id);
            });
        }

        [Route("api/users")]
        [HttpPut]
        public void Put([FromBody] UpdateUserUiCommand command)
        {
            Execute(() =>
            {
                _userLogic.Update(command);
            });
        }

        [Route("api/users")]
        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            return Execute(() =>_userLogic.GetAll());
        }

        [Route("api/users/filter")]
        [HttpPost]
        public IEnumerable<UserViewModel> Filter([FromBody] FilterUserUiCommand command)
        {
            return Execute(() => _userLogic.GetAll(command));
        }

        [Route("api/users/pagination")]
        [HttpPost]
        public PagedCollection<UserViewModel> Pagination([FromBody] UserPageAndSortingUiCommand pageAndSorting)
        {
            return Execute(() => _userLogic.GetAllByPageAndSorting(pageAndSorting));
        }
    }
}