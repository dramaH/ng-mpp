namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddAssignmentCommand : AssignmentCommand
    {
        public AddAssignmentCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}