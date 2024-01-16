using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"

            SELECT
                [UserId]
                ,[FirstName]
                ,[LastName]
                ,[Email]
                ,[Gender]
                ,[Active]
            FROM [DotNetCourseDatabase].[TutorialAppSchema].[Users]
        ";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
        // return _dapper.LoadDataSingle<DateTime>(@"SELECT GETDATE()");
    }

    [HttpGet("GetSingleUser/{UserId}")]
    public User GetSingleUser(int UserId)
    {
        string sql = @"
            SELECT
                [UserId]
                ,[FirstName]
                ,[LastName]
                ,[Email]
                ,[Gender]
                ,[Active]
            FROM [DotNetCourseDatabase].[TutorialAppSchema].[Users]
            WHERE UserId = " + UserId.ToString();
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
            UPDATE TutorialAppSchema.Users
                SET [FirstName] = '" + user.FirstName +
                "',[LastName] = '" + user.LastName +
                "',[Email] = '" + user.Email +
                "',[Gender] = '" + user.Gender +
                "',[Active] = '" + user.Active +
            "' WHERE UserId = " + user.UserId;

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("failed to update user");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.Users(
                [FirstName]
                ,[LastName]
                ,[Email]
                ,[Gender]
                ,[Active]
            ) VALUES (" +
                "'" + user.FirstName +
                "', '" + user.LastName +
                "', '" + user.Email +
                "', '" + user.Gender +
                "', '" + user.Active +
            "')";

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("failed to add user");
    }

    [HttpDelete("DeleteUser/(UserId)")]
    public IActionResult DeleteUser(int UserId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.Users WHERE UserId = " + UserId.ToString();

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("failed to delete user");
    }
};
