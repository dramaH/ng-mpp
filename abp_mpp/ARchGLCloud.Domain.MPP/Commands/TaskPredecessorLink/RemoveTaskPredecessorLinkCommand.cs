namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveTaskPredecessorLinkCommand : TaskPredecessorLinkCommand
    {
        public RemoveTaskPredecessorLinkCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}