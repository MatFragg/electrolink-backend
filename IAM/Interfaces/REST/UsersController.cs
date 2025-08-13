using System.Net.Mime;
using System.Security.Claims;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Queries;
using Hampcoders.Electrolink.API.IAM.Domain.Services;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.IAM.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User endpoints")]
public class UsersController(IUserQueryService userQueryService, IUserCommandService userCommandService, ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a user by its id",
        Description = "Get a user by its id",
        OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was found", typeof(UserResource))]
    public async Task<IActionResult> GetUserById(int id)
    {
        var getUserByIdQuery = new GetUserByIdQuery(id);
        var user = await userQueryService.Handle(getUserByIdQuery);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all users",
        Description = "Get all users",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The users were found", typeof(IEnumerable<UserResource>))]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }
    
    [HttpPut("{id}/username")]
    [SwaggerOperation(
        Summary = "Update a user's username",
        Description = "Allows an authenticated user to update their username.",
        OperationId = "UpdateUsername")]
    [SwaggerResponse(StatusCodes.Status200OK, "Username updated successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid username or user not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User ID in token does not match route ID")]
    public async Task<IActionResult> UpdateUsername(int id, [FromBody] UpdateUsernameResource resource)
    {
        try
        {
            // **CORRECCIÓN**: Obtener el usuario desde HttpContext.Items
            var authenticatedUser = HttpContext.Items["User"] as User;
            if (authenticatedUser == null || authenticatedUser.Id != id)
            {
                logger.LogWarning($"[IAM Controller] Intento de actualizar username por usuario no autorizado. TokenUserId: {authenticatedUser?.Id}, RouteId: {id}.");
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = "No estás autorizado para actualizar el username de este usuario."
                });
            }

            var command = UpdateUsernameCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            await userCommandService.Handle(command);
            return Ok(new { message = "Username actualizado correctamente" });
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, $"[IAM Controller] Error al actualizar username para ID: {id}. Mensaje: {ex.Message}.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"[IAM Controller] Error inesperado al actualizar username para ID: {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }
    
    [HttpPut("{id}/password")]
    [SwaggerOperation(
        Summary = "Update a user's password",
        Description = "Allows an authenticated user to update their password. Requires current password verification.",
        OperationId = "UpdatePassword")]
    [SwaggerResponse(StatusCodes.Status200OK, "Password updated successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid current password, new password, or user not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User ID in token does not match route ID")]
    public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordResource resource)
    {
        try
        {
            // **CORRECCIÓN**: Obtener el usuario desde HttpContext.Items
            var authenticatedUser = HttpContext.Items["User"] as User;
            if (authenticatedUser == null || authenticatedUser.Id != id)
            {
                logger.LogWarning($"[IAM Controller] Intento de actualizar contraseña por usuario no autorizado. TokenUserId: {authenticatedUser?.Id}, RouteId: {id}.");
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = "You are not authorized to update this user's password."
                });
            }
            
            var command = UpdatePasswordCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            await userCommandService.Handle(command);
            return Ok(new { message = "Password updated successfully" });
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, $"[IAM Controller] Error al actualizar contraseña para ID: {id}. Mensaje: {ex.Message}.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"[IAM Controller] Error inesperado al actualizar contraseña para ID: {id}.");
            return BadRequest(new { message = ex.Message });
        }
    }
}