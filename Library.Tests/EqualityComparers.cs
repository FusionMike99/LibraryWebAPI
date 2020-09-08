using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Business.Models;
using Data.Entities;

namespace Library.Tests
{
    internal class CardEqualityComparer : IEqualityComparer<Card>
    {
        public bool Equals([AllowNull] Card x, [AllowNull] Card y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Created == y.Created
                && x.ReaderId == y.ReaderId;
        }

        public int GetHashCode([DisallowNull] Card obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class CardModelEqualityComparer : IEqualityComparer<CardModel>
    {
        public bool Equals([AllowNull] CardModel x, [AllowNull] CardModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Created == y.Created
                && x.ReaderId == y.ReaderId;
        }

        public int GetHashCode([DisallowNull] CardModel obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class HistoryEqualityComparer : IEqualityComparer<History>
    {
        public bool Equals([AllowNull] History x, [AllowNull] History y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.BookId == y.BookId
                && x.CardId == y.CardId
                && x.TakeDate == y.TakeDate
                && x.ReturnDate == y.ReturnDate;
        }

        public int GetHashCode([DisallowNull] History obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class BookEqualityComparer : IEqualityComparer<Book>
    {
        public bool Equals([AllowNull] Book x, [AllowNull] Book y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Year == y.Year
                && x.Author == y.Author;
        }

        public int GetHashCode([DisallowNull] Book obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class BookModelEqualityComparer : IEqualityComparer<BookModel>
    {
        public bool Equals([AllowNull] BookModel x, [AllowNull] BookModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Year == y.Year
                && x.Author == y.Author;
        }

        public int GetHashCode([DisallowNull] BookModel obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ReaderModelEqualityComparer : IEqualityComparer<ReaderModel>
    {
        public bool Equals([AllowNull] ReaderModel x, [AllowNull] ReaderModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && string.Equals(x.Name, y.Name)
                && string.Equals(x.Email, y.Email)
                && string.Equals(x.Phone, y.Phone)
                && string.Equals(x.Address, y.Address);
        }

        public int GetHashCode([DisallowNull] ReaderModel obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ReaderEqualityComparer : IEqualityComparer<Reader>
    {
        public bool Equals([AllowNull] Reader x, [AllowNull] Reader y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && string.Equals(x.Name, y.Name)
                   && string.Equals(x.Email, y.Email);
        }

        public int GetHashCode([DisallowNull] Reader obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ReaderActivityModelEqualityComparer : IEqualityComparer<ReaderActivityModel>
    {
        public bool Equals([AllowNull] ReaderActivityModel x, [AllowNull] ReaderActivityModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.ReaderId == y.ReaderId &&
                   x.BooksCount == y.BooksCount &&
                   string.Equals(x.ReaderName, y.ReaderName);
        }

        public int GetHashCode([DisallowNull] ReaderActivityModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
