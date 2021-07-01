using System;
using ARchGLCloud.Domain.MPP.Commands;
using FluentValidation;

namespace ARchGLCloud.Domain.MPP.Validations
{
    public abstract class TaskPredecessorLinkValidation<T> : AbstractValidator<T> where T : TaskPredecessorLinkCommand
    {
        
    }
}

