using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Persistence.CustomAttribute;
using Core.Persistence.Repositories;

namespace Core.Security.Extensions;
public static class LimitProperties
{
    public static void LimitStringProperties<T>(this ICollection<T> collection) where T : class
    {

        
        foreach (var entity in collection)
        {
            var hashValue = string.Empty;
            var properties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite && p.CanRead);




            foreach (var property in properties)
            {
                var maxLengthAttribute = property.GetCustomAttribute<MaxLengthsAttribute>();
                var hash = property.GetCustomAttribute<HashAttribute>();
                var value = (string)property.GetValue(entity);
                if (maxLengthAttribute != null)
                {
                    var maxLength = maxLengthAttribute.MaxLength;


                    if (value != null && value.Length > maxLength)
                    {
                        value = value.Substring(0, maxLength);
                        property.SetValue(entity, value);


                    }

                }
                if (hash != null && value != null)
                    hashValue += value.ToString();




            }
            foreach (var property in properties)
            {
                var mainhash = property.GetCustomAttribute<MainHashAttribute>();

                if (mainhash != null)
                {
                    hashValue = MD5Hash.MD5Hashed(hashValue);
                    property.SetValue(entity, hashValue);
                }

            }


        }


    }
    public static void StringProperties<T>(this T collection) where T : class
    {

            var hashValue = string.Empty;
            var properties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite && p.CanRead);
        foreach (var property in properties)
        {
            var maxLengthAttribute = property.GetCustomAttribute<MaxLengthsAttribute>();
            var attributes = property.GetCustomAttributes();
            var x = !attributes.Equals(maxLengthAttribute);
            if (attributes != null && attributes.Any() && (attributes.Count() > 1))
            {

                property.SetValue(collection, null);
            }
        }


    }

    public static T ProcessStringProperties<T>(this T item) where T : class
    {
        var properties = item.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string) && p.CanWrite && p.CanRead);

        foreach (var property in properties)
        {
            var maxLengthAttribute = property.GetCustomAttribute<MaxLengthsAttribute>();
            var attributes = property.GetCustomAttributes();

            if (attributes != null && attributes.Any() && attributes.Count() > 1)
            {
                property.SetValue(item, null);
            }
        }

        return item;
    }

    public static void ProcessItems<T>(this IEnumerable<T?> items) where T : class
    {
        foreach (var item in items)
        {
            item.ProcessStringProperties();
        }
    }

    public static void ProcessList<T>(this List<T> list) where T : class
    {

            foreach (var item in list)
            {
                if (item is T clientSmBIOS)
                {
                    clientSmBIOS.ProcessStringProperties();
                }
            }
        
    }
    public static void ProcessListItems<T>(this List<T> list, Action<T> processItem) where T : class
    {
        foreach (var item in list)
        {
            processItem(item);
        }
    }




}
