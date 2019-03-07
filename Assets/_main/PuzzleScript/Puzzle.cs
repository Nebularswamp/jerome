using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Bobby's logic for this:
// pieces scatter around the world, gather them up
// when you place the pieces, it get on the board automatically

/**
 * Problem incounter 
 *
 * - when assign to child object, it won't move with the parent while position are lock
 * 
 *
 * TODO: detect when we are near the Board
 * TODO: build a test condition if we have one or two pieces to place
 * TODO: test the pieces while scatter around
 * TODO: in the player script, player must be able to hold value to test witch our dictionary values
 * TODO: test, test, test.....
 * TODO: test witch original game scene
 *
 * interesting features(just a though...):
 * board size increase to match image pieces  
 * 
 */

public class Puzzle : MonoBehaviour
{
    //texture file or image to add 
    public Texture2D image;
    public int numberOfPiecesPerRow = 3;//set the number of rows and colum you wish to cut

    [SerializeField]
    private Sprite[,] sprites;//to turn your image into a sprite multidimensional array
    private List<GameObject> gameObjects; //assign my sprite to gameObject 

    private List<Vector3> savedPosition;
    
    //dictionary to save final position and the piece number
    private Dictionary<Vector3,int> savedBlockNumberAndPosition;

    //Testing Value for now
    private bool hasPieces = true;
    private bool nearTheBoard = true ;//I need to detect if I'm near the board if so place the image pieces


    //a test to display the image as whole
    public Sprite test;



    // Start is called before the first frame update
    void Start()
    {
        //testing...
        savedPosition = new List<Vector3>();
        
        //initiale a dictionnary
        savedBlockNumberAndPosition = new Dictionary<Vector3, int>();//testing...

        //main things
        sprites = new Sprite[numberOfPiecesPerRow,numberOfPiecesPerRow]; //an array of pieces
        gameObjects = new List<GameObject>(); //gameObject to be assign(your image pieces witch will be sprite)
        splitTexture(image,numberOfPiecesPerRow);//a custom method to split your image into pieces and assign them to sprite gameObjects

        
        //testHoleImage
        test = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        GameObject testObject = new GameObject("test");
        SpriteRenderer testRenderer = testObject.AddComponent<SpriteRenderer>();
        testRenderer.sprite = test;
        testObject.transform.position = new Vector3(-2,1,0);

        
    }

    

    void splitTexture(Texture2D source, float dividedPiecesWanted) {

        //verify if got an image of the right size //otherwise show error
        if (!(source.width % 2 == 0 && source.height % 2 == 0 && source.height == source.width) ) {

            throw new Exception("image size is not power of two on both sides eg. 32x32, 128x128, 256x256 etc "); 

        }

        // a single image pieces calculation
        float singleImagePiece = source.width / dividedPiecesWanted;

        int count = 0;//keep count of gameObject and serve as value for our Dictionary

        //multidimentionnal array, looping every values //GetLength(0) -> [i,] looping i only
        for (int i = 0; i < sprites.GetLength(0); i++) {
           
            print("our loop0"+ i);//debug,just ignore it

            
            for (int j = 0; j < sprites.GetLength(1); j++) {//GetLength(1) -> [,j] looping j only

                float x, y;
                x = i;
                y = j;

                // add names while creating gameObject for our Sprite
                gameObjects.Add(new GameObject("no " +  count ));

                //this assign sprites by creating them, "source"-> image : "Rect" -> part of the image we want cut : "Vector2" -> having the pivot point at center 
                sprites[i,j] = Sprite.Create(source, new Rect( x * singleImagePiece, y * singleImagePiece, singleImagePiece, singleImagePiece), new Vector2(0.5f, 0.5f));
                
                print("ImageSize" + "[ "+ (singleImagePiece)+" ]"); //Debug for single image piece
                
                //add Sprite component to our newly created gameObjects
                gameObjects[count].AddComponent<SpriteRenderer>();
                
                //assign our sprites to the component
                gameObjects[count].GetComponent<SpriteRenderer>().sprite = sprites[i,j];
                
                //set it as a child of the "Puzzle" GameObject 
                gameObjects[count].transform.parent = transform;
               
                
                //save position of our gameObject //for now testing 
                savedPosition.Add(new Vector3(i,j));
                
                
                //saved position and block number for later use
                savedBlockNumberAndPosition.Add(new Vector3(i,j), count );
                
                
                //set pieces to their original position // align the pieces showing the image in finished state 
                //gameObjects[count].transform.position = new Vector3(i,j);
                
                
                
                // increment count for our gameObjects
                count++;
                
                
              

            }



        }




    }

    // Update is called once per frame
    void Update()
    {

        //THE PLAYER WOULD NEED TO TEST IT VALUES TO KNOW WITCH PIECES IT HAS THEN LOCK IT IN THE BOARD
        for (int i = 0; i < savedPosition.Count; i++)
        {
            //UNFINISHED need to think this through...
            // need to add in the child view here...
            
            //if (hasPieces && nearTheBoard)
            //{
                //get the saved position 
                Vector3 savedPosition = savedBlockNumberAndPosition.ElementAt(i).Key;
                
                
                //GOOD FOR DEBUGGING VECTOR3 VALUES AND IMAGINING THE FINISHED STATE
                //DISABLE IT TO HAVE THE PIECES UNORGANISED
                gameObjects[i].transform.position = savedPosition;//lock Position of the image at finish state
            //}

            
        }
        
        
        
        
        
        
    }

   

    //UNFINISHED need to think through it more...
    //TEST Method to detect pieces collected and verify there value
    void gotPiece()
    {
        switch (savedBlockNumberAndPosition.ElementAt(1).Value)//dictonnary number
        {
            case 0 :
                print("no good");
                break;
            case 1 :
                print("no good");
                break;
            case 2:
                print("good");
                break;
                
                
                
            
            
        }

      
    }

}
