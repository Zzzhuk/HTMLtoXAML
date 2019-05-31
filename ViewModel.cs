using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;
using System.IO;
using System.ServiceProcess;

namespace HTMLtoXAML
{
	class ViewModel : INotifyPropertyChanged
	{
		public Model model = new Model();
		public FileWatcher fw = new FileWatcher();
		public string filePath;
		
		private DefaultDialogService dialogService = new DefaultDialogService();

		private string bgcolor = "#FF00AB5E";
		public string bgColor
		{
			get { return bgcolor; }
			set
			{
				bgcolor = value;
				OnPropertyChanged("bgColor");
			}
		}
		private string btntext = "Start";
		public string btnText
		{
			get { return btntext; }
			set
			{
				btntext = value;
				OnPropertyChanged("btnText");
			}
		}

		private RelayCommand openFile;
		public RelayCommand OpenFile
		{
			get
			{
				return openFile ??
				  (openFile = new RelayCommand(obj =>
				  {
					  try
					  {
						  if (dialogService.OpenFileDialog() == true)
						  {
							  filePath = dialogService.FilePath;
						  }
					  }
					  catch (Exception ex)
					  {
						  dialogService.ShowMessage(ex.Message);
					  }
				  }));
			}
		}
		public FileSystemWatcher watcher;
		private void funcChangedHandler(object sender, FileSystemEventArgs e)
		{
			if (e.Name == Path.GetFileName(filePath))
			{
				MessageBox.Show("File updated");
			}
		}


		private RelayCommand startWatcher;
		public RelayCommand StartWatcher
		{
			get
			{
				return startWatcher ??
				  (startWatcher = new RelayCommand(obj =>
				  {
					  if (watcher != null)
						  return;
					  bgColor = "#FFAB0000";
					  btnText = "Stop";
					  try
					  {
						  watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath));

						  watcher.Changed += funcChangedHandler;
						  watcher.EnableRaisingEvents = true; 
						}
					  catch (Exception ex)
					  {
						  dialogService.ShowMessage(ex.Message);
					  }
				  }));
			}
		}
		private RelayCommand stopWatcher;
		public RelayCommand StopWatcher
		{
			get
			{
				return stopWatcher ??
				  (stopWatcher = new RelayCommand(obj =>
				  {
					  if(watcher == null)
						  return;
					  bgColor = "#FF00AB5E";
					  btnText = "Start";
					  try
					  {
						  watcher.Changed -= funcChangedHandler;
						  watcher.Dispose();
						  watcher = null;
					  }
					  catch (Exception ex)
					  {
						  dialogService.ShowMessage(ex.Message);
					  }
				  }));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
