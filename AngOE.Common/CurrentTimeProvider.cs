using System;

namespace AngOE.Common
{
    public interface ICurrentTimeProvider
    {
        DateTime CurrentTime();
    }

    public class CurrentTimeProvider: ICurrentTimeProvider
    {
        public DateTime CurrentTime()
        {
            return DateTime.Now;
        }
    }
}