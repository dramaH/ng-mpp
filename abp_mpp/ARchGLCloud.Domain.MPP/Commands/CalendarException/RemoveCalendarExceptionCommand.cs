namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveCalendarExceptionCommand : CalendarExceptionCommand
    {
        public RemoveCalendarExceptionCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}