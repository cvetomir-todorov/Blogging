using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDeploy.Api;
using CloudDeploy.Management.App.Util;
using CloudDeploy.Management.Data;
using CloudDeploy.Management.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudDeploy.Management.App.Api.Server
{
	[Route("api/server")]
	[ApiController]
	public sealed class ServerController : ControllerBase
	{
		private readonly ManagementDbContext _managementDbContext;

		public ServerController(ManagementDbContext managementDbContext)
		{
			_managementDbContext = managementDbContext;
		}

		[HttpPost]
		public async Task<ActionResult<ServerDto>> CreateServer([FromBody] CreateServerRequest request)
		{
			ServerEntity serverEntity = new ServerEntity
			{
				Cloud = Map.CloudProviderDtoToEntity(request.Cloud),
				Address = request.Address
			};

			_managementDbContext.Servers.Add(serverEntity);
			int recordsAffected = await _managementDbContext.SaveChangesAsync();
			if (recordsAffected == 1)
			{
				ServerDto serverDto = Map.ServerEntityToDto(serverEntity);
				return Ok(serverDto);
			}
			else
			{
				throw new InvalidOperationException("Failed to create server.");
			}
		}

		[HttpGet]
		public async Task<ActionResult<List<ServerDto>>> GetAllServers()
		{
			List<ServerDto> serverEntities = await _managementDbContext.Servers
				.Select(server => Map.ServerEntityToDto(server))
				.ToListAsync();

			return serverEntities;
		}

		[HttpPost("{id}")]
		public async Task<ActionResult> UpdateServer([FromRoute] int id, [FromBody] UpdateServerRequest request)
		{
			ServerEntity serverEntity = new ServerEntity
			{
				ID = id,
				Cloud = Map.CloudProviderDtoToEntity(request.Cloud),
				Address = request.Address
			};

			_managementDbContext.Servers.Update(serverEntity);
			int recordsAffected = await _managementDbContext.SaveChangesAsync();
			if (recordsAffected == 1)
			{
				return Ok();
			}
			else if (recordsAffected == 0)
			{
				return NotFound();
			}
			else
			{
				throw new InvalidOperationException($"Updated {recordsAffected} servers instead of just 1.");
			}
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteServer([FromRoute] int id)
		{
			ServerEntity serverEntity = new ServerEntity
			{
				ID = id
			};

			_managementDbContext.Servers.Remove(serverEntity);
			int recordsAffected = await _managementDbContext.SaveChangesAsync();
			if (recordsAffected == 1)
			{
				return Ok();
			}
			else if (recordsAffected == 0)
			{
				return NotFound();
			}
			else
			{
				throw new InvalidOperationException($"Deleted {recordsAffected} servers instead of just 1.");
			}
		}
	}
}
