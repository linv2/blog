using Nancy;
using Nancy.ModelBinding;
using Nancy.Authentication.Token;
using MetroBlog.Core.Model.ViewModel;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Common;

namespace MetroBlog.Admin.Module
{
    public class AuthModule : NancyModule
    {
        private IUserService UserService { get; set; }
        private ITokenizer Tokenizer { get; set; }
        public AuthModule(IUserService userService, ITokenizer tokenizer) : base("auth")
        {

            UserService = userService;
            Tokenizer = tokenizer;
            Get["/"] = _ => Index();
            Post["/"] = _ => Login(this.Bind<User>());
        }
        public dynamic Index()
        {
            return 404;
        }

        public dynamic Login(User mUser)
        {
            var rsp = UserService.Login(mUser);
            if (!rsp.error)
            {
                var userIdentity = new CustomUserIdentity(rsp.data.UserName);
                var token = Tokenizer.Tokenize(userIdentity, Context);
                Session["token"] = token;

                return Response.AsJson(Rsp.Success);
            }
            else
            {
                return Response.AsJson(rsp);
            }
        }
    }
}
