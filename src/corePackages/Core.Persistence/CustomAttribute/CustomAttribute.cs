using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.CustomAttribute;

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthsAttribute : Attribute
{
    public int MaxLength { get; }

    public MaxLengthsAttribute(int maxLength)
    {
        MaxLength = maxLength;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class MainHashAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class HashAttribute : Attribute
{
}
