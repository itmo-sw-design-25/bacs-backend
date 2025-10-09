namespace BaCS.Application.Abstractions.Integrations;

public interface IAddressSuggestionsService
{
    Task<IReadOnlyCollection<string>> SuggestAddresses(
        string query,
        int count,
        CancellationToken cancellationToken = default
    );
}
