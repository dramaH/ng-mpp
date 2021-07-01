using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace ARchGLCloud.Domain.Core.Events
{
    public abstract class Event : Message, INotification
    {
        [Column("CreationDate")]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}