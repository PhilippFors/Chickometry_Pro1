namespace Interaction.Interactables
{
    public class TestInteractable : BaseInteractable
    {
        public override void OnInteract()
        {
            print($"An interaction happend with {name}");
        }
    }
}
