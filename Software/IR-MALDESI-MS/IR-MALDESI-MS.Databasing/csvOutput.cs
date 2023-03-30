using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace IR_MALDESI.Databasing
{
    public class csvOutput
    {
        // Raw values and intensities
        public string Filename { get; set; }

        public double Time { get; set; }
        public double Time24hours { get; set; }
        public double PPM { get; set; }
        public double StartingMaterial { get; set; }
        public double StartingMaterialCV { get; set; }
        public double StartingMaterialmz { get; set; }
        public double Product { get; set; }
        public double ProductCV { get; set; }
        public double Productmz { get; set; }
        public double InternalStandard { get; set; }
        public double InternalStandardCV { get; set; }
        public double InternalStandardmz { get; set; }
        public string IntensityCalculation { get; set; }
        public double TIC { get; set; }

        // Derived Values
        public double PercentConversion { get; set; }

        public double PercentConversionCV { get; set; }
        public double ProductToInternalStandard { get; set; }
        public double ProductToTIC { get; set; }

        // Acquisition value
        public int row { get; set; }

        public int column { get; set; }
        public int spot { get; set; }

        // Laser
        public bool AlignmentLaserState { get; set; }

        public int PulsesPerBurst { get; set; }
        public int BurstsPerSpot { get; set; }
        public int SpotsPerTrigger { get; set; }
        public int DelayAfterTrigger { get; set; }
        public int DelayAfterCtrapOpen { get; set; }
        public int DelayBetweenBursts { get; set; }
        public int DelayBetweenSpots { get; set; }

        // Syringe

        public string ESISolventName { get; set; }

        public double ESISolventFlowRate { get; set; }

        public string wellAddress { get; set; }
    }

    public sealed class csvOutputMAP : ClassMap<csvOutput>
    {
        public csvOutputMAP()
        {
            var ind = 0;

            // Can order csv by changing order below
            Map(m => m.Filename).Index(ind); ind += 1;
            Map(m => m.wellAddress).Index(ind); ind += 1;
            Map(m => m.Time).Index(ind); ind += 1;
            Map(m => m.Time24hours).Index(ind); ind += 1;
            Map(m => m.PPM).Index(ind); ind += 1;
            Map(m => m.IntensityCalculation).Index(ind); ind += 1;
            Map(m => m.StartingMaterialmz).Index(ind); ind += 1;
            Map(m => m.StartingMaterial).Index(ind); ind += 1;
            Map(m => m.StartingMaterialCV).Index(ind); ind += 1;
            Map(m => m.Productmz).Index(ind); ind += 1;
            Map(m => m.Product).Index(ind); ind += 1;
            Map(m => m.ProductCV).Index(ind); ind += 1;
            Map(m => m.InternalStandardmz).Index(ind); ind += 1;
            Map(m => m.InternalStandard).Index(ind); ind += 1;
            Map(m => m.InternalStandardCV).Index(ind); ind += 1;
            Map(m => m.TIC).Index(ind); ind += 1;
            Map(m => m.PercentConversion).Index(ind); ind += 1;
            Map(m => m.PercentConversionCV).Index(ind); ind += 1;
            Map(m => m.ProductToInternalStandard).Index(ind); ind += 1;
            Map(m => m.ProductToTIC).Index(ind); ind += 1;
            Map(m => m.row).Index(ind); ind += 1;
            Map(m => m.column).Index(ind); ind += 1;
            Map(m => m.spot).Index(ind); ind += 1;
            Map(m => m.ESISolventName).Index(ind); ind += 1;
            Map(m => m.ESISolventFlowRate).Index(ind); ind += 1;
            Map(m => m.AlignmentLaserState).Index(ind); ind += 1;
            Map(m => m.PulsesPerBurst).Index(ind); ind += 1;
            Map(m => m.BurstsPerSpot).Index(ind); ind += 1;
            Map(m => m.SpotsPerTrigger).Index(ind); ind += 1;
            Map(m => m.DelayAfterTrigger).Index(ind); ind += 1;
            Map(m => m.DelayAfterCtrapOpen).Index(ind); ind += 1;
            Map(m => m.DelayBetweenBursts).Index(ind); ind += 1;
            Map(m => m.DelayBetweenSpots).Index(ind); ind += 1;
        }
    }

    public sealed class tsvOutputMAP : ClassMap<csvOutput>
    {
        public tsvOutputMAP()
        {
            var ind = 0;

            // Can order tsv by changing order below
            Map(m => m.wellAddress).Index(ind); ind += 1;

            //Map(m => m.PercentConversion).TypeConverterOption.NumberStyles(NumberStyles.Number).Index(ind); ind += 1;// So E-5 doesn't show up
            Map(m => m.PercentConversion); ind += 1;// So E-5 doesn't show up
        }
    }

    public sealed class CYPOutputMAP : ClassMap<csvOutput>
    {
        public CYPOutputMAP()
        {
            var ind = 0;

            // Can order tsv by changing order below
            Map(m => m.wellAddress).Index(ind); ind += 1;
            Map(m => m.AlignmentLaserState).Name("Found"); ind += 1;
            Map(m => m.Filename).Name("A number"); ind += 1;

            //Map(m => m.PercentConversion).TypeConverterOption.NumberStyles(NumberStyles.Number).Index(ind); ind += 1;// So E-5 doesn't show up
            Map(m => m.Product).Name("H Adduct"); ind += 1;
            Map(m => m.StartingMaterial).Name("Na/Cl Adduct"); ind += 1;
            Map(m => m.InternalStandard).Name("Custom Adduct"); ind += 1;
        }
    }

    public sealed class ProteinaceousOutputMAP : ClassMap<csvOutput>
    {
        public ProteinaceousOutputMAP()
        {
            var ind = 0;

            // Can order tsv by changing order below
            Map(m => m.wellAddress).Index(ind); ind += 1;
            Map(m => m.AlignmentLaserState).Name("Found"); ind += 1;
            Map(m => m.Filename).Name("A number"); ind += 1;

            //Map(m => m.PercentConversion).TypeConverterOption.NumberStyles(NumberStyles.Number).Index(ind); ind += 1;// So E-5 doesn't show up
            Map(m => m.Product).Name("Bound Protein Intensity"); ind += 1;
            Map(m => m.StartingMaterial).Name("Unbound Protein Intensity"); ind += 1;
            Map(m => m.InternalStandard).Name("Unbound Protein TIC Normalized"); ind += 1;
        }
    }
}