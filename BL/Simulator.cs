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
                                if(skimmer.Location==bL.GetCustomer(skimmer.PackageInTransfer.CustomerSends.Id).Location)
                                {
                                    bL.CollectingPackageBySkimmer(skimmer.Id);
                                }
                                else
                                {
                                    bL.UploadLocation(skimmer);
                                }
                                break;
                            case ParcelStatus.Collection:
                                if (skimmer.Location == bL.GetCustomer(skimmer.PackageInTransfer.CustomerReceives.Id).Location)
                                {
                                    bL.DeliveryOfPackageBySkimmer(skimmer.Id);
                                }
                                else
                                {
                                    bL.UploadLocation(skimmer);
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
                            SkimmerToList skimmerToList = bL.GetSkimmerToList(skimmer.Id);
                            DO.BaseStation baseStation = bL.ChecksSmallDistanceBetweenSkimmerAndBaseStation(skimmerToList);
                            Location location = new Location { Latitude = baseStation.Latitude, Longitude = baseStation.Longitude };
                            if (skimmer.Location == location)
                            {
                                try
                                {
                                    bL.SendingSkimmerForCharging(skimmer.Id);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                bL.UploadLocation(skimmer);
                            } 
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
