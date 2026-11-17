using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;

namespace BBQ;

public class P2pEngine
{
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    public event Action<string>? OnMessageReceived;
    public void StartServer(int myPort)
    {
        StopServer();
        _cts = new CancellationTokenSource();
        _listener = new TcpListener(IPAddress.IPv6Any, myPort);
        try
        {
            _listener.Server.DualMode = true;
            _listener.Start();
            Console.WriteLine($"[СЕРВЕР] УСПЕХ! Порт {myPort} успешно открыт и слушает сеть.");
            Task.Run(() => { ListenLoop(_cts.Token); });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[СЕРВЕР] КРИТИЧЕСКАЯ ОШИБКА: ОС не дала запустить сервер!");
            Console.WriteLine($"[СЕРВЕР] Детали: {ex.Message}");
        }
    }
    private async Task ListenLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                if (_listener == null) break;

                TcpClient client = await _listener.AcceptTcpClientAsync(token);
                _ = Task.Run(() => ReadClientData(client, token), token);
            }
            catch
            {
                break;
            }
        }
    }
    private async Task ReadClientData(TcpClient client, CancellationToken token)
    {
        using (client)
        {
            try
            {
                using NetworkStream stream = client.GetStream();
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                string? message = await reader.ReadLineAsync(token);
                if (!string.IsNullOrEmpty(message))
                {
                    OnMessageReceived?.Invoke(message);
                }
            }
            catch
            {

            }
        }
    }
    public async Task SendMessageAsync(string targetIP, int targetPort, string message)
    {
        using TcpClient client = new TcpClient();
        try
        {
            await client.ConnectAsync(targetIP, targetPort);

            using NetworkStream stream = client.GetStream();
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            await writer.WriteLineAsync(message);
        }
        catch (Exception ex)
        {
            throw new Exception($"Не удалось отправить сообщение на {targetIP}:{targetPort}: {ex.Message}");
        }
    }
    public void StopServer()
    {
        _cts?.Cancel();
        _listener?.Stop();
    }
}