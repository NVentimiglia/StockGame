using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Framework
{

    public static class BinaryHelper
    {
        /// <summary>
        /// considers unity sanity of -1 (true) and 0 (false)
        /// </summary>
        public static bool IsFlagged(this int field, int mask)
        {
            if (mask == -1)
                return true;

            if (mask == 0)
                return false;

            int layer = 1 << field;

            return ((mask & layer) != 0);
        }

        public static bool IsBitSet(this int b, int pos)
        {
            if (pos < 0 || pos > 32)
                throw new ArgumentOutOfRangeException("pos", "Index must be in the range of 0-32.");

            return (b & (1 << pos)) != 0;
        }

        public static int SetBits(this int b, int[] map)
        {
            var mask = b;
            for (int i = 0; i < map.Length; ++i)
            {
                mask = SetBit(mask, map[i]);
            }
            return mask;
        }

        public static int SetBit(this int b, int pos)
        {
            if (pos < 0 || pos > 32)
                throw new ArgumentOutOfRangeException("pos", "Index must be in the range of 0-32.");

            return (int)(b | (1 << pos));
        }

        //

        public static bool IsBitSet(this byte b, int pos)
        {
            if (pos < 0 || pos > 32)
                throw new ArgumentOutOfRangeException("pos", "Index must be in the range of 0-7.");

            return (b & (1 << pos)) != 0;
        }

        public static byte SetBit(this byte b, int pos)
        {
            if (pos < 0 || pos > 7)
                throw new ArgumentOutOfRangeException("pos", "Index must be in the range of 0-7.");

            return (byte)(b | (1 << pos));
        }

        public static byte UnsetBit(this byte b, int pos)
        {
            if (pos < 0 || pos > 7)
                throw new ArgumentOutOfRangeException("pos", "Index must be in the range of 0-7.");

            return (byte)(b & ~(1 << pos));
        }

        public static byte ToggleBit(this byte b, int pos)
        {
            if (pos < 0 || pos > 7)
                throw new ArgumentOutOfRangeException("pos", "Index must be in the range of 0-7.");

            return (byte)(b ^ (1 << pos));
        }

        public static string ToBinaryString(this byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }

        //

        /// <summary>
        /// Binary conact of two sets to form a unique id
        /// </summary>
        /// <param name="parentId">left half</param>
        /// <param name="childId">right half</param>
        /// <returns>byte[3]</returns>
        public static int BinaryCombine(ushort parentId, ushort childId)
        {
            return ((int)parentId << 16) + childId;
        }

        /// <summary>
        /// Binary conact of two sets to form a unique id
        /// </summary>
        /// <param name="parentId">left half</param>
        /// <param name="childId">right half</param>
        /// <returns>byte[3]</returns>
        public static int BinaryCombine(short parentId, short childId)
        {
            return ((int)parentId << 16) + childId;
        }

        /// <summary>
        /// Binary conact of two sets to form a unique id
        /// </summary>
        /// <param name="parentId">left half</param>
        /// <param name="childId">right half</param>
        /// <returns>byte[3]</returns>
        public static long BinaryCombine(int parentId, int childId)
        {
            return ((long)parentId << 32) + childId;
        }

        //


        private static byte ConvertBoolArrayToByte(params bool[] source)
        {
            byte result = 0;
            // This assumes the array never contains more than 8 elements!
            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }

        private static bool[] ConvertByteToBoolArray(byte b)
        {
            // prepare the return result
            bool[] result = new bool[8];

            // check each bit in the byte. if 1 set to true, if 0 set to false
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            // reverse the array
            Array.Reverse(result);

            return result;
        }

        //public unsafe static void SetByteBool(bool input, int index, ref byte result)
        //{
        //    result = *((byte*)(&input));
        //}

        //public unsafe static bool GetByteBool(byte input, int index)
        //{
        //    return (input & (1 << index)) == 0 ? false : true;
        //}

        //public unsafe static void GetByteBool(byte input, int index, ref bool result)
        //{
        //    result = GetByteBool(input, index);
        //}

        //https://stackoverflow.com/questions/17638800/storing-two-float-values-in-a-single-float-variable
        public static float Pack(Vector2 input, int precision = 4096)
        {
            Vector2 output = input;
            output.x = Mathf.Floor(output.x * (precision - 1));
            output.y = Mathf.Floor(output.y * (precision - 1));

            return (output.x * precision) + output.y;
        }

        public static float Pack(float inputx, float inputy, int precision = 4096)
        {
            return Pack(new Vector2 { x = inputx, y = inputy }, precision);
        }

        public static Vector2 Unpack(float input, int precision = 4096)
        {
            Vector2 output = Vector2.zero;

            output.y = input % precision;
            output.x = Mathf.Floor(input / precision);

            return output / (precision - 1);
        }

        public static int MakeInt(short left, short right)
        {
            //implicit conversion of left to a long
            int res = left;

            //shift the bits creating an empty space on the right
            // ex: 0x0000CFFF becomes 0xCFFF0000
            res = (res << 16);

            //combine the bits on the right with the previous value
            // ex: 0xCFFF0000 | 0x0000ABCD becomes 0xCFFFABCD
            res = res | (int)(ushort)right; //uint first to prevent loss of signed bit

            //return the combined result
            return res;
        }

        public static long MakeLong(int left, int right)
        {
            //implicit conversion of left to a long
            long res = left;

            //shift the bits creating an empty space on the right
            // ex: 0x0000CFFF becomes 0xCFFF0000
            res = (res << 32);

            //combine the bits on the right with the previous value
            // ex: 0xCFFF0000 | 0x0000ABCD becomes 0xCFFFABCD
            res = res | (long)(uint)right; //uint first to prevent loss of signed bit

            //return the combined result
            return res;
        }

        /// <summary>
        /// Interop services
        /// </summary>
        public static byte[] StructureToPtr<T>(T s) where T : struct
        {
            //http://genericgamedev.com/general/converting-between-structs-and-byte-arrays/
            var size = Marshal.SizeOf(typeof(T));
            var array = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(s, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }

        /// <summary>
        /// Interop services
        /// </summary>
        public static T PtrToStructure<T>(byte[] array) where T : struct
        {
            //http://genericgamedev.com/general/converting-between-structs-and-byte-arrays/
            var size = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, size);
            var s = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return s;
        }
    }
}