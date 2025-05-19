using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to hold values for bond lengths and angles
public class Values
{
    public float sigmaConectionLengthC_H = 1.09f; // C-H bond length
    public float sigmaConectionLengthC_C = 1.54f; // C-C bond length
    public float sigmaConectionLengthC_O = 1.43f; // C-O bond length
    public float sigmaConectionLengthC_N = 1.47f; // C-N bond length
    public float sigmaConectionLengthC_S = 1.80f; // C-S bond length
    public float sigmaConectionLengthH_H = 0.74f; // H-H bond length
    public float sigmaConectionLengthO_O = 1.48f; // O-O bond length
    public float sigmaConectionLengthN_N = 1.45f; // N-N bond length

    // Bond angles (in degrees)
    public float sp3Angle = 109.5f; // Tetrahedral (sp3 hybridization)
    public float sp2Angle = 120.0f; // Trigonal planar (sp2 hybridization)
    public float spAngle = 180.0f; // Linear (sp hybridization)

    // Specific bond lengths for double and triple bonds
    public float sigmaConectionLengthC_O_Double = 1.20f; // C=O bond length
    public float sigmaConectionLengthC_C_Double = 1.34f; // C=C bond length
    public float sigmaConectionLengthC_N_Double = 1.34f; // C=N bond length
    public float sigmaConectionLengthC_C_Triple = 1.20f; // C≡C bond length
    public float sigmaConectionLengthC_N_Triple = 1.10f; // C≡N bond length

    // Pi bond spacing
    public float piBondSpacing = 0.17f; // Spacing between pi bonds
}
