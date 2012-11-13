using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
	public class Logger
	{
		private static StreamWriter file;

		private Logger()
		{
		}

		static Logger()
		{
			StreamWriter file = new StreamWriter("log.txt");
		}

		public static void Log(string text)
		{
			file.Write(text + "\n");
		}
	}
}
