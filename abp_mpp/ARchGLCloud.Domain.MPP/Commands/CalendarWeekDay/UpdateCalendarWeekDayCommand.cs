namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateCalendarWeekDayCommand : CalendarWeekDayCommand
    {
        public UpdateCalendarWeekDayCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}