using System;
using System.Windows.Markup;

namespace ApartmentPanel.Presentation.MarkupExtensions
{
    internal class EnumSourceExtention : MarkupExtension
    {
        private readonly Type _enumType;

        public EnumSourceExtention(Type enumType)
        {
            if (enumType == null || !enumType.IsEnum)
                throw new ArgumentNullException($"{nameof(enumType)} must not be null and of type Enum");
            _enumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_enumType);
        }
    }
}
