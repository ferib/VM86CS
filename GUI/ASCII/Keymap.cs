using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x86CS.GUI.ASCII
{
    public static class Keymap
    {
        // arrays to map pixels to char
        public static byte[][] KeyTableB = { K_A, K_B };
        public static char[] KeyTableA = {'A', 'B'};

        #region UpperCase
        public static byte[] K_A = new byte[]
        {
            0x00, // 0000 0000
            0x00, // 0000 0000
            0xFC, // 1111 1100
            0x10, // 0001 0000
            0x38, // 0011 1000
            0x6C, // 0110 1100
            0xC6, // 1100 0110
            0xC6, // 1100 0110
            0xFE, // 1111 1110
            0xC6, // 1100 0110
            0xC6, // 1100 0110
            0xC6, // 1100 0110
            0xC6, // 1100 0110
            0x00, // 0000 0000
            0x00, // 0000 0000
        };

        public static byte[] K_B = new byte[]
        {
            0x00, // 0000 0000
            0x00, // 0000 0000
            0xFC, // 1111 1100
            0x66, // 0110 0110
            0x66, // 0110 0110
            0x66, // 0110 0110
            0x7C, // 0111 1100
            0x66, // 0110 0110
            0x66, // 0110 0110
            0x66, // 0110 0110
            0x66, // 0110 0110
            0xFC, // 1111 1100
            0x00, // 0000 0000
            0x00, // 0000 0000
        };

        // TODO: CDEF... 

        public static byte[] K_F = new byte[]
        {
            0x00, // 0000 0000
            0x00, // 0000 0000
            0xFC, // 1111 1100
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
            0x00, // 0000 0000
        };
        #endregion

        #region LowerCase
        // TODO
        #endregion

        #region Specials
        // TODO
        #endregion


    }
}
