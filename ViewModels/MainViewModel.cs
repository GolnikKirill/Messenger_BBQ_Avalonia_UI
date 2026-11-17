using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using BBQ.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BBQ.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly UserBBQLocal _currentLocalUser;
    private readonly P2pEngine _p2pEngine = new();
    [ObservableProperty]
    private string _myNickname;
    [ObservableProperty]
    private string _myIp;
    [ObservableProperty]
    private int _myPort;
    [ObservableProperty]
    private string _remoteIp = "127.0.0.1";
    [ObservableProperty]
    private int _remotePort = 8080;
    [ObservableProperty]
    private string _messageText = string.Empty;
    public ObservableCollection<MessageModel> ChatMessages { get; } = new();
    public MainViewModel()
    {
        _currentLocalUser = new UserBBQLocal
        {
            UserName = Program.LaunchedNickname,
            Port = Program.LaunchedPort
        };
        _myIp = _currentLocalUser.IpAdress;
        _myPort = _currentLocalUser.Port;
        _myNickname = _currentLocalUser.UserName;

        _p2pEngine.OnMessageReceived += ReceiveMessage;
        try
        {
            _p2pEngine.StartServer(MyPort);
        }
        catch (Exception ex)
        {
            AddSystemMessage($": Ошибка запуска сервера: {ex.Message}");
        }
    }
    [RelayCommand]
    public async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(MessageText)) return;
        Console.WriteLine("========================================");
        Console.WriteLine($"[ОТПРАВКА] Попытка отправить сообщение...");
        Console.WriteLine($"[ОТПРАВКА] Мой никнейм: {MyNickname}");
        Console.WriteLine($"[ОТПРАВКА] Мой порт (слушатель): {MyPort}");
        Console.WriteLine($"[ОТПРАВКА] Куда шлем (IP): {RemoteIp}");
        Console.WriteLine($"[ОТПРАВКА] Куда шлем (Порт): {RemotePort}");
        Console.WriteLine($"[ОТПРАВКА] Текст сообщения: \"{MessageText}\"");
        Console.WriteLine("========================================");
        try
        {
            string messageToSend = $"{MyNickname}|{MessageText}";
            await _p2pEngine.SendMessageAsync(RemoteIp, RemotePort, messageToSend);
            Console.WriteLine($"[СТАТУС] ОК: Сообщение успешно улетело на {RemoteIp}:{RemotePort}!");
            ChatMessages.Add(new MessageModel
            {
                SenderName = $"{MyNickname} (You)",
                Text = MessageText,
                BorderColor = Brushes.DarkBlue,
                Alignment = HorizontalAlignment.Right
            });
            MessageText = string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[СТАТУС] ОШИБКА: Сетевой движок вернул сбой при отправке!");
            Console.WriteLine($"[СТАТУС] Детали ошибки: {ex.Message}");
            AddSystemMessage($": {ex.Message}");
        }
    }
    public void ReceiveMessage(string IncomingMessage)
    {
        string[] PartsOfIncomingMessage=IncomingMessage.Split('|',2);
        string SenderName=PartsOfIncomingMessage[0];
        string Text=PartsOfIncomingMessage[1];

        Console.WriteLine($"[ПРИЕМ] Внимание! Поймали входящую строку из сети: \"{Text}\"");
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            ChatMessages.Add(new MessageModel
            {
                SenderName = SenderName,
                Text = Text,
                BorderColor = Brushes.DarkGreen,
                Alignment = HorizontalAlignment.Left
            });
        });
    }
    private void AddSystemMessage(string text)
    {
        ChatMessages.Add(new MessageModel
        {
            SenderName = "[System]",
            Text = text,
            BorderColor = Brushes.Red,
            Alignment = HorizontalAlignment.Left
        });
    }
    private void OnMyNicknameChanged()
    {
        _currentLocalUser.UserName = MyNickname;
    }
}
