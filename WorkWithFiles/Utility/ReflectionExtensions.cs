using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithFiles.Utility
{
    internal static class ReflectionExtensions
    {
        public static void AssertProperty<T>(this T holder, string propertyName)
        {
            // Empty propertyName indicates to raise PropertyChanged for all properties by WPF design
            if (String.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            var property = holder
                .GetType()
                .GetProperties()
                .FirstOrDefault(prop => String.Equals(prop.Name, propertyName, StringComparison.Ordinal));

#if DEBUG
            System.Diagnostics.Debug.Assert(property != null,
                $"[BindableBase].[ValidatePropertyExists]: Property does not exist, type: {holder.GetType()}");
#endif
        }
    }
}
