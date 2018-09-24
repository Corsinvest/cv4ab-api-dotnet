using System;
using System.Runtime.InteropServices;

namespace Corsinvest.AllenBradley.PLC.Api
{
    internal static class NativeMethod
    {
        private const string DLL_NAME = "plctag";

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr plc_tag_create([MarshalAs(UnmanagedType.LPStr)] string lpString);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_destroy", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_destroy(IntPtr tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_status", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_status(IntPtr tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_size", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_get_size(IntPtr tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_decode_error", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr plc_tag_decode_error(int error);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_read", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_read(IntPtr tag, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_write", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_write(IntPtr tag, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint16", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ushort plc_tag_get_uint16(IntPtr tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int16", CallingConvention = CallingConvention.Cdecl)]
        internal static extern short plc_tag_get_int16(IntPtr tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint16", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_uint16(IntPtr tag, int offset, ushort val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int16", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_int16(IntPtr tag, int offset, short val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint8", CallingConvention = CallingConvention.Cdecl)]
        internal static extern byte plc_tag_get_uint8(IntPtr tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int8", CallingConvention = CallingConvention.Cdecl)]
        internal static extern sbyte plc_tag_get_int8(IntPtr tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint8", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_uint8(IntPtr tag, int offset, byte val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int8", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_int8(IntPtr tag, int offset, sbyte val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_float32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float plc_tag_get_float32(IntPtr tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_float32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_float32(IntPtr tag, int offset, float val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint plc_tag_get_uint32(IntPtr tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_get_int32(IntPtr tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_uint32(IntPtr tag, int offset, uint val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_set_int32(IntPtr tag, int offset, int val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_lock", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_lock(IntPtr tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_unlock", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int plc_tag_unlock(IntPtr tag);
    }
}