namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddCalendarExceptionCommand : CalendarExceptionCommand
    {
        public AddCalendarExceptionCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}