using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Helper Tag
    /// </summary>
    public static class TagHelper
    {
        /// <summary>
        /// Create object from Type.
        /// </summary>
        /// <param name="length"></param>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static TType CreateObject<TType>(int length)
        {
            var obj = default(TType);
            var typeTType = typeof(TType);

            if (typeTType == typeof(string))
            {
                obj = (TType)((object)"");
            }
            else if (typeTType.IsArray)
            {
                obj = (TType)Activator.CreateInstance(typeTType, length);
            }
            else
            {
                obj = (TType)Activator.CreateInstance(typeTType);
            }

            FixStringNullToEmpty(obj);

            return obj;
        }

        /// <summary>
        /// Fix string null to empty.
        /// </summary>
        /// <param name="obj"></param>
        private static void FixStringNullToEmpty(object obj)
        {
            var type = obj.GetType();
            if (type == typeof(string))
            {
                if (obj == null) { obj = string.Empty; }
            }
            else if (type.IsArray && type.GetElementType() == typeof(string))
            {
                var array = GetArray(obj);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array.GetValue(i) == null) { array.SetValue(string.Empty, i); }
                }
            }
            else if (type.IsClass && !type.IsAbstract)
            {
                foreach (var property in GetAccessableProperties(type))
                {
                    if (property.PropertyType == typeof(string))
                    {
                        if (property.GetValue(obj) == null) { property.SetValue(obj, string.Empty); }
                    }
                    else
                    {
                        FixStringNullToEmpty(property.GetValue(obj));
                    }
                }
            }
        }

        internal static IEnumerable<PropertyInfo> GetAccessableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public)
                       .Where(p => p.GetSetMethod() != null);
        }

        internal static Array GetArray(object value)
        {
            var array = (Array)value;
            if (array.Length <= 0)
            {
                throw new Exception("Cannot determine size of class, " +
                                    "because an array is defined which has no fixed size greater than zero.");
            }
            return array;
        }

        /// <summary>
        /// Performs Linear scaling conversion. 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="minRaw"></param>
        /// <param name="maxRaw"></param>
        /// <param name="minScale"></param>
        /// <param name="maxScale"></param>
        /// <returns></returns>
        public static double ScaleLinear(this ITag tag, double minRaw, double maxRaw, double minScale, double maxScale)
        {
            if (minRaw > maxRaw || minScale > maxScale) { throw new InvalidOperationException(); }
            return (((maxScale - minScale) / (maxRaw - minRaw)) * (((double)tag.Value) - minRaw)) + minScale;
        }

        /// <summary>
        /// Performs SquareRoot conversion. 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="minRaw"></param>
        /// <param name="maxRaw"></param>
        /// <param name="minScale"></param>
        /// <param name="maxScale"></param>
        /// <returns></returns>
        public static double ScaleSquareRoot(this ITag tag, double minRaw, double maxRaw, double minScale, double maxScale)
        {
            if (minRaw > maxRaw || minScale > maxScale) { throw new InvalidOperationException(); }
            return (Math.Sqrt((((double)tag.Value) - minRaw) / (maxRaw - minRaw)) * (maxScale - minScale)) + minScale;
        }

        /// <summary>
        /// Number to bit array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BitArray NumberToBits(int value) { return new BitArray(new[] { value }); }

        /// <summary>
        /// Bite array to number
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static int BitsToNumber(BitArray bits)
        {
            if (bits == null) { throw new ArgumentNullException("binary"); }

            var result = new int[1];
            bits.CopyTo(result, 0);
            return result[0];
        }
    }
}