using System.IO;
using SQLite;
using Xunit;

public class PalmBizDbTests
{
    private string GetDbPath() => "PalmBiz.db";

    [Fact]
    public void PalmBizDb_IsValidSqliteFile()
    {
        var dbPath = GetDbPath();
        Assert.True(File.Exists(dbPath), $"Database file not found at {dbPath}");

        using var conn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
        // Ejecuta una consulta simple para comprobar acceso
        var result = conn.ExecuteScalar<int>("SELECT count(*) FROM sqlite_master WHERE type='table'");
        Assert.True(result > 0, "No tables found in the database.");
    }

    [Theory]
    [InlineData("Product")]
    [InlineData("Sale")]
    [InlineData("SaleDetail")]
    [InlineData("User")]
    [InlineData("ExchangeRate")]
    public void PalmBizDb_ContainsExpectedTables(string tableName)
    {
        var dbPath = GetDbPath();
        using var conn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
        var exists = conn.ExecuteScalar<string>(
            "SELECT name FROM sqlite_master WHERE type='table' AND name=?",
            tableName);
        Assert.False(string.IsNullOrEmpty(exists), $"Table '{tableName}' not found in the database.");
    }
}