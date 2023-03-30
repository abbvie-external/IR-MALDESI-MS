# IR-MALDESI-MS
Open source CAD models and C# software for making a low-cost IR-MALDESI instrument

[Video](https://github.com/abbvie-external/IR-MALDESI-MS/tree/main/Media/6-axisRobot.mp4)

![alt text](../main/Media/3DModel.png "3D Model")


## Relevant publications

* [High-Throughput Label-Free Biochemical Assays Using Infrared Matrix-Assisted Desorption Electrospray Ionization Mass Spectrometry](https://doi.org/10.1021/acs.analchem.1c00737)
* [Ultra-High-Throughput Ambient MS: Direct Analysis at 22 Samples per Second by Infrared Matrix-Assisted Laser Desorption Electrospray Ionization Mass Spectrometry](https://doi.org/10.1021/acs.analchem.1c04605)
* [High Throughput Intact Protein Analysis Using Infrared Matrix-Assisted Laser Desorption Electrospray Ionization Mass Spectrometry](https://doi.org/10.1101/2021.11.08.467755)

## BOM

### Purchased
* Mid-IR ablation laser - JGM Associates Inc
* 6-axis robot and gripper - TEKMATIC Meca500 robot system + MEGP25 + MGC-AA25
* 3D scanner - Keyence IX-360 + IX-H2000 + OP-88347 + OP-87903 + DL-EN1
* Safety laser scanner - Keyence SZ-V series
* Arduino Mega Rev3
* Power supplies and terminals - Phoenix Contact 2866763 +  2866284 + 2315256 
* Optics - Thorlabs KCB05 + LA5183-E + PF05-03-M02 + SM05V10 + SM05L10
* Syringe pump
* Tecan Roma Gripper
* High voltage connector - Thermo 80100-63141

### Custom fabrication
* (optional) 3D printer - Mark Forged Onyx-One

## Laser

[Characterization of a novel miniaturized burst-mode infrared laser system for IR-MALDESI mass spectrometry imaging](https://doi.org/10.1007/s00216-018-0918-9)

JGM Associates Inc. (JGMA; Burlington, MA) designs and makes custom OEM lasers for instrument applications, including LIBS, mass spectrometry, microscopy, and flow cytometry among other. JGMA works closely with prospective OEM customers who may eventually purchase lasers in some volume for use in their instruments and prefers to get involved early in the customer's product development cycle. JGMA may be willing to provide a loaner laser, and do customization NRE, at no charge to the customer during development of a first custom prototype. The company has been developing lasers for IR-MALDESI ion sources for more than 6 years now.

## Electrospray electrode fabrication

Parts:
* IDEX F-333NX nut and F-245x nanotight tubing sleeve
* Mouser 571-0460-202-1631 Automotive Connectors DT CON PIN #16 16-20 AWG
* Digikey ED1082-ND CONN PC PIN CIRC 0.020DIA GOLD

Steps:
1) Cut off small segment of electrode on the other side of the larger diameter collar
2) Crimp small diameter electrode and pin together
3) Heat shrink just around largest diameter of pin
4) Jam pin/heat shrink as deep as you can in ferrule
5) Cut segment of F-245 to sheath
6) Screw in tight to screw hole to lock

![Electrode](https://github.com/abbvie-external/IR-MALDESI-MS/blob/master/Media/ElectrodeFab.png)
