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
        private bool activeUser1 = true;
        private User user1 = null;
        private User user2 = null;


        public Entry()
        {
            InitializeComponent();
            viewModel = new EntryViewModel();
            DataContext = viewModel;
            
        }

        private void Game_Click(object sender, EventArgs e)
        {
            MainWindow m = new MainWindow(user1, user2,showValidMoves);
            m.Show();
            Hide();
            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            username = Username.Text;
            password = Password.Text;
            var fetchedUser = viewModel.login(username, password);
            if(fetchedUser != null)
            {
                if(activeUser1)
                {
                    user1 = fetchedUser;
                }
                else
                {
                    user2 = fetchedUser;
                }
                (activeUser1 ? User1 : User2).Text = "User " + (activeUser1 ? "1" : "2") + ": " + username + " (" + fetchedUser.Id + ")";
                Username.Text = "Logged innn";
            }
            else
            {
                Username.Text = "Incorrect deets";
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            username = Username.Text;
            password = Password.Text;
            viewModel.addPlayer(username, password);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            showValidMoves = !showValidMoves;
        }

        private void ToggleUser_Click(object sender, RoutedEventArgs e)
        {
            activeUser1 = !activeUser1;
            ToggleText.Text = "Active User: " + (activeUser1 ? "1" : "2");
        }
    }
}
