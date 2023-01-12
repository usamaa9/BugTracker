namespace PurpleRock.WebApi.Common.Swagger;

/// <summary>
/// Swagger schema filter to modify description of enum types so they
/// show the XML docs attached to each member of the enum.
/// </summary>
internal class DescribeEnumMembers : ISchemaFilter
{
    private readonly XDocument? _mXmlComments;

    /// <summary>
    /// Initializes a new instance of the <see cref="DescribeEnumMembers"/> class.
    /// </summary>
    /// <param name="fileInfo">Document containing XML docs for enum members.</param>
    public DescribeEnumMembers(FileInfo fileInfo)
    {
        _mXmlComments = XDocument.Load(fileInfo.FullName);
    }

    /// <summary>
    /// Apply this schema filter.
    /// </summary>
    /// <param name="schema">Target schema object.</param>
    /// <param name="context">Schema filter context.</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var enumType = context.Type;

        if (!enumType.IsEnum) return;

        var sb = new StringBuilder(schema.Description);

        var enumValues = Enum.GetValues(enumType).Cast<int>().ToList();
        var enumNames = Enum.GetNames(enumType);

        var index = 0;

        sb.AppendLine("<ul>");

        foreach (var enumMemberName in enumNames)
        {
            var fullEnumMemberName = $"F:{enumType.FullName}.{enumMemberName}";

            var enumMemberDescription =
                _mXmlComments!.XPathEvaluate($"normalize-space(//member[@name = '{fullEnumMemberName}']/summary/text())") as
                    string;

            if (string.IsNullOrEmpty(enumMemberDescription)) continue;

            sb
                .Append("<li><b>")
                .Append(enumValues[index])
                .Append(" - ")
                .Append(enumMemberName)
                .Append("</b>: ")
                .Append(enumMemberDescription)
                .AppendLine("</li>");

            index++;
        }

        if (index == 0) return;

        sb.AppendLine("</ul>");
        schema.Description = sb.ToString();
    }
}
