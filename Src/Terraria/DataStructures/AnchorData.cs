﻿// AnchorData

using GameManager.Enums;

namespace GameManager.DataStructures
{
    public struct AnchorData
    {
        public static AnchorData Empty = new AnchorData();
        public AnchorType type;
        public int tileCount;
        public int checkStart;

        public AnchorData(AnchorType t, int count, int start)
        {
            type = t;
            tileCount = count;
            checkStart = start;
        }

        public static bool operator ==(AnchorData data1, AnchorData data2)
        {
            if (data1.type == data2.type && data1.tileCount == data2.tileCount)
                return data1.checkStart == data2.checkStart;

            return false;
        }

        public static bool operator !=(AnchorData data1, AnchorData data2)
        {
            if (data1.type == data2.type && data1.tileCount == data2.tileCount)
                return data1.checkStart != data2.checkStart;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is AnchorData && type == ((AnchorData)obj).type && tileCount == ((AnchorData)obj).tileCount)
                return checkStart == ((AnchorData)obj).checkStart;

            return false;
        }

        public override int GetHashCode()
        {
            return (int)(ushort)type << 16 | (int)(byte)tileCount << 8 | (int)(byte)checkStart;
        }
    }
}
