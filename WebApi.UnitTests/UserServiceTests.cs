using Xunit;
using Moq;
using System.Collections.Generic;
using WebApi.Entities;
using System;
using BCryptNet = BCrypt.Net.BCrypt;
using WebApi.Repositories;
using System.Threading.Tasks;
using WebApi.Services;
using System.Linq;
using WebApi.Models.Users;
using WebApi.Helpers;

namespace WebApi.UnitTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

    public UserServiceTests(){
        _userRepositoryMock = new Mock<IUserRepository>();
    }

    [Fact]
    public Task GetAll_GetListOfUsers_ReturnListOfUsers()
    {
        // Arrange
        var users = GetSampleUsers();
        _userRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(users);
        var service = new UserService(_userRepositoryMock.Object);

        // Act
        var actionResult = service.GetAll();
        var actual = actionResult.Result as IEnumerable<User>;

        // Assert
        Assert.IsType<List<User>>(actual);
        Assert.Equal(GetSampleUsers().Count(), actual.Count());
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }
    [Fact]
    public Task GetAll_EmptyList_ReturnListOfNone()
    {
        // Arrange
        var users = new List<User>();
        _userRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(users);
        var service = new UserService(_userRepositoryMock.Object);

        // Act
        var actionResult = service.GetAll();
        var actual = actionResult.Result as IEnumerable<User>;

        // Assert
        Assert.IsType<List<User>>(actual);
        Assert.Equal(0, actual.Count());
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task GetById_GetUserById_ReturnUser()
    {
        // Arrange
        var users = GetSampleUsers();
        //var user = users.FirstOrDefault(x => x.Id == 1);
        var user = users.First(x => x.Id == 1);
        _userRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(user);
        var service = new UserService(_userRepositoryMock.Object);

        // Act
        var actionResult = service.GetById(1);
        var actual = actionResult.Result as User;

        // Assert
        Assert.IsType<User>(actual);
        Assert.Equal(1, actual.Id);
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task CreateUser_CreateValidUser_ReturnUser()
    {
        // Arrange
        var newUser = new UserCreateModel
        {
            Firstname = "test1",
            Lastname = "test1",
            JoinDate = new DateTime(2022, 04, 19),
            Type = "Admin",
            DoB = new DateTime(2001, 01, 01),
            Gender = "Male"
        };
        _userRepositoryMock.Setup(x => x.CreateUser(newUser, "Hochiminh", 1)).ReturnsAsync(new CreateUserResponse { Username = "test1t", Password = "test1t@01012001" });
        var service = new UserService(_userRepositoryMock.Object);

        // Act
        var actionResult = service.CreateUser(newUser, "Hochiminh", 1);
        var actual = actionResult.Result as CreateUserResponse;

        // Assert
        Assert.IsType<CreateUserResponse>(actual);
        Assert.NotNull(actual);
        Assert.Equal("test1t", actual.Username);
        Assert.Equal("test1t@01012001", actual.Password);

        return Task.CompletedTask;
    }

    
    public IEnumerable<User> GetSampleUsers()
    {
        return new List<User>
        {
            new User {
                Id = 1,
                StaffCode = "SD0001",
                Firstname = "John",
                Lastname = "Test1",
                Username = "johnt",
                DoB = new DateTime(1990,01,01),
                PasswordHash = BCryptNet.HashPassword("johnt@01011990"),
                JoinDate = new DateTime(2022,01,01),
                Type = Role.Admin,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hochiminh",
                IsFirstLogin = false
            },
            new User {
                Id = 2,
                StaffCode = "SD0002",
                Firstname = "Martha",
                Lastname = "Test2",
                Username = "marthat",
                DoB = new DateTime(1991,02,02),
                PasswordHash = BCryptNet.HashPassword("marthat@02021991"),
                JoinDate = new DateTime(2022,02,02),
                Type = Role.Staff,
                Gender = "Female",
                IsDisabled = false,
                Location = "Hochiminh",
                IsFirstLogin = false
            },
            new User {
                Id = 3,
                StaffCode = "SD0003",
                Firstname = "Doe",
                Lastname = "Test3",
                Username = "doet",
                DoB = new DateTime(1992,03,03),
                PasswordHash = BCryptNet.HashPassword("doet@03031992"),
                JoinDate = new DateTime(2022,03,03),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hochiminh",
                IsFirstLogin = false
            },
            new User {
                Id = 4,
                StaffCode = "SD0004",
                Firstname = "Marcus",
                Lastname = "Test4",
                Username = "marcust",
                DoB = new DateTime(1991,04,04),
                PasswordHash = BCryptNet.HashPassword("marcust@04041991"),
                JoinDate = new DateTime(2022,04,04),
                Type = Role.Admin,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hanoi",
                IsFirstLogin = true
            },
            new User {
                Id = 5,
                StaffCode = "SD0005",
                Firstname = "Tom",
                Lastname = "Test5",
                Username = "tomt",
                DoB = new DateTime(1991,05,05),
                PasswordHash = BCryptNet.HashPassword("tomt@05051991"),
                JoinDate = new DateTime(2022,05,05),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hanoi",
                IsFirstLogin = true
            },
            new User {
                Id = 6,
                StaffCode = "SD0006",
                Firstname = "Maria",
                Lastname = "Test6",
                Username = "mariat",
                DoB = new DateTime(1991,06,06),
                PasswordHash = BCryptNet.HashPassword("mariat@06061991"),
                JoinDate = new DateTime(2022,06,06),
                Type = Role.Staff,
                Gender = "Female",
                IsDisabled = false,
                Location = "Hanoi",
                IsFirstLogin = true
            },
        };
    }
}