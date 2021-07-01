namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddTaskBaselineCommand : TaskBaselineCommand
    {
        public AddTaskBaselineCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}