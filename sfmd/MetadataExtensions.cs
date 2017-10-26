using System.Linq;
using System.Text;
using sfmd.SalesforceMetadataClient;

namespace sfmd
{
    public static class MetadataExtensions
    {
        public static string ToFriendlyString(this SaveResult result)
        {
            var sb = new StringBuilder();

            sb.Append(result.fullName);
            sb.Append(": ");
            sb.Append(result.success ? "Success!" : "Failure: ");
            if (result.errors != null)
            {
                sb.Append(string.Join(",", result.errors.Select(err => $"{err.message}: {err.extendedErrorDetails}")));
            }
            return sb.ToString();
        }

        public static string ToFriendlyString(this DeleteResult result)
        {
            var sb = new StringBuilder();

            sb.Append(result.fullName);
            sb.Append(": ");
            sb.Append(result.success ? "Success!" : "Failure: ");
            if (result.errors != null)
            {
                sb.Append(string.Join(",", result.errors.Select(err => $"{err.message}: {err.extendedErrorDetails}")));
            }
            return sb.ToString();
        }

        public static string ToFriendlyString(this CustomObject obj)
        {
            var sb = new StringBuilder();

            sb.AppendLine(obj.fullName);
            sb.AppendLine($"  Name field: {obj.nameField}");
            obj.fields?.ToList().ForEach(field => sb.AppendLine($"  field: {field.fullName}"));
            return sb.ToString();
        }
    }
}