using FluentValidation;
using MetroBlog.Core.Common;
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
        /// <param name="source"></param>
        /// <returns></returns>
        public static Rsp<TSource> Check<TValidator, TSource>(this TSource source) where TSource : class, IDbModelface
            where TValidator : AbstractValidator<TSource>, new()
        {
            var validator = new TValidator();
            var results = validator.Validate(source);

            if (results.IsValid)
            {
                return Rsp.Create<TSource>(null);
            }
            else
            {
                return Rsp.Error<TSource>(string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage)));
            }

        }




        public static void SetEntityValue<TSource>(this TSource model, NameValueCollection collection)
        {
            var sourceType = typeof(TSource);
            var properties = sourceType.GetProperties();
            foreach (var key in collection.AllKeys)
            {
                var value = collection[key];
                var property =
                    properties.FirstOrDefault(x => x.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase));
                if (property == null) continue;
                if (!property.PropertyType.FullName.StartsWith("System.")) continue;
                if (string.IsNullOrEmpty(value))
                {
                    continue;

                }
                try
                {

                    if ((value == "on" || value == "off") && property.PropertyType == typeof(bool))
                    {
                        value = value=="on" ? "True" : "False";
                    }
                    var setValue = Convert.ChangeType(value, property.PropertyType);
                    if (setValue == null) throw new ArgumentNullException(nameof(setValue));
                    property.SetValue(model, setValue, null);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
