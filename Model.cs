using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using System.Security.Permissions;

namespace HTMLtoXAML
{
	class Model
	{
		bool activeWatcher = false;
		/*[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public void Run(string path)
		{
			activeWatcher = true;
			string fileName = Path.GetFileName(path);
			using (FileSystemWatcher watcher = new FileSystemWatcher())
			{
				if (activeWatcher)
				{
					watcher.EnableRaisingEvents = true;
					watcher.Path = path.Replace(fileName, "");


					watcher.NotifyFilter = NotifyFilters.LastWrite;

					watcher.Filter = fileName;

					watcher.Changed += OnChanged;
				}
			}
		}
		public void Stop()
		{
			activeWatcher = false;
		}
		private static void OnChanged(object source, FileSystemEventArgs e) =>
			MessageBox.Show("WORK!");*/
	}
	public class RelayCommand : ICommand
	{
		private Action<object> execute;
		private Func<object, bool> canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return this.canExecute == null || this.canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			this.execute(parameter);
		}
	}
	public class DefaultDialogService : IDialogService
	{
		public string FilePath { get; set; }

		public bool OpenFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				FilePath = openFileDialog.FileName;
				return true;
			}
			return false;
		}
		public void ShowMessage(string message)
		{
			MessageBox.Show(message);
		}
	}
	}
