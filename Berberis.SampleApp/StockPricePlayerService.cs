﻿using Berberis.Messaging;
using Berberis.Recorder;

namespace Berberis.SampleApp;

public sealed class StockPricePlayerService : BackgroundService
{
    private readonly ILogger<StockPricePlayerService> _logger;
    private readonly ICrossBar _xBar;

    public StockPricePlayerService(ILogger<StockPricePlayerService> logger, ICrossBar xBar)
    {
        _logger = logger;
        _xBar = xBar;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var destination = "stock.prices.";

        var serialiser = new StockPriceSerialiser();

        using var fs = File.Open(@"c:\temp\trayport.stream", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        await Task.Delay(2000);

        var player = _xBar.Replay(destination, fs, serialiser, PlayMode.AsFastAsPossible, stoppingToken);

        await Task.Delay(50);

        await player.Pause(stoppingToken);

        await Task.Delay(1000);

        player.Resume();

        await Task.Delay(50);

        await player.Pause(stoppingToken);

        await Task.Delay(1000);

        player.Resume();


        await player.MessageLoop;
    }
}
