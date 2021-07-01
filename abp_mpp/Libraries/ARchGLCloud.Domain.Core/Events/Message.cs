using System;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace ARchGLCloud.Domain.Core.Events
{
    public abstract class Message : IRequest
    {
        public Guid AggregateId { get; protected set; }

        [Column("Action")]
        public string MessageType { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}