using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.User;

namespace MetroBlog.Core.Data.Service
{
    public class UserService : IUserService
    {
        private readonly UserSqlMap _sqlMap;
        public UserService(UserSqlMap sqlMap)
        {
            _sqlMap = sqlMap;
        }

        public Rsp<Model.ViewModel.User> Login(Model.ViewModel.User mUser)
        {
            var checkInfo = mUser.Check<SelectUserValidator, Model.ViewModel.User>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            var userInfo = _sqlMap.SelectUserByName(mUser.UserName);
            if (userInfo == null ||
                !userInfo.PassWord.Equals(Utility.Sha1(mUser.PassWord)))
            {
                return Rsp.Error<Model.ViewModel.User>("用户名或密码错误");
            }
            else
            {
                return Rsp.Create(userInfo);
            }
        }
    }
}
