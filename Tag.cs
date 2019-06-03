using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLtoXAML
{
	class Tag
	{
		string begin, end, contentText;
		List<string> attrs;
		public Tag()
		{
			attrs = new List<string>();
		}
	}
}
