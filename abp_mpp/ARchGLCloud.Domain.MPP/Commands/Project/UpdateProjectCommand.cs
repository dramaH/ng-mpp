namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateProjectCommand : ProjectCommand
    {
        public UpdateProjectCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}