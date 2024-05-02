using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Common.DTOs;

namespace Labb3Databaser.Models;

public class StoreModel: INotifyPropertyChanged
{
    public string StoreId { get; set; }

	private string _storeName;

	public string StoreName
	{
		get { return _storeName; }
        set
        {
            _storeName = value;
            OnPropertyChanged();
        }
	}

    private string _storeCity;

    public string StoreCity
    {
        get { return _storeCity; }
        set
        {
            _storeCity = value;
            OnPropertyChanged();
        }
    }

    private string _storeAddress;
    public string StoreAddress
    {
        get { return _storeAddress; }
        set
        {
            _storeAddress = value;
            OnPropertyChanged();
        }
    }

    private List<ProductModel> _storeStock;
    public List<ProductModel> StoreStock
    {
        get { return _storeStock; }
        set
        {
            _storeStock = value;
            OnPropertyChanged();
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}