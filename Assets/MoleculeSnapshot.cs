using System.Collections.Generic;
using UnityEngine;

public class MoleculeSnapshot
{
    public GameObject snapshotObject;  // The cloned molecule root

    public List<string> carbonIDs;
    public List<string> hydrogenIDs;
    public List<string> hydrogenConnectionIDs;
    public List<string> carbonConnectionIDs;

    public MoleculeSnapshot(GameObject obj,
                            List<string> carbonIDs,
                            List<string> hydrogenIDs,
                            List<string> hydrogenConnectionIDs,
                            List<string> carbonConnectionIDs)
    {
        this.snapshotObject = obj;
        this.carbonIDs = carbonIDs;
        this.hydrogenIDs = hydrogenIDs;
        this.hydrogenConnectionIDs = hydrogenConnectionIDs;
        this.carbonConnectionIDs = carbonConnectionIDs;
    }
}
