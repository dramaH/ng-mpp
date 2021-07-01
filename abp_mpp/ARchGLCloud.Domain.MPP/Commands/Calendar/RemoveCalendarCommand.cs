namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveCalendarCommand : CalendarCommand
    {
        public RemoveCalendarCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}