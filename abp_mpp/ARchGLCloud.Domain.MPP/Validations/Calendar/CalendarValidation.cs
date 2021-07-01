using System;
using ARchGLCloud.Domain.MPP.Commands;
using FluentValidation;

namespace ARchGLCloud.Domain.MPP.Validations
{
    public abstract class CalendarValidation<T> : AbstractValidator<T> where T : CalendarCommand
    {
        
    }
}

