using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;

namespace BL
{
    class Simulator
    {
        public Simulator(BL bL, int id, Action act, Func<bool> func)
        {
            while (!func.Invoke())
            {
                Skimmer skimmer = bL.GetSkimmerr(id);
                switch(skimmer.SkimmerStatus)
                {
                    case SkimmerStatuses.shipping:
                        switch (skimmer.PackageInTransfer.PackageMode)
                        {
                            case ParcelStatus.Assignment:
                                bL.CollectingPackageBySkimmer(skimmer.Id);
                                break;
                            case ParcelStatus.Collection:
                                bL.DeliveryOfPackageBySkimmer(skimmer.Id);
                                break;
                        }
                        break;
                    case SkimmerStatuses.maintenance:                        
                        if (skimmer.BatteryStatus == 100) 
                            bL.ReleaseSkimmerFromCharging(id);
                        else
                            bL.UploadCharge(skimmer);
                        break;
                    case SkimmerStatuses.free:
                        try
                        {
                            bL.AssigningPackageToSkimmer(skimmer.Id);
                        }
                        catch (Exception ex)
                        {
                            bL.SendingSkimmerForCharging(skimmer.Id);
                        }
                        break;
                }
                Thread.Sleep(StepTimer);
                act();
            }
        }
        public int StepTimer = 1000;
        public int SkimmerSpeed = 20;

    }
}
