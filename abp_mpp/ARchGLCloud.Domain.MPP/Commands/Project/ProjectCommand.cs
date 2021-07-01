using ARchGLCloud.Domain.Core.Commands;
using ARchGLCloud.Domain.MPP.Models;
using System;
using System.Collections.Generic;

namespace ARchGLCloud.Domain.MPP.Commands
{
    public abstract class ProjectCommand : Command
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int SaveVersion { get; set; }

        public string UID { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Category { get; set; }

        public string Company { get; set; }

        public string Manager { get; set; }

        public string Author { get; set; }

        public DateTime? CreationDate { get; set; }

        public int Revision { get; set; }

        public DateTime? LastSaved { get; set; }

        public bool ScheduleFromStart { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public int FYStartDate { get; set; }

        public int CriticalSlackLimit { get; set; }

        public int CurrencyDigits { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyCode { get; set; }

        public int CurrencySymbolPosition { get; set; }

        public int CalendarUID { get; set; }

        public DateTime? DefaultStartTime { get; set; }

        public DateTime? DefaultFinishTime { get; set; }

        public int MinutesPerDay { get; set; }

        public int MinutesPerWeek { get; set; }

        public int DaysPerMonth { get; set; }

        public int DefaultTaskType { get; set; }

        public int DefaultFixedCostAccrual { get; set; }

        public float DefaultStandardRate { get; set; }

        public float DefaultOvertimeRate { get; set; }

        public int DurationFormat { get; set; }

        public int WorkFormat { get; set; }

        public bool EditableActualCosts { get; set; }

        public bool HonorConstraints { get; set; }

        public int EarnedValueMethod { get; set; }

        public bool InsertedProjectsLikeSummary { get; set; }

        public bool MultipleCriticalPaths { get; set; }

        public bool NewTasksEffortDriven { get; set; }

        public bool NewTasksEstimated { get; set; }

        public bool SplitsInProgressTasks { get; set; }

        public bool SpreadActualCost { get; set; }

        public bool SpreadPercentComplete { get; set; }

        public bool TaskUpdatesResource { get; set; }

        public bool FiscalYearStart { get; set; }

        public int WeekStartDay { get; set; }

        public bool MoveCompletedEndsBack { get; set; }

        public bool MoveRemainingStartsBack { get; set; }

        public bool MoveRemainingStartsForward { get; set; }

        public bool MoveCompletedEndsForward { get; set; }

        public int BaselineForEarnedValue { get; set; }

        public bool AutoAddNewResourcesAndTasks { get; set; }

        public DateTime? StatusDate { get; set; }

        public DateTime? CurrentDate { get; set; }

        public bool MicrosoftProjectServerURL { get; set; }

        public bool Autolink { get; set; }

        public int NewTaskStartDate { get; set; }

        public int DefaultTaskEVMethod { get; set; }

        public bool ProjectExternallyEdited { get; set; }

        public DateTime? ExtendedCreationDate { get; set; }

        public bool ActualsInSync { get; set; }

        public bool RemoveFileProperties { get; set; }

        public bool AdminProject { get; set; }

        public List<ExtendedAttribute> ExtendedAttributes { get; set; }

        public string ExtendedAttributesUUIDs { get; set; }

        public List<Calendar> Calendars { get; set; }

        public string CalendarsUUIDs { get; set; }

        public List<Task> Tasks { get; set; }

        public string TasksUUIDs { get; set; }

        public List<Resource> Resources { get; set; }

        public string ResourcesUUIDs { get; set; }

        public List<Assignment> Assignments { get; set; }

        public string AssignmentsUUIDs { get; set; }
    }
}
