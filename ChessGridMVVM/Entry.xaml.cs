using ChessGridMVVM.Models;
using ChessGridMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessGridMVVM
{
    /// <summary>
    /// Interaction logic for Entry.xaml
    /// </summary>
    public partial class Entry : Window
    {
        private string username = "";
        private string password = "";
        private bool showValidMoves = false;
        private EntryViewModel viewModel;
        public Entry()
        {
            InitializeComponent();
            viewModel = new EntryViewModel();
            DataContext = viewModel;
            
        }

        private void Game_Click(object sender, EventArgs e)
        {
            MainWindow m = new MainWindow(showValidMoves);
            m.Show();
            Hide();
            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            username = Username.Text;
            password = Password.Text;
            viewModel.addPlayer(username, password);

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            username = Username.Text;
            password = Password.Text;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            showValidMoves = true;
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            showValidMoves = false;
        }

    }
}
