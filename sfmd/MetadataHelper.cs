using System.Linq;
using System.Text;
using sfmd.SalesforceMetadataClient;

namespace sfmd
{
    public class MetadataHelper
    {
        public static string ToFriendlyString(SaveResult result)
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

        public static string ToFriendlyString(CustomObject obj)
        {
            var sb = new StringBuilder();

            sb.AppendLine(obj.fullName);
            sb.AppendLine($"  Name field: {obj.nameField}");
            obj.fields.ToList().ForEach(field => sb.AppendLine($"  field: {field.fullName}"));

            return sb.ToString();
        }
    }
}