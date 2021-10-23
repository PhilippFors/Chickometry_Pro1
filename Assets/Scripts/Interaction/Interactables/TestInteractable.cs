namespace Interaction.Interactables
{
    public class TestInteractable : BaseInteractable
    {
        public override void OnUse()
        {
            print($"An interaction happend with {name}");
        }
    }
}
