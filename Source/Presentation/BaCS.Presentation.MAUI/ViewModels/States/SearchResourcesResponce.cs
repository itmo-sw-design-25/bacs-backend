namespace BaCS.Presentation.MAUI.ViewModels.States;

using Services;

public class SearchResourcesResponce
{
    public List<ResourceVm> Resources { get; }= new();
    public int TotalCount => Resources.Count;

    public void Clear()
    {
        Resources.Clear();
    }
}
