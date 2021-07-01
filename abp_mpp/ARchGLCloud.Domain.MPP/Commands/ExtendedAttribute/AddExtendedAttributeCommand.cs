namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddExtendedAttributeCommand : ExtendedAttributeCommand
    {
        public AddExtendedAttributeCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}