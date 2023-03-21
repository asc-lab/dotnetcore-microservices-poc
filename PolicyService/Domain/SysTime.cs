using System;

namespace PolicyService.Domain;

public class SysTime
{
    public static Func<DateTime> CurrentTimeProvider { get; set; } = () => DateTime.Now;
    public static DateTime CurrentTime => CurrentTimeProvider();
}