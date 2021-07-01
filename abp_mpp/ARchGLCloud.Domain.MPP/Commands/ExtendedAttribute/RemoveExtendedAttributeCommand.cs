namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveExtendedAttributeCommand : ExtendedAttributeCommand
    {
        public RemoveExtendedAttributeCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}