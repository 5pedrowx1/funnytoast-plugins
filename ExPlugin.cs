using System.Numerics;
using FunnyToast.Sdk.Events;
using FunnyToast.Sdk.Plugins;
using FunnyToast.Sdk.Toasts;
using FunnyToast.Sdk.Widgets;

namespace FunnyToast.Plugin.HelloWorld;

/// <summary>
/// Plugin de exemplo que demonstra TODAS as capacidades da API:
///   1. Mostrar toasts
///   2. Subscrever eventos do sistema
///   3. Registar uma hotkey global
///   4. Registar um widget que se desenha na overlay
///   5. Guardar/ler dados na pasta privada do plugin
///
/// Usa-o como ponto de partida: copia, muda o Info, e fica só com o que precisas.
/// </summary>
public sealed class HelloWorldPlugin : IPlugin
{
    // Metadados (obrigatório)
    // O Id deve ser único e estável (estilo domínio invertido).
    // Não mexas no ApiVersion — vem do SDK e o host valida-o.
    public PluginInfo Info { get; } = new()
    {
        Id = "com.exemplo.helloworld",
        Name = "Hello World",
        Version = new Version(1, 0, 0),
        Author = "O teu nome",
        Description = "Plugin de exemplo que demonstra toda a API.",
    };

    private IPluginContext? _ctx;
    private IDisposable? _hotkey;
    private IDisposable? _usbSubscription;
    private int _clickCount;

    // Chamado uma vez quando o plugin carrega 
    public void Initialize(IPluginContext ctx)
    {
        _ctx = ctx;

        // Mostra um toast de boas-vindas. Repara nos helpers de
        //     conveniência: Info/Success/Warning/Error.
        ctx.Toasts.Success("Hello World!", "O plugin de exemplo está ativo.");

        // Subscreve um evento do sistema. O handler corre sempre que
        //     uma pen/dispositivo USB é inserido. Guarda o IDisposable
        //     para cancelar a subscrição no Shutdown.
        _usbSubscription = ctx.Events.Subscribe<UsbInsertedEvent>(OnUsbInserted);

        // Regista uma hotkey global: Ctrl+Shift+H.
        //     0x48 é o virtual-key code de 'H'. Guarda o IDisposable.
        _hotkey = ctx.Hotkeys.Register(
            ctrl: true, alt: false, shift: true, virtualKey: 0x48,
            callback: OnHotkey);

        // Regista um widget que aparece na overlay.
        ctx.Widgets.Register(new HelloWidget());

        // Lê/escreve dados na pasta privada (persiste entre sessões).
        var counterFile = Path.Combine(ctx.DataDirectory, "launches.txt");
        int launches = 0;
        if (File.Exists(counterFile) && int.TryParse(File.ReadAllText(counterFile), out var n))
            launches = n;
        launches++;
        File.WriteAllText(counterFile, launches.ToString());

        ctx.Logger.Info($"[HelloWorld] Pronto. Arranque nº {launches}. Ctrl+Shift+H para testar.");
    }

    private void OnUsbInserted(UsbInsertedEvent e)
    {
        _ctx?.Toasts.Info("USB inserido", e.FriendlyName);
    }

    private void OnHotkey()
    {
        _clickCount++;
        _ctx?.Toasts.Show(new Toast
        {
            Title = "Hotkey premida!",
            Message = $"Já carregaste {_clickCount} vez(es).",
            Type = ToastType.Info,
            Duration = TimeSpan.FromSeconds(3),
        });
    }

    // Chamado quando o plugin descarrega. Liberta TUDO.
    // Isto é crítico para o hot-reload funcionar sem leaks.
    public void Shutdown()
    {
        _hotkey?.Dispose();
        _usbSubscription?.Dispose();
        _ctx?.Widgets.Unregister("hello");
        _ctx?.Logger.Info("[HelloWorld] Shutdown.");
        _ctx = null;
    }
}

/// <summary>
/// Widget de exemplo. Mostra um relógio simples num cartão arredondado.
/// Desenha-se via IWidgetCanvas (API segura) — nunca toca no renderer real.
/// </summary>
public sealed class HelloWidget : IWidget
{
    public string Name => "hello";

    // Tamanho reservado para o widget na overlay.
    public Vector2 PreferredSize => new(180, 56);

    private string _time = "";

    // Chamado uma vez por frame. Mantém-no barato — corre a cada frame.
    public void Update(float dt)
    {
        _time = DateTime.Now.ToString("HH:mm:ss");
    }

    // Desenha o widget. 'origin' é o canto superior-esquerdo atribuído.
    public void Render(IWidgetCanvas canvas, ITheme theme, Vector2 origin)
    {
        float x = origin.X, y = origin.Y;
        float w = PreferredSize.X, h = PreferredSize.Y;

        // Cartão de fundo com o tema atual.
        canvas.FillRoundedRect(x, y, w, h, theme.CornerRadius, theme.Background);

        // Um ponto de accent à esquerda.
        canvas.FillCircle(x + 18, y + h / 2, 6, theme.AccentColor);

        // Etiqueta e relógio.
        canvas.DrawText("Hello World", x + 36, y + 10, theme.TitleColor,
                        font: theme.Font, size: 13f, bold: true);
        canvas.DrawText(_time, x + 36, y + 30, theme.MessageColor,
                        font: theme.Font, size: 12f);
    }
}
