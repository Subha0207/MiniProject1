using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
           
        }
        
        [HttpPost("Register")]
        [ProducesResponseType(typeof(RegisterReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterReturnDTO>> Register(RegisterDTO userRegisterDTO)
        {
            try
            {
                RegisterReturnDTO returnDTO = await _userService.Register(userRegisterDTO);
                return Ok(returnDTO);
            }
            catch (UserException e)
            {
                
                return BadRequest(new ErrorModel(400, e.Message));
            }
            catch (UnableToRegisterException e)
            {
                
                return BadRequest(new ErrorModel(400, e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorModel(400, e.Message));
            }
        }
        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginReturnDTO>> Login(LoginDTO userLoginDTO)
        {
            try
            {
                var result = await _userService.Login(userLoginDTO);
                return Ok(result);
            }
            catch (UserException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UserInfoException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (UnAuthorizedUserException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch (UnableToLoginException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
        }
        [HttpPut("admin/UpdateUserActivation")]
        [ProducesResponseType(typeof(ReturnUserActivationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnUserActivationDTO>> UserActivation([FromBody] UserActivationDTO userActivationDTO)
        {
            try
            {
                var result = await _userService.UserActivation(userActivationDTO.userId);
                var returnUserActivationDTO = new ReturnUserActivationDTO
                {
                    userId = result,
                    status = "active"
                };
                return Ok(returnUserActivationDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(501, ex.Message));
            }
        }

    }
}
