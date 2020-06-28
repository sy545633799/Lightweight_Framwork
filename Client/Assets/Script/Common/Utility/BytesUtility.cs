// ========================================================
// des：
// author: 
// time：2020-06-26 11:24:52
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game {
	public class BytesUtility : MonoBehaviour
	{
        public static ushort SwapUInt16(ushort n)
        {
            //Debug.Log("SwapUInt16() BitConverter.IsLittleEndian : " + BitConverter.IsLittleEndian.ToCString());
            if (BitConverter.IsLittleEndian)
                return (ushort)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
            else
                return n;
        }
        public static short SwapInt16(short n)
        {
            if (BitConverter.IsLittleEndian)
                return (short)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
            else
                return n;
        }
        public static int SwapInt32(int n)
        {
            if (BitConverter.IsLittleEndian)
                return (int)(((SwapInt16((short)n) & 0xffff) << 0x10) |
                    (SwapInt16((short)(n >> 0x10)) & 0xffff));
            else
                return n;
        }

        public static uint SwapUInt32(uint n)
        {
            return (uint)(((SwapUInt16((ushort)n) & 0xffff) << 0x10) |
                (SwapUInt16((ushort)(n >> 0x10)) & 0xffff));
        }

        public static long SwapInt64(long n)
        {
            return (long)(((SwapInt32((int)n) & 0xffffffffL) << 0x20) |
                (SwapInt32((int)(n >> 0x20)) & 0xffffffffL));
        }

        public static ulong SwapUInt64(ulong n)
        {
            return (ulong)(((SwapUInt32((uint)n) & 0xffffffffL) << 0x20) |
                (SwapUInt32((uint)(n >> 0x20)) & 0xffffffffL));
        }

        public static int FindMaterial(Material[] materials, string materialName)
        {
            // var materials = renderer.materials;
            int materialIndex = -1;
            for (int i = 0; i < materials.Length; i++)
            {
                // Debug.Log("material name :"+materials[i].name+"  "+materials[i].name.IndexOf(materialName));
                if (-1 != materials[i].name.IndexOf(materialName))
                {
                    materialIndex = i;
                    break;
                }
            }
            return materialIndex;
        }
    }
}
