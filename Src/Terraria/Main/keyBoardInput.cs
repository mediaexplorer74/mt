// keyBoardInput

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace GameManager
{
    public class keyBoardInput
    {
        public static bool slashToggle = true;

        public static event Action<char> newKeyEvent;

        //RnD
        static keyBoardInput()
        {
            //Application.AddMessageFilter((IMessageFilter)new keyBoardInput.inKey());
        }

        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static /*extern*/ bool TranslateMessage(IntPtr message)
        {
            return default;
        }

        public class inKey : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == 258)
                {
                    char ch = (char)(int)m.WParam;
                    Debug.WriteLine(ch);
                    if (keyBoardInput.newKeyEvent != null)
                        keyBoardInput.newKeyEvent(ch);
                }
                else if (m.Msg == 256)
                {
                    IntPtr num = Marshal.AllocHGlobal(Marshal.SizeOf((object)m));
                    Marshal.StructureToPtr((object)m, num, true);
                    //keyBoardInput.TranslateMessage(num);
                }
                return false;
            }
        }
    }
}
