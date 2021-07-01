namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveTaskExtendedAttributeCommand : TaskExtendedAttributeCommand
    {
        public RemoveTaskExtendedAttributeCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}