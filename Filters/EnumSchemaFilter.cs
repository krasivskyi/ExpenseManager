using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;

namespace ExpenseManager.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                schema.Type = "string";
                schema.Format = null;
                
                var enumValues = Enum.GetValues(context.Type);
                foreach (var value in enumValues)
                {
                    if (value != null)
                    {
                        var name = value.ToString();
                        if (name != null)
                        {
                            var member = context.Type.GetMember(name).FirstOrDefault();
                            if (member != null)
                            {
                                var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
                                schema.Enum.Add(new OpenApiString(displayAttribute?.Name ?? name));
                            }
                        }
                    }
                }
            }
        }
    }
} 