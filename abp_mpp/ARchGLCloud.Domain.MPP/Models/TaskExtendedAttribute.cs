using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   The value of an extended attribute.  Two pieces of data are
    ///   needed: a pointer back to the extended attribute table that
    ///   is specified either by the unique ID or the Field ID, and
    ///   the value that is specified either with the value or a
    ///   pointer back to the value list.
    /// </summary>
    [Table("TaskExtendedAttributes", Schema = "mpp")]
    public class TaskExtendedAttribute : MppAggregateRoot<Guid>
    {
        public TaskExtendedAttribute(): base(Guid.NewGuid()) { }
        public TaskExtendedAttribute(Guid id) : base(id) { }

        // The project ID (PID) of the custom field.
        public string FieldID { get; set; }

        // The actual value of the extended attribute.
        public string Value { get; set; }

        // The GUID of the value in the extended attribute lookup
        // table.
        public string ValueGUID { get; set; }

        // The format for expressing the bulk duration.  Values are:
        // 3=m, 4=em, 5=h, 6=eh, 7=d, 8=ed, 9=w, 10=ew, 11=mo, 12=emo,
        // 19=%, 20=e%, 21=null, 35=m?, 36=em?, 37=h?, 38=eh?, 39=d?,
        // 40=ed?, 41=w?, 42=ew?, 43=mo?, 44=emo?, 51=%?, 52=e%? and
        // 53=null.
        public int DurationFormat { get; set; }
    }
}