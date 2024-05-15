using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas, gridCell, pokemonListContent, pokemonListController, parrent;
    private int pokeNum = 0;
    
    //Method for creating and setting grid cells
    public void GridCreator(int cellNumber)
    {
        Pokemon[] Pokemons = pokemonListController.GetComponent<PokeAPIController>().Pokemons;
       
        //Create grid
        for (int x = cellNumber; x > 0; x--, pokeNum++)
        {                
                //Create grid cell
                GameObject TemporaryCell = Instantiate(gridCell, new Vector3(1,1,0),Quaternion.identity);

                //Visuals of grid cell
                TemporaryCell.SetActive(true);
                TemporaryCell.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "# " + Pokemons[pokeNum].index.ToString();
                TemporaryCell.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Pokemons[pokeNum].name;
                TemporaryCell.transform.GetChild(1).gameObject.GetComponent<RawImage>().texture = Pokemons[pokeNum].image;
                TemporaryCell.transform.GetChild(1).gameObject.GetComponent<RawImage>().texture.filterMode = FilterMode.Point;
                
                //Change Scale of grid cell
                TemporaryCell.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                
                //Assign pokemon object to specific grid cell
                TemporaryCell.GetComponent<CreateParent>().Pokemon = Pokemons[pokeNum];
        }
    }
}
