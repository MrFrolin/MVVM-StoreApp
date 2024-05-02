using Labb3Databaser.Enum;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;

namespace Labb3Databaser.Models;

public class UserModel:INotifyPropertyChanged
{
    public string Id { get; set; }

    private string _firstName;
	public string FirstName
	{
		get { return _firstName; }
        set
        {
            _firstName = value;
            OnPropertyChanged();
        }
	}

    private string _lastName;
    public string LastName
    {
        get { return _lastName; }
        set
        {
            _lastName = value;
            OnPropertyChanged();
        }
    }

    private string _emailAddress;
    public string EmailAddress
    {
        get { return _emailAddress; }
        set
        {
            _emailAddress = value;
            OnPropertyChanged();
        }
    }

    private string _password;

    public string Password
	{
		get { return _password; }
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    private List<ProductModel> _cart;

    public List<ProductModel> Cart
    {
        get { return _cart; }
        set
        {
            _cart = value;
            OnPropertyChanged();
        }
    }

    public UserType Type { get; set;}

    public bool Authenticate(string password)
    {
        return Password.Equals(password);
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