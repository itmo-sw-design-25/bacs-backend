namespace BaCS.Presentation.MAUI.ViewModels.States;

using Services;

public abstract class AbstractState
{
    protected Client Client;

    public AbstractState(Client client)
    {
        this.Client = client;
    }

    public abstract Task SendRequestAsync();
}
