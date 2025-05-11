namespace BaCS.Presentation.MAUI.Views;

using Services;

public class RootVm
{
    public IAuthentificationService authentificationService;
    public ApiClient apiClient;

    public RootVm(IAuthentificationService authentificationService, ApiClient apiClient)
    {
        this.authentificationService = authentificationService;
        this.apiClient = apiClient;
    }


}
