using AutoFixture.Kernel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TaskManagement.Api.UnitTests.SpecimanBuilders
{
    public class PrivateSetterSpecimenBuilder : ISpecimenBuilder
    {
        private readonly IEnumerable<Type> _types;

        public PrivateSetterSpecimenBuilder(params Type[] types)
        {
            _types = types;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (request is not Type type || !_types.Contains(type))
                return new NoSpecimen();

            var instance = RuntimeHelpers.GetUninitializedObject(type);

            foreach (var prop in type.GetProperties(
                         BindingFlags.Instance | BindingFlags.Public))
            {
                var setter = prop.GetSetMethod(nonPublic: true);
                if (setter == null) continue;

                var value = context.Resolve(prop.PropertyType);
                if (value is not NoSpecimen)
                    prop.SetValue(instance, value);
            }

            return instance;
        }
    }
}
