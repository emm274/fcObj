/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 06/24/2015
 * Time: 10:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace fcDmw
{
	/// <summary>
	/// Description of sodbFeatureToIntf.
	/// </summary>
	public interface sodbFeatureToIntf
	{
		void Close();

		void Extent(double x1,double y1,double x2, double y2);

        void workDir(string s);

		void Features(sodb db, List<SOAPService.Feature> list);

		int GetCount();
		string GetPath();
	}
}
