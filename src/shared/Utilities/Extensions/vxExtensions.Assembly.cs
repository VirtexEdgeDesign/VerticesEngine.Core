using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static partial class vxExtensions
{
    public static IEnumerable<Type> GetTypesWithAttribute(this System.Reflection.Assembly assembly, Type attributeType)
    {
        return assembly.GetTypes().Where(m => m.GetCustomAttributes(attributeType, false).Length > 0).ToArray();
    }
}