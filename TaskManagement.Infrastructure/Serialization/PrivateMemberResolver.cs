using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace TaskManagement.Infrastructure.Serialization
{
    public class PrivateMemberResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object && jsonTypeInfo.CreateObject == null)
            {
                var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if (constructor != null)
                {
                    jsonTypeInfo.CreateObject = () => Activator.CreateInstance(type, true)!;
                }
            }

            foreach (var property in jsonTypeInfo.Properties)
            {
                if (property.Set == null)
                {
                    var propInfo = type.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (propInfo != null && propInfo.GetSetMethod(true) != null)
                    {
                        property.Set = (obj, value) => propInfo.SetValue(obj, value);
                    }
                }
            }

            return jsonTypeInfo;
        }
    }
}