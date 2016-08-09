using FluentValidation;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Validator.User
{
    public class AddUserValidator : AbstractValidator<Model.ViewModel.User>
    {
        public AddUserValidator()
        {
            base.CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("用户名不能为空！");

            RuleFor(x => x.PassWord).NotEmpty()
                .WithMessage("密码不能为空");

            RuleFor(x => x.Email).EmailAddress()
                .WithMessage("Email格式错误");
            RuleFor(x => x.UserName).Length(5, 16);

            RuleFor(x => x.UserName).Must(CheckUserName)
                .WithMessage("登录用户名已存在");
        }
        public bool CheckUserName(string userName)
        {
            UserSqlMap sqlMap = new UserSqlMap();
            return sqlMap.SelectUserByName(userName) != null;
        }
    }
}
