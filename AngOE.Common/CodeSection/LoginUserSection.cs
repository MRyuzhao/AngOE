namespace AngOE.Common.CodeSection
{
    public class LoginUserSection : BaseCodeSection
    {
        private LoginUserSection(LoginUserInformationForCodeSection systemUser)
        {
            this.currentUser = systemUser;
            BeginSection();
        }

        private LoginUserInformationForCodeSection currentUser { get; set; }


        public static LoginUserSection Start(LoginUserInformationForCodeSection systemUser)
        {
            var section = new LoginUserSection(systemUser);
            return section;
        }

        public static bool IsInSection
        {
            get
            {
                return BaseCodeSection.IsThreadInSection<LoginUserSection>();
            }
        }

        public static LoginUserInformationForCodeSection CurrentUser
        {
            get
            {
                if (IsInSection)
                {
                    var root = BaseCodeSection.GetSectionRoot<LoginUserSection>();
                    return root.currentUser;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}