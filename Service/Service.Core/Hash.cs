using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public static class Hash
    {
        private static ulong FNV_Prime = 1099511628211;
        private static ulong Offest_basis = 14695981039346656037;

        private static uint FNV_32_PRIME = 0x01000193; //Which is Integer 16777619
        private static uint FNV1_32_INIT = 0x811c9dc5; //Which is Integer 2166136261

        public static uint GenerateHash32(String str)
        {
            uint hash = FNV1_32_INIT;
            foreach (char c in str)
            {
                /* xor the bottom with the current octet */
                hash ^= (uint)c;

                /* multiply by the 64 bit FNV magic prime mod 2^32 */
                hash += (hash << 1) + (hash << 4) + (hash << 7) + (hash << 8) + (hash << 24);
            }

            return hash;
        }

        public static ulong GenerateHash64(String str)
        {
            ulong hash = Offest_basis;
            foreach (char c in str)
            {
                hash ^= (ulong)c;

                /* multiply by the 64 bit FNV magic prime mod 2^64 */
                hash += (hash << 1) + (hash << 4) + (hash << 5) + (hash << 7) + (hash << 8) + (hash << 40);
            }

            return hash;
        }
    }
}
