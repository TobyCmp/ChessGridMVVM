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
        private User guest = new User(-1, "Guest");
        private User user1 = null;
        private User user2 = null;
        private bool altColour = false;

        public Entry()
        {
            InitializeComponent();
            viewModel = new EntryViewModel();
            DataContext = viewModel;
            user1 = guest;
            user2 = guest;
            
        }

        private void Game_Click(object sender, EventArgs e)
        {
            MainWindow m = new MainWindow(user1, user2,showValidMoves, filename.Text, altColour);
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
                Username.Text = "";
                Password.Text = "";
                if(user1.Id != 1 && user2.Id != -1)
                {
                    string headtohead = "";
                    headtohead = viewModel.headToHead(user1.Id, user2.Id);
                    User2.Text = User2.Text + "\nHeadToHead: " + headtohead;
                }
            }
            else
            {
                ErrorMessage("Login Error - Incorrect details");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            username = Username.Text;
            password = Password.Text;
            if(username == "")
            {
                ErrorMessage("INVALID USERNAME - Username is empty.");
            }
            else if(password == "")
            {
                ErrorMessage("INVALID PASSWORD - Password is empty.");
            }
            else
            {
                if(viewModel.login(username,password) == null)
                {
                    viewModel.addPlayer(username, password);
                    ErrorMessage("Registered");


                }
                else
                {
                    ErrorMessage("User Exists Already!");
                }
            }


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


        private void ToggleScheme_Checked(object sender, RoutedEventArgs e)
        {
            altColour = !altColour;
        }

        private void ErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
