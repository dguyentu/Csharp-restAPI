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

    // get all records from the table TutorialAppSchema.Users
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


    // get single record from the table TutorialAppSchema.Users
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
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
            WHERE UserId = " + userId.ToString();
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }

    // get a single record from the table TutorialAppSchema.UserSalary
    [HttpGet("UserSalary/{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        string sql = @"
            SELECT
                [UserId]
               ,[Salary]   
            FROM [DotNetCourseDatabase].[TutorialAppSchema].[UserSalary]
            WHERE UserId = " + userId.ToString();
        UserSalary userSalary = _dapper.LoadDataSingle<UserSalary>(sql);
        return userSalary;
    }


    // edit a single record from the table TutorialAppSchema.Users
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

    // edit a single record from the table TutorialAppSchema.UserSalary
    [HttpPut("EditUserSalary")]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
        string sql = @"
            UPDATE TutorialAppSchema.UserSalary
                SET [Salary] = '" + userSalary.Salary +
            "' WHERE UserId = " + userSalary.UserId;

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("failed to update user salary record");
    }

    // add a single record to the table TutorialAppSchema.Users

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


    // delete a single record from the table TutorialAppSchema.Users
    [HttpDelete("DeleteUser/(userId)")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.Users WHERE UserId = " + userId.ToString();

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("failed to delete user");
    }
};
