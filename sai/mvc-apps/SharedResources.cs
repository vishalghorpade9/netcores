using Microsoft.Extensions.Localization;
using System.Reflection;

namespace mvc_apps
{
    public class SharedResources
    {
    }
    public interface ISharedViewLocalizer
    {
        public LocalizedString this[string key]
        {
            get;
        }
        LocalizedString GetLocalizedString(string key);
    }

    public class SharedViewLocalizer : ISharedViewLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public SharedViewLocalizer(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString this[string key] => _localizer[key];

        public LocalizedString GetLocalizedString(string key)
        {
            return _localizer[key];
        }
    }
}
