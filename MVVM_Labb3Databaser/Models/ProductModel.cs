using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labb3Databaser.Enum;

namespace Labb3Databaser.Models;

public class ProductModel : INotifyPropertyChanged
{
    public string Id { get; set; }

	private string _productName;

	public string ProductName
	{
		get { return _productName; }
        set
        {
            _productName = value;
            OnPropertyChanged();
        }
	}

	private double _productPrice;

	public double ProductPrice
	{
		get { return _productPrice; }
        set
        {
            _productPrice = value;
            OnPropertyChanged();
        }
	}

    public ProductType ProductType { get; set; }

    public int ProductCount { get; set; } = 1;


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