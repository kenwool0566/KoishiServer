using System;

namespace KoishiServer.Common.Util
{
    public static class TimeUtils
    {
        public static long GetTimestampMs()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
