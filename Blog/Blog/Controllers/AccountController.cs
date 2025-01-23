﻿using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    
    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpPost("v1/accounts")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model, [FromServices] BlogDataContext context, [FromServices] EmailService emailService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

        var user = new User
        {
            Name = model.Username,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        var password = PasswordGenerator.Generate(25, true, false);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(user.Name, user.Email, subject: "Your account has been created.", body: $"Your Password has been created: <strong>{password}</strong>.");
            
            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email
            }));
        }
        catch (DbUpdateException e)
        {
           return BadRequest(new ResultViewModel<string>(e.Message));
        }
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromServices] BlogDataContext context)
    {
        if(!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));
        
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == model.Email);
        
        if(user is null)
            return Unauthorized(new ResultViewModel<string>($"Email or password is invalid"));
        
        if(!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return Unauthorized(new ResultViewModel<string>($"Email or password is invalid"));

        try
        {
            var token = _tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, errors: null));
        }
        catch (Exception e)
        {
           return StatusCode(500, new ResultViewModel<string>(e.Message));
        }
    }
    
    /*
     [Authorize(Roles = "user")]
    [HttpGet("v1/user")]
    public IActionResult GetUser() => Ok(User.Identity.Name);
    
    [Authorize(Roles = "author")]
    [HttpGet("v1/author")]
    public IActionResult GetAuthor() => Ok(User.Identity.Name);
    
    [Authorize(Roles = "admin")]
    [HttpGet("v1/admin")]
    public IActionResult GetAdmin() => Ok(User.Identity.Name);
    */
}