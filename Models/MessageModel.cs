using Avalonia.Layout;
using Avalonia.Media;

namespace BBQ.Models;
public class MessageModel
{
    public string SenderName{get;set;}=string.Empty;
    public string Text{get;set;}=string.Empty;
    public IBrush BorderColor{get;set;}=Brushes.DarkBlue;
    public HorizontalAlignment Alignment{get;set;}=HorizontalAlignment.Left;
}