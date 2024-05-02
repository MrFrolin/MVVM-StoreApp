using Common.DTOs;
using DataAccess.Services;
using Labb3Databaser.Enum;
using Labb3Databaser.Models;
using System.Collections.ObjectModel;

namespace Labb3Databaser.Manager;

public class UserManager
{

    public static ObservableCollection<UserModel> UserList { get; set; } = new();

    private static UserModel _currentUser;
    public static UserModel? CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            CurrentUserChanged?.Invoke();
        }
    }

    public static bool LoggedIn { get; set; }

    public static event Action CurrentUserChanged;

    public static event Action UserListChanged;

    public static bool IsAdminLoggedIn => CurrentUser.Type is UserType.Admin;

    public static void ChangeCurrentUser(string emailAddress)
    {
        var user = UserList?.FirstOrDefault(loginUser => loginUser.EmailAddress == emailAddress);
        CurrentUser = user;
    }

    public static void LogOut()
    {
        LoggedIn = false;
        CurrentUserChanged.Invoke();
    }

    public static void AddUser(UserModel user)
    {
        UserList.Add(user);
        UserListChanged.Invoke();
    }

    public static void RemoveUser(UserModel user)
    {
        UserList.Remove(user);
        UserListChanged.Invoke();
    }

    public static void GetAllUsersFromDb(UserRepository userRepo)
    {
        var addAllUsers = userRepo.GetAllUsers();
        UserList.Clear();

        foreach (var user in addAllUsers)
        {
            var userCart = new List<ProductModel>();

            foreach (var product in user.Cart)
            {
                userCart.Add(new ProductModel()
                {
                    ProductName = product.ProductName,
                    Id = product.Id,
                    ProductPrice = product.ProductPrice,
                    ProductType = product.ProductType
                });
            }

            UserList.Add(new UserModel() { Id = user.Id, FirstName = user.Firstname, LastName = user.Lastname, EmailAddress = user.EmailAddress, Password = user.Password, Type = user.Type, Cart = userCart});
        }
        UserListChanged?.Invoke();
    }

    public static UserRecord GetUserRecordFromModel(UserModel user)
    {
        var userCart = new List<ProductRecord>();

        foreach (var product in user.Cart)
        {
            userCart.Add(new ProductRecord(product.Id, product.ProductName, product.ProductPrice,
                product.ProductType, product.ProductCount));
        }

        var newUserRecord = new UserRecord(user.Id, user.FirstName, user.LastName, user.EmailAddress, user.Password, user.Type, userCart);

        return newUserRecord;
    }

    public static void UpdateUser(UserModel user)
    {
        var existingUser = UserList.FirstOrDefault(u => u.Id == user.Id);

        if (existingUser != null)
        {
            existingUser.Id = user.Id;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Password = user.Password;
            existingUser.EmailAddress = user.EmailAddress;
            existingUser.Type = user.Type;
            existingUser.Cart = user.Cart;

            if (CurrentUser.Id == existingUser.Id)
            {
                CurrentUserChanged.Invoke();
            }

            UserListChanged.Invoke();
        }
    }
}