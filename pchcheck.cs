using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pchcheck
{
	class Program
	{
		const string ERROR_NO_PATH = "Error: no PCH file specified.";
		const string ERROR_WRONG_EXTENSION = "Error: not a PCH file.";
		const string USAGE = "Usage: pchcheck <path/to/file.pch>";
		const string PCH_EXTENSION = ".pch";
		const string PCH_HEADER = "VCPCH0";

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine(ERROR_NO_PATH);
				Console.WriteLine(USAGE);
				Environment.Exit(1);
			}
	
			string pchPath = args[0];
			if (!pchPath.EndsWith(PCH_EXTENSION))
			{
				Console.WriteLine(ERROR_WRONG_EXTENSION);
				Console.WriteLine(USAGE);
				Environment.Exit(2);
			}

			// Check first bytes
			// C.f. http://www.dotnetperls.com/binaryreader
			bool doRemove = false;
			if (!File.Exists(pchPath))
			{
				Console.Write(pchPath + " does not exist.");
			}
			else
			{
				using (BinaryReader br = new BinaryReader(File.Open(pchPath, FileMode.Open)))
				{
					char[] wantedChars = PCH_HEADER.ToCharArray();
					char[] actualChars = br.ReadChars(PCH_HEADER.Length);
					if (wantedChars.SequenceEqual(actualChars))
					{
						Console.Write(pchPath + " is OK.");
					}
					else
					{
						Console.Write(pchPath + " is corrupted: removing.");
						doRemove = true;
					}
				}
			}

			// Remove if not matching with the wanted header
			if (doRemove)
			{
				File.Delete(pchPath);
			}
		}
	}
}
