namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateTaskExtendedAttributeCommand : TaskExtendedAttributeCommand
    {
        public UpdateTaskExtendedAttributeCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}