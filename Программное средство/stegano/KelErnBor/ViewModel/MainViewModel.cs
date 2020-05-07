using GalaSoft.MvvmLight;
using System;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Microsoft.Win32;
using Stegano.View;

namespace Stegano.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand OpenShowWindowCommand { get; private set; }
        public MainViewModel()
        {
            OpenShowWindowCommand = new RelayCommand(OpenShowWindow);
        }

        public void Close()
        {
            this.Close();
        }
        private void OpenShowWindow()
        {
            ShowWindow showWindow = new ShowWindow();
            showWindow.Show();
        }
    }
}