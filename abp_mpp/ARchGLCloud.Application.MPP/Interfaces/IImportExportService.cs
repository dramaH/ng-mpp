using System;
using System.Xml;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Application.MPP.Filters;
using ARchGLCloud.Application.Core.Interfaces;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ARchGLCloud.Application.MPP.ViewModels;

namespace ARchGLCloud.Application.MPP.Interfaces
{
    public interface IImportExportService : IService<Guid, ImportExportQueryFilter, Task>
    {
        int Execute(string cmd, out string stdout);
        string GetUploadPath(bool bCreate = true);
        string GetTemplateFile();

        string XmlFromMpp(string mppFilePath, bool bOverwrite = false);

        bool Xml2Model(string xmlFilePath, Project models);

        Project TryImport(string xmlFilePath, Project project);
        List<TaskViewModel> TryImportWithoutSave(string xmlFilePath);
        bool SaveAll(Project project);

        TimeSpan Duration2TimeSpan(string dura);

        XmlDocument Export2Xml(Guid projectId);
        Byte[] Xml2Bytes(XmlDocument doc);
    }
}