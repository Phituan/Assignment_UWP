using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ASM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string CurrentTag = "";
        public static long currentMemberId = 1538626293330;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (CurrentTag == radio.Tag.ToString())
            {
                return;
            }
            switch (radio.Tag.ToString())
            {
                //case "MyAccount":
                //    CurrentTag = "MyAccount";
                //    this.MyFrame.Navigate(typeof(Views.ListMusic));
                //    break;
                case "Register":
                    CurrentTag = "Register";
                    this.MyFrame.Navigate(typeof(Views.Register));
                    break;
                case "Login":
                    CurrentTag = "Login";
                    this.MyFrame.Navigate(typeof(Views.Login));
                    break;
                case "Music":
                    CurrentTag = "Music";
                    this.MyFrame.Navigate(typeof(Views.ListMusic));
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.SplitVia.IsPaneOpen = !this.SplitVia.IsPaneOpen;
        }
    }
}
