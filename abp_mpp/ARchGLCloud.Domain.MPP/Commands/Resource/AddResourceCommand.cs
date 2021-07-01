namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddResourceCommand : ResourceCommand
    {
        public AddResourceCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}