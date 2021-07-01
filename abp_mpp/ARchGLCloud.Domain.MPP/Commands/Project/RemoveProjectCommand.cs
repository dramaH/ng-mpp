namespace ARchGLCloud.Domain.MPP.Commands
{
    public class RemoveProjectCommand : ProjectCommand
    {
        public RemoveProjectCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}