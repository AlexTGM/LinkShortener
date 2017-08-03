using System;

namespace LinkShortener.API.Exceptions
{
    public class MaximumAttemptsReachedException : Exception
    {
        private readonly int _maximumAttemptsCount;

        public MaximumAttemptsReachedException(int maximumAttemptsCount)
        {
            _maximumAttemptsCount = maximumAttemptsCount;
        }

        public override string Message => $"Maximum Attempts Count Reached: {_maximumAttemptsCount}";
    }
}