using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLtoXAML
{
	public interface IDialogService
	{
		void ShowMessage(string message);
		string FilePath { get; set; }
		bool OpenFileDialog();
	}
}
