/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 18.02.2015
 * Time: 7:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Convert
{
	public static class convert
	{
		
		public static bool IsString(string s) {
			if ((s != null) && 
				(s.Length > 0)) return true;
			return false;
		}
	
		
	}
}
