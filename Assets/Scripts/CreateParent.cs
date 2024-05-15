using UnityEngine;

public class CreateParent : MonoBehaviour
{
    [SerializeField]
    private GameObject parent, pokeInfoPanelController, button;
    [SerializeField]
    public Pokemon Pokemon;

    //Assigne cell to PokemonListContent as child
    void Awake()
    {
        transform.SetParent(parent.transform);
    }

    //Button interactions
    public void OnButtonClick()
    {
        pokeInfoPanelController.GetComponent<PokeInfoPanelController>().GoToPokemonInfoPanel();
        pokeInfoPanelController.GetComponent<PokeInfoPanelController>().OnButtonclick(button);
    }
}
