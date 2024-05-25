﻿using SandboxGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server
{
    public class RemoteServer : BaseServer
    {
        private TcpListener _listener;
        private CancellationTokenSource _cancellationTokenSource;

        public RemoteServer() 
        {
            _listener = new TcpListener(IPAddress.Any, 5050);
        }

        public override async Task StartAsync()
        {
            _listener.Start();

            _ = Task.Run(serverLoopAsync, _cancellationTokenSource.Token);
        }

        public override async Task StopAsync()
        {
            _listener.Stop();
        }

        private async Task serverLoopAsync()
        {
            while (true)
            {
                var client = await _listener.AcceptSocketAsync();

                _ = Task.Run(() => clientLoopAsync(client), _cancellationTokenSource.Token);
            }
        }

        private async Task clientLoopAsync(Socket client)
        {
            var buffer = new ArraySegment<byte>();

            while (true)
            {
                var receivedBytes = await client.ReceiveAsync(buffer);
                // TODO handle packet
            }
        }
    }
}