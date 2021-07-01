namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateCalendarCommand : CalendarCommand
    {
        public UpdateCalendarCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}