using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARchGLCloud.Domain.MPP.Models
{
    /// <summary>
    ///   When values of extended attributes are specified as
    ///   properties of elements in the schema, they can be specified
    ///   either by values or by references to the values contained in
    ///   this list.  Applications can assume ordering of the list by
    ///   ordering specified here.
    ///   The values that make up the value list.
    /// </summary>
    [Table("ExtendedAttributeValues", Schema = "mpp")]
    public class ExtendedAttributeValue : MppAggregateRoot<Guid>
    {
        public ExtendedAttributeValue() : base(Guid.NewGuid()) { }
        public ExtendedAttributeValue(Guid id) : base(id) { }

        // Unique ID of value across the project.
        public int _ID { get; set; }

        // The actual value. maxLength: N/A
        public string Value { get; set; }

        // The description of the value in the list. maxLength: N/A
        public string Description { get; set; }

        // Phonetic information for custom field names. maxLength: N/A
        public string Phonetic { get; set; }
    }
}
