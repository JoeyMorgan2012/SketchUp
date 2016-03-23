using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SketchUp
{
	public static class Globals
	{
		public static List<String> LivingSquareFootTypesList()
		{
			List<string> livSqFtTypes = new List<string>();
			livSqFtTypes.Add("BASE");
			livSqFtTypes.Add("ADD");
			livSqFtTypes.Add("OH");
			livSqFtTypes.Add("LAG");
			livSqFtTypes.Add("NBAD");
			return livSqFtTypes;
		}
	}
}