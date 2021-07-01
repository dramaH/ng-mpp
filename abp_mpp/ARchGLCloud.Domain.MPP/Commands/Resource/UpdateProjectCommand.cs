namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateResourceCommand : ResourceCommand
    {
        public UpdateResourceCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}