﻿using System.Collections;

namespace x86CS
{
    public static class Util
    {
        public static int CountSet(this BitArray bits)
        {
            int count = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                    count++;
            }

            return count;
        }

        public static byte GetLow(this byte b)
        {
            return (byte)(b & 0x0f);
        }

        public static byte SetLow(this byte b, byte value)
        {
            return (byte)((b & 0xf0) + (value & 0x0f));
        }

        public static byte SetHigh(this byte b, byte value)
        {
            return (byte)((value.GetLow() << 4) + b.GetLow());
        }

        public static byte GetHigh(this byte b)
        {
            return (byte)((b >> 4) & 0x0f);
        }

        public static ushort GetLow(this ushort b)
        {
            return (ushort)(b & 0x00ff);
        }

        public static ushort SetLow(this ushort b, ushort value)
        {
            return (byte)((b & 0xff00) + (value & 0x00ff));
        }

        public static ushort SetHigh(this ushort b, ushort value)
        {
            return (ushort)((value.GetLow() << 8) + b.GetLow());
        }

        public static ushort GetHigh(this ushort b)
        {
            return (ushort)((b >> 8) & 0x00ff);
        }
    }
}
