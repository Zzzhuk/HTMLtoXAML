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
using System.Windows.Controls;

namespace HTMLtoXAML
{
	class Model
	{
		
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
	public class WebBrowserUtility
	{
		public static readonly DependencyProperty BindableSourceProperty =
	   DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

		public static string GetBindableSource(DependencyObject obj)
		{
			return (string)obj.GetValue(BindableSourceProperty);
		}

		public static void SetBindableSource(DependencyObject obj, string value)
		{
			obj.SetValue(BindableSourceProperty, value);
		}

		public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (o is WebBrowser browser)
			{
				string uri = e.NewValue as string;

				browser.Navigate(new Uri(uri)); 
			}
		}
	}
}
