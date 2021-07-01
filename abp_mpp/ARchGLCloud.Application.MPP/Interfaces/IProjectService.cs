using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Application.Core.Interfaces;
using ARchGLCloud.Application.MPP.Filters;
using ARchGLCloud.Application.MPP.Dtos;

namespace ARchGLCloud.Application.MPP.Interfaces
{
    public interface IProjectService : IService<Guid, ProjectQueryFilter, Project>
    {
        IEnumerable<Project> GetProjects(Guid parentId);
        GetProjectDto GetProject(Guid projectId);
        Project UpdateProject(Guid projectId, JObject fields);
        Project DeleteProject(Guid projectId);

        IEnumerable<ExtendedAttribute> GetAttributes(Guid projectId);

        ExtendedAttribute CreateAttribute(Guid projectId, ExtendedAttribute fields);
        ExtendedAttribute DeleteAttribute(Guid projectId, Guid attrId);

        Assignment DeleteAssignment(Guid projectId, Guid assId);
        Assignment CreateAssignment(Guid projectId, JObject fields);

        Resource CreateResource(Guid projectId, JObject input);

        Resource DeleteResource(Guid projectId, Guid resId);
    }
}