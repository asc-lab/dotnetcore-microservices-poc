using System;

namespace PolicyService.Domain
{
    public class SysTime
    {
        public static Func<DateTimeOffset> CurrentTimeProvider { get; set; } = () => DateTimeOffset.Now;
        public static DateTimeOffset CurrentTime => CurrentTimeProvider();
    }
}