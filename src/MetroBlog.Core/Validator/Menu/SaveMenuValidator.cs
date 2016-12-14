using FluentValidation;

namespace MetroBlog.Core.Validator.Menu
{
    public class SaveMenuValidator : AbstractValidator<Model.ViewModel.Menu>
    {
        public SaveMenuValidator()
        {
            base.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("名称不能为空！");
            RuleFor(x => x.Name).Length(0, 10)
                .WithMessage("名称最大长度不能超过10");
            //RuleFor(x => x.Name).Length(0, 50)
            //    .WithMessage("链接地址最大长度不能超过10");
            RuleFor(x => x.Target).NotEmpty()
                .WithMessage("Target不能为空！");


            RuleFor(x => x).Must(CheckLink)
           .WithMessage("链接地址/分类名称不能为空");
        }


        public bool CheckLink(Model.ViewModel.Menu mMenu)
        {
            if (mMenu.CategoryId == 0)
            {
                return !string.IsNullOrEmpty(mMenu.Link);
            }
            else
            {
                return true;
            }
        }
    }
}
