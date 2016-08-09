using FluentValidation;
using FluentValidation.Results;
using MetroBlog.Core.Common;
using MetroBlog.Core;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace MetroBlog.Core
{
    public static class DbModelEx
    {

        /// <summary>
        /// 验证实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Rsp<TSource> Check<TValidator, TSource>(this TSource source) where TSource : class, IDbModelface
            where TValidator : AbstractValidator<TSource>, new()
        {
            TValidator validator = new TValidator();
            ValidationResult results = validator.Validate(source);

            if (results.IsValid)
            {
                return Rsp.Create<TSource>(null);
            }
            else
            {
                var error = results.Errors.Select(x => x.ErrorMessage);
                return Rsp.Error<TSource>(String.Join(System.Environment.NewLine, results.Errors.Select(x => x.ErrorMessage)));
            }

        }




        public static void SetEntityValue<TSource>(this TSource model, NameValueCollection collection)
            where TSource : IDbModelface
        {
            var sourceType = typeof(TSource);
            var properties = sourceType.GetProperties();
            foreach (var key in collection.AllKeys)
            {
                var value = collection[key];
                var property = properties.Where(x => x.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (property != null)
                {
                    object _value = null;
                    if (property.PropertyType.FullName.StartsWith("System."))
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                value = null;
                            }
                            _value = Convert.ChangeType(value, property.PropertyType);
                            property.SetValue(model, _value, null);
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
