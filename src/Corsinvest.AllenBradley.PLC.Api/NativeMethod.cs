using System;
using System.Runtime.InteropServices;

namespace Corsinvest.AllenBradley.PLC.Api
{
    internal static class NativeMethod
    {
        private const string DLL_NAME = "plctag";

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_create([MarshalAs(UnmanagedType.LPStr)] string lpString, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_destroy", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_destroy(int tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_abort", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_abort(int tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_status", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_status(int tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_size", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_get_size(int tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_decode_error", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr plc_tag_decode_error(int error);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_read", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_read(int tag, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_write", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_write(int tag, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint16", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ushort plc_tag_get_uint16(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int16", CallingConvention = CallingConvention.Cdecl)]
        internal static extern short plc_tag_get_int16(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint16", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_uint16(int tag, int offset, ushort val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int16", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_int16(int tag, int offset, short val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint8", CallingConvention = CallingConvention.Cdecl)]
        internal static extern byte plc_tag_get_uint8(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int8", CallingConvention = CallingConvention.Cdecl)]
        internal static extern sbyte plc_tag_get_int8(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint8", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_uint8(int tag, int offset, byte val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int8", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_int8(int tag, int offset, sbyte val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_float32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float plc_tag_get_float32(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_float32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_float32(int tag, int offset, float val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_float64", CallingConvention = CallingConvention.Cdecl)]
        internal static extern double plc_tag_get_float64(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_float64", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_float64(int tag, int offset, double val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint plc_tag_get_uint32(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_get_int32(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_uint32(int tag, int offset, uint val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_int32(int tag, int offset, int val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint64", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong plc_tag_get_uint64(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int64", CallingConvention = CallingConvention.Cdecl)]
        internal static extern long plc_tag_get_int64(int tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint64", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_uint64(int tag, int offset, ulong val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int64", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_int64(int tag, int offset, long val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_lock", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_lock(int tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_unlock", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_unlock(int tag);
    }
}