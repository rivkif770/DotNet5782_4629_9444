﻿using IBL.BO;
using System;

namespace BL
{
    public class BlObject : IBl
    {
        IDAL.DO.IDal dal;
        public BlObject()
        {
            dal = new DalObject.DalObject();
        }
        public Customer GetCustomer(int id)
        {
            IDAL.DO.Client somoeone;
            try
            {
                somoeone = dal.GetClient(id);
            }
            catch (IDAL.DO.ClientException cex)
            {
                //TO DO
                throw;
            }
            return new Customer
            {
                Id = somoeone.ID,
                Name = somoeone.Name,
                Phone = somoeone.Telephone,
                Location = new Location { Latitude = somoeone.Latitude, Longitude = somoeone.Longitude }
            };
        }
    }
}
