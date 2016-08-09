using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Validator.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IService
{
    public interface IUserService
    {
        Rsp<Model.ViewModel.User> Login(Model.ViewModel.User mUser);
    }
}
