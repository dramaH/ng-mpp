using ARchGLCloud.Application.MPP.Dtos;
using ARchGLCloud.Application.MPP.ViewModels;
using ARchGLCloud.Domain.MPP.Commands;
using ARchGLCloud.Domain.MPP.Events;
using ARchGLCloud.Domain.MPP.Models;
using AutoMapper;

namespace ARchGLCloud.Application.MPP.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            #region Command
            CreateMap<Assignment, AddAssignmentCommand>();
            CreateMap<Calendar, AddCalendarCommand>();
            CreateMap<CalendarException, AddCalendarExceptionCommand>();
            CreateMap<CalendarWeekDay, AddCalendarWeekDayCommand>();
            CreateMap<ExtendedAttribute, AddExtendedAttributeCommand>();
            CreateMap<Project, AddProjectCommand>();
            CreateMap<Resource, AddResourceCommand>();
            CreateMap<Task, AddTaskCommand>();
            CreateMap<TaskBaseline, AddTaskBaselineCommand>();
            CreateMap<TaskExtendedAttribute, AddTaskExtendedAttributeCommand>();
            CreateMap<TaskPredecessorLink, AddTaskPredecessorLinkCommand>();

            CreateMap<Assignment, UpdateAssignmentCommand>();
            CreateMap<Calendar, UpdateCalendarCommand>();
            CreateMap<CalendarException, UpdateCalendarExceptionCommand>();
            CreateMap<CalendarWeekDay, UpdateCalendarWeekDayCommand>();
            CreateMap<ExtendedAttribute, UpdateExtendedAttributeCommand>();
            CreateMap<Project, UpdateProjectCommand>();
            CreateMap<Resource, UpdateResourceCommand>();
            CreateMap<Task, UpdateTaskCommand>();
            CreateMap<TaskBaseline, UpdateTaskBaselineCommand>();
            CreateMap<TaskExtendedAttribute, UpdateTaskExtendedAttributeCommand>();
            CreateMap<TaskPredecessorLink, UpdateTaskPredecessorLinkCommand>();

            CreateMap<Assignment, RemoveAssignmentCommand>();
            CreateMap<Calendar, RemoveCalendarCommand>();
            CreateMap<CalendarException, RemoveCalendarExceptionCommand>();
            CreateMap<CalendarWeekDay, RemoveCalendarWeekDayCommand>();
            CreateMap<ExtendedAttribute, RemoveExtendedAttributeCommand>();
            CreateMap<Project, RemoveProjectCommand>();
            CreateMap<Resource, RemoveResourceCommand>();
            CreateMap<Task, RemoveTaskCommand>();
            CreateMap<TaskBaseline, RemoveTaskBaselineCommand>();
            CreateMap<TaskExtendedAttribute, RemoveTaskExtendedAttributeCommand>();
            CreateMap<TaskPredecessorLink, RemoveTaskPredecessorLinkCommand>();
            #endregion

            #region Event
            CreateMap<Assignment, AssignmentAddedEvent>();
            CreateMap<Calendar, CalendarAddedEvent>();
            CreateMap<CalendarException, CalendarExceptionAddedEvent>();
            CreateMap<CalendarWeekDay, CalendarWeekDayAddedEvent>();
            CreateMap<ExtendedAttribute, ExtendedAttributeAddedEvent>();
            CreateMap<Project, ProjectAddedEvent>();
            CreateMap<Resource, ResourceAddedEvent>();
            CreateMap<Task, TaskAddedEvent>();
            CreateMap<TaskBaseline, TaskBaselineAddedEvent>();
            CreateMap<TaskExtendedAttribute, TaskExtendedAttributeAddedEvent>();
            CreateMap<TaskPredecessorLink, TaskPredecessorLinkAddedEvent>();

            CreateMap<Assignment, AssignmentUpdatedEvent>();
            CreateMap<Calendar, CalendarUpdatedEvent>();
            CreateMap<CalendarException, CalendarExceptionUpdatedEvent>();
            CreateMap<CalendarWeekDay, CalendarWeekDayUpdatedEvent>();
            CreateMap<ExtendedAttribute, ExtendedAttributeUpdatedEvent>();
            CreateMap<ExtendedAttribute, ProjectUpdatedEvent>();
            CreateMap<Project, ProjectUpdatedEvent>(); 
            CreateMap<Project, ExtendedAttributeUpdatedEvent>();
            CreateMap<Resource, ResourceUpdatedEvent>();
            CreateMap<Task, TaskUpdatedEvent>();
            CreateMap<TaskBaseline, TaskBaselineUpdatedEvent>();
            CreateMap<TaskExtendedAttribute, TaskExtendedAttributeUpdatedEvent>();
            CreateMap<TaskPredecessorLink, TaskPredecessorLinkUpdatedEvent>();

            CreateMap<Assignment, AssignmentRemovedEvent>();
            CreateMap<Calendar, CalendarRemovedEvent>();
            CreateMap<CalendarException, CalendarExceptionRemovedEvent>();
            CreateMap<CalendarWeekDay, CalendarWeekDayRemovedEvent>();
            CreateMap<ExtendedAttribute, ExtendedAttributeRemovedEvent>();
            CreateMap<Project, ProjectRemovedEvent>();
            CreateMap<Resource, ResourceRemovedEvent>();
            CreateMap<Task, TaskRemovedEvent>();
            CreateMap<TaskBaseline, TaskBaselineRemovedEvent>();
            CreateMap<TaskExtendedAttribute, TaskExtendedAttributeRemovedEvent>();
            CreateMap<TaskPredecessorLink, TaskPredecessorLinkRemovedEvent>();
            #endregion

            CreateMap<Task, TaskViewModel>();
            CreateMap<Project, GetProjectDto>();
            CreateMap<CalendarException, CalendarExceptionDto>();
            CreateMap<Calendar, CalendarDto>();
            CreateMap<CalendarWeekDay, CalendarWeekDayDto>();
            CreateMap<CalendarException, CalendarExceptionDto>();
        }
    }
}
