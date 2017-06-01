using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace javascripttest
{
    public class NetRecognizePic
    {
        [DllImport("CoFalcon.dll", SetLastError = true, EntryPoint = "C1_UserReg")]
        public static extern int C1_UserReg(string lpUser, string lpPass);

        [DllImport("CoFalcon.dll", SetLastError = true, EntryPoint = "C1_UserPay")]
        public static extern int C1_UserPay(string lpUser, string lpCard, ref int OutInt);

        [DllImport("CoFalcon.dll", EntryPoint = "C1_GetScore")]
        public static extern int C1_GetScore(string lpUser, string lpPass, ref int OutInt);

        [DllImport("CoFalcon.dll", SetLastError = true, EntryPoint = "C1_UpFile")]
        public static extern int C1_UpFile(string lpPicPath, string lpUser, string lpPass, int lpSoftId, int lpCodeType, int lpLenMin, int lpTimeAdd, string lpStrAdd, ref long OutInt, StringBuilder OutStr);

        [DllImport("CoFalcon.dll", SetLastError = true, EntryPoint = "C1_UpBytes")]
        public static extern int C1_UpBytes(byte[] pPicBytes, int LenBytes, string lpUser, string lpPass, int lpSoftId, int lpCodeType, int lpLenMin, int lpTimeAdd, string lpStrAdd, ref long OutInt, StringBuilder OutStr);

        [DllImport("CoFalcon.dll", SetLastError = true, EntryPoint = "C1_ReportError")]
        public static extern int C1_ReportError(string lpUser, string lpPass, long lpPicId, int lpSoftId);
    }
}
