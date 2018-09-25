using System;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Helper Tag
    /// </summary>
    public static class TagHelper
    {
        internal static TType CreateObject<TType>(int length)
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

            TagValueManager.FixStringNullToEmpty(obj);

            return obj;
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
    }
}