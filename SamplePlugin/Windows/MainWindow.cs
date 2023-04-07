using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using Lumina.Models.Materials;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private TextureWrap GoatImage;
    private TextureWrap PepeImage;
    private ChatHandler chatHandler;

    public MainWindow(ChatHandler chatHandler, TextureWrap goatImage, TextureWrap pepeImage) : base(
        "Nome da Janela", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(800, 600),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.GoatImage = goatImage;
        this.PepeImage = pepeImage;
        this.chatHandler = chatHandler;
    }

    public void Dispose()
    {
        this.GoatImage.Dispose();
    }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {this.chatHandler.Configuration.Enabled}");
        if(ImGui.Button("Botao de quem é top"))
        {
            this.chatHandler.DrawConfigUI();
        }
        if (ImGui.Button("Show Settings"))
        {
            this.chatHandler.DrawConfigUI();
        }

        ImGui.Spacing();
        
        ImGui.Text("Have a goat:");
        ImGui.Indent(55);
        ImGui.Image(this.GoatImage.ImGuiHandle, new Vector2(this.GoatImage.Width, this.GoatImage.Height));
        ImGui.Unindent(55);
        
        ImGui.Text("And a pepe");
        ImGui.Indent(55);
        ImGui.Image(this.PepeImage.ImGuiHandle, new Vector2(PepeImage.Width, PepeImage.Height));
        ImGui.Unindent(55);
    }
}
