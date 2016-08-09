using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class UserSqlMap : SqlMapBase
    {
        public int AddUser(Model.ViewModel.User mUser)
        {
            return base.Insert<int>("addUser", mUser);
        }

        public int UpdateUser(Model.ViewModel.User mUser)
        {
            return base.Update("updateUser", mUser);
        }

        public Model.ViewModel.User SelectUserByName(string userName)
        {
            return base.QueryForObject<Model.ViewModel.User>("selectUserByName", userName);

        }
    }
}
