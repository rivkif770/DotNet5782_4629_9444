using IBL.BO;
using System.Windows;

namespace PL
{
    internal class SkimmerView : Window
    {
        private IBL.IBL bL;
        private SkimmerToList skimmerToList;

        public SkimmerView(IBL.IBL bL, SkimmerToList skimmerToList)
        {
            this.bL = bL;
            this.skimmerToList = skimmerToList;
        }
    }
}