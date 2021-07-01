namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddCalendarCommand : CalendarCommand
    {
        public AddCalendarCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}