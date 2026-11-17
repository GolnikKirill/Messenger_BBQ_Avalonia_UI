# Multi-connection Messenger BBQ

A cross-platform desktop messenger built with C# and Avalonia UI, supporting peer-to-peer (P2P) real-time communication.

## Features
* **Local P2P Connection**: Direct peer-to-peer messaging over a local or virtual local network (e.g., Radmin VPN, Hamachi).
* **Base UI**: Clean interface for configuring P2P connection settings and real-time chatting.

## Planned Features
* **Personal Server Connection**: User authorization and message routing through a dedicated server to store contacts and undelivered messages.
* **STUN-Server Connection**: NAT traversal using a STUN server to establish direct P2P connections over the internet.
* **Rich Media Support**: Support for voice messages, video sharing, and voice/video calls.
* **Message History**: Persistent chat history stored in a local database.
* **Enhanced UI/UX**: Further interface improvements for a smoother user experience.

## Screenshots
<img width="1348" height="762" alt="image" src="https://github.com/user-attachments/assets/1a6fd935-c412-4f42-a0e6-58f628298eea" />

## Technology Stack
* **Language**: C# (.NET)
* **UI Framework**: Avalonia UI
* **Architecture**: MVVM (Model-View-ViewModel)

## How to Run Locally

### Prerequisites
* [.NET SDK 8.0](https://microsoft.com) or higher
* Git
* Visual Studio Code (with the **C# Dev Kit** extension) or any other .NET IDE

### Installation & Launch
1. **Clone the repository:**
   ```bash
   git clone https://github.com/GolnikKirill/Messenger_BBQ_Avalonia_UI
   cd Messenger_BBQ_Avalonia_UI
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the application:**
   * **Using Visual Studio Code:**
     1. Open the cloned folder in VS Code.
     2. Press `Ctrl + Shift + D` to open the **Run and Debug** panel.
     3. Select your preferred launch configuration from the dropdown menu at the top and press `F5`.
   * **Using Terminal / CLI:**
     ```bash
     dotnet run --project BBQ.csproj
     ```
