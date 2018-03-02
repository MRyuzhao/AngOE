using AngOE.Entities;
using AngOE.Logic.ViewModels;

namespace AngOE.Logic.Converter
{
    //扩展方法
    public static class UserConverter
    {
        public static UserViewModel ToViewModel(this User me)
        {
            return new UserViewModel
            {
                Id = me.Id,
                Name = me.Name,
                Email = me.Email
            };
        }

    }
}