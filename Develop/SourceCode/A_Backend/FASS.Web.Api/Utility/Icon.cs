using Common.Frame.Dtos.Account;
using Common.Frame.Models.Account;
using System.Text.RegularExpressions;

namespace FASS.Web.Api.Utility
{
    public static class Icon
    {
        private const string iconValue = "fa-solid";

        private static readonly string[] iconList = ["ep:", "ri:", "fa-solid:"];

        private static readonly Regex replaceRegex = new(@"^.*fa-solid\s*fa-", RegexOptions.IgnoreCase);
        private const string replaceValue = "fa-solid:";

        public static string? ToIcon(string? icon)
        {
            if (icon is not null)
            {
                if (iconList.Any(icon.StartsWith))
                {
                    return icon;
                }
                if (icon.Contains(iconValue))
                {
                    return replaceRegex.Replace(icon, replaceValue);
                }
            }
            return null;
        }

        public static IEnumerable<PermissionDto> ToIcon(IEnumerable<PermissionDto> icons)
        {
            foreach (var icon in icons)
            {
                icon.Icon = ToIcon(icon.Icon);
                yield return icon;
            }
        }

        public static IEnumerable<Permission> ToIcon(IEnumerable<Permission> icons)
        {
            foreach (var icon in icons)
            {
                icon.Icon = ToIcon(icon.Icon);
                yield return icon;
            }
        }
    }
}