using System;
using ChatHandlerPlugin.Attributes;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using ChatHandlerPlugin.Windows;
using Dalamud.Game.Gui;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;

namespace ChatHandlerPlugin
{
    public sealed class ChatHandler : IDalamudPlugin
    {
        public string Name => "Chat Handler";
        private DalamudPluginInterface PluginInterface { get; init; }
        private PluginCommandManager<ChatHandler> commandManager;        
        public Configuration Configuration { get; init; }
        //public WindowSystem WindowSystem = new("ChatHandlerPlugin");

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

            commandManager = new PluginCommandManager<ChatHandler>(this, commands);

            // you might normally want to embed resources and load them from the manifest stream
            /*  
              var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
              var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);
              ConfigWindow = new ConfigWindow(this);
              MainWindow = new MainWindow(this, goatImage);
              
              WindowSystem.AddWindow(ConfigWindow);
              WindowSystem.AddWindow(MainWindow);
  */


            //this.PluginInterface.UiBuilder.Draw += DrawUI;
            // this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
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
                        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAA MANDA AJUDA");
                    }
                }    
                
            } 
        }

        private string? sendTextToApi(string text)
        {
            
            return "Ok to salvo";
        }
        
        [Command("/vozon")]
        [HelpMessage("VOZ LIGADA NA 220V FAMILIA.")]
        public void VozonCommand(string command, string args)
        {
            config.Enabled = true;
            config.Save();
            // You may want to assign these references to private variables for convenience.
            // Keep in mind that the local player does not exist until after logging in.
            _chatGui.Print($"O PAI TA ON");
            PluginLog.Verbose("VOZ enabled.");
        }

        [Command("/vozoff")]
        [HelpMessage("A mimir")]
        public void VozoffCommand(string command, string args)
        {
            config.Enabled = false;
            config.Save();
            // You may want to assign these references to private variables for convenience.
            // Keep in mind that the local player does not exist until after logging in.
            _chatGui.Print($"Perdemo família");
            PluginLog.Verbose("Voz disabled.");
        }
        
        public void Dispose()
        {
            //this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();

            _chatGui.ChatMessage -= Chat_onChatMessage;
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;
        }

        private void DrawUI()   
        {
           // this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
