using System;
using ARchGLCloud.Domain.MPP.Commands;
using FluentValidation;

namespace ARchGLCloud.Domain.MPP.Validations
{
    public abstract class TaskValidation<T> : AbstractValidator<T> where T : TaskCommand
    {
        
    }
}

