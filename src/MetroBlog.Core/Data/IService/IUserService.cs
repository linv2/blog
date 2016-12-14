using MetroBlog.Core.Common;

namespace MetroBlog.Core.Data.IService
{
    public interface IUserService
    {
        Rsp<Model.ViewModel.User> Login(Model.ViewModel.User mUser);
    }
}
