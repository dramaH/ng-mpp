namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveResourceCommand : ResourceCommand
    {
        public RemoveResourceCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}