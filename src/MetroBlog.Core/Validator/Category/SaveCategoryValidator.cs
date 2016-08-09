﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Validator.Category
{
    public class SaveCategoryValidator : AbstractValidator<Model.ViewModel.Category>
    {
        public SaveCategoryValidator()
        {
            base.CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("名称不能为空！");
            RuleFor(x => x.Name).Length(0, 10)
                .WithMessage("名称最大长度不能超过10");
        }
    }
}
