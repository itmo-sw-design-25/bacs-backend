namespace BaCS.Application.Integrations.DaData.Services;

using BaCS.Application.Abstractions.Integrations;
using Dadata;
using Dadata.Model;

public class AddressSuggestionsService(SuggestClientAsync suggestClient) : IAddressSuggestionsService
{
    public async Task<IReadOnlyCollection<string>> SuggestAddresses(
        string query,
        int count,
        CancellationToken cancellationToken = default
    )
    {
        var request = new SuggestAddressRequest(query, count)
        {
            from_bound = new AddressBound("street"),
            to_bound = new AddressBound("house"),
            division = AddressDivision.ADMINISTRATIVE,
            restrict_value = true
        };

        var response = await suggestClient.SuggestAddress(request, cancellationToken: cancellationToken);
        var suggestions = response.suggestions;

        return suggestions.Select(x => x.value).ToList();
    }
}
