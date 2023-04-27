using Extender;

namespace SolaERP.Persistence.Utils
{
    public static class AttributeExtensions
    {
        public static void SetAttribuute(this Attribute attribute, string className, string propertyName, Type propertyType, bool isReadonly = false, object[] attributeParams = null)
        {
            var attributeType = attribute.GetType();

            var typeExtender = new TypeExtender(className);
            typeExtender.AddProperty(propertyName, propertyType, attributeType, attributeParams, isReadonly);
        }
    }
}
