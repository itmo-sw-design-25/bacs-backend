namespace BaCS.Presentation.MAUI.ViewModels;

using System.Windows.Input;
using Domain.Core.Enums;
using Domain.Core.Extensions;
using Resource = BaCS.Presentation.MAUI.Models.Resource;
public class ResourceVM : NotifyPropertyChanged
{
    private readonly Resource resource;

    public ResourceVM(Resource resource)
    {
        this.resource = resource;
    }


    public string Name
    {
        get => resource.Name;
        set
        {
            if (resource.Name == value)
            {
                return;
            }
            resource.Name = value;
            OnPropertyChanged(Name);
        }
    }

    public string Description
    {
        get => resource.Description;
        set
        {
            if (resource.Description == value)
            {
                return;
            }
            resource.Description = value;
            OnPropertyChanged(Description);
        }
    }

    public int Floor
    {
        get => resource.Floor;
        set
        {
            if (resource.Floor == value)
            {
                return;
            }
            resource.Floor = value;
            OnPropertyChanged(FloorStr);
        }
    }

    public string FloorStr
    {
        get => String.Format("{0} этаж", Floor);
    }


    public string[] Equipment
    {
        get => resource.Equipment;
    }

    public ResourceType Type
    {
        get => resource.Type;
        set
        {
            if(resource.Type == value) return;
            resource.Type = value;
            OnPropertyChanged(TypeStr);
        }
    }

    public string TypeStr
    {
        get => resource.Type.GetDisplayName();
    }

    public string ImageUrl
    {
        get => resource.ImageUrl;
    }

    //TODO Сделать кнопку для бронирования
    public ICommand BookCommand { get; }
}
