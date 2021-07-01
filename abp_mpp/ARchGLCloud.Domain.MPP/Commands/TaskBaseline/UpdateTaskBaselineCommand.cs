namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateTaskBaselineCommand : TaskBaselineCommand
    {
        public UpdateTaskBaselineCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}