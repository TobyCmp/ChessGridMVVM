using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;

namespace ChessGridMVVM
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User(int id, string name)
        {

            Id = id;
            Name = name;

        }
    }
}
