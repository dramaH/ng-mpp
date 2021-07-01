namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateCalendarExceptionCommand : CalendarExceptionCommand
    {
        public UpdateCalendarExceptionCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}