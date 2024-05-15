using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokeInfoPanelController : MonoBehaviour
{
    [SerializeField]
    private PokeAPIController PokemonListController;
    [SerializeField]
    private TextMeshProUGUI pokemonName, pokemonIndex, type1, type2, healthPoints, speed, attack, defence, specialAttack, specialDefence, height, weight;
    [SerializeField]
    private GameObject pokemonListPanel , pokemonInfoPanel;
    [SerializeField]
    private RawImage image;

    public void OnButtonclick(GameObject button)
    {
        //Asign variable from pokemon object assigned to button
        var panel = button.GetComponent<CreateParent>().Pokemon;

        pokemonIndex.text = "# " + panel.index.ToString();
        image.texture = panel.image;
        pokemonName.text = panel.name;
        type1.text = panel.Types[0];
        if(panel.Types.Count()>=2)
        {
        type2.text = panel.Types[1];
        }

        else
        {
        type2.text = "";
        }

        healthPoints.text = panel.healthPoints.ToString();
        speed.text = panel.speed.ToString();
        attack.text = panel.attack.ToString();
        defence.text = panel.defence.ToString();
        specialAttack.text = panel.specialAttack.ToString();
        specialDefence.text = panel.specialDefence.ToString();
        height.text = panel.height.ToString();
        weight.text = panel.weight.ToString();

    }

    //Switch between Panels
    public void GoToPokemonInfoPanel()
    {
        pokemonListPanel.SetActive(false);
        pokemonInfoPanel.SetActive(true);
        
    }
}
