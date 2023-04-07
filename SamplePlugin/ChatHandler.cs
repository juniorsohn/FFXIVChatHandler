using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Game.Gui;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Windowing;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using SamplePlugin.Windows;

namespace SamplePlugin
{
    public sealed class ChatHandler : IDalamudPlugin
    {
        public string Name => "Sample Plugin";
        private const string CommandName = "/voz";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SamplePlugin");

        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }
        // Interface que dá pra puxar coisas tipo path do plugin e umas parada aí
        private DalamudPluginInterface _pi; 
        // nosso manipulador de chat
        private ChatGui _chatGui; 
        // sei pra que serve isso não
        private Configuration config; 
        
        
        public ChatHandler([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface, [RequiredVersion("1.0")] ChatGui chat,
                           [RequiredVersion("1.0")] CommandManager commands)
        {
            _pi = pluginInterface;
            _chatGui = chat;
            this.config = (Configuration)_pi.GetPluginConfig() ?? new Configuration();
            this.config.Initialize(_pi);

            _chatGui.ChatMessage += Chat_onChatMessage;
            
        









            // you might normally want to embed resources and load them from the manifest stream
            /*
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var ImagePathPepe = Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "pepe.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);
            var pepeImage = PluginInterface.UiBuilder.LoadImage(ImagePathPepe);
            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this, goatImage, pepeImage);
            */
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }


        private void Chat_onChatMessage(Dalamud.Game.Text.XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (config.Enabled)
            {
                foreach (var payload in message.Payloads)
                {
                    if (payload is TextPayload textPayload)
                    {
                        textPayload.Text = sendTextToApi(textPayload.Text);
                    }
                }    
                
            } 
        }

        private string? sendTextToApi(string text)
        {
            
            return "ok!";
        }
        
        
        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
