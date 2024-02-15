/* Date: 15/02/2024
 * Author: Leonardo Trevisan Silio
 */
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RiotApi;

public class RateLimit(int secLimit = 20, int minLimit = 50)
{
    Queue<DateTime> SecRequests = new Queue<DateTime>();
    Queue<DateTime> MinRequests = new Queue<DateTime>();
    int inLastSecond = 0;
    int inLastMinute = 0;

    public async Task ControlRequest()
    {
        if (NeedsWait)
            await WaitToRequest();
        RegisterRequest();
    }

    public void RegisterRequest()
    {
        var now = DateTime.Now;
        inLastMinute++;
        SecRequests.Enqueue(now);
        inLastSecond++;
        MinRequests.Enqueue(now);
    }

    public bool NeedsWait
    {
        get
        {
            updateQueue();
            if (10 * inLastSecond > 9 * secLimit)
                return true;
            
            if (10 * inLastMinute > 9 * minLimit)
                return true;
            
            return false;
        }
    }

    public async Task WaitToRequest()
    {
        do 
            await Task.Delay(1000 / secLimit);
        while (NeedsWait);
    }

    private void updateQueue()
    {
        var now = DateTime.Now;

        while (MinRequests.Count > 0)
        {
            var time = now - MinRequests.Peek();
            if (time.TotalMinutes < 1.0)
                break;
            MinRequests.Dequeue();
            inLastMinute--;
        }

        while (SecRequests.Count > 0)
        {
            var time = now - SecRequests.Peek();
            if (time.TotalSeconds < 1.0)
                break;
            SecRequests.Dequeue();
            inLastSecond--;
        }
    }
}