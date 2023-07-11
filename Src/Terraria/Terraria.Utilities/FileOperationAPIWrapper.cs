using System;
using System.Runtime.InteropServices;

namespace GameManager.Utilities
{
	public static class FileOperationAPIWrapper
	{
		[Flags]
		private enum FileOperationFlags : ushort
		{
			FOF_SILENT = 0x4,
			FOF_NOCONFIRMATION = 0x10,
			FOF_ALLOWUNDO = 0x40,
			FOF_SIMPLEPROGRESS = 0x100,
			FOF_NOERRORUI = 0x400,
			FOF_WANTNUKEWARNING = 0x4000
		}

		private enum FileOperationType : uint
		{
			FO_MOVE = 1u,
			FO_COPY,
			FO_DELETE,
			FO_RENAME
		}

		//RnD
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		private struct SHFILEOPSTRUCT
		{
			public IntPtr hwnd;

			[MarshalAs(UnmanagedType.U4)]
			public FileOperationType wFunc;

			public string pFrom;

			public string pTo;

			public FileOperationFlags fFlags;

			[MarshalAs(UnmanagedType.Bool)]
			public bool fAnyOperationsAborted;

			public IntPtr hNameMappings;

			public string lpszProgressTitle;
		}

		//RnD
		//[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		private static /*extern*/ int SHFileOperation(SHFILEOPSTRUCT FileOp)
		{
			return default;
		}

		private static bool Send(string path, FileOperationFlags flags)
		{
			try
			{
				SHFILEOPSTRUCT sHFILEOPSTRUCT = default(SHFILEOPSTRUCT);
				sHFILEOPSTRUCT.wFunc = FileOperationType.FO_DELETE;
				sHFILEOPSTRUCT.pFrom = path + "\0\0";
				sHFILEOPSTRUCT.fFlags = FileOperationFlags.FOF_ALLOWUNDO | flags;
				SHFILEOPSTRUCT FileOp = sHFILEOPSTRUCT;
				SHFileOperation(FileOp);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static bool Send(string path)
		{
			return Send(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_WANTNUKEWARNING);
		}

		public static bool MoveToRecycleBin(string path)
		{
			return Send(path, FileOperationFlags.FOF_SILENT | FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI);
		}

		private static bool DeleteFile(string path, FileOperationFlags flags)
		{
			try
			{
				SHFILEOPSTRUCT sHFILEOPSTRUCT = default(SHFILEOPSTRUCT);
				sHFILEOPSTRUCT.wFunc = FileOperationType.FO_DELETE;
				sHFILEOPSTRUCT.pFrom = path + "\0\0";
				sHFILEOPSTRUCT.fFlags = flags;
				SHFILEOPSTRUCT FileOp = sHFILEOPSTRUCT;
				SHFileOperation(FileOp);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static bool DeleteCompletelySilent(string path)
		{
			return DeleteFile(path, FileOperationFlags.FOF_SILENT | FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI);
		}
	}
}
