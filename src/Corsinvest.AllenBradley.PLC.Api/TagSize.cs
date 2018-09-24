using System;
using System.Collections.Generic;
using System.Reflection;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Tag size definition
    /// </summary>
    public static class TagSize
    {
        /// <summary>
        /// 
        /// </summary>
        public const int INT8 = 1;

        /// <summary>
        /// 
        /// </summary>
        public const int UINT8 = INT8;

        /// <summary>
        /// 
        /// </summary>
        public const int INT16 = 2;

        /// <summary>
        /// 
        /// </summary>
        public const int UINT16 = INT16;

        /// <summary>
        /// 
        /// </summary>
        public const int INT32 = 4;

        /// <summary>
        /// 
        /// </summary>
        public const int UINT32 = INT32;

        /// <summary>
        /// 
        /// </summary>
        public const int FLOAT32 = 4;

        /// <summary>
        /// 
        /// </summary>
        public const int STRING = 88;

        /// <summary>
        /// Native type definition.
        /// </summary>
        /// <value></value>
        public static IReadOnlyDictionary<Type, int> NativeTypes { get; } = new Dictionary<Type, int>
        {
            { typeof(int) , INT32 },
            { typeof(uint) , UINT32 },
            { typeof(short) , INT16 },
            { typeof(ushort) , UINT16 },
            { typeof(sbyte) , INT8 },
            { typeof(byte) , UINT8 },
            { typeof(string) , STRING },
            { typeof(float) , FLOAT32},
            { typeof(double) , FLOAT32},
        };

        /// <summary>
        /// Get size from object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetSizeFromObject(object obj)
        {
            var size = 0;

            var type = obj.GetType();
            if (type.IsArray)
            {
                foreach (var el in TagValueManager.GetArray(obj)) { size += GetSizeFromObject(el); }
            }
            else
            {
                if (!NativeTypes.TryGetValue(type, out size))
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        foreach (var property in TagValueManager.GetAccessableProperties(type))
                        {
                            size += GetSizeFromObject(property.GetValue(obj));
                        }
                    }
                }
            }

            return size;
        }

        /// <summary>
        /// Check type is native type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNativeType(Type type) { return NativeTypes.ContainsKey(type); }
    }
}