using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace javascripttest
{ 
    public class TOCRdeclares
    {
        public const int TOCR_OK = 0;
        public const int TOCRCONFIG_DEFAULTJOB = -1;
        public const int TOCRCONFIG_DLL_ERRORMODE = 0;
        public const int TOCRCONFIG_SRV_ERRORMODE = 1;
        public const int TOCRCONFIG_SRV_THREADPRIORITY = 2;
        public const int TOCRCONVERTFORMAT_DIBFILE = 1;
        public const int TOCRCONVERTFORMAT_MMFILEHANDLE = 3;
        public const int TOCRCONVERTFORMAT_TIFFFILE = 0;
        public const int TOCRDEFERRORMODE = -1;
        public const int TOCRERRORMODE_LOG = 2;
        public const int TOCRERRORMODE_MSGBOX = 1;
        public const int TOCRERRORMODE_NONE = 0;
        public const int TOCRGETRESULTS_EXTENDED = 1;
        public const int TOCRGETRESULTS_NORESULTS = -1;
        public const int TOCRGETRESULTS_NORMAL = 0;
        public const short TOCRJOBMSGLENGTH = 0x200;
        public const byte TOCRJOBORIENT_180 = 2;
        public const byte TOCRJOBORIENT_270 = 3;
        public const byte TOCRJOBORIENT_90 = 1;
        public const byte TOCRJOBORIENT_AUTO = 0;
        public const byte TOCRJOBORIENT_OFF = 0xff;
        public const int TOCRJOBSLOT_BLOCKEDBYOTHER = -2;
        public const int TOCRJOBSLOT_BLOCKEDBYYOU = 2;
        public const int TOCRJOBSLOT_FREE = 0;
        public const int TOCRJOBSLOT_OWNEDBYOTHER = -1;
        public const int TOCRJOBSLOT_OWNEDBYYOU = 1;
        public const int TOCRJOBSTATUS_BUSY = 0;
        public const int TOCRJOBSTATUS_DONE = 1;
        public const int TOCRJOBSTATUS_ERROR = -1;
        public const int TOCRJOBSTATUS_IDLE = 2;
        public const int TOCRJOBTYPE_DIBCLIPBOARD = 2;
        public const int TOCRJOBTYPE_DIBFILE = 1;
        public const int TOCRJOBTYPE_MMFILEHANDLE = 3;
        public const int TOCRJOBTYPE_TIFFFILE = 0;
        public const int TOCRLICENCE_EURO = 2;
        public const int TOCRLICENCE_EUROUPGRADE = 3;
        public const int TOCRLICENCE_STANDARD = 1;
        public const int TOCRLICENCE_V3PRO = 6;
        public const int TOCRLICENCE_V3PROUPGRADE = 7;
        public const int TOCRLICENCE_V3SE = 4;
        public const int TOCRLICENCE_V3SEPROUPGRADE = 8;
        public const int TOCRLICENCE_V3SEUPGRADE = 5;
        public const int TOCRMAXPPM = 0x13395;
        public const int TOCRMINPPM = 0x3d8;
        public const int TOCRSHUTDOWNALL = -1;
        public const int TOCRWAIT_CONNECTIONBROKEN = 2;
        public const int TOCRWAIT_FAILED = -1;
        public const int TOCRWAIT_NOJOBSFOUND = -2;
        public const int TOCRWAIT_OK = 0;
        public const int TOCRWAIT_SERVICEABORT = 1;

        public TOCRdeclares()
        {
            
        }

        [DllImport("TOCRDll", CharSet = CharSet.Ansi)]
        public static extern int TOCRConvertFormat(int JobNo, string InputAddr, int InputFormat, string OutputAddr, int OutputFormat, int PageNo);
        [DllImport("TOCRDll", CharSet = CharSet.Ansi)]
        public static extern int TOCRConvertFormat(int JobNo, string InputAddr, int InputFormat, ref IntPtr OutputAddr, int OutputFormat, int PageNo);
        [DllImport("TOCRDll", CharSet = CharSet.Ansi)]
        public static extern int TOCRDoJob(int JobNo, ref TOCRJOBINFO JobInfo);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetErrorMode(int JobNo, ref int ErrorMode);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetJobDBInfo(int Zero);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetJobDBInfo(IntPtr JobSlotInf);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetJobResults(int JobNo, ref int ResultsInf, ref byte Bytes);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetJobResults(int JobNo, ref int ResultsInf, int Zero);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetJobResultsEx(int JobNo, int Mode, ref int ResultsInf, ref byte Bytes);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetJobResultsEx(int JobNo, int Mode, ref int ResultsInf, int Zero);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetJobStatus(int JobNo, ref int JobStatus);
        [DllImport("TOCRDll")]
        public static extern int TOCRGetJobStatusEx(int JobNo, ref int JobStatus, ref float Progress, ref int AutoOrientation);
        [DllImport("TOCRDll", CharSet = CharSet.Ansi)]
        public static extern int TOCRGetJobStatusMsg(int JobNo, StringBuilder Msg);
        [DllImport("TOCRDll", CharSet = CharSet.Ansi)]
        public static extern int TOCRGetLicenceInfoEx(int JobNo, StringBuilder Licence, ref int Volume, ref int Time, ref int Remaining, ref int Features);
        [DllImport("TOCRDll", CharSet = CharSet.Ansi)]
        public static extern int TOCRGetNumPages(int JobNo, string Filename, int JobType, ref int NumPages);
        [DllImport("TOCRDll")]
        public static extern int TOCRInitialise(ref int JobNo);
        [DllImport("TOCRDll")]
        public static extern int TOCRRotateMonoBitmap(ref IntPtr hBmp, int Width, int Height, int Orientation);
        [DllImport("TOCRDll")]
        public static extern int TOCRSetErrorMode(int JobNo, int ErrorMode);
        [DllImport("TOCRDll")]
        public static extern int TOCRShutdown(int JobNo);
        [DllImport("TOCRDll")]
        public static extern int TOCRWaitForAnyJob(ref int WaitAnyStatus, ref int JobNo);
        [DllImport("TOCRDll")]
        public static extern int TOCRWaitForJob(int JobNo, ref int JobStatus);

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOCRJOBINFO
        {
            public int StructId;
            public int JobType;
            public string InputFile;
            public int PageNo;
            public TOCRdeclares.TOCRPROCESSOPTIONS ProcessOptions;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOCRPROCESSOPTIONS
        {
            public int StructId;
            public short InvertWholePage;
            public short DeskewOff;
            public byte Orientation;
            public short NoiseRemoveOff;
            public short LineRemoveOff;
            public short DeshadeOff;
            public short InvertOff;
            public short SectioningOn;
            public short MergeBreakOff;
            public short LineRejectOff;
            public short CharacterRejectOff;
            public short LexOff;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
            public short[] DisableCharacter;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOCRRESULTS
        {
            public TOCRdeclares.TOCRRESULTSHEADER Hdr;
            public TOCRdeclares.TOCRRESULTSITEM[] Item;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOCRRESULTSEX
        {
            public TOCRdeclares.TOCRRESULTSHEADER Hdr;
            public TOCRdeclares.TOCRRESULTSITEMEX[] Item;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOCRRESULTSHEADER
        {
            public int StructId;
            public int XPixelsPerInch;
            public int YPixelsPerInch;
            public int NumItems;
            public float MeanConfidence;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOCRRESULTSITEM
        {
            public short StructId;
            public short OCRCha;
            public float Confidence;
            public short XPos;
            public short YPos;
            public short XDim;
            public short YDim;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOCRRESULTSITEMEX
        {
            public short StructId;
            public short OCRCha;
            public float Confidence;
            public short XPos;
            public short YPos;
            public short XDim;
            public short YDim;
            public TOCRdeclares.TOCRRESULTSITEMEXALT[] Alt;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOCRRESULTSITEMEXALT
        {
            public short Valid;
            public short OCRCha;
            public float Factor;
        }
    }
}
