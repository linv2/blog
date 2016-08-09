using Nancy;
using Nancy.ModelBinding;
using Nancy.Authentication.Token;
using MetroBlog.Core.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Common;

namespace MetroBlog.Admin
{
    public class AuthModule : NancyModule
    {
        private IUserService userService { get; set; }
        private ITokenizer tokenizer { get; set; }
        public AuthModule(IUserService userService, ITokenizer tokenizer) : base("auth")
        {
            
            this.userService = userService;
            this.tokenizer = tokenizer;
            Get["/"] = _ => Index();
            Post["/"] = _ => Login(this.Bind<User>());
        }
        public dynamic Index()
        {
            return 404;
        }

        public dynamic Login(User mUser)
        {
            var rsp = userService.Login(mUser);
            if (!rsp.error)
            {
                var userIdentity = new DefaultUserIdentity(rsp.data.UserName);
                var token = tokenizer.Tokenize(userIdentity, Context);
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
