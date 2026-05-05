using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Max.BotClient;
using Xunit;

namespace Max.BotClient.Tests.Unit;

public class ThrottleTests
{
    // ─── Опт-аут ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task WaitAsync_RpsZero_NoThrottling()
    {
        using var throttle = new Throttle();
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 100; i++)
            await throttle.WaitAsync(0, CancellationToken.None);
        sw.ElapsedMilliseconds.Should().BeLessThan(100);
    }

    [Fact]
    public async Task WaitAsync_NegativeRps_NoThrottling()
    {
        using var throttle = new Throttle();
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 100; i++)
            await throttle.WaitAsync(-5, CancellationToken.None);
        sw.ElapsedMilliseconds.Should().BeLessThan(100);
    }

    // ─── Базовый throttling ──────────────────────────────────────────────────

    [Fact]
    public async Task WaitAsync_FirstCall_DoesNotDelay()
    {
        using var throttle = new Throttle();
        var sw = Stopwatch.StartNew();
        await throttle.WaitAsync(2, CancellationToken.None);
        sw.ElapsedMilliseconds.Should().BeLessThan(50);
    }

    [Fact]
    public async Task WaitAsync_SequentialCalls_RespectMinimumInterval()
    {
        // 5 calls at RPS=10 (100ms interval): first is immediate, then 4 intervals × 100ms = ≥400ms
        using var throttle = new Throttle();
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 5; i++)
            await throttle.WaitAsync(10, CancellationToken.None);
        sw.ElapsedMilliseconds.Should().BeGreaterThanOrEqualTo(350);
    }

    [Theory]
    [InlineData(3,  334)]   // 1000/3  = 333.33 → ceiling = 334 (not 333 from integer division)
    [InlineData(7,  143)]   // 1000/7  = 142.86 → ceiling = 143 (not 142)
    [InlineData(30,  34)]   // 1000/30 =  33.33 → ceiling =  34 (not 33 — 33ms would allow 30.3 RPS)
    public async Task WaitAsync_FractionalRps_RoundsIntervalUp(int rps, int expectedMinIntervalMs)
    {
        using var throttle = new Throttle();
        var sw = Stopwatch.StartNew();
        await throttle.WaitAsync(rps, CancellationToken.None);
        await throttle.WaitAsync(rps, CancellationToken.None);
        sw.ElapsedMilliseconds.Should().BeGreaterThanOrEqualTo(expectedMinIntervalMs - 20);
    }

    [Fact]
    public async Task WaitAsync_LongIdleBetweenCalls_NoExtraDelay()
    {
        using var throttle = new Throttle();
        await throttle.WaitAsync(10, CancellationToken.None);
        await Task.Delay(200); // longer than minInterval=100ms
        var sw = Stopwatch.StartNew();
        await throttle.WaitAsync(10, CancellationToken.None);
        sw.ElapsedMilliseconds.Should().BeLessThan(50);
    }

    // ─── Конкурентность ──────────────────────────────────────────────────────

    [Fact(Timeout = 10_000)]
    public async Task WaitAsync_ConcurrentCalls_AreSerialized()
    {
        // 5 concurrent calls at RPS=10: semaphore serializes them, total ≥ 4 intervals × 100ms
        using var throttle = new Throttle();
        var sw = Stopwatch.StartNew();
        var tasks = Enumerable.Range(0, 5)
            .Select(_ => throttle.WaitAsync(10, CancellationToken.None))
            .ToList();
        await Task.WhenAll(tasks);
        sw.ElapsedMilliseconds.Should().BeGreaterThanOrEqualTo(350);
    }

    // ─── Отмена ──────────────────────────────────────────────────────────────

    [Fact(Timeout = 10_000)]
    public async Task WaitAsync_Cancelled_ThrowsOperationCanceled()
    {
        using var throttle = new Throttle();
        await throttle.WaitAsync(2, CancellationToken.None); // set lastRequestMs
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(50);
        await FluentActions.Awaiting(() => throttle.WaitAsync(2, cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}