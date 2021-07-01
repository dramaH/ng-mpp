namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveTaskBaselineCommand : TaskBaselineCommand
    {
        public RemoveTaskBaselineCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}