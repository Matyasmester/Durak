using System;

namespace Cards
{
    public class WrongMoveException : Exception
    {
        public WrongMoveException(string message) : base(message)
        {

        }

    }
}
