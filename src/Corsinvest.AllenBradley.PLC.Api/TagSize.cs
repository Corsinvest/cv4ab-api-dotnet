using System;
using System.Collections.Generic;
using System.Linq;

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
        public const int INT64 = 8;

        /// <summary>
        /// 
        /// </summary>
        public const int UINT64 = INT64;

        /// <summary>
        /// 
        /// </summary>
        public const int FLOAT32 = 4;

        /// <summary>
        /// 
        /// </summary>
        public const int FLOAT64 = 8;

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
            { typeof(Int64), INT64 },
            { typeof(UInt64), UINT64 },
            { typeof(Int32), INT32 },
            { typeof(UInt32), UINT32 },
            { typeof(Int16), INT16 },
            { typeof(UInt16), UINT16 },
            { typeof(sbyte), INT8 },
            { typeof(byte), UINT8 },
            { typeof(float), FLOAT32 },
            { typeof(double), FLOAT64 },
            { typeof(string), STRING },
        };

        /// <summary>
        /// Get size from object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetSizeObject(object obj)
        {
            var size = 0;

            var type = obj.GetType();
            if (type.IsArray)
            {
                foreach (var el in TagHelper.GetArray(obj)) { size += GetSizeObject(el); }
            }
            else
            {
                if (!NativeTypes.TryGetValue(type, out size))
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        size += TagHelper.GetAccessableProperties(type)
                                         .Select(a => GetSizeObject(a.GetValue(obj)))
                                         .Sum();
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