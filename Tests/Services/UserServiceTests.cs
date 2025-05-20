using Xunit;
using SQLite;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

public class UserServiceTests
{
    private SQLiteAsyncConnection CreateInMemoryDb()
    {
        var dbPath = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.db");
        var conn = new SQLiteAsyncConnection(dbPath);
        conn.CreateTableAsync<User>().Wait();
        return conn;
    }

    [Fact]
    public async Task AddAndGetUser_WorksCorrectly()
    {
        var db = CreateInMemoryDb();
        var repo = new UserRepository(db);
        var service = new UserService(repo);

        var user = new User { Name = "Test User" };
        await service.AddAsync(user);

        var users = await service.GetAllAsync();
        Assert.Contains(users, u => u.Name == "Test User");
    }

    [Fact]
    public async Task AddUser_ShouldPersistUser()
    {
        var db = CreateInMemoryDb();
        var repo = new UserRepository(db);
        var service = new UserService(repo);
        var user = new User { Name = "Alice" };

        await service.AddAsync(user);
        var users = await service.GetAllAsync();

        Assert.Contains(users, u => u.Name == "Alice");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var db = CreateInMemoryDb();
        var repo = new UserRepository(db);
        var service = new UserService(repo);

        await service.AddAsync(new User { Name = "User1" });
        await service.AddAsync(new User { Name = "User2" });

        var users = (await service.GetAllAsync()).ToList();

        Assert.Equal(2, users.Count);
        Assert.Contains(users, u => u.Name == "User1");
        Assert.Contains(users, u => u.Name == "User2");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectUser()
    {
        var db = CreateInMemoryDb();
        var repo = new UserRepository(db);
        var service = new UserService(repo);

        var user = new User { Name = "Bob" };
        await service.AddAsync(user);
        var users = (await service.GetAllAsync()).ToList();
        var insertedUser = users.First();

        var fetched = await service.GetByIdAsync(insertedUser.Id);

        Assert.NotNull(fetched);
        Assert.Equal("Bob", fetched.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyUser()
    {
        var db = CreateInMemoryDb();
        var repo = new UserRepository(db);
        var service = new UserService(repo);

        var user = new User { Name = "Charlie" };
        await service.AddAsync(user);
        var users = (await service.GetAllAsync()).ToList();
        var insertedUser = users.First();

        insertedUser.Name = "Charlie Updated";
        await service.UpdateAsync(insertedUser);

        var updated = await service.GetByIdAsync(insertedUser.Id);
        Assert.Equal("Charlie Updated", updated.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        var db = CreateInMemoryDb();
        var repo = new UserRepository(db);
        var service = new UserService(repo);

        var user = new User { Name = "Dave" };
        await service.AddAsync(user);
        var users = (await service.GetAllAsync()).ToList();
        var insertedUser = users.First();

        await service.DeleteAsync(insertedUser.Id);
        var afterDelete = await service.GetAllAsync();

        Assert.DoesNotContain(afterDelete, u => u.Id == insertedUser.Id);
    }
}