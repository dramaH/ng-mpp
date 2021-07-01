namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateAssignmentCommand : AssignmentCommand
    {
        public UpdateAssignmentCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}