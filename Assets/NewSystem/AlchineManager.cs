using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlchineManager : MonoBehaviour
{
    public MainLogic mainLogicScript;
    public alcanScript alcanScript;
    public logicManager logicManager;
    public int animationSpeed;
   
    
    private void Start()
    {
        mainLogicScript = GameObject.FindGameObjectWithTag("MainLogic").GetComponent<MainLogic>();
        logicManager = GameObject.FindGameObjectWithTag("logicManager").GetComponent<logicManager>();
        alcanScript = mainLogicScript.AlcanScript;
        animationSpeed = 6;
        mainLogicScript.hasSubstitutionOccuredAlchine = false;
    }
    [ContextMenu("Aditie Hidrogen")]
    public void AditieHidrogenPartial()
    {

        
        AdditionReactionAlchine(mainLogicScript.hidrogen,mainLogicScript.hidrogen, animationSpeed);
    }
    public void AditieHidrogenTotala()
    {
        Full2AdditionReactionAlchine(mainLogicScript.hidrogen,mainLogicScript.hidrogen,mainLogicScript.hidrogen,mainLogicScript.hidrogen);
    }
    public void AditieHalogen()
    {
        AdditionReactionAlchine(mainLogicScript.Clor2,mainLogicScript.Clor2, animationSpeed);
    }
    public void AditieHalogenTotala()
    {
        Full2AdditionReactionAlchine(mainLogicScript.Clor2,mainLogicScript.Clor2,mainLogicScript.Clor2,mainLogicScript.Clor2);
    }
    public void AditieHidracidPartiala()
    {
        AdditionReactionAlchine(mainLogicScript.hidrogen,mainLogicScript.Clor2,animationSpeed);
    }
    public void AditieHidracidTotal()
    {
        Full2AdditionReactionAlchine(mainLogicScript.Clor2, mainLogicScript.hidrogen,mainLogicScript.Clor2, mainLogicScript.hidrogen);
    }


    public void AdditionReactionAlchine(GameObject elementToAdd, GameObject elementToAdd2, int speed)
    {
        Vector3 initialSpawnPosition = new Vector3(1.11000001f, 9.29999995f, 0.0900000036f);
        List<GameObject> carbonList = new List<GameObject>();
        carbonList = gameObject.GetComponent<spawnAlchina>().carbonList;
        GameObject firstConection = logicManager.CarbonConection[0];
        GameObject secondConection = Instantiate(firstConection);
        mainLogicScript.MakeAlcanParent(new List<GameObject> { secondConection });
        mainLogicScript.MoveAndRotateToObject(firstConection, firstConection.transform.position + new Vector3(-0.526f, 0.396f, -0.394f), Quaternion.Euler(firstConection.transform.rotation.x, 90, -54), speed);
        mainLogicScript.MoveAndRotateToObject(secondConection, new Vector3(1.319f, -0.009f, -0.394f), Quaternion.Euler(0, 90, -123), speed);
        if (mainLogicScript.carbonCount == 2)
        {
            GameObject firstParent = new GameObject();
            GameObject firstHidrogen = mainLogicScript.spawnAlchinaScript.carbonList[0].transform.GetChild(0).gameObject;
            GameObject firstConectionHidrogen = mainLogicScript.spawnAlchinaScript.carbonList[0].transform.GetChild(1).gameObject;
            firstHidrogen.transform.parent = firstParent.transform;
            firstConectionHidrogen.transform.parent = firstParent.transform;
            mainLogicScript.MakeAlcanParent(new List<GameObject> { firstParent });
            //mainLogicScript.MoveAndRotateToObject(firstParent, new Vector3(0.000f, 1.635f, 1.000f), Quaternion.Euler(-0, -0, -0), speed);
            mainLogicScript.MoveAndRotateToObject(firstParent, firstParent.transform.position, Quaternion.Euler(0, 90, 0), speed);
            ///
            GameObject secondParent = new GameObject();
            secondParent.transform.position = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.position;
            // Access the second child of the first carbonList element
            GameObject secondHydrogen = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.GetChild(0).gameObject;
            GameObject secondConnectionHydrogen = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.GetChild(1).gameObject;

            // Set the parent of these objects to the new second parent
            secondHydrogen.transform.parent = secondParent.transform;
            secondConnectionHydrogen.transform.parent = secondParent.transform;

            // Make the second parent the main parent for the given objects
            mainLogicScript.MakeAlcanParent(new List<GameObject> { secondParent });

            // Move and rotate the second parent
            mainLogicScript.MoveAndRotateToObject(secondParent, secondParent.transform.position, Quaternion.Euler(0, -90, 0), speed);

            ////
            ///
            GameObject replaceElement1 = Instantiate(elementToAdd, initialSpawnPosition, Quaternion.identity);
            GameObject replaceElement2;
            replaceElement2 = Instantiate(elementToAdd2, initialSpawnPosition, Quaternion.identity);

            mainLogicScript.MoveAndRotateToObject(replaceElement1, new Vector3(1.266f, -0.623f, -1.013f), Quaternion.Euler(-0, 0, 0), speed);


            mainLogicScript.MoveAndRotateToObject(replaceElement2, new Vector3(-0.000f, 1.635f, -1.068f), Quaternion.Euler(-0, -0, -0), speed);
            mainLogicScript.MakeAlcanParent(new List<GameObject> { replaceElement1, replaceElement2 });

        }
    }

   

    public void Full2AdditionReactionAlchine(GameObject element1,GameObject element2,GameObject element3, GameObject element4)
    {
        int speed = animationSpeed;
        Vector3 initialSpawnPosition = new Vector3(1.11000001f, 3.29999995f, 0.0900000036f);
        List<GameObject> carbonList = new List<GameObject>();
        carbonList = gameObject.GetComponent<spawnAlchina>().carbonList;
        GameObject firstConection = logicManager.CarbonConection[0];
        GameObject firstConection_2 = logicManager.CarbonConection[1];
        GameObject secondConection = Instantiate(firstConection);
        GameObject secondConection_2 = Instantiate(firstConection_2);
        mainLogicScript.MakeAlcanParent(new List<GameObject> { secondConection, secondConection_2 });

        // Start the coroutine to handle all the MoveAndRotateToObject calls
        StartCoroutine(MoveAndRotateCoroutineFull2AdditionReactionAlchine(speed, firstConection, secondConection, secondConection_2, firstConection_2,element1,element2,element3,element4));
    }

    private IEnumerator MoveAndRotateCoroutineFull2AdditionReactionAlchine(int speed, GameObject firstConection, GameObject secondConection, GameObject secondConection_2, GameObject firstConection_2, GameObject element1,GameObject element2,GameObject element3,GameObject element4)
    {
        float delaySpeed = animationSpeed / 5;
        // First MoveAndRotateToObject call
        mainLogicScript.MoveAndRotateToObject(firstConection, firstConection.transform.position + new Vector3(-0.526f, 0.396f, -0.394f), Quaternion.Euler(firstConection.transform.rotation.x, 90, -54), speed);
        yield return new WaitForSeconds(delaySpeed); // Delay between each action

        // Second MoveAndRotateToObject call
        mainLogicScript.MoveAndRotateToObject(secondConection, new Vector3(1.319f, -0.009f, -0.394f), Quaternion.Euler(0, 90, -123), speed);
        yield return new WaitForSeconds(delaySpeed);

        if (mainLogicScript.carbonCount == 2)
        {
            GameObject firstParent = new GameObject();
            GameObject firstHidrogen = mainLogicScript.spawnAlchinaScript.carbonList[0].transform.GetChild(0).gameObject;
            GameObject firstConectionHidrogen = mainLogicScript.spawnAlchinaScript.carbonList[0].transform.GetChild(1).gameObject;
            firstHidrogen.transform.parent = firstParent.transform;
            firstConectionHidrogen.transform.parent = firstParent.transform;
            mainLogicScript.MakeAlcanParent(new List<GameObject> { firstParent });

            // Move and rotate firstParent
            mainLogicScript.MoveAndRotateToObject(firstParent, firstParent.transform.position, Quaternion.Euler(0, 90, 0), speed);
            yield return new WaitForSeconds(delaySpeed);

            // Second parent setup
            GameObject secondParent = new GameObject();
            secondParent.transform.position = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.position;
            GameObject secondHydrogen = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.GetChild(0).gameObject;
            GameObject secondConnectionHydrogen = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.GetChild(1).gameObject;

            secondHydrogen.transform.parent = secondParent.transform;
            secondConnectionHydrogen.transform.parent = secondParent.transform;

            mainLogicScript.MakeAlcanParent(new List<GameObject> { secondParent });

            // Move and rotate secondParent
            mainLogicScript.MoveAndRotateToObject(secondParent, secondParent.transform.position, Quaternion.Euler(0, -90, 0), speed);
            yield return new WaitForSeconds(delaySpeed);

            // Move and rotate firstConection_2
            mainLogicScript.MoveAndRotateToObject(firstConection_2, new Vector3(1.64f, 0.119f, 0.0f), new Quaternion(0.49545985f, 0.86863095f, 0.0f, 0.0f), speed*0.6f);
            yield return new WaitForSeconds(delaySpeed);

            // Move and rotate secondConection_2
            mainLogicScript.MoveAndRotateToObject(secondConection_2, new Vector3(-0.3160000145435333f, 0.9300000071525574f, -0.1340000033378601f), new Quaternion(0.0f, 0.0f, -0.4539904296398163f, 0.891006588935852f), speed *0.6f);

            yield return new WaitForSeconds(delaySpeed);
            int height = 6;
            mainLogicScript.AddAtomsHigher(element1, new Vector3(-0.939999998f, 0.497000009f, -0.114f),height,animationSpeed/2);
            yield return new WaitForSeconds(delaySpeed);
            mainLogicScript.AddAtomsHigher(element2, new Vector3(-0.135000005f, 1.56299996f, -0.939999998f), height,animationSpeed/2);
            yield return new WaitForSeconds(delaySpeed);
            mainLogicScript.AddAtomsHigher(element3, new Vector3(1.31299996f, -0.612999976f, -1.079f), height,animationSpeed/2);
            yield return new WaitForSeconds(delaySpeed);
            mainLogicScript.AddAtomsHigher(element4, new Vector3(2.37700009f, 0.549000025f, 0.0130000003f), height,animationSpeed/2);
        }
    }

    public void WaterAdditionReactionAlchine()
    {
       StartCoroutine(WaterAdditionReactionAlchineCourutine());
    }

    IEnumerator WaterAdditionReactionAlchineCourutine()
    {
        GameObject elementToAdd2 = mainLogicScript.hidrogen;
        GameObject elementToAdd = mainLogicScript.OH;
        int speed = animationSpeed;
        Vector3 initialSpawnPosition = new Vector3(1.11000001f, 9.29999995f, 0.0900000036f);
        List<GameObject> carbonList = new List<GameObject>();
        carbonList = gameObject.GetComponent<spawnAlchina>().carbonList;
        GameObject firstConection = logicManager.CarbonConection[0];
        GameObject secondConection = Instantiate(firstConection);
        mainLogicScript.MakeAlcanParent(new List<GameObject> { secondConection });
        mainLogicScript.MoveAndRotateToObject(firstConection, firstConection.transform.position + new Vector3(-0.526f, 0.396f, -0.394f), Quaternion.Euler(firstConection.transform.rotation.x, 90, -54), speed);
        mainLogicScript.MoveAndRotateToObject(secondConection, new Vector3(1.319f, -0.009f, -0.394f), Quaternion.Euler(0, 90, -123), speed);
        if (mainLogicScript.carbonCount == 2)
        {
            GameObject firstParent = new GameObject();
            GameObject firstHidrogen = mainLogicScript.spawnAlchinaScript.carbonList[0].transform.GetChild(0).gameObject;
            GameObject firstConectionHidrogen = mainLogicScript.spawnAlchinaScript.carbonList[0].transform.GetChild(1).gameObject;
            firstHidrogen.transform.parent = firstParent.transform;
            firstConectionHidrogen.transform.parent = firstParent.transform;
            mainLogicScript.MakeAlcanParent(new List<GameObject> { firstParent });
            //mainLogicScript.MoveAndRotateToObject(firstParent, new Vector3(0.000f, 1.635f, 1.000f), Quaternion.Euler(-0, -0, -0), speed);
            mainLogicScript.MoveAndRotateToObject(firstParent, firstParent.transform.position, Quaternion.Euler(0, 90, 0), speed);
            ///
            GameObject secondParent = new GameObject();
            secondParent.transform.position = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.position;
            // Access the second child of the first carbonList element
            GameObject secondHydrogen = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.GetChild(0).gameObject;
            GameObject secondConnectionHydrogen = mainLogicScript.spawnAlchinaScript.carbonList[1].transform.GetChild(1).gameObject;

            // Set the parent of these objects to the new second parent
            secondHydrogen.transform.parent = secondParent.transform;
            secondConnectionHydrogen.transform.parent = secondParent.transform;

            // Make the second parent the main parent for the given objects
            mainLogicScript.MakeAlcanParent(new List<GameObject> { secondParent });


            // Move and rotate the second parent
            mainLogicScript.MoveAndRotateToObject(secondParent, secondParent.transform.position, Quaternion.Euler(0, -90, 0), speed);

            ////
            ///
            GameObject replaceElement1 = Instantiate(elementToAdd, initialSpawnPosition, Quaternion.identity);
            GameObject replaceElement2;
            replaceElement2 = Instantiate(elementToAdd2, initialSpawnPosition, Quaternion.identity);

            mainLogicScript.MoveAndRotateToObject(replaceElement1, new Vector3(1.266f, -0.623f, -1.013f), Quaternion.Euler(-0, 0, 0), speed);


            mainLogicScript.MoveAndRotateToObject(replaceElement2, new Vector3(-0.000f, 1.635f, -1.068f), Quaternion.Euler(-0, -0, -0), speed);
            mainLogicScript.MakeAlcanParent(new List<GameObject> { replaceElement1, replaceElement2 });

        }
        yield return new WaitForSeconds(animationSpeed);
        Destroy(logicManager.CarbonConection[2]);
        GameObject OH =mainLogicScript.GetChildByIdentifier(mainLogicScript.AlcanParent, "OH", false);
        //OH.SetActive(false);
        GameObject strayCarbon= mainLogicScript.TransferChildrenToNewObject(OH);
        
        //strayCarbon.SetActive(false);
        mainLogicScript.MoveAndRotateToObject(strayCarbon, new Vector3(-0.621999979f, 0.864000022f, -0.0939999968f),     Quaternion.Euler(3.33505334e-09f, 171.975006f, 359.459991f),animationSpeed);
        mainLogicScript.MakeAlcanParent(new List<GameObject> { strayCarbon});
        Vector3 thirdConectionPosition = secondConection.transform.position;

        GameObject thirdConection = Instantiate(secondConection, new Vector3(thirdConectionPosition.x, thirdConectionPosition.y-0.15f, thirdConectionPosition.z), secondConection.transform.rotation);
        mainLogicScript.MakeAlcanParent(new List<GameObject> { thirdConection});
    }

    public void SubstitutionReactionAlchine(GameObject elementToAdd)
    {
        if (!mainLogicScript.hasSubstitutionOccuredAlchine)
        {


            StartCoroutine(SubstitutionReactionAlchineCoroutine(elementToAdd));
            mainLogicScript.hasSubstitutionOccuredAlchine = true;
        }
    }
    IEnumerator SubstitutionReactionAlchineCoroutine(GameObject elementToAdd)
    {
        GameObject firstHidrogen = mainLogicScript.spawnAlchinaScript.carbonList[0].transform.GetChild(0).gameObject;
        Vector3 firstHidrogenPosition = firstHidrogen.transform.position;
        GameObject addedElement = Instantiate(elementToAdd, new Vector3(firstHidrogenPosition.x, firstHidrogenPosition.y + 8, firstHidrogenPosition.z), Quaternion.identity);
        mainLogicScript.MoveAndRotateToObject(addedElement, firstHidrogenPosition, Quaternion.identity, animationSpeed / 1.6f);
       
        yield return new WaitForSeconds(animationSpeed/1.7f);
        mainLogicScript.MoveAndRotateToObject(firstHidrogen, firstHidrogenPosition + new Vector3(-10, 0, 0), Quaternion.identity, animationSpeed/2);
        mainLogicScript.MakeAlcanParent(new List<GameObject> { addedElement} );
        yield return new WaitForSeconds(animationSpeed / 2);
        Destroy(firstHidrogen);

    }



private void Update()
    {
        if(Input.GetKeyUp(KeyCode.V))
        {
            SubstitutionReactionAlchine(mainLogicScript.Natrium);
        }
    }
}
