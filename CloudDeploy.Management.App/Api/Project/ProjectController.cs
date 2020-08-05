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

namespace CloudDeploy.Management.App.Api.Project
{
	[Route("api/project")]
	[ApiController]
	public sealed class ProjectController : ControllerBase
	{
		private readonly ManagementDbContext _managementDbContext;

		public ProjectController(ManagementDbContext managementDbContext)
		{
			_managementDbContext = managementDbContext;
		}

		[HttpPost]
		public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectRequest request)
		{
			ProjectEntity projectEntity = new ProjectEntity
			{
				Name = request.Name,
				PackageSource = request.PackageSource
			};

			_managementDbContext.Projects.Add(projectEntity);
			int recordsAffected = await _managementDbContext.SaveChangesAsync();
			if (recordsAffected == 1)
			{
				ProjectDto projectDto = Map.ProjectEntityToDto(projectEntity);
				return Ok(projectDto);
			}
			else
			{
				throw new InvalidOperationException("Failed to create project.");
			}
		}

		[HttpGet]
		public async Task<ActionResult<List<ProjectDto>>> GetAllProjects()
		{
			List<ProjectDto> projectDtos = await _managementDbContext.Projects
				.Include(project => project.Deployments)
				.Select(project => Map.ProjectEntityToDto(project))
				.ToListAsync();

			return Ok(projectDtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProjectDto>> GetProject([FromRoute] int id)
		{
			ProjectEntity projectEntity = await _managementDbContext.Projects
				.FirstOrDefaultAsync(project => project.ID == id);

			if (projectEntity == null)
			{
				return NotFound();
			}

			ProjectDto projectDto = Map.ProjectEntityToDto(projectEntity);
			return Ok(projectDto);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateProject([FromRoute] int id, [FromBody] UpdateProjectRequest request)
		{
			ProjectEntity projectEntity = new ProjectEntity
			{
				ID = id,
				Name = request.Name,
				PackageSource = request.PackageSource
			};
			_managementDbContext.Projects.Update(projectEntity);

			int recordsAffected = await _managementDbContext.SaveChangesAsync();
			if (recordsAffected == 1)
			{
				return Ok();
			}
			else if (recordsAffected < 1)
			{
				return NotFound();
			}
			else
			{
				throw new InvalidOperationException($"Updated {recordsAffected} projects instead of just 1.");
			}
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteProject([FromRoute] int id)
		{
			ProjectEntity projectEntity = new ProjectEntity
			{
				ID = id
			};
			_managementDbContext.Projects.Remove(projectEntity);

			int recordsAffected = await _managementDbContext.SaveChangesAsync();
			if (recordsAffected == 1)
			{
				return Ok();
			}
			else if (recordsAffected < 1)
			{
				return NotFound();
			}
			else
			{
				throw new InvalidOperationException($"Deleted {recordsAffected} projects instead of just 1.");
			}
		}
	}
}
