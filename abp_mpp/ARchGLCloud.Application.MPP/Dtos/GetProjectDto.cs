using System;
using System.Collections.Generic;
using System.Text;
using ARchGLCloud.Domain.MPP.Models;

namespace ARchGLCloud.Application.MPP.Dtos
{
    public class GetProjectDto
    {
        public Guid Id { get; set; }
        // The version of Microsoft Office Project from which the
        // project was saved. Values are: 12=Project 2007.
        public int SaveVersion { get; set; }

        // The unique ID of the project. maxLength: 16
        public string UID { get; set; }

        // The name of the project. maxLength: 255
        public string Name { get; set; }

        // The title of the project. maxLength: 512
        public string Title { get; set; }

        // The subject of the project. maxLength: 512
        public string Subject { get; set; }

        // The category of the project. maxLength: 512
        public string Category { get; set; }

        // The company that owns the project. maxLength: 512
        public string Company { get; set; }

        // The manager of the project. maxLength: 512
        public string Manager { get; set; }

        // The author of the project. maxLength: 512
        public string Author { get; set; }

        // The date that the project was created.
        public DateTime? CreationDate { get; set; }

        // The number of times a project has been saved.
        public int Revision { get; set; }

        // The date that the project was last saved.
        public DateTime? LastSaved { get; set; }

        // Whether the project is schduled from the start date or
        // finish date. default: true
        public bool ScheduleFromStart { get; set; }

        // The start date of the project. Required if
        // ScheduleFromStart is true.
        public DateTime? StartDate { get; set; }

        // The finish date of the project. Required if
        // ScheduleFromStart is false.
        public DateTime? FinishDate { get; set; }

        // Fiscal Year starting month. Values are: 1=January,
        // 2=February, 3=March, 4=April, 5=May, 6=June, 7=July,
        // 8=August, 9=September, 10=October, 11=November,
        // 12=December.
        public int FYStartDate { get; set; }

        // The number of days past its end date that a task can go
        // before Microsoft Office Project marks that task as a
        // critical task.
        public int CriticalSlackLimit { get; set; }

        // The number of digits after the decimal symbol.
        public int CurrencyDigits { get; set; }

        // The currency symbol used in the project. maxLength: 20
        public string CurrencySymbol { get; set; }

        // The three-letter currency character code as defined in ISO
        // 4217. Valid values is: USD, CNY. maxLength: 3
        public string CurrencyCode { get; set; }

        // The position of the currency symbol.  Values are: 0=Before,
        // 1=After, 2=Before With Space, 3=After with space.
        public int CurrencySymbolPosition { get; set; }

        // The project calendar.  Refers to a valid UID in the
        // Calendars element of the Microsoft Office Project 2007 XML
        // Schema.
        public int CalendarUID { get; set; }

        // The default start time of new tasks.
        public DateTime? DefaultStartTime { get; set; }

        // The default finish time of new tasks.
        public DateTime? DefaultFinishTime { get; set; }

        // The number of minutes per day.
        public int MinutesPerDay { get; set; }

        // The number of minutes per week.
        public int MinutesPerWeek { get; set; }

        // The number of days per month.
        public int DaysPerMonth { get; set; }

        // The default type of new tasks. Values are: 0=Fixed Units,
        // 1=Fixed Duration, 2=Fixed Work. default: 1
        public int DefaultTaskType { get; set; }

        // The default from where fixed costs are accrued.  Values
        // are: 1=Start, 2=Prorated, 3=End.
        public int DefaultFixedCostAccrual { get; set; }

        // The default overtime rate for new resources.
        public int DefaultStandardRate { get; set; }

        // The default overtime rate for new resources.
        public int DefaultOvertimeRate { get; set; }

        // The format for expressing the bulk duration.  Values are:
        // 3=m, 4=em, 5=h, 6=eh, 7=d, 8=ed, 9=w, 10=ew, 11=mo, 12=emo,
        // 19=%, 20=e%, 21=null, 35=m?, 36=em?, 37=h?, 38=eh?, 39=d?,
        // 40=ed?, 41=w?, 42=ew?, 43=mo?, 44=emo?, 51=%?, 52=e%? and
        // 53=null.
        public int DurationFormat { get; set; }

        // The default work unit format. Values are: 1=m, 2=h, 3=d,
        // 4=w, 5=mo.
        public int WorkFormat { get; set; }

        // Whether or not actual costs are editable. default: false
        public bool EditableActualCosts { get; set; }

        // Whether tasks honour their constraint dates. default: true
        public bool HonorConstraints { get; set; }

        // The default method for calculating earned value. Values
        // are: 0=Percent Complete, 1=Physical Percent Complete.
        public int EarnedValueMethod { get; set; }

        // Whether to calculate subtasks as summary tasks. default:
        // true
        public bool InsertedProjectsLikeSummary { get; set; }

        // Whether multiple critical paths are calculated. default:
        // false
        public bool MultipleCriticalPaths { get; set; }

        // Whether new tasks are effort driven. default: true
        public bool NewTasksEffortDriven { get; set; }

        // Whether to show the estimated duration by default. default:
        // true
        public bool NewTasksEstimated { get; set; }

        // Whether in-progress tasks can be split. default: true
        public bool SplitsInProgressTasks { get; set; }

        // Whether actual costs are spread to the status
        // date. default: true
        public bool SpreadActualCost { get; set; }

        // Whether percent complete is spread to the status
        // date. default: false
        public bool SpreadPercentComplete { get; set; }

        // Whether updates to tasks update resources. default: N/A
        public bool TaskUpdatesResource { get; set; }

        // Specifies whether to use fiscal year numbering. default:
        // N/A
        public bool FiscalYearStart { get; set; }

        // Start day of the week. Values are: 0=Sunday, 1=Monday,
        // 2=Tuesday, 3=Wednesday, 4=Thursday, 5=Friday, 6=Saturday.
        public int WeekStartDay { get; set; }

        // Specifies whether the end of completed portions of tasks
        // scheduled to begin after the status date but begun early
        // should be moved back to the status date. default: false
        public bool MoveCompletedEndsBack { get; set; }

        // Specifies whether the beginning of remaining portions of
        // tasks scheduled to begin after the status date but begun
        // early should be moved back to the status date. default:
        // false
        public bool MoveRemainingStartsBack { get; set; }

        // Specifies whether the beginning of remaining portions of
        // tasks scheduled to have begun late should be moved up to
        // the status date. default: false
        public bool MoveRemainingStartsForward { get; set; }

        // Specifies whether the end of completed portions of tasks
        // scheduled to have been completed before the status date but
        // begun late should be moved up to the status date. default:
        // false
        public bool MoveCompletedEndsForward { get; set; }

        // The specific baseline used to calculate Variance values.
        // Values are: 0=Baseline, 1=Baseline 1, 2=Baseline 2,
        // 3=Baseline 3, 4=Baseline 4, 5=Baseline 5, 6=Baseline 6,
        // 7=Baseline 7, 8=Baseline 8, 9=Baseline 9, 10=Baseline 10.
        public int BaselineForEarnedValue { get; set; }

        // Whether to automatically add new resources to the resource
        // pool. default: true
        public bool AutoAddNewResourcesAndTasks { get; set; }

        // Date used for calculation and reporting.
        public DateTime? StatusDate { get; set; }

        // The system date that the XML was generated.
        public DateTime? CurrentDate { get; set; }

        // Whether the project was created by a Project Server user as
        // opposed to an NT user. default: N/A
        public bool MicrosoftProjectServerURL { get; set; }

        // Whether to autolink inserted or moved tasks. default: N/A
        public bool Autolink { get; set; }

        // The default start date for new tasks.  Values are:
        // 0=Project Start Date, 1=Current Date.
        public int NewTaskStartDate { get; set; }

        // The default earned value method for tasks.  Values are:
        // 0=Percent Complete, 1=Physical Percent Complete.
        public int DefaultTaskEVMethod { get; set; }

        // Whether the project XML was edited. default: N/A
        public bool ProjectExternallyEdited { get; set; }

        // Date used for calculation and reporting.
        public DateTime? ExtendedCreationDate { get; set; }

        // Whether all actual work has been synchronized with the
        // project. default: N/A
        public bool ActualsInSync { get; set; }

        // Whether to remove all file properties on save. default: N/A
        public bool RemoveFileProperties { get; set; }

        // Whether the project is an administrative project. default: N/A
        public bool AdminProject { get; set; }

        // <xsd:element name="OutlineCodes" minOccurs="0"> . . . </xsd:element>
        // <xsd:element name="WBSMasks" minOccurs="0"> . . . </xsd:element>
        // <xsd:element name="ExtendedAttributes" minOccurs="0"> . . . </xsd:element>
        public List<ExtendedAttribute> ExtendedAttributes { get; set; }

        // <xsd:element name="Calendars" minOccurs="0"> . . . </xsd:element>
        public List<CalendarDto> Calendars { get; set; }

        // <xsd:element name="Tasks" minOccurs="0"> . . . </xsd:element>
        public List<Task> Tasks { get; set; }

        // <xsd:element name="Resources" minOccurs="0"> . . . </xsd:element>
        public List<Resource> Resources { get; set; }

        // <xsd:element name="Assignments" minOccurs="0"> . . . </xsd:element>
        public List<Assignment> Assignments { get; set; }

    }
}
