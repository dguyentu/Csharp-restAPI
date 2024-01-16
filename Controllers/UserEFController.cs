using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IMapper _mapper;
    public UserEFController(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
        }));
    }

    // get all records from the table TutorialAppSchema.Users
    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsersEF()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }

    // get a single record from the table TutorialAppSchema.Users
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUserEF(int userId)
    {
        User? user = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();
        if (user != null)
        {
            return user;
        }
        throw new Exception("failed to get user. they do not exist as a record in db");
    }

    // get a single record from the table TutorialAppSchema.UserSalary
    // [HttpGet("GetUserSalary/{userId}")]
    // public IEnumerable<UserSalary> GetUserSalaryEF(int userId)
    // {
    //     return _entityFramework.UserSalary
    //         .Where(u => u.UserId == userId)
    //         .ToList();
    // }

    [HttpGet("GetUserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserSalaryEF(int userId)
    {
        IEnumerable<UserSalary> UserSalary = _entityFramework.UserSalary
        .Where(u => u.UserId == userId)
            .ToList<UserSalary>();
        return UserSalary;

    }

    // edit a single record from the table TutorialAppSchema.Users
    [HttpPut("EditUser")]
    public IActionResult EditUserEF(User user)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == user.UserId)
            .FirstOrDefault<User>();
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("failed to update user. they probably do not exist in this db. check the id you entered to search.");
        }
        throw new Exception("failed to get user. they do not exist as a record in db");
    }

    // add a single record from the table TutorialAppSchema.Users
    [HttpPost("AddUser")]
    public IActionResult AddUserEF(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);

        _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("failed to add user. check the payload values you are sending.");

    }

    // delete a single record from the table TutorialAppSchema.Users
    [HttpDelete("DeleteUser/(userId)")]
    public IActionResult DeleteUserEF(int userId)
    {
        User? userDb = _entityFramework.Users
       .Where(u => u.UserId == userId)
       .FirstOrDefault<User>();
        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("failed to delete user. they probably do not exist in this db. check the id you entered to search.");
        }
        throw new Exception("failed to get user. they do not exist as a record in db");
    }
};