namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveTaskCommand : TaskCommand
    {
        public RemoveTaskCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}