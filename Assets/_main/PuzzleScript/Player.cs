using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public List<int> acquiredPieces;
    
    bool hasImagePiece = false;

    public float speed = 5; 
    // Start is called before the first frame update
    void Start()
    {
        acquiredPieces = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        
            
        transform.Translate(new Vector3(horizontal,0, vertical).normalized * Time.deltaTime * speed);      
        
    
        
    }

    void OnTriggerEnter(Collider other)
    {
        //get class puzzle 
        Puzzle puzzle = GameObject.Find("Puzzle").GetComponent<Puzzle>();

        

        int numberOfImagePieces = puzzle.numberOfPiecesPerRow * puzzle.numberOfPiecesPerRow;
        
        //here we pick up image by disabling it object
        for (int i = 0; i < numberOfImagePieces ; i++)
        {
            if (other.gameObject.name == "no " + i)
            {
                if (puzzle.spriteObjects[i].transform.parent == null)
                {
                    acquiredPieces.Add(i);
                    other.gameObject.SetActive(false);
                }


                
                
            }
            
        }


        if (other.gameObject.name == "BoardFor9Pieces")
        {
            

            for (int x = 0; x < acquiredPieces.Count; x++)
            {   
                
                int value = puzzle.savedBlockNumberAndPosition.ElementAt(x).Value;
                
                Debug.Log(value);
                
                Vector3 key = puzzle.savedBlockNumberAndPosition.ElementAt(x).Key;
                
                if (acquiredPieces.Contains(value))
                {
                    puzzle.spriteObjects[value].SetActive(true);
                    puzzle.spriteObjects[value].transform.parent = puzzle.transform;
                    puzzle.spriteObjects[value].transform.localPosition = key;

                    if (puzzle.transform.childCount == numberOfImagePieces)
                    {
                        print("!!!!PUZZLE SOLVED!!!!!");
                        
                    }


                }
            }
            
        }




    }
    
    
}
