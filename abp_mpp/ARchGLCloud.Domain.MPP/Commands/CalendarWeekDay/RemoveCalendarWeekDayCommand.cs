namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveCalendarWeekDayCommand : CalendarWeekDayCommand
    {
        public RemoveCalendarWeekDayCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}