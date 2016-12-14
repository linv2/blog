using FluentValidation;

namespace MetroBlog.Core.Validator.User
{
    public class SelectUserValidator : AbstractValidator<Model.ViewModel.User>
    {
        public SelectUserValidator()
        {
            base.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("登录账号不能为空！");

            RuleFor(x => x.PassWord).NotEmpty()
                .WithMessage("密码不能为空");


            RuleFor(x => x.UserName).Length(5, 20)
                .WithMessage("登录账号格式错误！");

            RuleFor(x => x.PassWord).Length(5, 20)
                .WithMessage("登录密码格式错误");
        }
    }
}
