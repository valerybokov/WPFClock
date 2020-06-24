using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MyShortcut
{
	[ComImport,
	Guid("00021401-0000-0000-C000-000000000046"),
	ClassInterface(ClassInterfaceType.None)]
	internal class ShellLink { }

	[ComImport,
	Guid("000214F9-0000-0000-C000-000000000046"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellLink
	{
		[PreserveSig]
		int GetPath(
			[Out, MarshalAs(UnmanagedType.LPWStr)]
             StringBuilder pszFile,
			int cch, ref IntPtr pfd, uint fFlags);

		[PreserveSig]
		int GetIDList(out IntPtr ppidl);

		[PreserveSig]
		int SetIDList(IntPtr pidl);

		[PreserveSig]
		int GetDescription(
			[Out, MarshalAs(UnmanagedType.LPWStr)]
             StringBuilder pszName, int cch);

		[PreserveSig]
		int SetDescription(
			[MarshalAs(UnmanagedType.LPWStr)]
             string pszName);

		[PreserveSig]
		int GetWorkingDirectory(
			[Out, MarshalAs(UnmanagedType.LPWStr)]
             StringBuilder pszDir, int cch);

		[PreserveSig]
		int SetWorkingDirectory(
			[MarshalAs(UnmanagedType.LPWStr)]
             string pszDir);

		[PreserveSig]
		int GetArguments(
			[Out, MarshalAs(UnmanagedType.LPWStr)]
             StringBuilder pszArgs, int cch);

		[PreserveSig]
		int SetArguments(
			[MarshalAs(UnmanagedType.LPWStr)]
             string pszArgs);

		[PreserveSig]
		int GetHotkey(out ushort pwHotkey);

		[PreserveSig]
		int SetHotkey(ushort wHotkey);

		[PreserveSig]
		int GetShowCmd(out int piShowCmd);

		[PreserveSig]
		int SetShowCmd(int iShowCmd);

		[PreserveSig]
		int GetIconLocation(
			[Out, MarshalAs(UnmanagedType.LPWStr)]
             StringBuilder pszIconPath, int cch, out int piIcon);

		[PreserveSig]
		int SetIconLocation(
			[MarshalAs(UnmanagedType.LPWStr)]
             string pszIconPath, int iIcon);

		[PreserveSig]
		int SetRelativePath(
			[MarshalAs(UnmanagedType.LPWStr)]
             string pszPathRel, uint dwReserved);

		[PreserveSig]
		int Resolve(IntPtr hwnd, uint fFlags);

		[PreserveSig]
		int SetPath(
			[MarshalAs(UnmanagedType.LPWStr)]
             string pszFile);
	}

	internal static class Shortcut
	{
		/// <summary>Создать ярлык</summary>
		/// <param name="pathToFile">Путь к файлу на который будет создан ярлык</param>
		/// <param name="pathToLink">Путь, по которому будет создан ярлык</param>
		/// <param name="description">Описание, которое будет отображаться при наведении на ярлык</param>
		/// <param name="iconFile">Путь для иконки к ярлыку (путь к приложению)</param>
		/// <param name="iconIndex">Индекс иконки в файле приложения от нуля</param>
		/// <returns></returns>
		public static bool Create(
			string pathToFile, string pathToLink,
			string description, string iconFile, int iconIndex)
		{
			IShellLink shlLink = (IShellLink)(new ShellLink());//CreateShellLink

			Marshal.ThrowExceptionForHR(shlLink.SetIconLocation(iconFile, iconIndex));
			Marshal.ThrowExceptionForHR(shlLink.SetDescription(description));
			Marshal.ThrowExceptionForHR(shlLink.SetPath(pathToFile));

/*			if (System.IO.File.Exists(pathToLink))
			try
			{
				System.IO.File.Delete(pathToLink);//может бросить исключение
			}
			catch(Exception)
			{				
				return false;
			}*/
			bool isExecutable = pathToFile.EndsWith(".exe", StringComparison.CurrentCultureIgnoreCase);

			((System.Runtime.InteropServices.ComTypes.IPersistFile)shlLink).Save(pathToLink, isExecutable);

			return true;
		}
	}
}