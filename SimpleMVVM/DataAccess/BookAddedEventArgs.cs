using System;
using SimpleMVVM.Model;

namespace SimpleMVVM.DataAccess
{
    public class BookAddedEventArgs : EventArgs
    {
        public BookAddedEventArgs(Book newBook)
        {
            NewBook = newBook;
        }

        public Book NewBook { get; private set; }
    }
}
