using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthExample.Models;
using AuthExample.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthExample.Controllers
{
  [Route("auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {


    private readonly DatabaseContext db;

    public AuthController(DatabaseContext context)
    {
      this.db = context;
    }

    [HttpPost("signup")]
    public async Task<ActionResult> SignUpUser(NewUserModel userData)
    {

      var existingUser = await this.db.Users.FirstOrDefaultAsync(f => f.Username == userData.Username);
      if (existingUser != null)
      {
        return BadRequest(new { Message = "user already exists" });
      }

      var user = new User
      {
        Email = userData.Email,
        FullName = userData.FullName,
        Username = userData.Username,
        HashedPassword = ""
      };
      // hash the password

      var hashed = new PasswordHasher<User>().HashPassword(user, userData.Password);
      user.HashedPassword = hashed;

      this.db.Users.Add(user);
      await this.db.SaveChangesAsync();
      return Ok(user);
    }


    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(LoginViewModel loginData)
    {
      var user = await this.db.Users.FirstOrDefaultAsync(f => f.Username == loginData.Username);
      if (user == null)
      {
        return BadRequest(new { Message = "User does not exist" });
      }

      var verificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.HashedPassword, loginData.Password);

      if (verificationResult == PasswordVerificationResult.Success)
      {
        return Ok(user);
      }
      else
      {
        return BadRequest(new { message = "Wrong password" });
      }
    }

  }
}