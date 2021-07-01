namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddTaskPredecessorLinkCommand : TaskPredecessorLinkCommand
    {
        public AddTaskPredecessorLinkCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}