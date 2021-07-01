namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddProjectCommand : ProjectCommand
    {
        public AddProjectCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}