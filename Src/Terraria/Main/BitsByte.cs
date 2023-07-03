﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

namespace GameManager
{
    public struct BitsByte
    {
        private static bool Null;
        private byte value;

        public bool this[int key]
        {
            get
            {
                return ((int)this.value & 1 << key) != 0;
            }
            set
            {
                if (value)
                    this.value |= (byte)(1 << key);
                else
                    this.value &= (byte)~(1 << key);
            }
        }

        public BitsByte(bool b1 = false, bool b2 = false, bool b3 = false, bool b4 = false, bool b5 = false, bool b6 = false, bool b7 = false, bool b8 = false)
        {
            this.value = (byte)0;
            this[0] = b1;
            this[1] = b2;
            this[2] = b3;
            this[3] = b4;
            this[4] = b5;
            this[5] = b6;
            this[6] = b7;
            this[7] = b8;
        }

        public static implicit operator byte(BitsByte bb)
        {
            return bb.value;
        }

        public static implicit operator BitsByte(byte b)
        {
            return new BitsByte()
            {
                value = b
            };
        }

        public void ClearAll()
        {
            this.value = (byte)0;
        }

        public void SetAll()
        {
            this.value = byte.MaxValue;
        }

        public void Retrieve(ref bool b0)
        {
            this.Retrieve(ref b0, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
        }

        public void Retrieve(ref bool b0, ref bool b1)
        {
            this.Retrieve(ref b0, ref b1, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
        }

        public void Retrieve(ref bool b0, ref bool b1, ref bool b2)
        {
            this.Retrieve(ref b0, ref b1, ref b2, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
        }

        public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3)
        {
            this.Retrieve(ref b0, ref b1, ref b2, ref b3, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
        }

        public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4)
        {
            this.Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
        }

        public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5)
        {
            this.Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref b5, ref BitsByte.Null, ref BitsByte.Null);
        }

        public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5, ref bool b6)
        {
            this.Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref b5, ref b6, ref BitsByte.Null);
        }

        public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5, ref bool b6, ref bool b7)
        {
            b0 = this[0];
            b1 = this[1];
            b2 = this[2];
            b3 = this[3];
            b4 = this[4];
            b5 = this[5];
            b6 = this[6];
            b7 = this[7];
        }
    }
}
