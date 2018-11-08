using System;
using PolicyService.Domain;

namespace PolicyService.Tests.Domain
{
    public class TimeMachine : IDisposable
    {
        private readonly Func<DateTimeOffset> oldTimeProvider;
        
        public TimeMachine(DateTimeOffset newNow)
        {
            oldTimeProvider = SysTime.CurrentTimeProvider;
            SysTime.CurrentTimeProvider = () => newNow;
        }
        
        public void Dispose()
        {
            SysTime.CurrentTimeProvider = oldTimeProvider;
        }
    }
}