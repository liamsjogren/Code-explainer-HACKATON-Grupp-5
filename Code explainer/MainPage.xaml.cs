using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace Code_explainer;

/// <summary>
/// Dotnet packages required: "OpenAI", "Microsoft.Extensions.Configuration.Json"
/// </summary>
/// <summary>
/// Init + apikey init
/// </summary>

public partial class MainPage : ContentPage
{
    private readonly OpenAIClient _openAi;
    private readonly string _model = "gpt-5-mini";
    private string test = "Nada";

    public MainPage(IConfiguration config)
    {
        InitializeComponent();

        try
        {
            string apiKey = config["OpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("mp.x.cs ln 29: no apikey found");

            _openAi = new OpenAIClient(new ApiKeyCredential(apiKey));
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Init error {ex.Message}";
        }
    }

    private void OnMenuClicked(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Play button updates the status text + handles the prompt interface + has the error catch
    /// </summary>

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        string code = CodeEditor.Text;
        if (string.IsNullOrWhiteSpace(code))
        {
            StatusLabel.Text = "empty";
            return;
        }

        StatusLabel.Text = "explaining code...";

        try
        {
            string prompt = BuildPrompt(code);

            var messages = new List<ChatMessage>()
            {
                new SystemChatMessage("You are a programming teacher who focuses on explaining code"),
                new UserChatMessage(prompt)
            };

            var chat = _openAi.GetChatClient(_model);
            var response = await chat.CompleteChatAsync(messages.ToArray());

            string explanation = string.Join("\n",
                response.Value.Content
                    .Where(part => !string.IsNullOrEmpty(part.Text))
                    .Select(part => part.Text.Trim()));

            StatusLabel.Text = explanation;
        }
        catch (Exception ex)
        {
            StatusLabel.Text = "error " + ex.Message;
        }
    }

    /// <summary>
    /// BuildPrompt makes a list for the prompt which adds checked items and text block into one unified prompt
    /// </summary>
    private string BuildPrompt(string code)
    {
        var parts = new List<string>();
        if (CheckControl.IsChecked) parts.Add("control structures");
        if (CheckLoop.IsChecked) parts.Add("loops");
        if (CheckMethod.IsChecked) parts.Add("methods");

        // UI branch changed this to a language swtcher, please change checkboxes to fit 

        string focus = parts.Count > 0
            ? $"Focus on {string.Join(", ", parts)}."
            : "explain it in a beginner friendly way.";

        return $"""
        You are an expert programmer
        Explain this code clearly for a beginner
        {focus}

        ```
        {code}
        ```
        """;
    }
}
