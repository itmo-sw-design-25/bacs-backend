namespace BaCS.Presentation.MAUI.ViewModels.States;

using Services;

public class SearchOfficeState : AbstractState
{
    public SearchOfficeState(Client client) : base(client)
    {
    }

    public SearchOfficeRequest SearchOfficeRequest { get; set; }

    public override async Task SendRequestAsync()
    {

    }
}
