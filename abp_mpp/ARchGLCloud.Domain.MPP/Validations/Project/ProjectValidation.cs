using System;
using ARchGLCloud.Domain.MPP.Commands;
using FluentValidation;

namespace ARchGLCloud.Domain.MPP.Validations
{
    public abstract class ProjectValidation<T> : AbstractValidator<T> where T : ProjectCommand
    {
        
    }
}

