using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
                if (user == null) return NoContent();


                return Ok(user);
            }
            catch (System.Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao recuperar usuário. Error: {e.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            try
            {
                if (await _accountService.UserExists(userDto.Username))
                    return BadRequest("Usuário já existe.");

                var user = await _accountService.CreateAccountAsync(userDto);
                if (user != null)
                    return Ok(user);



                return BadRequest("Erro ao cadastrar usuário.");
            }
            catch (System.Exception e)

            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao registrar usuário. Error: {e.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.Username);
                if (user == null) return Unauthorized("Usuário inválido.");

                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
                if (!result.Succeeded) return Unauthorized("Senha inválida.");

                return Ok(
                    new
                    {
                        userName = user.Username,
                        PrimeiroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result
                    }
                );
            }
            catch (System.Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao realizar login. Error: {e.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário não existe.");

                var userRetorno = await _accountService.UpdateAccount(userUpdateDto);
                if (user == null)
                    return BadRequest("Erro ao atualizar usuário.");

                return Ok(userRetorno);

            }
            catch (System.Exception e)

            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao atualizar usuário. Error: {e.Message}");
            }
        }



    }
}