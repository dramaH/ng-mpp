namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateTaskPredecessorLinkCommand : TaskPredecessorLinkCommand
    {
        public UpdateTaskPredecessorLinkCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}