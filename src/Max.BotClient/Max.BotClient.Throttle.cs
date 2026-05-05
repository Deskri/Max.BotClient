using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Max.BotClient
{
    internal sealed class Throttle : IDisposable
    {
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private long _lastRequestMs = -1;

        public async Task WaitAsync(int rps, CancellationToken token)
        {
            if (rps <= 0) return;
            var minInterval = (int)Math.Ceiling(1000.0 / rps);

            await _lock.WaitAsync(token);
            try
            {
                if (_lastRequestMs >= 0)
                {
                    var elapsed = _stopwatch.ElapsedMilliseconds - _lastRequestMs;
                    var waitMs = minInterval - elapsed;
                    if (waitMs > 0)
                        await Task.Delay((int)waitMs, token);
                }
                _lastRequestMs = _stopwatch.ElapsedMilliseconds;
            }
            finally
            {
                _lock.Release();
            }
        }

        public void Dispose() => _lock.Dispose();
    }
}