namespace ARchGLCloud.Domain.MPP.Commands
{
    public class UpdateTaskCommand : TaskCommand
    {
        public UpdateTaskCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}