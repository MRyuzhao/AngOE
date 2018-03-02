namespace AngOE.Common
{
    public class ErrorMessage
    {
        public const string InternalServerError = "服务器内部错误，请联系管理员";
        public const string UserLoginFault = "用户名或密码不正确！";
        public const string AuthenticationTokenMissing = "Token无效或者已过期！";
        public const string UserIsNotExist = "当前用户不存在！";
        public const string ResetPasswordTokenIsNotNull = "重置密码Token错误！";
        public const string ResetPasswordTokenMissing = "重置密码Token错误！";
        public const string ResetPasswordTokenExpired = "重置密码Token过期！";
        public const string ResetPasswordFault = "重置密码失败！";
        public const string OldPasswordError = "旧密码错误！";
        public const string ChangePasswordFault = "修改密码失败！";
        public const string AuthenticationTokenExpired = "Token无效或者已过期！";

        public const string UserNameIsEmpty = "姓名不能為空！";
        public const string UserEmailIsEmpty = "Email不能為空！";
        public const string UserEmailIsExist = "Email已存在！";
        public const string UserPasswordIsEmpty = "初始密碼不能為空！";
        public const string UserPasswordIsNotEqualConfirmPassword = "密碼和確認密碼不符！";
    }
}