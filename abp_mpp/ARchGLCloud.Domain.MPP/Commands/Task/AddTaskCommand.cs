namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddTaskCommand : TaskCommand
    {
        public AddTaskCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}