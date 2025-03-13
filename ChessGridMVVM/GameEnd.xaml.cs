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
    /// Interaction logic for GameEnd.xaml
    /// </summary>
    public partial class GameEnd : Window
    {
        public GameEnd(string result, User winner, User loser)
        {
            InitializeComponent();
            if(result == "s")
            {
                resultBox.Text = string.Format("The game between {0} and {1} was a draw.", winner.Name, loser.Name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Entry entry = new Entry();
            entry.Show();
            Close();
        }
    }
}
