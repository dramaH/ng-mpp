namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateExtendedAttributeCommand : ExtendedAttributeCommand
    {
        public UpdateExtendedAttributeCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}