using ARchGLCloud.Application.MPP.Filters;
using ARchGLCloud.Application.MPP.Interfaces;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Repositories;
using ARchGLCloud.Domain.MPP.Interfaces;
using ARchGLCloud.Domain.MPP.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ARchGLCloud.Application.MPP.ViewModels;

namespace ARchGLCloud.Application.MPP.Services
{
    public class ImportExportService : IImportExportService
    {
        private readonly IMediatorHandler _bus;
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _store;

        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;

        private readonly MppServiceHelper _helper;

        private readonly IProjectRepository _projectRepo;
        private readonly ICalendarRepository _calendarsRepo;
        private readonly ICalendarWeekDayRepository _calendarWeekdayRepo;
        private readonly ICalendarExceptionRepository _calendarExceptionRepo;
        private readonly IExtendedAttributeRepository _attrsRepo;
        private readonly ITaskRepository _taskRepo;
        private readonly ITaskPredecessorLinkRepository _predecessorLinkRepo;
        private readonly ITaskExtendedAttributeRepository _extendedAttributeRepo;
        private readonly ITaskBaselineRepository _baselineRepo;
        private readonly IResourceRepository _resourceRepo;
        private readonly IAssignmentRepository _assignRepo;

        public ImportExportService(IMediatorHandler bus,
                                      IMapper mapper,
                                      IEventStoreRepository store,
                                      IHostingEnvironment env,
                                      ILogger<ImportExportService> logger,
                                      MppServiceHelper helper,
                                      IProjectRepository projectRepo,
                                      IExtendedAttributeRepository attrsRepo,
                                      ICalendarRepository calendarsRepo,
                                      ICalendarWeekDayRepository calendarWeekdayRepo,
                                      ICalendarExceptionRepository calendarExceptionRepo,
                                      ITaskRepository taskRepo,
                                      ITaskPredecessorLinkRepository predecessorLinkRepo,
                                      ITaskExtendedAttributeRepository extendedAttributeRepo,
                                      ITaskBaselineRepository baselineRepo,
                                      IResourceRepository resourceRepo,
                                      IAssignmentRepository assignRepo)
        {
            _bus = bus;
            _mapper = mapper;
            _store = store;
            _env = env;
            _logger = logger;

            _helper = helper;

            _projectRepo = projectRepo;
            _calendarsRepo = calendarsRepo;
            _calendarWeekdayRepo = calendarWeekdayRepo;
            _calendarExceptionRepo = calendarExceptionRepo;
            _attrsRepo = attrsRepo;
            _taskRepo = taskRepo;
            _predecessorLinkRepo = predecessorLinkRepo;
            _extendedAttributeRepo = extendedAttributeRepo;
            _baselineRepo = baselineRepo;
            _resourceRepo = resourceRepo;
            _assignRepo = assignRepo;
        }

        #region NotImplemented
        public void Add(Task viewModel)
        {
            throw new NotImplementedException();
        }

        public void Update(Task viewModel)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> Pager(ImportExportQueryFilter filter, out int count)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Execute command on different OS platform
        /// </summary>
        public int Execute(string cmd, out string stdout)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return _DoExecute("cmd", $"/c {cmd}", out stdout);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return _DoExecute("/bin/sh", $"-c \"{cmd}\"", out stdout);
            }
            else
            {
                throw new Exception("Unexpected OS platform, abort execution.");
            }
        }
        /// <summary>
        ///   Execute command to run in another process
        /// </summary>
        private int _DoExecute(string cmd, string args, out string stdout)
        {
            var info = new ProcessStartInfo(cmd, args);
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            Console.WriteLine("Execute: {0} {1}", info.FileName, info.Arguments);
            _logger.LogDebug("Execute: {0}", info.Arguments);

            int re = 0;
            using (Process proc = new Process())
            {
                proc.StartInfo = info;

                try
                {
                    proc.Start();
                    proc.WaitForExit();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Execute process failed, command: {0}", info.Arguments);
                    throw e;
                }

                stdout = proc.StandardOutput.ReadToEnd();
                stdout += proc.StandardError.ReadToEnd();

                re = proc.ExitCode;

                proc.Close();
            }

            return re;
        }
        /// <summary>
        ///   Get configuration based on environment name, default is Development.
        /// </summary>
        public IConfigurationRoot GetConfig(IHostingEnvironment env, string envName = null)
        {
            var config = new ConfigurationBuilder().SetBasePath(env.ContentRootPath);
            if (envName == EnvironmentName.Production)
            {
                config.AddJsonFile("appsettings.json");
            }
            else if (envName == EnvironmentName.Development)
            {
                config.AddJsonFile("appsettings.Development.json");
            }
            else
            {
                string envFile = "appsettings." + envName + ".json";

                if (!File.Exists(envFile))
                {
                    throw new FileNotFoundException($"Could not found environment config file: {envFile}");
                }
                else
                {
                    config.AddJsonFile("appsettings." + envName + ".json");
                }
            }

            return config.Build();
        }

        private string GetStringFromConfig(string key)
        {
            IConfigurationRoot cfg = GetConfig(_env, _env.EnvironmentName);

            string path = cfg.GetValue<string>(key);
            if (String.IsNullOrEmpty(path))
            {
                throw new Exception($"The config entry {key} MUST exists on appsettings.json");
            }

            return path;
        }

        /// <summary>
        ///   Get mpp upload path from config file, directory will be created if not exist.
        /// </summary>
        public string GetUploadPath(bool bCreate = true)
        {
            string pathFull = Path.GetFullPath(GetStringFromConfig("MppUploadPath"), _env.ContentRootPath);

            if (!Directory.Exists(pathFull) && bCreate)
            {
                try
                {
                    Directory.CreateDirectory(pathFull);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return pathFull;
        }

        /// <summary>
        ///   Get template which is used for create new project
        /// </summary>
        public string GetTemplateFile()
        {
            string templatePath = Path.GetFullPath(GetStringFromConfig("MppTemplate"), _env.ContentRootPath);

            if (!templatePath.EndsWith(".xml") || !File.Exists(templatePath))
            {
                Exception e = new FileNotFoundException($"Could not find xml template under the path: {templatePath}");
                _logger.LogError(e, "Please check your configuration and place the template to the right place");
                throw e;
            }

            return templatePath;
        }

        /// <summary>
        ///   Get mpp2xml command path from config file, FileNotFoundException raised if not found.
        /// </summary>
        public string GetMpp2XmlCmd()
        {
            // Console.Write("################# Mpp2XmlPath start##########:  "+GetStringFromConfig("Mpp2XmlPath")+"############## end #######");
            Console.Write("################# Mpp2XmlPath start##########:  " + Path.GetFullPath(GetStringFromConfig("Mpp2XmlPath"), _env.ContentRootPath) + "############## end #######");
            //string cmdPath = Path.GetFullPath(GetStringFromConfig("Mpp2Xml"), GetStringFromConfig("Mpp2XmlPath"));
            string cmdPath = Path.GetFullPath(GetStringFromConfig("Mpp2XmlPath"), _env.ContentRootPath);
            //string cmdPath = Path.GetFullPath(GetStringFromConfig("Mpp2Xml"), GetStringFromConfig("Mpp2XmlPath"));

            //string cmdPath = Path.GetFullPath(GetStringFromConfig("Mpp2XmlPath"), _env.ContentRootPath);

            if (!File.Exists(cmdPath))
            {
                Exception e = new FileNotFoundException($"Could not find mpp2xml command under the path: {cmdPath}");
                _logger.LogError(e, "Please check your configuration and place the command to the right place");
                throw e;
            }

            return cmdPath;
        }

        /// <summary>
        ///   Format and execute the mpp2xml conversion command.
        /// </summary>
        public int Convert2Xml(string mppFilePath, string xmlFilePath)
        {
            // force overwrite
            var cmd = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "" : GetStringFromConfig("MonoPath");//"/usr/bin/mono"
            string stdout;

            Console.WriteLine(mppFilePath);
            Console.WriteLine(xmlFilePath);
            int re = Execute($"{cmd} {GetMpp2XmlCmd()} -f {mppFilePath} {xmlFilePath}", out stdout);

            if (re == 0)
            {
                return re;
            }
            else if (re == 126)
            {
                _logger.LogWarning(stdout);
                throw new Exception("Permission denied, please add execution permission to the command!");
            }
            else
            {
                _logger.LogWarning(stdout);
            }

            return re;
        }

        /// <summary>
        ///   Convert MPP file to XML and return
        ///   return empty string if conversion failed.
        /// </summary>
        public string XmlFromMpp(string mppFilePath, bool bOverwrite = false)
        {
            string path = Path.GetDirectoryName(mppFilePath);
            string file = Path.GetFileNameWithoutExtension(mppFilePath) + ".xml";
            string xmlFilePath = path + Path.DirectorySeparatorChar + file;

            if (File.Exists(xmlFilePath))
            {
                if (bOverwrite)
                {
                    File.Delete(xmlFilePath);
                }
                else
                {
                    _logger.LogWarning("XML file already exist: {0}, skip converting...", xmlFilePath);
                    return xmlFilePath;
                }
            }

            int re = Convert2Xml(mppFilePath, xmlFilePath);

            if (re == 0)
                return xmlFilePath;
            else
                return String.Empty;
        }

        public Project TryImport(string xmlFilePath, Project project)
        {
            Xml2Model(xmlFilePath, project);

            SaveAll(project);

            _logger.LogInformation("Import Summary:\n Attributes: {0} Tasks: {1} Calendars: {2} Resources: {3} Assignments: {4}",
                                   project.ExtendedAttributes.Count,
                                   project.Tasks.Count,
                                   project.Calendars.Count,
                                   project.Resources.Count,
                                   project.Assignments.Count);

            return _projectRepo.Find(project.Id);
        }

        public List<TaskViewModel> TryImportWithoutSave(string xmlFilePath)
        {
            Project project = new Project();
            Xml2Model(xmlFilePath, project);
            var viewlist = new List<TaskViewModel>();
            foreach (var item in project.Tasks)
            {
                var newTask = _mapper.Map<TaskViewModel>(item);
                viewlist.Add(newTask);
            }
            return viewlist;
        }

        /// <summary>
        ///   Update field FIELDNAME of PRIMARY MODEL with models's UUID list.
        /// </summary>
        private void UpdateUUIDs<T1, T2>(T1 primaryModel, string fieldName, List<T2> models) where T1 : MppAggregateRoot<Guid> where T2 : MppAggregateRoot<Guid>
        {
            string result = String.Empty;
            foreach (var m in models)
            {
                if (!String.IsNullOrEmpty(result))
                {
                    result += ",";
                }

                result += m.Id.ToString();

                // update foreign key
                m.ParentId = primaryModel.Id;
            }

            object[] @params = new object[] { result };
            primaryModel.GetType().GetProperty(fieldName).SetMethod.Invoke(primaryModel, @params);
        }

#warning 这里未添加消息记录
        /// <summary>
        ///   Update Repository
        /// </summary>
        private bool SaveModels<TModel, TRepository>(List<TModel> models, TRepository repo)
            where TRepository : IAddRepository<TModel, Guid>
            where TModel : MppAggregateRoot<Guid>
        {
            foreach (var m in models)
            {
                repo.Add(m);
            }
            return true;
        }

        private bool SaveModel<TModel, TRepository>(TModel model, TRepository repo) where TRepository : IAddRepository<TModel, Guid> where TModel : MppAggregateRoot<Guid>
        {
            repo.Add(model);
            return true;
        }
        /// <summary>
        ///   Save Mpp project data to database
        /// </summary>
        public bool SaveAll(Project project)
        {
            SaveModels(project.ExtendedAttributes, _attrsRepo);
            SaveModels(project.Assignments, _assignRepo);
            SaveModels(project.Resources, _resourceRepo);

            List<Calendar> wds = project.Calendars;
            foreach (var wd in wds)
            {
                SaveModels(wd.WeekDays, _calendarWeekdayRepo);
                SaveModels(wd.Exceptions, _calendarExceptionRepo);
            }

            SaveModels(project.Calendars, _calendarsRepo);

            List<Task> tasks = project.Tasks;
            foreach (var task in tasks)
            {
                SaveModels(task.Baseline, _baselineRepo);
                SaveModels(task.ExtendedAttribute, _extendedAttributeRepo);
                SaveModels(task.PredecessorLink, _predecessorLinkRepo);
            }

            SaveModels(tasks, _taskRepo);

            // Save project last
            SaveModel(project, _projectRepo);

            _helper.Commit();

            return true;
        }
        /// <summary>
        ///   Convert xml file to MppProject model
        /// </summary>
        public bool Xml2Model(string xmlFilePath, Project project)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(xmlFilePath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to load the xml file from {xmlFilePath}");
                throw e;
            }

            XmlNode root = NodeFromName(doc, "Project");
            if (root == null)
            {
                throw new XmlException($"Failed to parse the xml from {xmlFilePath}, missing node: Project");
            }

            return ParseProject(root, project);
        }
        /// <summary>
        /// 解析项目
        /// </summary>
        /// <returns></returns>
        public bool ParseProject(XmlNode root, Project project)
        {
            // project = new Project();
            try
            {
                XmlParseGeneral(root, ref project);
            }
            catch (Exception es)
            {

                throw;
            }

            

            return true;
        }
        /// <summary>
        ///   Parse the NODE to the MODEL object
        /// </summary>
        private void XmlParseGeneral<T>(XmlNode node, ref T model) where T : MppAggregateRoot<Guid>
        {
            XmlNodeList items = node.ChildNodes;
            foreach (XmlNode item in items)
            {
                PropertyInfo prop = model.GetType().GetProperty(item.Name);

                if (prop == null)
                {
                    // another shot with underline, convension.
                    prop = model.GetType().GetProperty("_" + item.Name);
                    if (prop == null)
                    {
                        // suppress warning, treat specially.
                        if (item.Name != "WorkingTimes")
                        {
                            _logger.LogWarning($"Could not found corresponding property of the model: {item.Name}");
                        }

                        continue;
                    }
                }

                MethodInfo setter = prop.GetSetMethod();
                Debug.Assert(prop.CanWrite);

                ParameterInfo[] @params = setter.GetParameters();
                Debug.Assert(@params.Length == 1);

                foreach (var param in @params)
                {
                    if (param.ParameterType == typeof(bool))
                    {
                        _ModelSetBool(model, setter, item.InnerText);
                    }
                    else if (param.ParameterType == typeof(int))
                    {
                        _ModelSetInt(model, setter, item.InnerText);
                    }
                    else if (param.ParameterType == typeof(float))
                    {
                        _ModelSetFloat(model, setter, item.InnerText);
                    }
                    else if (param.ParameterType == typeof(decimal))
                    {
                        _ModelSetDecimal(model, setter, item.InnerText);
                    }
                    else if (param.ParameterType == typeof(string))
                    {
                        _ModelSetString(model, setter, item.InnerText);
                    }
                    else if (param.ParameterType == typeof(Nullable<DateTime>))
                    {
                        _ModelSetDateTime(model, setter, item.InnerText);
                    }
                    else if (param.ParameterType == typeof(List<TaskPredecessorLink>))
                    {
                        ParseTaskPredecessorLink(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<TaskExtendedAttribute>))
                    {
                        ParseTaskExtendedAttribute(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<TaskBaseline>))
                    {
                        ParseTaskBaseline(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<Task>))
                    {
                        ParseTasks(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<ExtendedAttribute>))
                    {
                        ParseExtendedAttributes(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<Resource>))
                    {
                        ParseResources(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<Assignment>))
                    {
                        ParseAssignments(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<Calendar>))
                    {
                        ParseCalendars(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<CalendarWeekDay>))
                    {
                        ParseCalendarWeekDay(item, ref model);
                    }
                    else if (param.ParameterType == typeof(List<CalendarException>))
                    {
                        ParseCalendarExceptions(item, ref model);
                    }
                    else
                    {
                        throw new Exception($"Unexpected parameter type: {param.ParameterType}");
                    }
                }
            }
        }

        private bool ParseExtendedAttributes<T>(XmlNode attrsNode, ref T project) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Project).IsInstanceOfType(project));

            XmlNodeList items = attrsNode.ChildNodes;

            List<ExtendedAttribute> lst = (List<ExtendedAttribute>)project.GetType().GetProperty("ExtendedAttributes").GetMethod.Invoke(project, null);
            foreach (XmlNode item in items)
            {
                ExtendedAttribute attr = new ExtendedAttribute();

                XmlParseGeneral(item, ref attr);

                lst.Add(attr);
            }

            UpdateUUIDs(project, "ExtendedAttributesUUIDs", lst);
            return true;
        }

        private bool ParseCalendars<T>(XmlNode calNode, ref T project) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Project).IsInstanceOfType(project));

            XmlNodeList items = calNode.ChildNodes;

            List<Calendar> lst = (List<Calendar>)project.GetType().GetProperty("Calendars").GetMethod.Invoke(project, null);
            foreach (XmlNode item in items)
            {
                Calendar cal = new Calendar();

                XmlParseGeneral(item, ref cal);

                lst.Add(cal);
            }

            UpdateUUIDs(project, "CalendarsUUIDs", lst);
            return true;
        }

        private bool ParseCalendarWeekDay<T>(XmlNode wdNode, ref T model) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Calendar).IsInstanceOfType(model));

            List<CalendarWeekDay> lst = (List<CalendarWeekDay>)model.GetType().GetProperty("WeekDays").GetMethod.Invoke(model, null);

            // Node: WeekDays -> WeekDay
            XmlNodeList wds = wdNode.ChildNodes;
            foreach (XmlNode wd in wds)
            {
                CalendarWeekDay submodel = new CalendarWeekDay();

                // parse DayType, DayWorking here
                XmlParseGeneral(wd, ref submodel);

                foreach (XmlNode node in wd.ChildNodes)
                {
                    // Node: WeekDay -> TimePeriod
                    // only FromDate and ToDate exists
                    if (node.Name == "TimePeriod")
                    {
                        XmlParseGeneral(node, ref submodel);
                    }

                    // Node: WeekDay -> WorkingTimes
                    if (node.Name == "WorkingTimes")
                    {
                        int index = 0;
                        // Node: WeekTimes -> WorkingTime
                        foreach (XmlNode wt in node.ChildNodes)
                        {
                            foreach (XmlNode w in wt.ChildNodes)
                            {
                                string propName = w.Name + "_" + index;
                                MethodInfo setter = submodel.GetType().GetProperty(propName).GetSetMethod();
                                _ModelSetDateTime(submodel, setter, w.InnerText);
                            }

                            index++;
                        }
                    }
                }

                lst.Add(submodel);
            }

            UpdateUUIDs(model, "WeekDaysUUIDs", lst);
            return true;
        }

        private bool ParseCalendarExceptions<T>(XmlNode wdNode, ref T model) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Calendar).IsInstanceOfType(model));

            List<CalendarException> lst = (List<CalendarException>)model.GetType().GetProperty("Exceptions").GetMethod.Invoke(model, null);

            // Node: WeekDays -> WeekDay
            XmlNodeList wds = wdNode.ChildNodes;
            foreach (XmlNode wd in wds)
            {
                CalendarException submodel = new CalendarException();
                // parse DayType, DayWorking here
                XmlParseGeneral(wd, ref submodel);
                foreach (XmlNode node in wd.ChildNodes)
                {
                    // Node: WeekDay -> TimePeriod
                    // only FromDate and ToDate exists
                    if (node.Name == "TimePeriod")
                    {
                        XmlParseGeneral(node, ref submodel);
                    }

                    // Node: WeekDay -> WorkingTimes
                    if (node.Name == "WorkingTimes")
                    {
                        int index = 0;
                        // Node: WeekTimes -> WorkingTime
                        foreach (XmlNode wt in node.ChildNodes)
                        {
                            foreach (XmlNode w in wt.ChildNodes)
                            {
                                string propName = w.Name + "_" + index;
                                MethodInfo setter = submodel.GetType().GetProperty(propName).GetSetMethod();
                                _ModelSetDateTime(submodel, setter, w.InnerText);
                            }

                            index++;
                        }
                    }
                }
                lst.Add(submodel);
            }

            UpdateUUIDs(model, "ExceptionsUUIDs", lst);
            return true;
        }

        private bool ParseResources<T>(XmlNode resNode, ref T project) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Project).IsInstanceOfType(project));

            XmlNodeList items = resNode.ChildNodes;

            List<Resource> lst = (List<Resource>)project.GetType().GetProperty("Resources").GetMethod.Invoke(project, null);
            foreach (XmlNode item in items)
            {
                Resource res = new Resource();
                XmlParseGeneral(item, ref res);
                lst.Add(res);
            }

            UpdateUUIDs(project, "ResourcesUUIDs", lst);
            return true;
        }

        private bool ParseAssignments<T>(XmlNode assiNode, ref T project) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Project).IsInstanceOfType(project));

            XmlNodeList items = assiNode.ChildNodes;

            List<Assignment> lst = (List<Assignment>)project.GetType().GetProperty("Assignments").GetMethod.Invoke(project, null);
            foreach (XmlNode item in items)
            {
                Assignment assi = new Assignment();
                XmlParseGeneral(item, ref assi);
                lst.Add(assi);
            }

            UpdateUUIDs(project, "AssignmentsUUIDs", lst);
            return true;
        }

        /// <summary>
        ///   Parse xml tasks NODE to PROJECT model
        /// </summary>
        private bool ParseTasks<T>(XmlNode tasks, ref T project) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Project).IsInstanceOfType(project));

            XmlNodeList items = tasks.ChildNodes;

            List<Task> lst = (List<Task>)project.GetType().GetProperty("Tasks").GetMethod.Invoke(project, null);
            foreach (XmlNode item in items)
            {
                Task model = new Task();
                if (model.UID == 122)
                {
                    Console.WriteLine(model);
                }
                XmlParseGeneral(item, ref model);
                

                UpdateUUIDs(model, "BaselineUUIDs", model.Baseline);
                UpdateUUIDs(model, "PredecessorLinkUUIDs", model.PredecessorLink);
                UpdateUUIDs(model, "ExtendedAttributeUUIDs", model.ExtendedAttribute);

                if(model.UID == 121)
                {
                    Console.WriteLine(model);
                }

                if(model.UID != 0)
                {
                    lst.Add(model);
                }
                
            }

            UpdateUUIDs(project, "TasksUUIDs", lst);

            return true;
        }

        private void ParseTaskPredecessorLink<T>(XmlNode link, ref T model) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Task).IsInstanceOfType(model));
            Debug.Assert(link.ChildNodes.Count > 0);

            TaskPredecessorLink subModel = new TaskPredecessorLink();
            XmlParseGeneral(link, ref subModel);

            object lst = model.GetType().GetProperty("PredecessorLink").GetMethod.Invoke(model, null);
            ((List<TaskPredecessorLink>)lst).Add(subModel);
        }

        private void ParseTaskExtendedAttribute<T>(XmlNode attr, ref T model) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Task).IsInstanceOfType(model));
            Debug.Assert(attr.ChildNodes.Count > 0);

            TaskExtendedAttribute subModel = new TaskExtendedAttribute();
            XmlParseGeneral(attr, ref subModel);

            object lst = model.GetType().GetProperty("ExtendedAttribute").GetMethod.Invoke(model, null);
            ((List<TaskExtendedAttribute>)lst).Add(subModel);
        }

        private void ParseTaskBaseline<T>(XmlNode baseline, ref T model) where T : MppAggregateRoot<Guid>
        {
            Debug.Assert(typeof(Task).IsInstanceOfType(model));
            Debug.Assert(baseline.ChildNodes.Count > 0);

            TaskBaseline subModel = new TaskBaseline();
            XmlParseGeneral(baseline, ref subModel);

            object lst = model.GetType().GetProperty("Baseline").GetMethod.Invoke(model, null);
            ((List<TaskBaseline>)lst).Add(subModel);
        }

        /// <summary>
        ///   set bool value on model use the setter.
        ///   only 0 or 1 is allowed for this type.
        /// </summary>
        private void _ModelSetBool<T>(T model, MethodInfo setter, string text)
        {
            Debug.Assert(!String.IsNullOrEmpty(text));

            object[] parameters;
            if (text == "0")
            {
                parameters = new object[] { false };
            }
            else if (text == "1")
            {
                parameters = new object[] { true };
            }
            else
            {
                throw new Exception("Unexpected bool number: {text}");
            }

            setter.Invoke(model, parameters);
        }

        private void _ModelSetInt<T>(T model, MethodInfo setter, string text)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(text));

            object[] parameters = new object[] { int.Parse(text) };
            setter.Invoke(model, parameters);
        }

        private void _ModelSetFloat<T>(T model, MethodInfo setter, string text)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(text));

            object[] parameters = new object[] { float.Parse(text) };
            setter.Invoke(model, parameters);
        }

        private void _ModelSetDecimal<T>(T model, MethodInfo setter, string text)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(text));

            object[] parameters = new object[] { decimal.Parse(text) };
            setter.Invoke(model, parameters);
        }

        private void _ModelSetString<T>(T model, MethodInfo setter, string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                object[] parameters = new object[] { text };
                setter.Invoke(model, parameters);
            }
        }

        private void _ModelSetDateTime<T>(T model, MethodInfo setter, string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                object[] parameters = new object[] { DateTime.Parse(text) };
                setter.Invoke(model, parameters);
            }
        }

        /// <summary>
        ///   Convert MS Project Duration to TimeSpan
        ///   A duration of time, provided in the format
        /// PnYnMnDTnHnMnS where nY represents the number of years, nM
        /// the number of months, nD the number of days, T the
        /// date/time separator, nH the number of hours, nM the number
        /// of minutes, and nS the number of seconds.
        /// For example, to indicate a duration of 1 year, 2 months, 3
        /// days, 10 hours, and 30 minutes, you write:
        /// P1Y2M3DT10H30M. You could also indicate a duration of
        /// minus 120 days as -P120D.
        /// </summary>
        public TimeSpan Duration2TimeSpan(string dura)
        {
            if (dura.StartsWith("PT"))
            {
                return _Duration2TimeSpanFormat1(dura);
            }
            else
            {
                return _Duration2TimeSpanFormat2(dura);
            }
        }

        private TimeSpan _Duration2TimeSpanFormat1(string dura)
        {
            string expr = @"PT(\d+)H(\d+)M(\d+)S";
            Match m = Regex.Match(dura, expr, RegexOptions.IgnoreCase);

            if (m.Success)
            {
                try
                {
                    // the match starting from index 1, zero store the original match string
                    int hours = int.Parse(m.Groups[1].Value);
                    int minutes = int.Parse(m.Groups[2].Value);
                    int seconds = int.Parse(m.Groups[3].Value);

                    return new TimeSpan(hours, minutes, seconds);

                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to parse the Duration string: {dura}, {e.Message}");
                }
            }
            else
            {
                throw new Exception($"Failed to parse the Duration string: {dura}");
            }
        }

        private TimeSpan _Duration2TimeSpanFormat2(string dura)
        {
            string expr = @"P(\d+)Y(\d+)M(\d+)DT(\d+)H(\d+)M(\d+)S";
            Match m = Regex.Match(dura, expr, RegexOptions.IgnoreCase);

            if (m.Success)
            {
                try
                {
                    // the match starting from index 1, zero store the original match string
                    int years = int.Parse(m.Groups[1].Value);
                    int months = int.Parse(m.Groups[2].Value);
                    int days = int.Parse(m.Groups[3].Value);
                    int hours = int.Parse(m.Groups[4].Value);
                    int minutes = int.Parse(m.Groups[5].Value);
                    int seconds = int.Parse(m.Groups[6].Value);

                    // FIXME: Add start time and calculate the days of years and months
                    return new TimeSpan(years * 365 + months * 30 + days, hours, minutes, seconds);
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to parse the Duration string: {dura}, {e.Message}");
                }
            }
            else
            {
                throw new Exception($"Failed to parse the Duration string: {dura}");
            }
        }

        /// <summary>
        ///   Dump Models, debug usage only
        /// </summary>
        private void DumpModels(List<Task> models)
        {
            foreach (var m in models)
            {
                DumpModel(m);
            }
        }

        /// <summary>
        ///   Dump only one Model, debug usage only
        /// </summary>
        private void DumpModel<T>(T model)
        {
            PropertyInfo[] props = model.GetType().GetProperties();

            foreach (var prop in props)
            {
                Console.WriteLine("{0}: {1}", prop.Name, prop.GetGetMethod().Invoke(model, null));
            }
        }

        /// <summary>
        ///   Helper method for child node selection
        ///   replacement of SelectSingleNode method, because bug exists for xpath search on .NET core 2.1.
        /// </summary>
        private XmlNode NodeFromName(XmlNode parent, string name)
        {
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node.Name == name)
                {
                    return node;
                }
            }

            return null;
        }

        public XmlDocument Export2Xml(Guid projectId)
        {
            Project project = GetProjectTree(projectId);

            XmlDocument doc = CreateProjectXmlTemplate();

            XmlElement root = doc.DocumentElement;
            DeserializeXmlNode(doc, root, project);

            XmlElement extendedAttributes = DeserializeXmlNodeList<ExtendedAttribute>(doc, project.ExtendedAttributes,
                                                                                          "ExtendedAttributes", "ExtendedAttribute");
            root.AppendChild(extendedAttributes);

            XmlElement calendarsRoot = doc.CreateElement("Calendars");
            foreach (var calendar in project.Calendars)
            {
                XmlNode calendarNode = DeserializeCalendar(doc, calendar);
                calendarsRoot.AppendChild(calendarNode);
            }

            root.AppendChild(calendarsRoot);

            XmlElement taskRoot = doc.CreateElement("Tasks");
            foreach (var task in project.Tasks)
            {
                XmlNode taskNode = DeserializeTask(doc, task);
                taskRoot.AppendChild(taskNode);
            }
            root.AppendChild(taskRoot);

            XmlElement resNode = DeserializeXmlNodeList<Resource>(doc, project.Resources, "Resources", "Resource");
            root.AppendChild(resNode);

            XmlElement assiNode = DeserializeXmlNodeList<Assignment>(doc, project.Assignments, "Assignments", "Assignment");
            root.AppendChild(assiNode);

            return doc;
        }

        private XmlNode DeserializeCalendar(XmlDocument doc, Calendar calendar)
        {
            XmlElement calendarNode = doc.CreateElement("Calendar");
            DeserializeXmlNode(doc, calendarNode, calendar);

            XmlElement weekdays = doc.CreateElement("WeekDays");
            calendarNode.AppendChild(weekdays);

            foreach (var weekday in calendar.WeekDays)
            {
                XmlElement weekdayNode = doc.CreateElement("WeekDay");
                weekdays.AppendChild(weekdayNode);

                XmlElement DayTypeNode = doc.CreateElement("DayType");
                DayTypeNode.InnerText = weekday.DayType.ToString();
                weekdayNode.AppendChild(DayTypeNode);

                XmlElement DayWorkingNode = doc.CreateElement("DayWorking");
                DayWorkingNode.InnerText = weekday.DayWorking == true ? "1" : "0";
                weekdayNode.AppendChild(DayWorkingNode);

                XmlElement workingTimesRoot = doc.CreateElement("WorkingTimes");
                weekdayNode.AppendChild(workingTimesRoot);

                for (var i = 0; i < 5; i++)
                {
                    object fromTimeValue = weekday.GetType().GetProperty($"FromTime_{i}").GetMethod.Invoke(weekday, null);

                    object toTimeValue = weekday.GetType().GetProperty($"ToTime_{i}").GetMethod.Invoke(weekday, null);
                    if (fromTimeValue == null || toTimeValue == null)
                        continue;

                    XmlElement workingTimeNode = doc.CreateElement("WorkingTime");

                    XmlElement fromTimeNode = doc.CreateElement("FromTime");
                    fromTimeNode.InnerText = ((DateTime)fromTimeValue).ToString("HH:mm:ssK");
                    workingTimeNode.AppendChild(fromTimeNode);

                    XmlElement toTimeNode = doc.CreateElement("ToTime");
                    toTimeNode.InnerText = ((DateTime)toTimeValue).ToString("HH:mm:ssK");
                    workingTimeNode.AppendChild(toTimeNode);

                    workingTimesRoot.AppendChild(workingTimeNode);
                }
            }

            return calendarNode;
        }

        public XmlNode DeserializeTask(XmlDocument doc, Task task)
        {
            XmlElement taskRoot = doc.CreateElement("Task");
            DeserializeXmlNode(doc, taskRoot, task);

            foreach (var link in task.PredecessorLink)
            {
                XmlElement linkRoot = doc.CreateElement("PredecessorLink");
                DeserializeXmlNode(doc, linkRoot, link);

                taskRoot.AppendChild(linkRoot);
            }

            foreach (var attr in task.ExtendedAttribute)
            {
                XmlElement attrRoot = doc.CreateElement("ExtendedAttribute");
                DeserializeXmlNode(doc, attrRoot, attr);

                taskRoot.AppendChild(attrRoot);
            }

            foreach (var baseline in task.Baseline)
            {
                XmlElement baselineRoot = doc.CreateElement("Baseline");
                DeserializeXmlNode(doc, baselineRoot, baseline);

                taskRoot.AppendChild(baselineRoot);
            }

            return taskRoot;
        }

        public XmlElement DeserializeXmlNodeList<TEntity>(XmlDocument doc, IEnumerable<TEntity> lst, string listName, string entityName)
            where TEntity : MppAggregateRoot<Guid>
        {
            XmlElement lstRoot = doc.CreateElement(listName);
            foreach (var l in lst)
            {
                XmlElement eRoot = doc.CreateElement(entityName);
                DeserializeXmlNode(doc, eRoot, l);

                lstRoot.AppendChild(eRoot);
            }

            return lstRoot;
        }

        public XmlNode DeserializeXmlNode<TEntity>(XmlDocument doc, XmlElement eRoot, TEntity entity) where TEntity : MppAggregateRoot<Guid>
        {
            string[] forbidFields = new string[] { "Id", "ParentId", "Enabled" };

            PropertyInfo[] props = entity.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                Type returnType = prop.GetMethod.ReturnType;

                // skip all List<T>
                if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(List<>))
                    continue;

                // skip internal fields
                if (forbidFields.Contains(prop.Name))
                    continue;

                // FIXME: skip additional uuid fields
                if (prop.Name.EndsWith("UUIDs"))
                    continue;


                object result = prop.GetMethod.Invoke(entity, null);

                if (result != null)
                {
                    // remove starting underline
                    string elementName = prop.Name.StartsWith("_") ? prop.Name.Substring(1) : prop.Name;

                    XmlElement el = doc.CreateElement(elementName);

                    if (returnType == typeof(Nullable<DateTime>))
                    {
                        el.InnerText = ((DateTime)result).ToString("yyyy-MM-ddTHH:mm:ssK");
                    }
                    else if (returnType == typeof(bool))
                    {
                        el.InnerText = (bool)result == true ? "1" : "0";
                    }
                    else
                    {
                        el.InnerText = result.ToString();
                    }

                    eRoot.AppendChild(el);
                }
            }

            return eRoot;
        }


        public XmlDocument CreateProjectXmlTemplate()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
            XmlElement root = doc.CreateElement("Project");
            doc.AppendChild(root);

            return doc;
        }

        public Byte[] Xml2Bytes(XmlDocument doc)
        {
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);

            return stream.GetBuffer();
        }

        public Project GetProjectTree(Guid projectId)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            Project project = _helper.GetProject(projectId);
            project.ExtendedAttributes = _helper.GetExtendedAttributes(projectId).ToList();

            project.Calendars = _helper.GetCalendars(projectId).ToList();
            foreach (var calendar in project.Calendars)
            {
                calendar.WeekDays = _helper.GetCalendarWeekDaysByParentId(calendar.Id).ToList();
            }

            project.Resources = _helper.GetResources(projectId).ToList();
            project.Assignments = _helper.GetAssignments(projectId).ToList();

            project.Tasks = _helper.GetTasks(projectId).ToList();
            foreach (var task in project.Tasks)
            {
                task.PredecessorLink = _helper.GetTaskPredecessorLinks(task.Id).ToList();
                task.Baseline = _helper.GetTaskBaselines(task.Id).ToList();
                task.ExtendedAttribute = _helper.GetTaskExtendedAttributes(task.Id).ToList();
            }

            return project;
        }
    }
}