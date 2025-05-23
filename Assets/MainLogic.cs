using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using JetBrains.Annotations;
using System;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class MainLogic : MonoBehaviour
{
    public GameObject AlchineAditieButton;
    public GameObject backupAlcanParent;
    public List<GameObject> animationButtons;
    public bool isAnimating;
    public ScreenWriterScript screenWriterScript;
    public bool hasSubstitutionOccuredAlchine;
    public GameObject hideOnProPlus;
    public bool isClorInMovement;
    public GameObject CH4;
    public GameObject Natrium;
    public int izomeriePosition;
    public TMP_InputField izomerizareInput;
    public string radical;
    public GameObject AlcanParent;
    public bool isControl;
    int theIndex;
    public TMP_InputField polimerizareInput;
    public string reactie;
    public TextMeshProUGUI reactieText;
    public TextMeshProUGUI recatieTextCatalizator;
    public TextMeshProUGUI numeHidrocarbura;
    public TextMeshProUGUI formulaHidrocarbura;
    public TextMeshProUGUI tipHidrocarburaText;
    public TMP_InputField inputField;
    public GameObject AlcheneAditieMenu;
    public GameObject AlchineAditieMenu;
    public GameObject AlcheneOxigneareMenu;
    public string denumire;
    public float typingSpeed = 0.05f;
    Logic spawnCarbonScript;
    alcanScript alcanScript;
    public Camera mainCamera;
    public string formula;
    public string formulaAlcan;
    public SpawnHydrogens SpawnHidrogensScript;
    public logicManager logicScript;
    public GameObject Clor;
    public GameObject Clor2;
    public alcanScript AlcanScript;
    public GameObject conection;
    public GameObject ClorObject;
    public GameObject conectionObject;
    public GameObject lastHidrogen;
    public int index;
    public GameObject smallConection;
    public float HalogenareSpeed = 5f;
    public GameObject CH3;
    public int carbonCount;
    public GameObject hidrogen;
    public int targetCarbon;
    public TextMeshProUGUI numeText;
    public GameObject hidrogenReplacer;
    public Transform carbonTransformLeaveAlone;
    public spawnAlchena spawnAlchenaScript;
    public spawnAlchina spawnAlchinaScript;
    public string tipHidrocarbura;
    public bool alcheneShowMenu;
    public bool alchineShowMenu;
    public bool alcheneOxigenareShowMenu;
    public GameObject Oxigen;
    public GameObject OH;
    public GameObject AlcanUI;
    public GameObject AlchenaUI;
    public GameObject AlchinaUI;
    public GameObject sideMenu;
    public TextMeshProUGUI error;
    public int theHidrogenCount;
    public GameObject EtanUI;
    public bool hasAlcheneSubstitutionHappened;
    public PrintEqationsScript writeOut;
    // Store original colors so we can restore them
    private Dictionary<GameObject, Color> originalTextColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Color> originalImageColors = new Dictionary<GameObject, Color>();

    private MoleculeSnapshot snapshot;
    public bool resetOnce;
  




    void Start()
    {
        resetOnce = false;
        isAnimating = false;
        writeOut = GameObject.FindAnyObjectByType<PrintEqationsScript>();

        screenWriterScript = FindObjectOfType<ScreenWriterScript>();
        hasAlcheneSubstitutionHappened = false;
        izomerizareInput.gameObject.SetActive(false);
        isClorInMovement = false;
        reactieText.text = "";
        recatieTextCatalizator.text = "";
        polimerizareInput.gameObject.SetActive(false);
        AlcanUI.SetActive(false);
        AlchenaUI.SetActive(false);
        AlcheneAditieMenu.SetActive(false);
        AlcheneOxigneareMenu.SetActive(false);
        AlchineAditieMenu.SetActive(false);
        AlchinaUI.SetActive(false);
        index = 0;
        HalogenareSpeed = 3f;
        spawnCarbonScript = GameObject.FindGameObjectWithTag("carbonSpawner").GetComponent<Logic>();
        spawnAlchenaScript = GameObject.FindGameObjectWithTag("spawnAlchena").GetComponent<spawnAlchena>();
        spawnAlchinaScript = GameObject.FindGameObjectWithTag("spawnAlchina").GetComponent<spawnAlchina>();
        logicScript = GameObject.FindGameObjectWithTag("logicManager").GetComponent<logicManager>();
        alcanScript = GameObject.FindGameObjectWithTag("Molecule").GetComponent<alcanScript>();
        EtanUI.SetActive(false);
        sideMenu.SetActive(false);


        inputField.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        SetButtonsInteractable(animationButtons, !isAnimating);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Load the specified scene
            SceneManager.LoadScene("start");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isControl = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isControl = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetBackgroundColor(Color.blue);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetBackgroundColor(Color.black);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetBackgroundColor(Color.red);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            TakeSnapshot(AlcanParent);
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            RestoreSnapshot(AlcanParent);
        }
        
    }

    public void ReadFormula(string formula)
    {
        formula = formula.ToUpper();
        FormatFormula(formula);
        //inputField.gameObject.SetActive(false);
        sideMenu.SetActive(true);
    }

    public void FormatFormula(string formula)
    {
        carbonCount = 0;
        int hydrogenCount = 0;
        for (int i = 0; i < formula.Length; i++)
        {
            char currentChar = formula[i];
            if (currentChar == 'C')
            {
                int nextIndex = i + 1;
                while (nextIndex < formula.Length && char.IsDigit(formula[nextIndex]))
                {
                    carbonCount = carbonCount * 10 + (formula[nextIndex] - '0');
                    nextIndex++;
                }
                if (carbonCount == 0)
                {
                    carbonCount = 1;
                }
            }
            if (currentChar == 'H')
            {
                int nextIndex = i + 1;
                while (nextIndex < formula.Length && char.IsDigit(formula[nextIndex]))
                {
                    hydrogenCount = hydrogenCount * 10 + (formula[nextIndex] - '0');
                    nextIndex++;
                }
                if (hydrogenCount == 0)
                {
                    hydrogenCount = 1;
                }
            }

        }

        DeterminaHidrocarbura(carbonCount, hydrogenCount);



    }

    public void InitSnapshot()
    {
        TakeSnapshot(AlcanParent);
    }
    public void DeterminaHidrocarbura(int carbonCount, int hidrogenCount)
    {
        Invoke("InitSnapshot", 1.5f);
        
        theHidrogenCount = hidrogenCount;

        if (carbonCount * 2 + 2 == hidrogenCount)
        {
            tipHidrocarbura = "Alcan";
            Debug.Log("Alcan");
            DenumesteAlcan(carbonCount);
            SpawnAlcan(carbonCount);
            tipHidrocarburaText.text = tipHidrocarbura;
            AlcanUI.SetActive(true);
            inputField.gameObject.SetActive(false);
            if (carbonCount == 1)
            {
                EtanUI.SetActive(true);
            }
        }
        else if (carbonCount * 2 == hidrogenCount)
        {
            Debug.Log("Alchena");
            SpawnAlchena(carbonCount);
            DenumesteAlchena(carbonCount);
            tipHidrocarbura = "Alchena";
            tipHidrocarburaText.text = tipHidrocarbura;
            AlchenaUI.SetActive(true);
            inputField.gameObject.SetActive(false);
        }
        else if (carbonCount * 2 - 2 == hidrogenCount)
        {

            Debug.Log("Alchina");
            SpawnAlchina(carbonCount);
            DenumesteAlchina(carbonCount);
            tipHidrocarbura = "Alchină";
            tipHidrocarburaText.text = tipHidrocarbura;
            AlchinaUI.SetActive(true);
            inputField.gameObject.SetActive(false);
            if(carbonCount >= 3) { 
                hideOnProPlus.SetActive(false);
            }
            else
            {
                hideOnProPlus.SetActive(true);
            }
            /*
            Error("Formula este gresita sau nu este suportata momentan");
            inputField.gameObject.SetActive(true);
            Debug.Log("Formula este gresita sau nu este suportata momentan");
            */
        }
        else
        {

            Error("Formula este gresita sau nu este suportata momentan");
            inputField.gameObject.SetActive(true);
            Debug.Log("Formula este gresita sau nu este suportata momentan");

        }

    }
    public string DenumesteAlcan(int carbonCount)
    {

        if (carbonCount == 1)
        {
            radical = "met";
        }
        else if (carbonCount == 2)
        {
            radical = "et";
        }
        else if (carbonCount == 3)
        {
            radical = "prop";
        }
        else if (carbonCount == 4)
        {
            radical = "but";
        }
        else if (carbonCount == 5)
        {
            radical = "pent";
        }
        else if (carbonCount == 6)
        {
            radical = "hex";
        }
        else if (carbonCount == 7)
        {
            radical = "hept";
        }
        else if (carbonCount == 8)
        {
            radical = "oct";
        }
        else if (carbonCount == 9)
        {
            radical = "non";
        }
        else if (carbonCount == 10)
        {
            radical = "dec";
        }
        else if (carbonCount == 11)
        {
            radical = "undec";
        }
        else if (carbonCount == 12)
        {
            radical = "dodec";
        }
        else if (carbonCount == 13)
        {
            radical = "tridec";
        }
        else if (carbonCount == 14)
        {
            radical = "tetradec";
        }
        else if (carbonCount == 15)
        {
            radical = "pentadec";
        }
        else if (carbonCount == 16)
        {
            radical = "hexadec";
        }
        else if (carbonCount == 17)
        {
            radical = "heptadec";
        }
        else if (carbonCount == 18)
        {
            radical = "octadec";
        }
        else if (carbonCount == 19)
        {
            radical = "nonadec";
        }
        else if (carbonCount == 20)
        {
            radical = "eicos";
        }
        else
        {
            radical = "ditamai animalul";
        }

        denumire = radical + "an";
        if (tipHidrocarbura == "Alcan")
        {
            numeHidrocarbura.text = denumire.ToUpper();
        }

        return denumire;
    }
    public string DenumesteAlchena(int carbonCount)
    {

        if (carbonCount == 1)
        {
            radical = "met";
        }
        else if (carbonCount == 2)
        {
            radical = "et";
        }
        else if (carbonCount == 3)
        {
            radical = "prop";
        }
        else if (carbonCount == 4)
        {
            radical = "but";
        }
        else if (carbonCount == 5)
        {
            radical = "pent";
        }
        else if (carbonCount == 6)
        {
            radical = "hex";
        }
        else if (carbonCount == 7)
        {
            radical = "hept";
        }
        else if (carbonCount == 8)
        {
            radical = "oct";
        }
        else if (carbonCount == 9)
        {
            radical = "non";
        }
        else if (carbonCount == 10)
        {
            radical = "dec";
        }
        else if (carbonCount == 11)
        {
            radical = "undec";
        }
        else if (carbonCount == 12)
        {
            radical = "dodec";
        }
        else if (carbonCount == 13)
        {
            radical = "tridec";
        }
        else if (carbonCount == 14)
        {
            radical = "tetradec";
        }
        else if (carbonCount == 15)
        {
            radical = "pentadec";
        }
        else if (carbonCount == 16)
        {
            radical = "hexadec";
        }
        else if (carbonCount == 17)
        {
            radical = "heptadec";
        }
        else if (carbonCount == 18)
        {
            radical = "octadec";
        }
        else if (carbonCount == 19)
        {
            radical = "nonadec";
        }
        else if (carbonCount == 20)
        {
            radical = "eicos";
        }
        else
        {
            radical = "ditamai animalul";
        }

        denumire = radical + "enă";
        numeHidrocarbura.text = "1-" + denumire.ToUpper();
        return denumire;
    }


    public string DenumesteAlchina(int carbonCount)
    {

        if (carbonCount == 1)
        {
            radical = "met";
        }
        else if (carbonCount == 2)
        {
            radical = "et";
        }
        else if (carbonCount == 3)
        {
            radical = "prop";
        }
        else if (carbonCount == 4)
        {
            radical = "but";
        }
        else if (carbonCount == 5)
        {
            radical = "pent";
        }
        else if (carbonCount == 6)
        {
            radical = "hex";
        }
        else if (carbonCount == 7)
        {
            radical = "hept";
        }
        else if (carbonCount == 8)
        {
            radical = "oct";
        }
        else if (carbonCount == 9)
        {
            radical = "non";
        }
        else if (carbonCount == 10)
        {
            radical = "dec";
        }
        else if (carbonCount == 11)
        {
            radical = "undec";
        }
        else if (carbonCount == 12)
        {
            radical = "dodec";
        }
        else if (carbonCount == 13)
        {
            radical = "tridec";
        }
        else if (carbonCount == 14)
        {
            radical = "tetradec";
        }
        else if (carbonCount == 15)
        {
            radical = "pentadec";
        }
        else if (carbonCount == 16)
        {
            radical = "hexadec";
        }
        else if (carbonCount == 17)
        {
            radical = "heptadec";
        }
        else if (carbonCount == 18)
        {
            radical = "octadec";
        }
        else if (carbonCount == 19)
        {
            radical = "nonadec";
        }
        else if (carbonCount == 20)
        {
            radical = "eicos";
        }
        else
        {
            radical = "ditamai animalul";
        }

        denumire = radical + "ină";
        numeHidrocarbura.text = "1-" + denumire.ToUpper();
        return denumire;
    }

    public void SpawnAlcan(int carbonCount)
    {
        spawnCarbonScript.GenerateMolecule(carbonCount);
        cameraLogic(carbonCount);
        FormulaAlcan(carbonCount);


    }

    public string FormulaAlcan(int carbonCount)
    {

        if (carbonCount > 3)
        {
            formulaAlcan = "CH3-" + "(CH2)" + (carbonCount - 2).ToString() + "-CH3";
        }
        else if (carbonCount == 3)
        {

            formulaAlcan = "CH3-" + "CH2" + "-CH3";
        }
        else if (carbonCount == 2)
        {

            formulaAlcan = "CH3-" + "CH3";
        }
        else if (carbonCount == 1)
        {

            formulaAlcan = "CH4";
        }

        formulaHidrocarbura.text = formulaAlcan;
        return formulaAlcan;

    }

    public void cameraLogic(int carbonCount)
    {
        float carbonCountFloat = (float)carbonCount;

        mainCamera.transform.position = new Vector3(0, 0, (-carbonCount * 5) / 2);
    }

    public void Clear()
    {
        resetOnce = true;
        RestoreSnapshot(AlcanParent);
        
        /*
        hasSubstitutionOccuredAlchine = false;
        hasAlcheneSubstitutionHappened = false;
        if (tipHidrocarbura == "Alchena")
        {
            AlcanParent.transform.rotation = Quaternion.Euler(0, 0, 0);
            alcanScript.rotateEnabled = false;
            spawnAlchenaScript.initialPosition.x = 0;
            GameObject molecule = GameObject.FindGameObjectWithTag("Molecule");
            logicScript.HidrogenList.Clear();
            logicScript.HidrogenConectionsList.Clear();
            logicScript.CarbonConection.Clear();
            spawnAlchenaScript.carbonList.Clear();
            logicScript.CarbonConection.Clear();
            for (int i = molecule.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(molecule.transform.GetChild(i).gameObject);
            }
            spawnAlchenaScript.GenerateMolecule(carbonCount);
            Invoke("ClearRestore", 4);
        }
        else if (tipHidrocarbura == "Alchină")
        {
            AlcanParent.transform.rotation = Quaternion.Euler(0, 0, 0);
            alcanScript.rotateEnabled = false;
            spawnAlchinaScript.initialPosition.x = 0;
            GameObject molecule = GameObject.FindGameObjectWithTag("Molecule");
            logicScript.HidrogenList.Clear();
            logicScript.HidrogenConectionsList.Clear();
            logicScript.CarbonConection.Clear();
            spawnAlchinaScript.carbonList.Clear();
            logicScript.CarbonConection.Clear();
            for (int i = molecule.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(molecule.transform.GetChild(i).gameObject);
            }
            spawnAlchinaScript.GenerateMolecule(carbonCount);
            Invoke("ClearRestore", 4);
        }
        else if (tipHidrocarbura == "Alcan")
        {
            HalogenareSpeed = 3f;
            index = 0;
            alcanScript.rotateEnabled = false;
            spawnCarbonScript.initialPosition.x = 0;
            GameObject molecule = GameObject.FindGameObjectWithTag("Molecule");
            logicScript.HidrogenList.Clear();
            logicScript.HidrogenConectionsList.Clear();
            spawnCarbonScript.carbonList.Clear();
            for (int i = molecule.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(molecule.transform.GetChild(i).gameObject);
            }
            logicScript.HidrogenList.Clear();
            logicScript.HidrogenConectionsList.Clear();
            spawnCarbonScript.carbonList.Clear();
            //spawnCarbonScript.GenerateMolecule(carbonCount);
            Invoke("JustLeaveMeAlone", 0.5f);
            Invoke("ClearRestore", 4);
        }
        numeText.text = " ";
        reactieText.text = " ";
        recatieTextCatalizator.text = " ";
        */
    }



    // ---------------------------
    // Helper: build a slash-delimited path from root to child
    // ---------------------------

    string GetTransformIndexPath(Transform child, Transform root)
    {
        List<int> indices = new List<int>();
        while (child != root && child != null)
        {
            indices.Insert(0, child.GetSiblingIndex());
            child = child.parent;
        }
        return string.Join("/", indices);
    }

    // Traverse a hierarchy using index path
    Transform FindByIndexPath(Transform root, string path)
    {
        string[] tokens = path.Split('/');
        Transform current = root;
        foreach (string token in tokens)
        {
            if (int.TryParse(token, out int index))
            {
                if (index >= 0 && index < current.childCount)
                    current = current.GetChild(index);
                else
                    return null;
            }
            else return null;
        }
        return current;
    }

    private string GetTransformPath(Transform child, Transform root)
    {
        string path = child.name;
        var current = child.parent;
        while (current != null && current != root)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        return path;
    }

    // ---------------------------
    // Helper: turn List<string> paths into List<GameObject>
    // ---------------------------
    private List<GameObject> ResolvePaths(Transform root, List<string> paths)
    {
        var result = new List<GameObject>();
        foreach (var p in paths)
        {
            var t = root.Find(p);
            if (t != null)
                result.Add(t.gameObject);
        }
        return result;
    }

    // ---------------------------
    // TakeSnapshot: record paths (List<string>), not references
    // ---------------------------
    public void TakeSnapshot(GameObject target)
    {
        if (snapshot != null && snapshot.snapshotObject != null)
            Destroy(snapshot.snapshotObject);

        // Clone the target molecule and disable it
        var clone = Instantiate(target);
        clone.SetActive(false);

        // Disable all scripts so it doesn’t run in scene
        foreach (var mb in clone.GetComponentsInChildren<MonoBehaviour>(true))
            mb.enabled = false;

        // Grab all relevant lists from your logic/spawn scripts.
        // Example: get the carbon list depending on hydrocarbon type
        var carbList = GetCarbonListForType(); // implement this to get your carbon list

        // Helper to get IDs from GameObject lists
        List<string> GetIDsFromList(List<GameObject> list)
        {
            return list.Select(go => go.GetComponent<ID>().id).ToList();
        }

        var carbonIDs = GetIDsFromList(carbList);
        var hydrogenIDs = GetIDsFromList(logicScript.HidrogenList);
        var hydrogenConnectionIDs = GetIDsFromList(logicScript.HidrogenConectionsList);
        var carbonConnectionIDs = GetIDsFromList(logicScript.CarbonConection);

        snapshot = new MoleculeSnapshot(clone, carbonIDs, hydrogenIDs, hydrogenConnectionIDs, carbonConnectionIDs);
    }


    // ---------------------------
    // RestoreSnapshot: instantiate clone, then rebuild lists by path
    // ---------------------------
    public void RestoreSnapshot(GameObject currentMolecule)
    {
        if (snapshot == null || snapshot.snapshotObject == null)
        {
            Debug.LogWarning("No snapshot to restore.");
            return;
        }

        // Instantiate the saved snapshot clone in the same position/rotation
        var restored = Instantiate(
            snapshot.snapshotObject,
            currentMolecule.transform.position,
            currentMolecule.transform.rotation
        );
        restored.transform.localScale = snapshot.snapshotObject.transform.localScale;
        restored.name = currentMolecule.name;

        // Replace the current molecule
        Destroy(currentMolecule);
        AlcanParent = restored;

        // Clear all lists before repopulating
        logicScript.HidrogenList.Clear();
        logicScript.HidrogenConectionsList.Clear();
        logicScript.CarbonConection.Clear();
        GetCarbonListForType().Clear(); // Clear your carbon list similarly

        // Helper: Resolve GameObjects by their stored IDs inside the restored clone
        List<GameObject> ResolveByID(GameObject root, List<string> ids)
        {
            var allIDs = root.GetComponentsInChildren<ID>(true);
            var result = new List<GameObject>();
            foreach (var id in ids)
            {
                var match = allIDs.FirstOrDefault(x => x.id == id);
                if (match != null)
                    result.Add(match.gameObject);
                else
                    Debug.LogWarning($"RestoreSnapshot: Missing object with ID {id}");
            }
            return result;
        }

        // Rebuild lists with resolved references
        var newCarbons = ResolveByID(restored, snapshot.carbonIDs);
        var newHydrogens = ResolveByID(restored, snapshot.hydrogenIDs);
        var newHConns = ResolveByID(restored, snapshot.hydrogenConnectionIDs);
        var newCConns = ResolveByID(restored, snapshot.carbonConnectionIDs);

        SetCarbonListForType(newCarbons); // Implement this to set carbon list in spawn script
        logicScript.HidrogenList.AddRange(newHydrogens);
        logicScript.HidrogenConectionsList.AddRange(newHConns);
        logicScript.CarbonConection.AddRange(newCConns);

        AlcanParent.SetActive(true);
    }


    private List<GameObject> GetCarbonListForType()
    {
        if (tipHidrocarbura == "Alchena") return spawnAlchenaScript.carbonList;
        if (tipHidrocarbura == "Alchină") return spawnAlchinaScript.carbonList;
        return spawnCarbonScript.carbonList;
    }

    private void SetCarbonListForType(List<GameObject> list)
    {
        if (tipHidrocarbura == "Alchena") spawnAlchenaScript.carbonList = list;
        else if (tipHidrocarbura == "Alchină") spawnAlchinaScript.carbonList = list;
        else spawnCarbonScript.carbonList = list;
    }


    public void JustLeaveMeAlone()
    {
        spawnCarbonScript.GenerateMolecule(carbonCount);
      
    }
    public void ClearRestore()
    {

        alcanScript.rotateEnabled = true;
    }
    public void HalogenareAlcan()
    {
        isAnimating = true;
        writeOut.HalogenareAlcan(carbonCount,FormulaAlcan(carbonCount));
        if (index < theHidrogenCount)
        {
            if (!isClorInMovement)
            {
                alcanScript.rotateEnabled = false;
                lastHidrogen = logicScript.HidrogenList[index];

                ClorObject = Instantiate(Clor, lastHidrogen.transform.position + new Vector3(8, 0, 0), Quaternion.identity);
                conectionObject = Instantiate(conection, ClorObject.transform.position + new Vector3(0.6f, 0, 0), Quaternion.Euler(0, 0, 90));
                GameObject ClorObject2 = Instantiate(Clor2, lastHidrogen.transform.position + new Vector3(9.5f, 0, 0), Quaternion.identity);
                conectionObject.tag = "clorConection";
                ClorObject2.transform.parent = conectionObject.transform;
                ClorObject.tag = "1";
                ClorObject2.tag = "2";
                conectionObject.transform.parent = ClorObject.transform;

                // Destroy(logicScript.HidrogenList[0]);
            }

        }
        else
        {
            Error("Toti hidrogenii au fost inlocuiti");
        }

    }
    public void TurnOnIzomerizare()
    {
        izomerizareInput.gameObject.SetActive(true);
    }
    public void ReactieIzomerie()
    {
        int indexIzomerie = int.Parse(izomerizareInput.text);
        Debug.Log("HELLO : " + indexIzomerie);
        if (carbonCount > 3)
        {
            targetCarbon = carbonCount - indexIzomerie + 1;
            if (targetCarbon > carbonCount)
            {
                Error("Recatie invalida, alcanul nu este suficient de lung");
            }
            else
            {
                GameObject Ch3 = Instantiate(CH3, spawnCarbonScript.carbonList[carbonCount - targetCarbon].transform.position + new Vector3(0, -2, 0), Quaternion.identity);
                Ch3.transform.parent = spawnCarbonScript.carbonList[carbonCount - targetCarbon].transform;
                hidrogenReplacer = Instantiate(hidrogen, spawnCarbonScript.carbonList[spawnCarbonScript.carbonList.Count - 1].transform.position, Quaternion.identity);
                hidrogenReplacer.transform.parent = GameObject.FindGameObjectWithTag("Molecule").transform;
                carbonTransformLeaveAlone = spawnCarbonScript.carbonList[targetCarbon].transform;
                Destroy(spawnCarbonScript.carbonList[spawnCarbonScript.carbonList.Count - 1]);
                Transform PagubituTransform = spawnCarbonScript.carbonList[carbonCount - targetCarbon].transform;
                Destroy(PagubituTransform.GetChild(0).gameObject);
                Destroy(PagubituTransform.GetChild(1).gameObject);
            }

        }

        else
        {
            Error("Izomerizarea este disponibila incepand cu butanul");
        }
        izomerizareInput.gameObject.SetActive(false);

    }
    public void placeHidrogen()
    {
        Debug.Log("BRO");

        hidrogenReplacer.transform.parent = GameObject.FindGameObjectWithTag("Molecule").transform;
    }
    ///////////////ALCHENE
    public void SpawnAlchena(int carbonCount)
    {
        spawnAlchenaScript.GenerateMolecule(carbonCount);
        cameraLogic(carbonCount);
        FormulaAlchena(carbonCount);
    }

    public void FormulaAlchena(int carbonCount)
    {

        if (carbonCount > 3)
        {
            string multipliedString = string.Concat(Enumerable.Repeat("CH2-", carbonCount - 3));
            formula = "CH2=" + "CH-" + "(CH2)" + (carbonCount - 3).ToString() + "-CH3";
            formulaHidrocarbura.fontSize = formula.Length * 0.75f;
        }
        else if (carbonCount == 3)
        {

            formula = "CH3-" + "CH2" + "-CH3";
        }
        else if (carbonCount == 2)
        {

            formula = "Ch2=" + "CH2";
        }
        else if (carbonCount == 1)
        {

            formula = "CH4";
        }

        formulaHidrocarbura.text = formula;
        formulaHidrocarbura.fontSize = 15;


    }
    public void alcheneMenuShower()
    {
        if (alcheneShowMenu == false)
        {
            AlcheneAditieMenu.SetActive(true);
            alcheneShowMenu = true;
        }
        else
        {
            AlcheneAditieMenu.SetActive(false);
            alcheneShowMenu = false;

        }


    }

    public void alchineMenuShower()
    {
        if (alchineShowMenu == false)
        {
            AlchineAditieMenu.SetActive(true);
            alchineShowMenu = true;
        }
        else
        {
            AlchineAditieMenu.SetActive(false);
            alchineShowMenu = false;

        }


    }

    public void alcheneOxigenareMenuShower()
    {
        if (alcheneOxigenareShowMenu == false)
        {
            AlcheneOxigneareMenu.SetActive(true);
            alcheneOxigenareShowMenu = true;
        }
        else
        {
            AlcheneOxigneareMenu.SetActive(false);
            alcheneOxigenareShowMenu = false;

        }


    }
    public void HidrogenareAlchene()
    {
        alcanScript.ResetRotation();

        AlcheneAditieMenu.SetActive(false);
        maxFontSize = 25;
        alcanScript.rotateEnabled = false;
        spawnAlchenaScript.initialPosition.x = 0;
        GameObject molecule = GameObject.FindGameObjectWithTag("Molecule");
        /*
        logicScript.HidrogenList.Clear();
        logicScript.HidrogenConectionsList.Clear();
        logicScript.CarbonConection.Clear();
        spawnCarbonScript.carbonList.Clear();
        spawnAlchenaScript.carbonList.Clear();
        for (int i = molecule.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(molecule.transform.GetChild(i).gameObject);
        }
        //spawnCarbonScript.GenerateMolecule(carbonCount);
        */
        StartCoroutine(AlcheneAnimatie("hidrogenare"));
        FormulaAlcan(carbonCount);
        string reactie = formula + " + H2" + "-> " + formulaAlcan;

       


    }

    public float minFontSize = 5f;
    public float maxFontSize = 50f;
    public float fontSizeMultiplier = 0.5f;

   

    public void HalogenareAlchene()
    {
        if (!isClorInMovement)
        {
            hasAlcheneSubstitutionHappened = true;

            alcanScript.rotateEnabledAlDoileaPtCaPrimulNuMere = false;
            
            AlcheneAditieMenu.SetActive(false);

            maxFontSize = 25;
            fontSizeMultiplier = 0.3f;
            theIndex = 1;
            //alcanScript.rotationToggle.isOn = false;
            /*
            alcanScript.rotateEnabledAlDoileaPtCaPrimulNuMere = false;
            spawnAlchenaScript.initialPosition.x = 0;
            GameObject molecule = GameObject.FindGameObjectWithTag("Molecule");
            logicScript.HidrogenList.Clear();
            logicScript.HidrogenConectionsList.Clear();
            spawnCarbonScript.carbonList.Clear();
            spawnAlchenaScript.carbonList.Clear();
            for (int i = molecule.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(molecule.transform.GetChild(i).gameObject);
            }

            spawnCarbonScript.GenerateMolecule(carbonCount);
            */
            //FormulaAlcan(carbonCount);

            if (carbonCount == 2)
            {
                formulaAlcan = "CH2Cl-" + "CH2Cl";
                reactie = formula + " + Cl2" + "-> " + formulaAlcan;
            }
            else if (carbonCount == 3)
            {
                formulaAlcan = "CH2Cl-" + "CH2Cl" + "-CH3";
                reactie = formula + " + Cl2" + "-> " + formulaAlcan;
            }
            else if (carbonCount > 3)
            {
                formulaAlcan = "CH2Cl-" + "CH2Cl-" + "(CH2)" + (carbonCount - 3).ToString() + "-CH3";
                reactie = formula + " + Cl2" + "-> " + formulaAlcan;
            }
            
            alcanScript.ResetRotation();
            ////
            //Invoke("FraNiciNuMaiStiu", 2);
            StartCoroutine(AlcheneAnimatie("halogenare"));


        }
    }


    public void StartRotation()
    {
        alcanScript.rotateEnabledAlDoileaPtCaPrimulNuMere = true;
    }


    // Coroutine to move the object over time
    private IEnumerator GlideToObject(Transform objTransform, Vector3 targetPosition, float duration)
    {
        isAnimating = true;
        float elapsed = 0f;
        Vector3 initialPosition = objTransform.position;

        while (elapsed < duration)
        {
            objTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches the exact target position
        objTransform.position = targetPosition;
        if (objTransform.position == targetPosition)
        {
            alcanScript.rotateEnabledAlDoileaPtCaPrimulNuMere = true;
        }
        isAnimating = false;
    }


    public void HidracizareAlchene()
    {
        AlcheneAditieMenu.SetActive(false);
        /*
        maxFontSize = 20;
        AlcheneAditieMenu.SetActive(false);
        theIndex = 1;
        alcanScript.rotateEnabledAlDoileaPtCaPrimulNuMere = false;
        spawnAlchenaScript.initialPosition.x = 0;
        GameObject molecule = GameObject.FindGameObjectWithTag("Molecule");
        logicScript.HidrogenList.Clear();
        logicScript.HidrogenConectionsList.Clear();
        spawnCarbonScript.carbonList.Clear();
        spawnAlchenaScript.carbonList.Clear();
        for (int i = molecule.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(molecule.transform.GetChild(i).gameObject);
        }
        spawnCarbonScript.GenerateMolecule(carbonCount);
        */
        //
        if (carbonCount == 2)
        {
            formulaAlcan = "CH3-" + "CH2Cl";
            reactie = formula + " + HCl" + "-> " + formulaAlcan;
        }
        else if (carbonCount == 3)
        {
            formulaAlcan = "CH3-" + "CH2Cl" + "-CH3";
            reactie = formula + " + HCl" + "-> " + formulaAlcan;
        }
        else if (carbonCount > 3)
        {
            formulaAlcan = "CH3-" + "CH2Cl-" + "(CH2)" + (carbonCount - 3).ToString() + "-CH3";
            reactie = formula + " + HCl" + "-> " + formulaAlcan;
        }


       
        //Invoke("FraNiciNuMaiStiu2", 1);
        StartCoroutine(AlcheneAnimatie("hidracizare"));



    }

    public void AditieApaAlchene()
    {
        alcanScript.rotateEnabledAlDoileaPtCaPrimulNuMere = false;
        maxFontSize = 20;
        AlcheneAditieMenu.SetActive(false);
        /*
        theIndex = 1;
        alcanScript.rotateEnabled = false;
        spawnAlchenaScript.initialPosition.x = 0;
        GameObject molecule = GameObject.FindGameObjectWithTag("Molecule");
        logicScript.HidrogenList.Clear();
        logicScript.HidrogenConectionsList.Clear();
        spawnCarbonScript.carbonList.Clear();
        spawnAlchenaScript.carbonList.Clear();
        for (int i = molecule.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(molecule.transform.GetChild(i).gameObject);
        }
        spawnCarbonScript.GenerateMolecule(carbonCount);
        */
        if (carbonCount == 2)
        {
            formulaAlcan = "CH3-" + "CH2Cl";
            reactie = formula + " + HOH" + "-> " + formulaAlcan;
        }
        else if (carbonCount == 3)
        {
            formulaAlcan = "CH3-" + "CHOH-" + "CH3";
            reactie = formula + " + HOH" + "-> " + formulaAlcan;
        }
        else if (carbonCount > 3)
        {
            formulaAlcan = "CH3-" + "CHOH-" + "(CH2)" + (carbonCount - 3).ToString() + "-CH3";
            reactie = formula + " + HOH" + "-> " + formulaAlcan;
        }


       
        //Invoke("FraNiciNuMaiStiu3", 2);
        StartCoroutine(AlcheneAnimatie("apa"));



    }

    public void OxigenareBlandaAlchene()
    {
        isAnimating = true;
        AlcheneOxigneareMenu.SetActive(false);
        if (carbonCount == 2)
        {
            formulaAlcan = "CH2OH-" + "CH2OH";
            reactie = formula + " + KMnO4/H2SO4" + "-> " + formulaAlcan;
        }
        else if (carbonCount == 3)
        {
            formulaAlcan = "CH3OH-" + "CHOH-" + "CH3";
            reactie = formula + " + KMnO4/H2SO4" + "-> " + formulaAlcan;
        }
        else if (carbonCount > 3)
        {
            formulaAlcan = "CHOH-" + "CHOH-" + "(CH2)" + (carbonCount - 4).ToString() + "-CH3";
            reactie = formula + " + 2KMnO4 + H20" + "-> " + formulaAlcan;
        }
        
        //TypeInTextCatalizator("2 KMnO4 + 3 H2SO4 → 5 O + 2 MnSO4 + K2SO4 + 3 H2O");
        StartCoroutine(AlcheneAnimatie("oxidareBlanda"));
    }


    IEnumerator AlcheneAnimatie(string reactieA)
    {
        alcanScript.ResetRotation();
        alcanScript.rotateEnabledAlDoileaPtCaPrimulNuMere = false;
        GameObject firstSubstitute = null;
        GameObject secondSubstitute = null;
        if (reactieA == "halogenare")
        {
            firstSubstitute = Clor2;
            secondSubstitute = Clor2;
        }
        else if (reactieA == "hidrogenare")
        {
            firstSubstitute = hidrogen;
            secondSubstitute = hidrogen;
        }
        else if (reactieA == "hidracizare")
        {
            firstSubstitute = hidrogen;
            secondSubstitute = Clor2;
        }
        else if (reactieA == "apa")
        {
            firstSubstitute = hidrogen;
            secondSubstitute = OH;
        }
        else if (reactieA == "oxidareBlanda")
        {
            firstSubstitute = OH;
            secondSubstitute = OH;
        }

        yield return new WaitForSeconds(2);
        if (carbonCount == 2)
        {
            GameObject necesaryParent = new GameObject();
            necesaryParent.transform.position = logicScript.HidrogenConectionsList[3].transform.position;
            logicScript.HidrogenConectionsList[3].transform.parent = necesaryParent.transform;
            logicScript.HidrogenList[3].transform.parent = necesaryParent.transform;
            necesaryParent.transform.parent = AlcanParent.transform;
            MoveAndRotateToObject(necesaryParent, new Vector3(1.93499994f, 0.226999998f, 0.0549999997f), Quaternion.Euler(53.459f, -92.38f, 0), 2);

        }
        GameObject brokenPi = logicScript.CarbonConection[0];
        RemoveParent(brokenPi);
        GameObject brokenPi2 = Instantiate(brokenPi);
        RemoveParent(brokenPi2);
        GameObject targetPi = logicScript.HidrogenConectionsList[2];
        //Vector2 3 z -1 rotation y 180
        MoveAndRotateToObject(brokenPi, targetPi.transform.position + new Vector3(0, 0, -1), Quaternion.Euler(123, 180, 0), 2);
        MoveAndRotateToObject(brokenPi2, new Vector3(-0.42899999f, 0.809000015f, 0.0649999976f), Quaternion.Euler(297.039917f, 250, -7.51212201e-06f), 2);

        //TypeInText(reactie);
        brokenPi.transform.parent = AlcanParent.transform;
        brokenPi2.transform.parent = AlcanParent.transform;
        yield return new WaitForSeconds(2);

        Vector3 clor1Position = new Vector3(-1.19000006f, 0.490999997f, 0.023f);
        Vector3 clor2Position = new Vector3(1.30004644f, -0.644999981f, -1.05299997f);
        // Spawn position offset
        float spawnDistance = 5f; // Change this value to adjust the spawn distance

        // Instantiate the first object
        GameObject clor2Object1 = Instantiate(firstSubstitute, clor1Position + Vector3.up * spawnDistance + new Vector3(0, 0, 0), Quaternion.identity * Quaternion.Euler(0, 0, 260));
        GameObject clor2Object2 = Instantiate(secondSubstitute, clor2Position + Vector3.up * spawnDistance + new Vector3(0, 0, 0), Quaternion.identity);
        StartCoroutine(GlideToObject(clor2Object1.transform, clor1Position, 2f));
        StartCoroutine(GlideToObject(clor2Object2.transform, clor2Position, 2f));
        clor2Object1.transform.parent = AlcanParent.transform;

        // Instantiate the second object

        clor2Object2.transform.parent = AlcanParent.transform;

        Invoke("StartRotation", 3);
        // Move objects towards the target over 2 seconds

    }

    public void Resetare()
    {/*
        Destroy(AlcanParent);
        GameObject backup = Instantiate(backupAlcanParent);
        AlcanParent = backup;
        */
        hasSubstitutionOccuredAlchine = false;
        hasAlcheneSubstitutionHappened = false;
        HalogenareSpeed = 3f;
        AlcanParent.transform.rotation = Quaternion.Euler(0, 0, 0);
        reactieText.text = " ";
        recatieTextCatalizator.text = " ";
        alcanScript.rotationToggle.isOn = false;

        alcanScript.rotateEnabled = false;
        spawnCarbonScript.initialPosition.x = 0;
        GameObject molecule = AlcanParent;
        logicScript.HidrogenList.Clear();
        logicScript.HidrogenConectionsList.Clear();
        logicScript.CarbonConection.Clear();
        spawnCarbonScript.carbonList.Clear();
        spawnAlchinaScript.carbonList.Clear();
        spawnAlchenaScript.carbonList.Clear();
        for (int i = molecule.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(molecule.transform.GetChild(i).gameObject);
        }
        inputField.gameObject.SetActive(true);
        Invoke("startTheRotation", 5f);
        AlcanUI.SetActive(false);
        AlchenaUI.SetActive(false);
        sideMenu.SetActive(false);
        EtanUI.SetActive(false);
        AlchinaUI.SetActive(false);
        numeText.text = "";
        index = 0;
    }
    public void SubstitutieHalogenALchena()
    {
        if (carbonCount > 2)
        {
            alcanScript.rotateEnabled = false;
            lastHidrogen = logicScript.HidrogenList[4];

            ClorObject = Instantiate(Clor, lastHidrogen.transform.position + new Vector3(8, 0, 0), Quaternion.identity);
            conectionObject = Instantiate(conection, ClorObject.transform.position + new Vector3(0.6f, 0, 0), Quaternion.Euler(0, 0, 90));
            GameObject ClorObject2 = Instantiate(Clor2, lastHidrogen.transform.position + new Vector3(9.5f, 0, 0), Quaternion.identity);
            conectionObject.tag = "clorConection";
            ClorObject2.transform.parent = conectionObject.transform;
            ClorObject.tag = "1";
            ClorObject2.tag = "2";
            conectionObject.transform.parent = ClorObject.transform;

            // Destroy(logicScript.HidrogenList[0]);
        }
        else
        {
            Error("Substitutia este disponibila doar de la propena in sus");
        }

    }

    public void startTheRotation()
    {
        alcanScript.rotateEnabled = true;
    }
    public void polimerizareInputA()
    {
        polimerizareInput.gameObject.SetActive(true);
    }
    public void Polimerizare()
    {
        maxFontSize = 40;


        AlcheneAditieMenu.SetActive(false);
        theIndex = 1;
        alcanScript.rotateEnabled = false;
        spawnAlchenaScript.initialPosition.x = 0;
        GameObject molecule = GameObject.FindGameObjectWithTag("Molecule");
        logicScript.HidrogenList.Clear();
        logicScript.HidrogenConectionsList.Clear();
        spawnCarbonScript.carbonList.Clear();
        spawnAlchenaScript.carbonList.Clear();
        for (int i = molecule.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(molecule.transform.GetChild(i).gameObject);
        }
        spawnCarbonScript.GenerateMolecule(carbonCount * 2);
        Invoke("NMK", 2);
    
    }

    //DISTRUGE ATOMII LA POLIMERIAZRE ORICUM E INCORECT
    public void NMK()
    {
        Destroy(logicScript.HidrogenList[1]);
        Destroy(logicScript.HidrogenList[logicScript.HidrogenList.Count - 2]);
    }

    public void SpawnAlchina(int carbonCount)
    {

        spawnAlchinaScript.GenerateMolecule(carbonCount);
        cameraLogic(carbonCount);
        FormulaAlchena(carbonCount);
    }



    void DestroyWithoutChildren(GameObject gameObject)
    {
        // Get the children of the gameObject
        Transform[] children = new Transform[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            children[i] = gameObject.transform.GetChild(i);
        }

        // Reparent each child to the parent of the gameObject
        foreach (Transform child in children)
        {
            child.parent = gameObject.transform.parent;
        }

        // Destroy the gameObject
        Destroy(gameObject);
    }

    public void MakeAlcanParent(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.parent = AlcanParent.transform;
        }

    }

    public void SetBackgroundColor(Color color)
    {
        mainCamera.backgroundColor = color;
    }



    public void OxigenareEnergica()
    {

        AlcheneOxigneareMenu.SetActive(false);
        
            GameObject firstConection = logicScript.CarbonConection[0];
            MoveAndRotateToObject(firstConection, new Vector3(0, 1f, 0.5f), Quaternion.Euler(306.043274f, 183.229477f, 357.387756f), 2);
            GameObject firstHidrogen = logicScript.HidrogenList[0];
            GameObject replacerOxigen = Instantiate(Oxigen, firstHidrogen.transform.position + Vector3.up * 6, Quaternion.identity);
            MoveAndRotateToObject(replacerOxigen, firstHidrogen.transform.position, replacerOxigen.transform.rotation, 3);
            //
            GameObject secondConection = logicScript.CarbonConection[1];
            MoveAndRotateToObject(secondConection, new Vector3(-0.0399999991f, 1.1f, -0.639999986f), Quaternion.Euler(306.042999f, 3.22899342f, 357.388f), 2);
            GameObject secondHidrogen = logicScript.HidrogenList[1];
            GameObject replacerOxigen2 = Instantiate(Oxigen, secondHidrogen.transform.position + Vector3.up * 6, Quaternion.identity);
            MoveAndRotateToObject(replacerOxigen2, secondHidrogen.transform.position, replacerOxigen2.transform.rotation, 3);
            MakeAlcanParent(new List<GameObject> { replacerOxigen, replacerOxigen2 });
            //
            /*
            GameObject thirdConection = logicScript.HidrogenConectionsList[2];
            GameObject thirdHidrogen = logicScript.HidrogenList[2];
            GameObject thirdParent = new GameObject();
            thirdParent.transform.position = thirdConection.transform.position;
            thirdConection.transform.parent = thirdParent.transform;

            thirdHidrogen.transform.parent=thirdParent.transform;
            MakeAlcanParent(new List<GameObject> { replacerOxigen,replacerOxigen2,thirdParent });
            */
            //

            if (carbonCount == 2)
            {
                formulaAlcan = "2CO2 + 2H2O";
                reactie = formula + " + K2Cr2O7/H2SO4" + "-> " + formulaAlcan;
            }
            else if (carbonCount == 3)
            {
                formulaAlcan = "CO2 + H2O" + "CH3-" + "COOH";
                reactie = formula + " + K2Cr2O7/H2SO4" + "-> " + formulaAlcan;
            }
            else if (carbonCount > 3)
            {
                formulaAlcan = "CO2 + H2O" + " (CH2)" + (carbonCount - 3).ToString() + "-CH3" + "-COOH";
                reactie = formula + " + O " + "-> " + formulaAlcan;
            }
        
            //StartCoroutine(AlcheneAnimatie("oxidareBlanda"));

            //DestroyWithoutChildren(replaceCarbon);
            if (carbonCount == 2)
            {

                MoveAndRotateToObject(secondHidrogen, new Vector3(1.27999997f, 1, -1), secondHidrogen.transform.rotation, 3);
                MoveAndRotateToObject(firstHidrogen, new Vector3(1.03999996f, 1, 1), firstHidrogen.transform.rotation, 3);
                GameObject replaceCarbon = spawnAlchenaScript.carbonList[1];
                GameObject replacerOxigen3 = Instantiate(Oxigen, replaceCarbon.transform.position + Vector3.up * 6, Quaternion.identity);
                MoveAndRotateToObject(replacerOxigen3, new Vector3(1.25999999f, 0.980000019f, 3.43425199e-08f), replaceCarbon.transform.rotation, 3);
                GameObject apaLegatura = Instantiate(conection, new Vector3(1.2209999f, 0.972571492f, -0.0270000007f), Quaternion.Euler(0, 86.9400024f, 90));
                MakeAlcanParent(new List<GameObject> { replacerOxigen3, apaLegatura });
                Destroy(AlcanParent.transform.GetChild(3).gameObject);


                Invoke("DuplicateAlcan", 6);

            }
            else
            {
                MoveAndRotateToObject(secondHidrogen, new Vector3(1.27999997f, 1, -1), secondHidrogen.transform.rotation, 3);
                MoveAndRotateToObject(firstHidrogen, new Vector3(1.03999996f, 1, 1), firstHidrogen.transform.rotation, 3);
                GameObject replaceCarbon = spawnAlchenaScript.carbonList[1];
                GameObject replacerOxigen3 = Instantiate(Oxigen, replaceCarbon.transform.position + Vector3.up * 6, Quaternion.identity);
                MoveAndRotateToObject(replacerOxigen3, new Vector3(1.25999999f, 0.980000019f, 3.43425199e-08f), replaceCarbon.transform.rotation, 3);
                GameObject apaLegatura = Instantiate(conection, new Vector3(1.2209999f, 0.972571492f, -0.0270000007f), Quaternion.Euler(0, 86.9400024f, 90));
                MakeAlcanParent(new List<GameObject> { replacerOxigen3, apaLegatura });
                // Destroy(AlcanParent.transform.GetChild(3).gameObject);
                //GameObject thirdParent2 = Instantiate(thirdParent, thirdParent.transform.position + new Vector3(0, 0, -0.5f), Quaternion.Euler(0, 180, 0));
                GameObject clor2Object1 = Instantiate(OH, logicScript.HidrogenList[2].transform.position + Vector3.up * 7 + new Vector3(0, 0, 0), Quaternion.identity * Quaternion.Euler(0, 0, 260));
                MoveAndRotateToObject(clor2Object1, logicScript.HidrogenList[2].transform.position, Quaternion.identity, 3);
                GameObject doubleLeg = InstantiateWithScale(conection, new Vector3(1.26800001f, -0.155000001f, -0.578999996f), Quaternion.Euler(0, 78.2399979f, 66.6999893f), new Vector3(0.0900000036f, 0.02f, 0.170000002f));
                GameObject doubleLeg2 = InstantiateWithScale(conection, new Vector3(1.26699996f, -0.0549999997f, -0.577000022f), Quaternion.Euler(0, 78.2399979f, 66.6999893f), new Vector3(0.0900000036f, 0.02f, 0.170000002f));
                ScaleToObject(doubleLeg, new Vector3(0.0900000036f, 0.45f, 0.170000002f), 3);
                ScaleToObject(doubleLeg2, new Vector3(0.0900000036f, 0.45f, 0.170000002f), 3);
                GameObject lastOxigen = Instantiate(Oxigen, new Vector3(1.26736915f, -0.453000009f + 10, -1.07599998f), Quaternion.Euler(0, 0, 0));
                MoveAndRotateToObject(lastOxigen, new Vector3(1.26736915f, -0.453000009f, -1.07599998f), Quaternion.Euler(0, 0, 0), 3);
                MakeAlcanParent(new List<GameObject> { doubleLeg, doubleLeg2, lastOxigen, clor2Object1 });
                Invoke("DuplicateAlcan", 6);
            }

        


    }
    public void DuplicateAlcan()
    {
        alcanScript.rotateEnabledAlDoileaPtCaPrimulNuMere = true;
        //GameObject Clone= Instantiate(AlcanParent, AlcanParent.transform.position + new Vector3(3, 0, 0), AlcanParent.transform.rotation);
        // Clone.transform.parent = AlcanParent.transform;
    }
    public GameObject InstantiateWithScale(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // Instantiate the GameObject at the given position and rotation
        GameObject instance = Instantiate(prefab, position, rotation);

        // Set the scale of the instantiated GameObject
        instance.transform.localScale = scale;

        // Return the instantiated GameObject
        return instance;
    }

    public void RemoveParent(GameObject child)
    {
        if (child != null)
        {
            child.transform.parent = null;
        }

    }
    public void MoveAndRotateToObject(GameObject obj, Vector3 targetPosition, Quaternion targetRotation, float speed)
    {
        isAnimating = true;
        StartCoroutine(MoveAndRotateCoroutine(obj, targetPosition, targetRotation, speed));
    }

    private IEnumerator MoveAndRotateCoroutine(GameObject obj, Vector3 targetPosition, Quaternion targetRotation, float speed)
    {
        float startTime = Time.time;
        Vector3 startPosition = obj.transform.position;
        Quaternion startRotation = obj.transform.rotation;

        while (Time.time - startTime < speed)
        {
            float t = (Time.time - startTime) / speed;
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            obj.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        // Ensure final position and rotation are exactly as desired
        obj.transform.position = targetPosition;
        obj.transform.rotation = targetRotation;
        isAnimating = false;
    }

    public void AddAtomsHigher(GameObject obj, Vector3 targetPosition, float height, float speed, Quaternion? targetRotation = null)
    {
        // If no target rotation is specified, use the object's current rotation
        targetRotation ??= obj.transform.rotation;

        // Instantiate the object at the target position with added height on the Y-axis
        Vector3 instantiationPosition = targetPosition + new Vector3(0, height, 0);
        GameObject newObj = Instantiate(obj, instantiationPosition, targetRotation.Value);

        // Start the coroutine with the newly instantiated object
        StartCoroutine(AddAtomsHigherCoroutine(newObj, targetPosition, targetRotation.Value, height, speed));
    }

    private IEnumerator AddAtomsHigherCoroutine(GameObject obj, Vector3 targetPosition, Quaternion targetRotation, float height, float speed)
    {
        // Start position is the target position, but with height offset added to Y-axis
        Vector3 startPosition = obj.transform.position;  // This will be the instantiated position
        Quaternion startRotation = obj.transform.rotation;

        float startTime = Time.time;
        float distance = Vector3.Distance(startPosition, targetPosition);

        while (Time.time - startTime < distance / speed) // Modify duration based on speed
        {
            float t = (Time.time - startTime) / (distance / speed);

            // Interpolate position
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // Interpolate rotation
            obj.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        // Ensure final position and rotation are exactly as desired
        obj.transform.position = targetPosition;
        obj.transform.rotation = targetRotation;
        MakeAlcanParent(new List<GameObject> { obj });
    }




    public void ScaleToObject(GameObject obj, Vector3 targetScale, float duration)
    {
        StartCoroutine(ScaleCoroutine(obj, targetScale, duration));
    }

    private IEnumerator ScaleCoroutine(GameObject obj, Vector3 targetScale, float duration)
    {
        float startTime = Time.time;
        Vector3 startScale = obj.transform.localScale;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        // Ensure final scale is exactly as desired
        obj.transform.localScale = targetScale;
    }
    public void Error(string errorText)
    {
        error.text = "";
        StartCoroutine(ErrorCorutine(errorText));
    }

    IEnumerator ErrorCorutine(string errorText)
    {
        error.text = errorText;
        yield return new WaitForSeconds(errorText.Length * 0.1f);
        error.text = "";
    }


    public void FormareaAldehidei()
    {


        //look my guy here I swaped the name , where its oxigen its hydrogen sorry NVM
        //its ok bro
        isAnimating = true;
        StartCoroutine(FormareaAldehideiCoroutine());
        writeOut.FormareAldehidaFormicaAlcani();
        
       

    }
    IEnumerator FormareaAldehideiCoroutine()
    {
        int animationSpeed = 3;
        GameObject oxigenPi = logicScript.HidrogenConectionsList[0];

        MoveAndRotateToObject(oxigenPi, new Vector3(0.0650000796f, 0.671999931f, -0.0170000363f), Quaternion.Euler(358.357452f, 348.665741f, 178.088486f), animationSpeed);
        GameObject firstHidrogen = logicScript.HidrogenList[1];
        GameObject secondHidrogen = logicScript.HidrogenList[3];
        Vector3 topOxigenPosition = firstHidrogen.transform.position;
        MoveAndRotateToObject(firstHidrogen, new Vector3(1.65999997f, 0.957000017f, 0.270000011f), Quaternion.Euler(1.34003222f, 211.409622f, 0.818137467f), animationSpeed);
        MoveAndRotateToObject(secondHidrogen, new Vector3(1.70200002f, 1.0160000f, -1.10599995f), Quaternion.Euler(0, 0, 0), animationSpeed);
        GameObject firstOxigen = Instantiate(Oxigen, topOxigenPosition + new Vector3(0, 10, 0), Quaternion.Euler(0, 0, 0));
        MoveAndRotateToObject(firstOxigen, topOxigenPosition, Quaternion.identity, animationSpeed);
        GameObject secondOxigen = Instantiate(Oxigen, topOxigenPosition + new Vector3(0, 10, 0), Quaternion.Euler(0, 0, 0));
        MoveAndRotateToObject(secondOxigen, new Vector3(1.66799998f, 1.63300002f, -0.379999995f), Quaternion.identity, animationSpeed);

        yield return new WaitForSeconds(animationSpeed);
        GameObject firsttie = Instantiate(smallConection, new Vector3(1.63999999f, 1.22000003f, 0.0710000023f), Quaternion.Euler(353.693939f, 72.5807266f, 148.732208f));
        GameObject secondtie = Instantiate(smallConection, new Vector3(1.63999999f, 1.22000003f, -0.828000009f), Quaternion.Euler(353.693939f, 252.581f, 148.732208f));
        MakeAlcanParent(new List<GameObject> { firstOxigen, secondOxigen, firsttie, secondtie, });
        isAnimating = false;
    }


    public void DimerizareMetan()
    {
       
        Vector3 secondMetanPosition = new Vector3(1.33200002f, 0.000277847052f, 5.26793301e-05f);
        GameObject secondMetan = Instantiate(CH4, secondMetanPosition + new Vector3(10, 0, 0), Quaternion.identity);
        MoveAndRotateToObject(secondMetan, secondMetanPosition, Quaternion.identity, 3);
        foreach (Transform child in secondMetan.transform)
        {
            // Perform operations on each child
            Debug.Log("Child name: " + child.name);

            if (child.tag == "smallConection")
            {
                logicScript.HidrogenConectionsList.Add(child.gameObject);
            }
        }
        MakeAlcanParent(new List<GameObject> { secondMetan });
        Invoke("dimerizarePart2", 3);
    }
    public void DestroyGameObjects(List<GameObject> objectsToDestroy)
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }


    public void dimerizarePart2()
    {
        GameObject hidrogenParent1 = new GameObject();
        hidrogenParent1.transform.position = logicScript.HidrogenConectionsList[1].transform.position;
        logicScript.HidrogenConectionsList[1].transform.parent = hidrogenParent1.transform;
        logicScript.HidrogenList[2].transform.parent = hidrogenParent1.transform;
        //
        GameObject hidrogenParent2 = new GameObject();
        hidrogenParent2.transform.position = logicScript.HidrogenConectionsList[4].transform.position;
        logicScript.HidrogenConectionsList[4].transform.parent = hidrogenParent2.transform;
        logicScript.HidrogenList[7].transform.parent = hidrogenParent2.transform;
        /*
        hidrogenParent1.transform.parent = secondMetan.transform;
        hidrogenParent2.transform.parent = secondMetan.transform;
        MakeAlcanParent(new List<GameObject> { secondMetan});
        */

        MoveAndRotateToObject(hidrogenParent1, new Vector3(-0.441002488f, 0.0320000015f, -0.0659999996f), Quaternion.Euler(332.469543f, 310.258301f, 347.267334f), 1);
        MoveAndRotateToObject(hidrogenParent2, new Vector3(1.86000001f, 0.0320000015f, -0.177000001f), Quaternion.Euler(355.813538f, 29.1925697f, 34.6748199f), 1);
        //MoveAndRotateToObject(hidrogenParent2,new )

        MakeAlcanParent(new List<GameObject> {hidrogenParent1,hidrogenParent2, Instantiate(conection, new Vector3(0.621999979f, -0.00700000022f, -0.0219999999f), Quaternion.Euler(0, 0, 90)),
        Instantiate(conection, new Vector3(0.621999979f, -0.00700000022f, -0.0219999999f) + new Vector3(0, 0.13f, 0), Quaternion.Euler(0, 0, 90)),
        Instantiate(conection, new Vector3(0.621999979f, -0.00700000022f, -0.0219999999f) + new Vector3(0, -0.13f, 0), Quaternion.Euler(0, 0, 90))
    });
        DestroyGameObjects(new List<GameObject> { logicScript.HidrogenList[1], logicScript.HidrogenList[0], logicScript.HidrogenList[3], logicScript.HidrogenList[4], logicScript.HidrogenList[5], logicScript.HidrogenList[6], logicScript.HidrogenConectionsList[0], logicScript.HidrogenConectionsList[2], logicScript.HidrogenConectionsList[3], logicScript.HidrogenConectionsList[5], logicScript.HidrogenConectionsList[6], logicScript.HidrogenConectionsList[7] });
    }

    public  GameObject GetChildByIdentifier(GameObject parent, string identifier, bool isName)
    {
        if (parent == null)
        {
            Debug.LogError("Parent GameObject is null.");
            return null; // Explicit return
        }

        foreach (Transform child in parent.transform)
        {
            if (isName && child.name == identifier)
            {
                return child.gameObject; // Return if found by name
            }
            else if (!isName && child.CompareTag(identifier))
            {
                return child.gameObject; // Return if found by tag
            }
        }

        // If no matching child is found, return null explicitly
        Debug.LogWarning($"Child with {(isName ? "name" : "tag")} '{identifier}' not found.");
        return null;
    }

    public  GameObject TransferChildrenToNewObject(GameObject parent)
    {
        if (parent == null)
        {
            Debug.LogError("Parent GameObject is null.");
            return null;
        }

        // Create a new GameObject
        GameObject newParent = new GameObject("NewParent");

        // Check if the parent has children
        if (parent.transform.childCount > 0)
        {
            // Get the position of the first child
            Transform firstChild = parent.transform.GetChild(0);
            newParent.transform.position = firstChild.position;
        }
        else
        {
            // If no children, place the new object at the parent's position
            newParent.transform.position = parent.transform.position;
        }

        // Transfer all children to the new GameObject
        for (int i = parent.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.transform.GetChild(i);
            child.SetParent(newParent.transform); // Change the parent of the child
        }

        return newParent;
    }

    public void SetButtonsInteractable(List<GameObject> buttonParents, bool interactable)
    {
        foreach (GameObject parentObj in buttonParents)
        {
            if (parentObj == null || !parentObj.activeInHierarchy)
                continue;

            // Find all Button components in this GameObject and all children
            UnityEngine.UI.Button[] buttons = parentObj.GetComponentsInChildren<UnityEngine.UI.Button>(includeInactive: false);

            foreach (var button in buttons)
            {
                if (button == null || button.gameObject == null)
                    continue;

                // Set interactable state
                button.interactable = interactable;

                GameObject buttonObj = button.gameObject;

                // Find TMP_Text in the button's children
                TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
                if (text != null)
                {
                    if (!originalTextColors.ContainsKey(buttonObj))
                        originalTextColors[buttonObj] = text.color;

                    text.color = interactable ? originalTextColors[buttonObj] : new Color(0.5f, 0.5f, 0.5f);
                }

                // Find Image component on the button itself (usually the background)
                UnityEngine.UI.Image image = buttonObj.GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    if (!originalImageColors.ContainsKey(buttonObj))
                        originalImageColors[buttonObj] = image.color;

                    image.color = interactable ? originalImageColors[buttonObj] : new Color(0.7f, 0.7f, 0.7f);
                }
            }
        }
    }
    public void AnimationManager(string Animation)
    {
        Clear();
        TakeSnapshot(AlcanParent);
        
        //Clear();
        Invoke(Animation, 0f);
    }
}