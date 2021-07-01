namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveAssignmentCommand : AssignmentCommand
    {
        public RemoveAssignmentCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}