using ARchGLCloud.Domain.Core.Commands;
using System;

namespace ARchGLCloud.Domain.MPP.Commands
{
    public abstract class ExtendedAttributeCommand : Command
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public string FieldID { get; set; }

        // The name of the custom field. maxLength: N/A
        public string FieldName { get; set; }

        // The custom field type. Values are: 0=Cost, 1=Date,
        // 2=Duration, 3=Finish, 4=Flag, 5=Number, 6=Start, 7=Text.
        public int CFType { get; set; }

        // The GUID of the custom field. maxLength: N/A
        public string _Guid { get; set; }

        // Specifies whether the extended attribute is associated with
        // a task, a resource, or an assignment. Values are: 20=Task,
        // 21=Resource, 22=Calendar, 23=Assignment.
        public int ElemType { get; set; }

        // Specifies the maximum number of values you can set in a
        // picklist.
        public int MaxMultiValues { get; set; }

        // Specifies whether the custom field is
        // user-defined. default: N/A
        public bool UserDef { get; set; }

        // The alias of the custom field. maxLength: 50
        public string Alias { get; set; }

        // The secondary PID of the custom field. maxLength: N/A
        public string SecondaryPID { get; set; }

        // Specifies whether automatic rolldown to assignments is
        // enabled. default: N/A
        public bool AutoRollDown { get; set; }

        // Specifies the GUID of the default lookup table
        // entry. maxLength: N/A
        public string DefaultGuid { get; set; }

        // The GUID of the lookup table associated with the custom
        // field.
        public string Ltuid { get; set; }

        // The phonetic pronunciation of the alias of the custom
        // field. maxLength: 50
        public string PhoneticAlias { get; set; }

        // How rollups are calculated. Values are: 0=Maximum (OR for
        // flag fields), 1=Minimum (AND for flag fields), 2=Count all,
        // 3=Sum, 4=Average, 5=Average First Sublevel, 6=Count First
        // Sublevel, 7=Count Nonsummaries.
        public int RollupType { get; set; }

        // Whether rollups are calculated for task and group summary
        // rows. Values are: 0=None, 1=Rollup, 2=Calculation.
        public int CalculationType { get; set; }

        // The formula that Microsoft Project uses to populate the
        // custom task field. maxLength: N/A
        public string Formula { get; set; }

        // If RestrictValues=True then only values in the list are
        // allowed in the file. default: N/A
        public bool RestrictValues { get; set; }

        // How value lists are sorted. Values are: 0=Descending,
        // 1=Ascending.
        public int ValuelistSortOrder { get; set; }

        // If AppendNewValues=True then any new values added in a
        // project are automatically appended to the list. default:
        // N/A
        public bool AppendNewValues { get; set; }

        // This points to the default value in the list.  Not present
        // if no default is set. maxLength: N/A
        public string Default { get; set; }
    }
}
