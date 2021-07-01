using ARchGLCloud.Application.MPP.AutoMapper;
using ARchGLCloud.Application.MPP.Interfaces;
using ARchGLCloud.Application.MPP.Services;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Events;
using ARchGLCloud.Domain.Core.Interfaces;
using ARchGLCloud.Domain.Core.Models;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.Core.Repositories;
using ARchGLCloud.Domain.MPP.Interfaces;
using ARchGLCloud.Infra.Core.EventSourcing;
using ARchGLCloud.Infra.Core.Repositories;
using ARchGLCloud.Infra.Core.UoW;
using ARchGLCloud.Infra.MPP.Context;
using ARchGLCloud.Infra.MPP.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ARchGLCloud.WebApi.MPP
{
    public class BootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddMappingProfiles();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            //services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();
            //services.AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
            //services.AddScoped<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            
            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSQLRepository<MPPContext>>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<IUser, User>();

            // Infra - Identity

            services.AddScoped<MPPContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork<MPPContext>>();

            services.AddScoped<MppServiceHelper>();
            
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IImportExportService, ImportExportService>();

            
            services.AddScoped<IExtendedAttributeRepository, ExtendedAttributeRepository>();
            
            services.AddScoped<ICalendarWeekDayRepository, CalendarWeekDayRepository>();
            services.AddScoped<ICalendarExceptionRepository, CalendarExceptionRepository>();
            
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<ITaskPredecessorLinkRepository, TaskPredecessorLinkRepository>();
            services.AddScoped<ITaskExtendedAttributeRepository, TaskExtendedAttributeRepository>();
            services.AddScoped<ITaskBaselineRepository, TaskBaselineRepository>();


            services.AddScoped<ICalendarRepository, CalendarRepository>();
            services.AddScoped<ICalendarService, CalendarService>();

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskService>();
        }
    }
}
