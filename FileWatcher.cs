using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.IO;
using System.Windows;

namespace HTMLtoXAML
{
	class FileWatcher
	{
		public Task WhenFileUpdate(string path)
		{
			if (File.Exists(path))
				return Task.FromResult(true);

			var tcs = new TaskCompletionSource<bool>();
			FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(path));

			FileSystemEventHandler changedHandler = null;
			changedHandler = (s, e) =>
			{
				if (e.Name == Path.GetFileName(path))
				{
					tcs.TrySetResult(true);
					watcher.Changed -= changedHandler;
					watcher.Dispose();
				}
			};


			watcher.Changed += changedHandler;
			MessageBox.Show("Maybe yes");
			watcher.EnableRaisingEvents = true;

			return tcs.Task;
		}
		
	}
}
