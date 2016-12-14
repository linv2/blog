using FluentValidation;

namespace MetroBlog.Core.Validator.Article
{
    public class SaveArticleValidator : AbstractValidator<Model.ViewModel.Article>
    {
        public SaveArticleValidator()
        {
            base.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("标题不能为空！");
            RuleFor(x => x.Title).Length(0, 200)
                .WithMessage("标题最大长度不能超过200");

            RuleFor(x => x.Content).NotEmpty()
                .WithMessage("内容不能为空");

            RuleFor(x => x.CategoryId).NotEqual(0)
                .WithMessage("请选择分类");
        }
    }
}
