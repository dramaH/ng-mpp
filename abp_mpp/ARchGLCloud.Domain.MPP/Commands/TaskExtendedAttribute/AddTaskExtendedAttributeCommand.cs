namespace ARchGLCloud.Domain.MPP.Commands
{
    public class AddTaskExtendedAttributeCommand : TaskExtendedAttributeCommand
    {
        public AddTaskExtendedAttributeCommand()
        {
        }

        public override bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}