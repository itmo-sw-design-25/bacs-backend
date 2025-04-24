namespace BaCS.Presentation.MAUI.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Utils;

public class LocationsListVm : NotifyPropertyChanged
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
