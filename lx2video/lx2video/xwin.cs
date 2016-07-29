/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 17.06.2015
 * Time: 19:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;

namespace xwin
{

	public class globalMem {
	
		const int PAGE_READONLY = 0x0002;
		const int PAGE_READWRITE = 0x0004;
		const int PAGE_EXECUTE = 0x0010;
		const int PAGE_EXECUTE_READ = 0x0020;
		const int PAGE_EXECUTE_READWRITE = 0x0040;
		const int PAGE_GUARD = 0x0100;
		const int PAGE_NOACCESS = 0x0001;
		const int PAGE_NOCACHE = 0x0200;
		const int FILE_MAP_COPY = 0x0001;
		const int FILE_MAP_WRITE = 0x0002;
		const int FILE_MAP_READ = 0x0004;
		const int FILE_MAP_ALL_ACCESS = 0x001F;
		
		[StructLayout( LayoutKind.Sequential )]
		class SECURITY_ATTRIBUTES
		{
			//int nLength;
			//int lpSecurityDescriptor;
			//int bInheritHandle;
		}

		[DllImport("kernel32.dll")]
		static extern int CreateFileMappingA (uint hFile,
		                                      SECURITY_ATTRIBUTES lpFileMappigAttributes,
		                                      int flProtect,
		                                      int dwMaximumSizeHigh,
		                                      int dwMaximumSizeLow,
		                                      string lpName);

		[DllImport("kernel32.dll")]
		static extern IntPtr MapViewOfFile(int hFileMappingObject,
		                                   int dwDesiredAccess,
		                                   int dwFileOffsetHigh,
		                                   int dwFileOffsetLow,
		                                   int dwNumberOfBytesToMap);

		[DllImport("kernel32.dll")]
		static extern int UnmapViewOfFile(IntPtr lpBaseAddress);

		[DllImport("kernel32.dll")]
		static extern int CloseHandle (int hObject);
		
		int m_mem;
		IntPtr m_data;
		
		public globalMem()
		{
			m_mem=0;
			m_data=IntPtr.Zero;
		}
		
		~globalMem() { close(); }
		
		public bool Enabled() 
		{
			return m_data != IntPtr.Zero;
		}
		
		public bool open(string Name, int len) 
		{
			close();
			m_mem=CreateFileMappingA(0xFFFFFFFF, null, PAGE_READWRITE,
									0, len, Name);
			
			if (m_mem != 0)
            m_data=MapViewOfFile(m_mem, FILE_MAP_WRITE, 0, 0, 0);				
			
			return Enabled();
		}
		
		public void close() 
		{
			if (Enabled()) UnmapViewOfFile(m_data);
			if (m_mem != 0) CloseHandle(m_mem);

			m_mem=0;
			m_data=IntPtr.Zero;
		}
			
		public void GetData(int[] o_data, int len) 
		{
			Marshal.Copy( m_data, o_data, 0, len );
		}
		
		public void SetData(int[] i_data, int len)
		{
			Marshal.Copy( i_data, 0, m_data, len );
		}
		
	}
}
