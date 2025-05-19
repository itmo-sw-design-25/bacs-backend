namespace BaCS.Presentation.MAUI.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public class LocationsListVm : ObservableObject
{
    private int offset = 0;
    private int limit = 10;

    public LocationsListVm(Action<ObservableCollection<LocationListItemVm>> updateLocationList)
    {
        this.updateLocationsCommand = new RelayCommand(() => updateLocationList.Invoke(Locations));
    }

    public ObservableCollection<LocationListItemVm> Locations { get;} =
        new ObservableCollection<LocationListItemVm>();
    public ICommand updateLocationsCommand { get; }

}
