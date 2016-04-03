using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irony.Samples.Frieght
{
	public interface IJavascriptGenerator
	{
		void GenerateScript(StringBuilder builder);
	}
}
