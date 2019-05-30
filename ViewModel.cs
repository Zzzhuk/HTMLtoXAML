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
		public async Task Foo(string path)
		{
			await fw.WhenFileUpdate(path);
			MessageBox.Show("Eeeboy");
		}
		private RelayCommand startWatcher;
		public RelayCommand StartWatcher
		{
			get
			{
				return startWatcher ??
				  (startWatcher = new RelayCommand(async obj =>
				  {
					  try
					  {
						  await Foo(filePath);
;					  }
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
					  try
					  {
						  
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
