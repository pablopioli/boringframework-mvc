using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Boring.Mvc
{
    public class DateModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(Date))
            {
                return new BinderTypeModelBinder(typeof(DateModelBinder));
            }

            return null;
        }
    }
}
