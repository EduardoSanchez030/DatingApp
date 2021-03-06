using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DatingApp.API.Data;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using DatingApp.API.Helpers;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currenUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var UserFromRepo = await _repo.GetUser(currenUserId);

            userParams.UserId = currenUserId;

            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = UserFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);
            
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            
            Response.AddPagination(users.CurrentPage, users.PageSize, 
                users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
             var userFromRepo = await _repo.GetUser(id);
             _mapper.Map(userForUpdateDto, userFromRepo);
             if (await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Updating user {id} failed on save");
        }

    }
}