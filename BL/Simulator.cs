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
        public int StepTimer = 1000;
        public int SkimmerSpeed = 5;
        double differenceLat, differenceLon, latPlus, lonPlus;
        public Simulator(BL bL, int id, Action act, Func<bool> func)
        {
            while (!func.Invoke())
            {
                Skimmer skimmer = bL.GetSkimmerr(id);
                switch (skimmer.SkimmerStatus)
                {
                    case SkimmerStatuses.shipping:
                        switch (skimmer.PackageInTransfer.PackageMode)
                        {
                            case ParcelStatus.Assignment:
                                if(skimmer.PackageInTransfer.TransportDistance < 5)
                                {
                                    bL.CollectingPackageBySkimmer(skimmer.Id);
                                }
                                else
                                {
                                    differenceLat = skimmer.PackageInTransfer.CollectionLocation.Latitude - skimmer.Location.Latitude;
                                    differenceLon = skimmer.PackageInTransfer.CollectionLocation.Longitude - skimmer.Location.Longitude;

                                    latPlus = differenceLat / skimmer.PackageInTransfer.TransportDistance * SkimmerSpeed;
                                    lonPlus = differenceLon / skimmer.PackageInTransfer.TransportDistance * SkimmerSpeed;

                                    bL.UploadLessBattry(skimmer.Id, bL.BatteryCalculation2((1 * SkimmerSpeed), -1));
                                    bL.UploadLocation(skimmer.Id, latPlus, lonPlus);
                                }
                                break;
                            case ParcelStatus.Collection:
                                if (skimmer.PackageInTransfer.TransportDistance < 5)
                                {
                                    bL.DeliveryOfPackageBySkimmer(skimmer.Id);
                                }
                                else
                                {
                                    differenceLat = skimmer.PackageInTransfer.DeliveryDestinationLocation.Latitude - skimmer.Location.Latitude;
                                    differenceLon = skimmer.PackageInTransfer.DeliveryDestinationLocation.Longitude - skimmer.Location.Longitude;

                                    latPlus = differenceLat / skimmer.PackageInTransfer.TransportDistance * SkimmerSpeed;
                                    lonPlus = differenceLon / skimmer.PackageInTransfer.TransportDistance * SkimmerSpeed;
                                    bL.UploadLessBattry(skimmer.Id, bL.BatteryCalculation2((1 * SkimmerSpeed), (int)skimmer.PackageInTransfer.WeightCategory));
                                    bL.UploadLocation(skimmer.Id, latPlus, lonPlus);
                                }
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
                            try
                            {
                                if (skimmer.BatteryStatus != 100)
                                    bL.SendingSkimmerForCharging(skimmer.Id);
                                else
                                    bL.AddPackages();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        break;
                }
                Thread.Sleep(StepTimer);
                act();
            }
        }
    }
}
