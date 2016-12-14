
namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class UserSqlMap : SqlMapBase
    {
        public int AddUser(Model.ViewModel.User mUser)
        {
            return Insert<int>("addUser", mUser);
        }

        public int UpdateUser(Model.ViewModel.User mUser)
        {
            return Update("updateUser", mUser);
        }

        public Model.ViewModel.User SelectUserByName(string userName)
        {
            return QueryForObject<Model.ViewModel.User>("selectUserByName", userName);

        }
    }
}
