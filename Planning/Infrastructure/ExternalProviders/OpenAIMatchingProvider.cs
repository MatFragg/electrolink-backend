using System.Text.Json;
using System.Text.RegularExpressions;
using Hampcoders.Electrolink.API.Shared.Infrastructure;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.ExternalProviders;

public partial class OpenAIMatchingProvider : IAIMatchingProvider
{
    private readonly ChatClient _chatClient;
    private readonly OpenAISettings _settings;
    private readonly ILogger<OpenAIMatchingProvider> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public OpenAIMatchingProvider(IOptions<OpenAISettings> settings, ILogger<OpenAIMatchingProvider> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        var credential = new ApiKeyCredential(_settings.ApiKey);
        var client = new OpenAIClient(credential);
        _chatClient = client.GetChatClient(_settings.ModelId);
    }

    public async Task<IReadOnlyList<ScoredCandidate>> ScoreTechnicianCandidatesAsync(
        MatchingContext context, CancellationToken cancellationToken = default)
    {
        var systemPrompt = await File.ReadAllTextAsync(_settings.SystemPromptPath, cancellationToken);
        var userPrompt = JsonSerializer.Serialize(context, JsonOptions);

        ChatMessage[] messages =
        [
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userPrompt)
        ];

        var options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = _settings.MaxTokens,
            ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
        };

        for (var attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(_settings.TimeoutSeconds));

                var response = await _chatClient.CompleteChatAsync(messages, options, cts.Token);
                var json = ExtractJson(response.Value.Content[0].Text);

                var candidates = JsonSerializer.Deserialize<List<ScoredCandidate>>(json, JsonOptions);
                return candidates?.AsReadOnly() ?? (IReadOnlyList<ScoredCandidate>)Array.Empty<ScoredCandidate>();
            }
            catch (OperationCanceledException) when (attempt < 3)
            {
                _logger.LogWarning("OpenAI matching timed out on attempt {Attempt}, retrying...", attempt);
            }
            catch (Exception ex) when (attempt < 3)
            {
                _logger.LogWarning(ex, "OpenAI matching failed on attempt {Attempt}, retrying...", attempt);
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new AIProviderException("OpenAI", "Failed to score technician candidates after 3 attempts", ex);
            }
        }

        return [];
    }

    private static string ExtractJson(string text)
    {
        var match = JsonBlockRegex().Match(text);
        return match.Success ? match.Groups[1].Value : text;
    }

    [GeneratedRegex(@"`(?:json)?\s*([\s\S]*?)\s*`", RegexOptions.Multiline)]
    private static partial Regex JsonBlockRegex();
}
