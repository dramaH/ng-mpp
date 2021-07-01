namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddCalendarWeekDayCommand : CalendarWeekDayCommand
    {
        public AddCalendarWeekDayCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}