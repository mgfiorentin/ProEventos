using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;

        public AccountService(UserManager<User> userManager,
         SignInManager<User> signInManager, IMapper mapper, IUserPersist userPersist)
        {
            _mapper = mapper;
            _userPersist = userPersist;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.UserName == userUpdateDto.Username.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (System.Exception e)
            {

                throw new Exception("Erro ao verificar senha." + e.Message);
            }
        }

        public async Task<UserDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userRetorno = _mapper.Map<UserDto>(user);
                    return userRetorno;
                }
                return null;

            }
            catch (System.Exception e)
            {

                throw new Exception("Erro ao criar usuário." + e.Message);
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string username)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(username);
                if (user == null) return null;

                var userRetorno = _mapper.Map<UserUpdateDto>(user);
                return userRetorno;

            }
            catch (System.Exception e)
            {

                throw new Exception("Erro ao carregar usuário" + e.Message);
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userUpdateDto.Username);
                if (user == null) return null;

                _mapper.Map(userUpdateDto, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);

                _userPersist.Update(user);

                if (await _userPersist.SaveChangesAsync())
                {
                    var userRetorno = await _userPersist.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }

                return null;
            }
            catch (System.Exception e)
            {

                throw new Exception("Erro ao atualiar usuário." + e.Message);
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _userManager.Users.AnyAsync(u => u.UserName == username.ToLower());
            }
            catch (System.Exception e)
            {

                throw new Exception("Erro ao verificar existência  de usuário." + e.Message);
            }
        }
    }
}